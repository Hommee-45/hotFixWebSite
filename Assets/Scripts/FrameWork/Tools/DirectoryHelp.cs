using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace HotfixFrameWork
{
    public class DirectoryHelp
    {
        /// <summary>
        /// 清空某个目录
        /// </summary>
        /// <param name="relativePath"></param>
        public static void CleanDirectory(string relativePath)
        {
            var fallPath = Path.Combine(Application.temporaryCachePath, relativePath);
            if (string.IsNullOrEmpty(relativePath))
            {
                Caching.ClearCache();
                return;
            }
            //获取子目录的名称
            var dirs = Directory.GetDirectories(fallPath);
            //获取目录中所有文件
            var files = Directory.GetFiles(fallPath);

            foreach (var file in files)
            {
                File.Delete(file);
            }

            foreach (var dir in dirs)
            {
                Directory.Delete(dir, true);
            }

            Debug.Log("CleanDirectory" + fallPath);
        }


        /// <summary>
        /// 递归创建目录
        /// </summary>
        /// <param name="relativePath">相对路径</param>
        /// <param name="rootDir">根目录</param>
        /// <returns>返回除文件名之外的路径</returns>
        public static string CreateDirectoryRecursive(string relativePath, string rootDir)
        {
            var list = relativePath.Split('/');
            //var temp = Application.temporaryCachePath;
            Debug.Log("tempDir: " + rootDir);
            //var temp = DownLoadUrlConfig.LOCAL_ANDROID_PATH;

            //Length - 1 是为了不让没有子文件夹的相对路径进入循环
            for (int i = 0; i < list.Length - 1; i++)
            {
                var dir = list[i];
                if (string.IsNullOrEmpty(dir))
                {
                    continue;
                }
                rootDir += "/" + dir;
                if (!Directory.Exists(rootDir))
                {
                    Directory.CreateDirectory(rootDir);
                }
            }
            return rootDir;
        }

        /// <summary>
        /// 递归创建目录
        /// </summary>
        /// <param name="relativePath">相对路径</param>
        /// <param name="rootDir">根目录</param>
        /// <returns>返回文件路径(包括文件名)</returns>
        public static string CreateDirectoryRecursiveInclude(string relativePath, string rootDir)
        {
            var list = relativePath.Split('/');
            //var temp = Application.temporaryCachePath;
            Debug.Log("rootDir: " + rootDir);
            //var temp = DownLoadUrlConfig.LOCAL_ANDROID_PATH;

            //Length - 1 是为了不让没有子文件夹的相对路径进入循环
            for (int i = 0; i < list.Length - 1; i++)
            {
                var dir = list[i];
                if (string.IsNullOrEmpty(dir))
                {
                    continue;
                }
                rootDir += "/" + dir;
                if (!Directory.Exists(rootDir))
                {
                    Directory.CreateDirectory(rootDir);
                }
            }

            rootDir += "/" + list[list.Length - 1];
            return rootDir;
        }



        /// <summary>
        /// 文件流写入
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="path"></param>
        public static void CreateFile(byte[] bytes, string path)
        {

            using (FileStream fsWrite = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                //清除里面原有的内容
                fsWrite.Seek(0, SeekOrigin.Begin);
                fsWrite.SetLength(0);
                //写入
                fsWrite.Write(bytes, 0, bytes.Length);

                fsWrite.Flush();
                fsWrite.Close();
                fsWrite.Dispose();
            };
        }



        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="path"></param>
        public static void CreateDictionary(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }



        ///// <summary>
        ///// 复制文件夹
        ///// </summary>
        ///// <param name="sourDir">源文件(夹)</param>
        ///// <param name="destDir">目标文件(夹)</param>
        //public static void CopyFilesDiretionary(string sourDir, string destDir)
        //{
        //    string rootDir = sourDir.Substring(0, sourDir.LastIndexOf('/'));
        //    Debug.Log("投机： " + rootDir);
        //    if (!Directory.Exists(rootDir))
        //    {
        //        Debug.LogError("no such a path: " + sourDir);
        //        return;
        //    }
        //    string[] directories = Directory.GetDirectories(sourDir);

        //    if (directories.Length > 0)
        //    {
        //        foreach (string dir in directories)
        //        {
        //            CopyFilesDiretionary(dir, destDir + dir.Substring(dir.LastIndexOf('/')));
        //        }
        //    }
        //    string[] files = Directory.GetFiles(sourDir);
        //    Debug.Log("files.Length: " + files.Length);
        //    if (files.Length > 0)
        //    {
        //        foreach (string s in files)
        //        {
        //            Debug.Log("source: " + s);
        //            Debug.Log("Dest: " + destDir + s.Substring(s.LastIndexOf('/')));
        //            File.Copy(s, destDir + s.Substring(s.LastIndexOf('/')), true);
        //        }
        //    }

        //}

        /// <summary>
        /// 复制文件夹内所有文件到另一个文件夹
        /// </summary>
        /// <param name="srcPath"></param>
        /// <param name="destPath"></param>
        public static void CopyDirectory(string srcPath, string destPath)
        {
            try
            {
                DirectoryInfo dir = new DirectoryInfo(srcPath); FileSystemInfo[] fileinfo = dir.GetFileSystemInfos();  //获取目录下（不包含子目录）的文件和子目录
                foreach (FileSystemInfo i in fileinfo)
                {
                    if (i is DirectoryInfo)     //判断是否文件夹
                    {
                        if (!Directory.Exists(destPath + "\\" + i.Name))
                        {
                            Directory.CreateDirectory(destPath + "\\" + i.Name);   //目标目录下不存在此文件夹即创建子文件夹
                        }
                        CopyDirectory(i.FullName, destPath + "\\" + i.Name);    //递归调用复制子文件夹
                    }
                    else
                    {
                        File.Copy(i.FullName, destPath + "\\" + i.Name, true);      //不是文件夹即复制文件，true表示可以覆盖同名文件
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }



        /// <summary>
        /// 复制单个文件
        /// </summary>
        /// <param name="sourDir">源文件路径</param>
        /// <param name="destDir">目标文件路径</param>
        public static void CopyFile(string sourDir, string destDir)
        {
            string rootDir = sourDir.Substring(0, sourDir.LastIndexOf('/'));
            Debug.Log("投机： " + rootDir);
            if (!Directory.Exists(rootDir))
            {
                Debug.LogError("no such a path: " + sourDir);
                return;
            }
            File.Copy(sourDir, destDir, true);
        }
    }


}
