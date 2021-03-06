using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using XLua;
namespace HotfixFrameWork
{

    [LuaCallCSharp]
    public enum DownloadResType
    {
        DownloadFail,

        DownloadSuccess,

        Different,

        Unusual,

        LatestVersion,
    }
    #region 下载版本文件 DownloadVersionFile
    public class Version
    {
        public string version;
    }

    [LuaCallCSharp]
    public class DownloadVersionFile
    {
        private string m_Url;

        private int m_FailRetryCount;

        private float m_FailRetryDelay;

        private Action<DownloadResType, Version, Version> m_OnCompleted;

        //服务器版本文件信息
        private Version remoteVersion;
        //本地版本信息文件
        private Version localVersion;

        public DownloadVersionFile(string url, int failRetryCount, float failRetryDelay, Action<DownloadResType, Version, Version> callback)
        {
            m_Url = url;
            m_FailRetryCount = failRetryCount;
            m_FailRetryDelay = failRetryDelay;
            m_OnCompleted = callback != null ? callback : (DownloadResType t, Version v, Version s) => { };
        }

        public void StartDownload(float delay)
        {
            WWWMgr.Instance.Download(m_Url, DownloadCompleted, delay);
        }

        private void DownloadCompleted(WWW www)
        {
            if (www == null)
            {
                if (m_FailRetryCount <= 0)
                {
                    Debug.Log("DownloadVersionFile Retrying!!!  Remain Time: " + m_FailRetryCount);
                    m_OnCompleted(DownloadResType.DownloadFail, localVersion, remoteVersion);
                    return;
                }
                m_FailRetryCount--;
                StartDownload(m_FailRetryDelay);
                return;
            }

            CheckVersion(www.text);
        }

        private void CheckVersion(string text)
        {
            //RemoteVersion.txt
            remoteVersion = VersionHelp.JsonForVersion(text);
            if (remoteVersion == null)
            {
                m_OnCompleted(DownloadResType.Unusual, localVersion, remoteVersion);
                return;
            }

            //获取本地版本文件
            localVersion = VersionHelp.GetLocalVersionForApp();
            //赋值给全局版本
            GlobalVariable.g_LocalVersion = localVersion;
            //更改下载版本文件配置
            GlobalVariable.VERISION_DIFF_FILENAME = localVersion.version + "-" + remoteVersion.version;
            //版本是否一致, 版本不一致的时候 的处理
            if (localVersion != null && localVersion.version != remoteVersion.version)
            {
                m_OnCompleted(DownloadResType.DownloadSuccess, localVersion, remoteVersion);
                //TODO:
                //应该提示UI ： 检查到新版本， 是否更新，现在是默认更新
                goto Exit;
                //return;
            }
            else if (localVersion != null && localVersion.version == remoteVersion.version)
            {
                m_OnCompleted(DownloadResType.LatestVersion, localVersion, remoteVersion);
            }


        Exit:
            DownloadNewVersionFile();
        }   


        public void DownloadNewVersionFile()
        {
            if (remoteVersion == null || remoteVersion.version.Equals(localVersion.version))
            {
                return;
            }
            GlobalVariable.g_LocalVersion = remoteVersion;
            //更新本地版本文件
            //VersionHelp.WriteLocalVersionFile(remoteVersion);
            //m_OnCompleted(VersionResType.DownloadSuccess, remoteVersion);
        }


        /// <summary>
        /// 更新本地版本文件
        /// </summary>
        public static void UpdateWriteLocalVersionFile()
        {
            Debug.Log("覆盖写入版本文件");
            //更新当前版本
            VersionHelp.WriteLocalVersionFile(GlobalVariable.g_LocalVersion);
        }
    }

    #endregion

    #region 下载更新列表文件DownloadUpdateListFile
    public class DownloadUpdateListFile
    {
        private string m_Url;
        //下载失败后重新下载次数
        private int m_FailRetryCount;
        //重新下载的延迟
        private float m_FailRetryDelay;

        private Action<DownloadResType> m_OnCompleted;

