using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using UnityEngine;

namespace HotfixFrameWork
{
    public class Util
    {
        public static T GetInstance<T>(ref T instance, string name, bool isDontDestroy = true) where T : UnityEngine.Object
        {
            if (instance != null) return instance;
            if (GameObject.FindObjectOfType<T>() != null)
            {
                return instance;
            }
            GameObject go = new GameObject(name, typeof(T));
            if (isDontDestroy)
            {
                UnityEngine.Object.DontDestroyOnLoad(go);
            }
            instance = go.GetComponent(typeof(T)) as T;
            return instance;
        }

        public static string DeviceResPath
        {
            get
            {
                #if UNITY_EDITOR
                    return string.Format("{0}/{1}", GamePathConfig.LOCAL_ANDROID_TEMP_TARGET, GamePathConfig.ANDROID_LUA_HOTFIX_FOLDERNAME);
                #elif UNITY_ANDROID
                    return string.Format("{0}/{1}", GamePathConfig.LOCAL_ANDROID_TEMP_TARGET, GamePathConfig.ANDROID_LUA_HOTFIX_FOLDERNAME);
                #endif
            }
        }



        public static string GetLuaExtension()
        {
            return ".lua.txt";
        }
        public static string GetLuaPath(string path)
        {
            if (path.IndexOf(GamePathConfig.ANDROID_LUA_HOTFIX_FOLDERNAME) == -1)
            {
                StringBuilder sb = new StringBuilder(path);
                path = sb.Replace("/", "/" + GamePathConfig.ANDROID_LUA_HOTFIX_FOLDERNAME + "/", path.IndexOf('/'), 1).ToString();
            }
            path = string.Format("{0}/{1}{2}", GamePathConfig.LOCAL_ANDROID_TEMP_TARGET, path, GetLuaExtension());
            
            return path;
        }


        /// <summary>
        /// 文件大小单位转换
        /// </summary>
        /// <param name="size">文件大小（单位：Bytes)</param>
        /// <returns></returns>
        public static string HumanReadableFilesize(double size)
        {
            string[] units = new string[]{"B", "KB", "MB", "GB", "TB", "PB"};
            double mod = 1024.0;
            int i = 0;
            while (size >= mod)
            {
                size /= mod;
                i++;
            }
            return Math.Round(size) + units[i];
        }
    }

}
