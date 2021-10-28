using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CopyVersion1AB
{
    private const string VERSION1_0_0 = "v1.0.0";
    private const string VERSION1_0_1 = "v1.0.1";
    private const string VERSION1_0_2 = "v1.0.2";
    private const string VERSION1_0_3 = "v1.0.3";
    private const string VERSION1_0_4 = "v1.0.4";

    private static HotfixFrameWork.Version m_Version = new HotfixFrameWork.Version();

    [MenuItem("AB包/还原文件夹/还原TempTarget文件夹至v1.0.0")]
    public static void RemakeVersion1AB()
    {
        HotfixFrameWork.DirectoryHelp.CopyDirectory(@"D:/Web Server/v1.0.0", HotfixFrameWork.GamePathConfig.LOCAL_ANDROID_TEMP_TARGET + "/AssetBundles");
        SetBackVersionFile(VERSION1_0_0);
    }
    [MenuItem("AB包/还原文件夹/还原TempTarget文件夹至v1.0.1")]
    public static void RemakeVersion1_0_1AB()
    {
        HotfixFrameWork.DirectoryHelp.CopyDirectory(@"D:/Web Server/v1.0.1", HotfixFrameWork.GamePathConfig.LOCAL_ANDROID_TEMP_TARGET + "/AssetBundles");
        SetBackVersionFile(VERSION1_0_1);
    }
    [MenuItem("AB包/还原文件夹/还原TempTarget文件夹至v1.0.2")]
    public static void RemakeVersion1_0_2AB()
    {
        HotfixFrameWork.DirectoryHelp.CopyDirectory(@"D:/Web Server/v1.0.1", HotfixFrameWork.GamePathConfig.LOCAL_ANDROID_TEMP_TARGET + "/AssetBundles");
        SetBackVersionFile(VERSION1_0_2);
    }
    [MenuItem("AB包/还原文件夹/还原TempTarget文件夹至v1.0.3")]
    public static void RemakeVersion1_0_3AB()
    {
        HotfixFrameWork.DirectoryHelp.CopyDirectory(@"D:/Web Server/v1.0.1", HotfixFrameWork.GamePathConfig.LOCAL_ANDROID_TEMP_TARGET + "/AssetBundles");
        SetBackVersionFile(VERSION1_0_3);
    }


    private static void SetBackVersionFile(string ver)
    {

        m_Version.version = ver;
        HotfixFrameWork.VersionHelp.WriteLocalVersionFile(m_Version);

        Debug.Log("成功还原Target文件夹至" + ver);
    }
}
