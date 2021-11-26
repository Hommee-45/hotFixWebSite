using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Diagnostics;
using GameFramework;

namespace UnityGameFramework.Editor
{

    public static class OpenFolder
    {
        /// <summary>
        /// 打开 Data Path 文件夹
        /// </summary>
        [MenuItem("Game Framework/Open Folder/Data Path", false, 10)]
        public static void OpenFolderDataPath()
        {
            Execute(Application.dataPath);
        }
        /// <summary>
        /// 打开 Persistent Data Path 文件夹
        /// </summary>
        [MenuItem("Game Framework/Open Folder/Persistent Data Path", false, 11)]
        public static void OpenFolderPersistentDataPath()
        {
            Execute(Application.persistentDataPath);
        }
        /// <summary>
        /// 打开 Streaming Assets Path 文件夹
        /// </summary>
        [MenuItem("Game Framework/Open Folder/Streaming Assets Path", false, 12)]
        public static void OpenFolderStreamingAssetsPath()
        {
            Execute(Application.streamingAssetsPath);
        }
        /// <summary>
        /// 打开 Temporary Data Path 文件夹
        /// </summary>
        [MenuItem("Game Framework/Open Folder/Temporary Data Path", false, 13)]
        public static void OpenFolderTemporaryCachePath()
        {
            Execute(Application.temporaryCachePath);
        }
        public static void Execute(string folder)
        {
            folder = Utility.Text.Format("\"{0}\"", folder);
            switch (Application.platform)
            {
                case RuntimePlatform.WindowsEditor:
                    Process.Start("Explorer.exe", folder.Replace('/', '\\'));
                    break;
                case RuntimePlatform.OSXEditor:
                    Process.Start("open", folder);
                    break;
                default:
                    throw new GameFrameworkException(Utility.Text.Format("Not Support open folder on '{0}' platform", Application.platform.ToString()));
            }
        }
    }

}
