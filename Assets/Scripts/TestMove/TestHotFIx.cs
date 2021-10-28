using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using XLua;
using HotfixFrameWork;
using UnityEngine.UI;

public class TestHotFIx : MonoBehaviour
{
    #region 关于下载
    public Text m_Progress;

    public Text m_DownloadFileName;

    //下载器
    private PatchFileDownloader fileDownloader;
    //版本文件下载
    private DownloadVersionFile downloadVersion;
    #endregion

    string fileName = "SphereMove1.lua.txt";

    string fileName1 = "SphereMove2.lua.txt";
    string pathOnline;
    private LuaEnv m_LuaEnv;
    private string pathOffline;
    private bool isReadLua = false;


    // Start is called before the first frame update
    void Start()
    {

        //fileDownloader = GetComponent<PatchFileDownloader>();
        fileDownloader = new PatchFileDownloader();

        m_LuaEnv = new LuaEnv();
        pathOffline = Path.Combine(Application.streamingAssetsPath, fileName);
        pathOnline = Path.Combine(Application.streamingAssetsPath, fileName1);

    }


    private void DownLoadVersionCompleted(VersionResType type, Version version)
    {
        switch (type)
        {
            case VersionResType.DownloadFail:
                Debug.Log("Version 下载失败  可能是版本是最新版");
                break;
            case VersionResType.DownloadSuccess:
                Debug.Log("Version 更新成功");
                break;
            case VersionResType.Different:
                Debug.Log("Version 版本不同");
                fileDownloader.DownLoad(Path.Combine(DownLoadUrlConfig.ANDROID_TAPTAP_ASSETSPATH, GamePathConfig.VERISION_DIFF_FILEDICT), GamePathConfig.ANDROID_UPDATELIST_NAME, downloadVersion.UpdateWriteLocalVersionFile);
                break;
            case VersionResType.Unusual:
                Debug.Log("Version 解析异常");
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //协程管理器刷新
        CoroutineManager.Instance.Tick();

        if (Input.GetKeyDown(KeyCode.J))
        {
            if (!isReadLua)
            {
                isReadLua = true;
                StartCoroutine(LoadLuaFileOffline(pathOffline));
            }
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isReadLua)
            {
                isReadLua = true;
                StartCoroutine(LoadLuaFileOnline(pathOnline));
                downloadVersion = new DownloadVersionFile(Path.Combine(DownLoadUrlConfig.ANDROID_TAPTAP, GamePathConfig.ANDROID_VERSION_FILENAME), GameConfig.DOWNLOAD_FAIL_COUNT, GameConfig.DOWNLOAD_FAIL_RETRY_DELAY, DownLoadVersionCompleted);
            }
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            StartCoroutine(LoadUnity3dFile(Path.Combine(Application.streamingAssetsPath, "luatestcube.unity3d")));
            isReadLua = false;
        }

        if (m_Progress != null && m_DownloadFileName != null)
        {
            m_DownloadFileName.text = fileDownloader.m_DownloadFilename;
            m_Progress.text = fileDownloader.m_Progress;
        }
    }

/// <summary>
/// 下载本地SphereMove1.lua.txt
/// </summary>
/// <param name="savePath"></param>
/// <returns></returns>
    public IEnumerator LoadLuaFileOffline(string savePath)
    {
        WWW www = new WWW(Path.Combine(DownLoadUrlConfig.LOCALHOST_XLUA, fileName));
        Debug.Log("url : " + Path.Combine(DownLoadUrlConfig.LOCALHOST_XLUA, fileName));
        yield return www;

        Debug.Log(fileName + " " + www.error);
        if (www.isDone)
        {
            byte[] bytes = www.bytes;
            Debug.Log("bytes lenght: " + bytes.Length);
            CreateFile(bytes, savePath);

            System.IO.StreamReader sr = new System.IO.StreamReader(savePath, System.Text.Encoding.UTF8);
            if (sr != null)
            {
                m_LuaEnv.DoString(sr.ReadToEnd());
            }
            sr.Close();
        }
        else
        {
            Debug.Log("加载失败");
        }
    }

/// <summary>
/// 拉取本地SphereMove2.lua.txt
/// </summary>
/// <param name="savePath"></param>
/// <returns></returns>
    public IEnumerator LoadLuaFileOnline(string savePath)
    {
        WWW www = new WWW(Path.Combine(DownLoadUrlConfig.LOCALHOST_XLUA, fileName1));
        Debug.Log("url : " + Path.Combine(DownLoadUrlConfig.LOCALHOST_XLUA, fileName1));
        yield return www;

        Debug.Log(fileName1 + " " + www.error);
        if (www.isDone)
        {
            byte[] bytes = www.bytes;
            Debug.Log("bytes lenght: " + bytes.Length);
            CreateFile(bytes, savePath);

            System.IO.StreamReader sr = new System.IO.StreamReader(savePath, System.Text.Encoding.UTF8);
            if (sr != null)
            {
                m_LuaEnv.DoString(sr.ReadToEnd());
            }
            sr.Close();
        }
        else
        {
            Debug.Log("加载失败");
        }
    }