        public DownloadUpdateListFile(string url, int failRetryCount, float failRetryDelay, Action<DownloadResType> callback)
        {
            m_Url = url;
            m_FailRetryCount = failRetryCount;
            m_FailRetryDelay = failRetryDelay;
            m_OnCompleted = callback != null ? callback : (DownloadResType t) => { };

            //StartDownload(0);
        }

        public void StartDownload(float delay = 0)
        {
            WWWMgr.Instance.Download(m_Url, DownloadCompleted, delay);
        }

        private void DownloadCompleted(WWW www)
        {
            if (www == null)
            {
                if (m_FailRetryCount <= 0)
                {
                    m_OnCompleted(DownloadResType.DownloadFail);
                    return;
                }
                m_FailRetryCount--;
                StartDownload(m_FailRetryDelay);
                return;
            }

            //保存updateList.txt文件
            DirectoryHelp.CreateFile(www.bytes, GamePathConfig.LOCAL_ANDROID_UPDATELIST_PATH);
            m_OnCompleted(DownloadResType.DownloadSuccess);
        }
    }

    #endregion

#region 下载DES密码文件
    public class DownloadDESKey
    {
        private string m_Url;

        private int m_FailRetryCount;

        private float m_FailRetryDelay;

        private Action<DownloadResType, string> m_OnCompleted;

        public DownloadDESKey(string url, int failRetryCount, float failRetryDelay, Action<DownloadResType, string> callback)
        {
            m_Url = url;
            m_FailRetryCount = failRetryCount;
            m_FailRetryDelay = failRetryDelay;
            m_OnCompleted = callback != null ? callback : (DownloadResType t, string s) => { };
        }

        public void StartDownload(float delay = 0)
        {
            WWWMgr.Instance.Download(m_Url, DownloadCompleted, delay);
        }

        private void DownloadCompleted(WWW www)
        {
            if (www == null)
            {
                if (m_FailRetryCount <= 0)
                {
                    Debug.Log("DownloadVersionFile Retrying!!!  Remain Time: " + m_FailRetryCount);
                    m_OnCompleted(DownloadResType.DownloadFail, GlobalVariable.g_DESKey);
                    return;
                }
                m_FailRetryCount--;
                StartDownload(m_FailRetryDelay);
                return;
            }
            CheckDESKey(www, GamePathConfig.LOCAL_ANDROID_DESKEY_PATH);
        }

        private void CheckDESKey(WWW www, string path)
        {
            DirectoryHelp.CreateFile(www.bytes, path);
            GlobalVariable.g_DESKey = DirectoryHelp.GetFileAllString(path);
            if (GlobalVariable.g_DESKey != null)
            {
                m_OnCompleted(DownloadResType.DownloadSuccess, GlobalVariable.g_DESKey);
            }
        }

    }
#endregion


    #region 差分文件下载器
    public class DiffFileInfo
    {

        //文件名
        public string FileName { get; private set; }
        //文件相对目录
        public string RelativePath { get; private set; }
        //下载差分文件(加压加密过的diff文件)
        public string DownloadCRCValue { get; private set; }
        //解压解密差分文件后的CRC
        public string UnzipFileCRCValue { get; private set; }
        //合并之后，新版资源文件的CRC
        public string OriginCRCValue { get; private set; }
        //文件大小
        public float FileSize { get; private set; }
        //str 的 格式是 url.txt|dir/a.txt
        public DiffFileInfo(string str)
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


    }
    public class DownloadDiffFile
    {
        #region public变量
        //是否下载完成
        public bool m_IsDownloadFinish = false;
        //是否下载失败
        public bool m_IsDownloadFailed = false;
        //下载进度
        public string m_Progress;
        public string m_DownloadFilename;
        public const string FINISH_PROGRESS = "100%";
        //下载完成回调
        public DelegateLoading m_OnDownLoading;
        //下载中回调
        public DelegateLoadOver m_OnDownLoadOver;
        public delegate void DelegateLoading(int idx, int total, string bundleName, string path);
        public delegate void DelegateLoadOver(bool success);
        #endregion

        #region private/protected
        //每个更新文件的描述信息

