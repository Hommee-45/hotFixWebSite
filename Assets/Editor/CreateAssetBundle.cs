using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class ExportAB
{
    [MenuItem("AB包/导出Windows")]
    public static void ForWindows()
    {
        Export(BuildTarget.StandaloneWindows);
    }
    [MenuItem("AB包/导出Mac")]
    public static void ForMac()
    {
        Export(BuildTarget.StandaloneOSX);
    }

    [MenuItem("AB包/导出iOS")]
    public static void ForiOS()
    {
        Export(BuildTarget.iOS);
    }
    [MenuItem("AB包/导出Android")]
    public static void ForAndroid()
    {
        Export(BuildTarget.Android);
    }


    public static void Export(BuildTarget platform)
    {

        string path = Application.dataPath;

        path = path.Substring(0, path.Length - 6) + "AssetBundles";
        Debug.Log(path);

        //string path2 = Application.streamingAssetsPath;

        //Debug.Log(path2);

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }



        BuildPipeline.BuildAssetBundles(
            path,
            BuildAssetBundleOptions.ChunkBasedCompression | BuildAssetBundleOptions.ForceRebuildAssetBundle,
            platform
            );

        Debug.Log("导出成功");
    }
}