    /// <summary>
    /// 下载本地localhost文件 的unity3d文件
    /// </summary>
    /// <param name="savePath"></param>
    /// <returns></returns>
    public IEnumerator LoadUnity3dFile(string savePath)
    {

        //第一步加载ab文件
        AssetBundle.UnloadAllAssetBundles(true);
        AssetBundle ab = AssetBundle.LoadFromFile(Path.Combine(GamePathConfig.LOCAL_ANDROID_TEMP_TARGET, "AssetBundles/luatestcube.unity3d"));
        Debug.Log("url: " + GamePathConfig.LOCAL_ASSETBUNDLES_PATH + "/luatestcube.unity3d");
        //第二部加载资源
        GameObject sp = ab.LoadAsset<GameObject>("exCube");
        Instantiate(sp);
        //ab.Unload(true);
        yield return null;



    }

    /// <summary>
    /// 测试下载林乐睿文件
    /// </summary>
    /// <param name="savePath"></param>
    /// <returns></returns>
    public IEnumerator LoadLerryFile(string savePath)
    {
        WWW www = new WWW(Path.Combine(DownLoadUrlConfig.IOS, "444.txt"));
        Debug.Log("url : " + Path.Combine(DownLoadUrlConfig.IOS, "444.txt"));
        yield return www;

        Debug.Log("444.txt " + www.error);
        if (www.isDone)
        {
            byte[] bytes = www.bytes;
            Debug.Log("LerryFile: " + bytes.Length);
            CreateFile(bytes, savePath);
        }
        else
        {
            Debug.Log("Lerry File 下载失败");
        }
    }

    /// <summary>
    /// 测试下载林乐睿ab文件
    /// </summary>
    /// <param name="savePath"></param>
    /// <returns></returns>
    public IEnumerator LoadLerryAssetBundleFile(string savePath)
    {
        WWW www = new WWW(Path.Combine(DownLoadUrlConfig.IOS, "enemy.ab"));
        Debug.Log("url : " + Path.Combine(DownLoadUrlConfig.IOS, "enemy.ab"));
        yield return www;

        Debug.Log("enemy.ab " + www.error);
        if (www.isDone)
        {
            byte[] bytes = www.bytes;
            Debug.Log("LerryABFile: " + bytes.Length);
            CreateFile(bytes, savePath);
        }
        else
        {
            Debug.Log("Lerry File 下载失败");
        }
    }



    // public IEnumerator UWRloadAndroidFile(string savePath)
    // {
    //     UnityWebRequest request = UnityWebRequest.Get()
    // }


    /// <summary>
    /// 文件流写入
    /// </summary>
    /// <param name="bytes"></param>
    /// <param name="path"></param>
     private void CreateFile(byte[] bytes, string path)
    {

        using (FileStream fsWrite = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite))
        {
            //清除里面原有的内容
            fsWrite.Seek(0, SeekOrigin.Begin);
            fsWrite.SetLength(0);
            //写入
            fsWrite.Write(bytes, 0, bytes.Length);

            fsWrite.Close();
            fsWrite.Dispose();
        }; 
    }



}