        //下载完成回调1
        private Action<DownloadResType> m_OnCompleted;
        //总共要下载的bundle个数
        private int m_TotalBundleCount = 0;
        //当前已下载的bundle个数
        private int m_BundleCount = 0;
        //文件信息列表
        private List<FileDiffTool.Tools.DiffConfig> m_FileInfoList;
        //下载更新列表文件url
        private string m_DownloadURL = "";
        //更新列表txt
        private string m_UpdateList = "";
        //下载失败后重新下载次数
        private int m_FailRetryCount;
        //重新下载的延迟
        private float m_FailRetryDelay;

        #endregion



        #region 协程 CorotineTask 和下载器

        //http下载器
        private HttpDownLoad m_HttpDownload;
        private CoroutineTask m_CoDownload;
        private CoroutineTask m_CheckLoadFinish;
        private List<CoroutineTask> m_CoDownloadAndWriteFiles;



        #endregion


        /// <summary>
        /// 构造函数初始化
        /// </summary>
        /// <param name="url">Diff文件所URL</param>
        /// <param name="failRetryCount">失败重试次数</param>
        /// <param name="failRetryDelay">失败重试延迟</param>
        /// <param name="complete">完成回调</param>
        public DownloadDiffFile(string url, int failRetryCount, float failRetryDelay, Action<DownloadResType> complete)
        {
            m_BundleCount = 0;
            m_TotalBundleCount = 0;
            m_OnCompleted = complete != null ? complete : (DownloadResType v) => { };
            m_OnDownLoadOver += (bool v) => { Debug.Log("==============差分文件下载完成： " + v); };
            m_DownloadURL = url;
            m_FailRetryCount = failRetryCount;
            m_FailRetryDelay = failRetryDelay;
            m_IsDownloadFailed = false;
            m_IsDownloadFinish = false;
            m_HttpDownload = new HttpDownLoad();
            m_CoDownloadAndWriteFiles = new List<CoroutineTask>();
        }

        /// <summary>
        /// 开始下载
        /// </summary>
        /// <param name="url">下载url地址</param>
        /// <param name="complete">完成回调</param>
        public void Download()
        {
            m_CoDownload = CoroutineManager.Instance.StartCoroutine(CoDownload(m_DownloadURL));
        }

        /// <summary>
        /// 检查是否该下载
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool CheckNeddDownLoad(FileDiffTool.Tools.DiffConfig info)
        {
            return true;
        }


        /// <summary>
        /// 下载差分文件 协程
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        [Obsolete]
        private IEnumerator CoDownload(string url)
        {
            //获得要更新的文件列表
            List<string> originFileList = new List<string>();
            
            UpdateInfoToList(ref originFileList);

            m_FileInfoList = new List<FileDiffTool.Tools.DiffConfig>();
            for (int i = 0; i < originFileList.Count; i++)
            {
                FileDiffTool.Tools.DiffConfig info = new FileDiffTool.Tools.DiffConfig();
                info.Parse(originFileList[i]);
                if (!CheckNeddDownLoad(info))
                {
                    continue;
                }
                m_FileInfoList.Add(info);
            }

            m_TotalBundleCount = m_FileInfoList.Count;


            //开始下载所有文件
            for (int i = 0; i < m_FileInfoList.Count; i++)
            {
                var info = m_FileInfoList[i];
                var fileUrl = Path.Combine(url, info.Get_MD5());
                CoroutineTask task = CoroutineManager.Instance.StartCoroutine(CoDownloadAndWriteFile(fileUrl, info));
                m_CoDownloadAndWriteFiles.Add(task);
            }

            //检查是否下载完成
            m_CheckLoadFinish = CoroutineManager.Instance.StartCoroutine(CheckLoadFinish());

            yield return null;
        }


