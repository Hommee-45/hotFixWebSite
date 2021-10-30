using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace HotfixFrameWork
{

    public static class DownLoadUrlConfig
    {



        public const string LOCALHOST_ASSETSBUNDLE = "http://localhost:81/AssetBundles/";

        public const string LOCALHOST_XLUA = "http://localhost:81/XLua/";

        public const string ANDROID_HUAWEI = "http://119.29.165.241:8088/Android/HuaWei/" ;

        public const string ANDROID_XIAOMI = "http://119.29.165.241:8088/Android/XiaoMI/";

        public const string ANDROID_TAPTAP = "http://119.29.165.241:8088/Android/TapTap/";

        public const string ANDROID_TAPTAP_ASSETSPATH = "http://119.29.165.241:8088/Android/TapTap/AssetBundles/";



        public const string IOS = "http://119.29.165.241:8088/IOS/";


    }



    public static class GamePathConfig
    {

        public static string LOCAL_ANDROID_TEMP_TARGET = Application.streamingAssetsPath + "/TempTarget";

        public static string LOCAL_ANDROID_TEMP_TARGET_1 = Application.streamingAssetsPath + "/TempTarget1";

        public static string LOCAL_ANDROID_UPDATELIST_PATH = Application.streamingAssetsPath + "/Android/updateList.txt";

        public static string LOCAL_ANDROID_PATH = Path.Combine(Application.streamingAssetsPath, "Android");

        //public static string LOCAL_ASSETBUNDLES_PATH = Application.dataPath.Substring(0, Application.dataPath.Length - 6) + "AssetBundles";

        public static string LOCAL_WEB_SERVER_PATH = Application.dataPath.Substring(0, Application.dataPath.Length - 6) + "Web Server/";

        public static string LOCAL_ASSETBUNDLES_PATH = Application.streamingAssetsPath + "/Android/AssetBundles";


        public const string ANDROID_UPDATELIST_NAME = "updateList2019.txt";

        public const string ANDROID_VERSION_FILENAME = "version.txt";

        public static string VERISION_DIFF_FILENAME = "";
    }
}
