using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using System.Text;
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


    private testFSM m_TestFSM;

    // Start is called before the first frame update
    void Start()
    {
        m_TestFSM = GetComponent<testFSM>();
        //fileDownloader = GetComponent<PatchFileDownloader>();
        //fileDownloader = new PatchFileDownloader();

        m_LuaEnv = new LuaEnv();
        pathOffline = Path.Combine(Application.streamingAssetsPath, fileName);
        pathOnline = Path.Combine(Application.streamingAssetsPath, fileName1);

    }

    // Update is called once per frame
    void Update()
    {
        //协程管理器刷新
        CoroutineManager.Instance.Tick();

        if (Input.GetKeyDown(KeyCode.J))
        {

        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isReadLua)
            {
                isReadLua = true;
                m_TestFSM.enabled = true;
                
            }
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            StartCoroutine(LoadUnity3dFile(Path.Combine(Application.streamingAssetsPath, "luatestcube.unity3d")));
            isReadLua = false;
        }

        if (m_Progress != null && m_DownloadFileName != null)
        {
            //m_DownloadFileName.text = fileDownloader.m_DownloadFilename;
            //m_Progress.text = fileDownloader.m_Progress;
        }
    }




    /// <summary>
    /// 下载本地localhost文件 的unity3d文件
    /// </summary>
    /// <param name="savePath"></param>
    /// <returns></returns>
    public IEnumerator LoadUnity3dFile(string savePath)
    {
        GameObject cubePre = LoadABManager.Instance.LoadAssetByAB<GameObject>("luatestcube.unity3d", "exCube");
        if (cubePre != null) Instantiate(cubePre);
        LoadABManager.Instance.UnloadAssetBundle("luatestcube.unity3d", false);
        yield return null;

    }




}