        /// <summary>
        /// 下载差分文件(无断点续传)
        /// </summary>
        /// <param name="url"></param>
        /// <param name="fileInfo"></param>
        /// <returns></returns>
        private IEnumerator CoDownloadAndWriteFile(string url, FileDiffTool.Tools.DiffConfig fileInfo)
        {
            if (m_IsDownloadFailed) yield break;
            yield return new WaitForSeconds(m_FailRetryDelay);
            using (WWW www = new WWW(url))
            {
                while (!www.isDone)
                {
                    //m_DownloadFilename = fileInfo.FileName;
                    m_Progress = (((int)(www.progress * 100)) % 100) + "%";
                    yield return null;
                }
                m_Progress = FINISH_PROGRESS;
                if (www.error != null)
                {
                    DownloadSingleRerty(url, fileInfo);
                    Debug.LogError(string.Format("read {0} failed: {1}", url, www.error));
                    yield break;
                }

                //var writePath = DirectoryHelp.CreateDirectoryRecursive(fileInfo.RelativePath, Application.temporaryCachePath) + "/" + fileInfo.FileName;
                var writePath = DirectoryHelp.CreateDirectoryRecursive(fileInfo.Get_RelativePath(), GamePathConfig.LOCAL_ANDROID_TEMP_TARGET_1) + "/" + fileInfo.Get_MD5();
                Debug.Log("writePath: " + writePath);
                DirectoryHelp.CreateFile(www.bytes, writePath);
                www.Dispose();
                m_BundleCount++;
                if (m_OnDownLoading != null)
                {
                    try
                    {
                        //这里可以显示UI
                        m_OnDownLoading(m_BundleCount, m_TotalBundleCount, writePath, url);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e.Message);
                    }
                }

            }
        }

        /// <summary>
        /// 下载重试
        /// </summary>
        /// <param name="url"></param>
        /// <param name="fileInfo"></param>
        private void DownloadSingleRerty(string url, FileDiffTool.Tools.DiffConfig fileInfo)
        {
            if (m_FailRetryCount <= 0)
            {//重试单个文件下载三次失败
                m_IsDownloadFailed = true;
                CoroutineManager.Instance.StopCoroutine(m_CoDownload);
                m_OnCompleted(DownloadResType.DownloadFail);
                goto Exit0;
            }
            Debug.LogError("url RemainRetry Time: " + m_FailRetryCount);
            m_FailRetryCount--;
            CoDownloadAndWriteFile(url, fileInfo);
        Exit0:
            return;
        }

        /// <summary>
        /// Http断点续传
        /// </summary>
        /// <param name="url">下载网址</param>
        /// <param name="fileInfo">文件信息</param>
        private IEnumerator HttpDownloadAndWriteFile(string url, FileDiffTool.Tools.DiffConfig fileInfo)
        {
            if (m_IsDownloadFailed)  yield break;
            yield return new WaitForSeconds(m_FailRetryDelay);
            var writePath = DirectoryHelp.CreateDirectoryRecursive(fileInfo.Get_RelativePath(), GamePathConfig.LOCAL_ANDROID_TEMP_TARGET_1) + "/" + fileInfo.Get_MD5();
            m_HttpDownload = new HttpDownLoad();
            m_HttpDownload.DownLoad(url, writePath, () =>{m_BundleCount++;});
        }

        /// <summary>
        /// 检查是否已经下载完成
        /// </summary>
        /// <returns></returns>
        private IEnumerator CheckLoadFinish()
        {
            while (m_BundleCount < m_TotalBundleCount)
            {
                yield return null;
            }
            if (m_OnDownLoadOver != null || m_OnCompleted != null)
            {
                try
                {
                    m_OnDownLoadOver?.Invoke(true);

                    m_OnCompleted?.Invoke(DownloadResType.DownloadSuccess);

                    GlobalVariable.g_FileInfoList = m_FileInfoList;
                }
                catch (Exception e)
                {
                    Debug.LogError(e.Message);
                }
            }
        }


        /// <summary>
        /// 读取更新列表文件到List中
        /// </summary>
        /// <param name="list"></param>
        private void UpdateInfoToList(ref List<string> list)
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


        public void OnDisable() 
        {
            m_HttpDownload.isStop = true;
            CoroutineManager.Instance.StopCoroutine(m_CoDownload);
            CoroutineManager.Instance.StopCoroutine(m_CheckLoadFinish);
            foreach (CoroutineTask single in m_CoDownloadAndWriteFiles)
            {
                CoroutineManager.Instance.StopCoroutine(single);
            }
            m_CoDownloadAndWriteFiles.Clear();
        }



    }


    #endregion
}

