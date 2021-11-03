using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace HotfixFrameWork
{

    public static class DownLoadUrlConfig
    {
        //本地服务器AssetBundles热更新资源地址
        public const string LOCALHOST_ASSETSBUNDLE = "http://localhost:81/AssetBundles/";
        //本地服务器XLua热更新资源地址
        public const string LOCALHOST_XLUA = "http://localhost:81/XLua/";
        //云服务器Android平台 华为渠道服url
        public const string ANDROID_HUAWEI = "http://119.29.165.241:8088/Android/HuaWei/" ;
        //云服务器Android平台 小米渠道服 url
        public const string ANDROID_XIAOMI = "http://119.29.165.241:8088/Android/XiaoMI/";
        //云服务器Android平台 TapTap渠道服 url
        public const string ANDROID_TAPTAP = "http://119.29.165.241:8088/Android/TapTap/";
        //云服务器Android平台 TapTap去到热更资源文件夹
        public const string ANDROID_TAPTAP_ASSETSPATH = "http://119.29.165.241:8088/Android/TapTap/AssetBundles/";
        //云服务器IOS平台
        public const string IOS = "http://119.29.165.241:8088/IOS/";


    }



    public static class GamePathConfig
    {
        //客户端 Android存储路径 TempTarget文件夹
        public static string LOCAL_ANDROID_TEMP_TARGET = Application.streamingAssetsPath + "/TempTarget";
        //客户端 Android存储路径 TempTarget1文件夹(目前作为临时文件夹)
        public static string LOCAL_ANDROID_TEMP_TARGET_1 = Application.streamingAssetsPath + "/TempTarget1";
        //客户端 Android 保存更新配置文件列表
        public static string LOCAL_ANDROID_UPDATELIST_PATH = Application.streamingAssetsPath + "/Android/updateList.txt";
        //客户端 Android 文件夹(暂时没用到)
        public static string LOCAL_ANDROID_PATH = Path.Combine(Application.streamingAssetsPath, "Android");

        //public static string LOCAL_ASSETBUNDLES_PATH = Application.dataPath.Substring(0, Application.dataPath.Length - 6) + "AssetBundles";
        //本地存储 热更新版本，版本源文件（用于还原我呢见驾）
        public static string LOCAL_WEB_SERVER_PATH = Application.dataPath.Substring(0, Application.dataPath.Length - 6) + "Web Server/";

#if UNITY_EDITOR
        //客户端 保存热更新所用到的资源 文件夹
        //public static string LOCAL_ASSETBUNDLES_PATH = Application.dataPath + "/Android/AssetBundles/";
        public static string LOCAL_ASSETBUNDLES_PATH = Application.streamingAssetsPath + "/TempTarget/AssetBundles/";
#elif UNITY_ANDROID
        public static string LOCAL_ASSETBUNDLES_PATH = Application.streamingAssetsPath + "/Android/AssetBundles/";
#endif
        //云服务器 配置文件名称
        public const string ANDROID_UPDATELIST_NAME = "updateList2019.txt";
        //云服务器 版本文件名称
        public const string ANDROID_VERSION_FILENAME = "version.txt";

        public const string ANDROID_LUA_HOTFIX_FOLDERNAME = "LuaHotfix";
    }
}
