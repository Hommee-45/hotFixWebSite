using HotfixFrameWork;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PatchFileDownloader
{



    public bool m_IsDownloadFinish = false;

    public string m_Progress;

    public string m_DownloadFilename;

    public const string FINISH_PROGRESS = "100%";


    private List<PatchFileInfo> m_FileInfoList;

    //下载更新列表文件url
    private string m_DownloadURL = "";
    //更新列表txt
    private string m_UpdateList = "";

    //每个更新文件的描述信息
    protected class PatchFileInfo
    {
        //str 的 格式是 url.txt|dir/a.txt 
        public PatchFileInfo(string str)
        {
            Parse(str);
        }

        protected virtual void Parse(string str)
        {
            var val = str.Split('|');
            if (val.Length == 1)
            {
                FileName = val[0];
                RelativePath = val[0];
            }
            else if (val.Length == 2)
            {
                FileName = val[0];
                RelativePath = val[1];
            }
            else
            {
                Debug.Log("PatchFileInfo parse error");
            }
        }

        //要被拼接得URL
        public string FileName { get; private set; }

        //文件相对目录
        public string RelativePath { get; private set; }

        //下载文件CRC(解压加密过的diff文件)
        public string DownloadCRCValue { get; private set; }

        //解压解密diff文件之后的CRC
        public string UnzipFileCRCValue { get; private set; }

        //合并之后，新版本ab文件的CRC
        public string OrgCRCValue { get; private set; }


        public float FileTotalSize { get; private set; }

    }


    public delegate void DelegateLoading(int idx, int total, string bundleName, string path);
    public delegate void DelegateLoadOver(bool success);
    //下载完成回调
    private Action m_OnCompleted;


    //正在下载中回调
    public DelegateLoading OnDownLoading;

    //下载完成回调
    public DelegateLoadOver OnDownLoadOver;

    //总共要下载的bundle个数
    private int mTotalBundleCount = 0;

    //当前已下载的bundle个数
    private int mBundleCount = 0;

    //开始下载
    public void DownLoad(string url, string fileName, Action complete)
    {
        mBundleCount = 0;
        mTotalBundleCount = 0;
        m_OnCompleted = complete != null ? complete : () => { };
        OnDownLoadOver += (bool v) => { m_IsDownloadFinish = v; Debug.Log("m_IsDownloadFinish: " + m_IsDownloadFinish); };
        OnDownLoadOver += MergeDiffToTarget;

        m_DownloadURL = url;
        m_UpdateList = fileName;

        CoroutineManager.Instance.StartCoroutine(CoDownLoad(m_DownloadURL, m_UpdateList));
        //StartCoroutine(CoDownLoad(m_DownloadURL, m_UpdateList));
    }


    /// <summary>
    /// 下载Coroutine
    /// </summary>
    /// <param name="url">请求下载地址</param>
    /// <param name="dir">更新配置文件的文件名</param>
    /// <returns></returns>
    private IEnumerator CoDownLoad(string url, string dir)
    {
        //先拼接url
        string fullUrl = Path.Combine(url, dir);
        Debug.Log("下载updateListUrl: " + fullUrl);
        //获得要更新的文件列表
        List<string> originFileList = new List<string>();

        //先下载列表文件
        using (WWW www = new WWW(fullUrl))
        {
            yield return www;
            //yield return new WaitUntil(() => www.isDone == true);
            if (www.error != null)
            {
                //下载失败
                if (null != OnDownLoadOver)
                {
                    try
                    {
                        OnDownLoadOver(false);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e.Message);
                    }
                }
                yield break;
            }

            //创建资源文件夹
            DirectoryHelp.CreateDictionary(GamePathConfig.LOCAL_ANDROID_PATH);
            //保存updateList.txt文件
            DirectoryHelp.CreateFile(www.bytes, GamePathConfig.LOCAL_ANDROID_UPDATELIST_PATH);
            //读取txt 保存到list
            PutIntoList(ref originFileList);

        }


        m_FileInfoList = new List<PatchFileInfo>();
        for (int i = 0; i < originFileList.Count; i++)
        {
            var info = new PatchFileInfo(originFileList[i]);

            if (!CheckNeddDownLoad(info))
            {
                continue;
            }
            m_FileInfoList.Add(info);
        }

        mTotalBundleCount = m_FileInfoList.Count;


        //TODO: 界面返回 0-尚未确定 1-确定更新 -1不更新
        //if (judge == 0) yield return null
        //if (judge == -1) yield break;
        //else 继续执行


        //开始下载所有文件
        for (int i = 0; i< m_FileInfoList.Count; i++)
        {
            var info = m_FileInfoList[i];
            var fileUrl = Path.Combine(url, info.FileName);
            CoroutineManager.Instance.StartCoroutine(CoDownloadAndWriteFile(fileUrl, info));
            //StartCoroutine(CoDownloadAndWriteFile(fileUrl, info));
        }

        //检查是否下载完成
        CoroutineManager.Instance.StartCoroutine(CheckLoadFinish());
        //StartCoroutine(CheckLoadFinish());
    }

    /// <summary>
    /// 检查是否改下载
    /// </summary>
    /// <param name="info"></param>
    /// <returns></returns>
    protected virtual bool CheckNeddDownLoad(PatchFileInfo info)
    {
        return true;
    }

    /// <summary>
    /// 下载并写入文件
    /// </summary>
    /// <param name="url">下载地址</param>
    /// <param name="fileName">文件名字</param>
    /// <returns></returns>
    private IEnumerator CoDownloadAndWriteFile(string url, PatchFileInfo fileInfo)
    {

        using (WWW www = new WWW(url))
        {
            while (!www.isDone)
            {
                m_DownloadFilename = fileInfo.FileName;
                m_Progress = (((int)(www.progress * 100)) % 100) + "%";
                yield return 1;
            }
            m_Progress = FINISH_PROGRESS;

            if (www.error != null)
            {
                Debug.LogError(string.Format("read {0} failed: {1}", url, www.error));
                yield break;
            }

            var writePath = DirectoryHelp.CreateDirectoryRecursive(fileInfo.RelativePath, Application.temporaryCachePath) + "/" + fileInfo.FileName;

            DirectoryHelp.CreateFile(www.bytes, writePath);

            www.Dispose();

            mBundleCount++;

            if (OnDownLoading != null)
            {
                try
                {
                    OnDownLoading(mBundleCount, mTotalBundleCount, writePath, url);
                }
                catch (Exception e)
                {
                    Debug.LogError(e.Message);
                }
            }
        }

    }





    /// <summary>
    /// 检查是否已经下载完毕
    /// </summary>
    /// <returns></returns>
    IEnumerator  CheckLoadFinish()
    {
        while (mBundleCount < mTotalBundleCount)
        {
            yield return null;
        }
        if (OnDownLoadOver != null || m_OnCompleted != null)
        {
            yield return new WaitForSeconds(.5f);
            try
            {
                OnDownLoadOver(true);
                //m_OnCompleted();
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
        }
    }

    private void PutIntoList(ref List<string> list)
    {
        using (StreamReader sr = new System.IO.StreamReader(GamePathConfig.LOCAL_ANDROID_UPDATELIST_PATH, System.Text.Encoding.UTF8))
        {
            string readLine;
            bool condition = true;
            if (sr != null)
            {
                while (true)
                {
                    readLine = sr.ReadLine();
                    if (readLine == null)
                    {
                        condition = false;
                    }
                    if (condition)
                    {
                        list.Add(readLine);
                    }
                    else
                    {
                        break;
                    }
                }
            }

        }
    }





    /// <summary>
    /// 下载完成之后的回调，合并VCDiff与本地生成新版本文件
    /// </summary>
    /// <param name="isSuc">是否进行合并</param>
    /// <param name="complete">完成回调</param>
    private void MergeDiffToTarget(bool isSuc)
    {
        if (!isSuc)
        {
            return;
        }

        //DirectoryHelp.CleanDirectory(GamePathConfig.LOCAL_ANDROID_TEMP_TARGET_1);
        foreach (PatchFileInfo fileSingle in m_FileInfoList)
        {
            //本来资源路径
            string localFilePath = Path.Combine(GamePathConfig.LOCAL_ANDROID_TEMP_TARGET, fileSingle.RelativePath);
            //目标资源路径
            string targetFilePath = DirectoryHelp.CreateDirectoryRecursiveInclude(fileSingle.RelativePath, GamePathConfig.LOCAL_ANDROID_TEMP_TARGET_1);
            //差分文件路径
            string diffFilePath =Path.Combine(DirectoryHelp.CreateDirectoryRecursive(fileSingle.RelativePath, Application.temporaryCachePath), fileSingle.FileName);



            Debug.Log("diffFile url: " + diffFilePath);
            Debug.Log("localFilePath: " + localFilePath);
            Debug.Log("targetFilePath: " + targetFilePath);

            if (!VCDiffHelp.DoDecode(localFilePath, diffFilePath, targetFilePath))
            {
                Debug.LogError("合并出错");
                goto Exit0;
            }
            DirectoryHelp.CopyFile(targetFilePath, localFilePath);

        }
        //DirectoryHelp.CleanDirectory(Application.temporaryCachePath);
        //完成回调
        m_OnCompleted();
    Exit0:
        DirectoryHelp.CleanDirectory(Application.temporaryCachePath);
    }


}
