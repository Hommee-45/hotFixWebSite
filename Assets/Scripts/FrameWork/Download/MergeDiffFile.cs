using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace HotfixFrameWork
{
    public enum MergeDiffResType
    {
        MergeSucc,
        MergeFail,
    }

    public class MergeDiffFile
    {

        public MergeDiffFile(Action<MergeDiffResType, int> done) 
        {
            m_OnCompleted = done != null ? done : (MergeDiffResType v, int res) => { };

        }


        private Action<MergeDiffResType, int> m_OnCompleted;

        /// <summary>
        /// 下载完成之后的回调，合并VCDiff与本地生成新版本文件
        /// </summary>
        /// <param name="isSuc">是否进行合并</param>
        /// <param name="complete">完成回调</param>
        public void MergeDiffToTarget()
        {
            foreach (FileDiffTool.Tools.DiffConfig fileSingle in GlobalVariable.g_FileInfoList)
            {
                //本来资源路径
                string localFilePath = Path.Combine(GamePathConfig.LOCAL_ANDROID_TEMP_TARGET, fileSingle.Get_RelativePath());
                //目标资源路径
                // string targetFilePath = DirectoryHelp.CreateDirectoryRecursiveInclude(fileSingle.Get_RelativePath(), GamePathConfig.LOCAL_ANDROID_TEMP_TARGET_1);
                string targetFilePath = DirectoryHelp.CreateDirectoryRecursiveInclude(fileSingle.Get_RelativePath(), GamePathConfig.LOCAL_ANDROID_TEMP_TARGET_1);
                //差分文件路径
                string diffFilePath = Path.Combine(DirectoryHelp.CreateDirectoryRecursive(fileSingle.Get_RelativePath(), GamePathConfig.LOCAL_ANDROID_TEMP_TARGET_1), fileSingle.Get_MD5());
                // Debug.Log("diffFile url: " + diffFilePath);
                // Debug.Log("localFilePath: " + localFilePath);
                // Debug.Log("targetFilePath: " + targetFilePath);
                fileSingle.SetLocalPath(localFilePath);
                fileSingle.SetTargetPath(targetFilePath);


                int res = FileDiffTool.Tools.FileProcessing.SingleRP(fileSingle, GlobalVariable.g_DESKey, localFilePath, diffFilePath, targetFilePath, GamePathConfig.LOCAL_ANDROID_TEMP_TARGET_1);
                Debug.Log("合并结果： " + res);
                if (res < 0)
                {
                    Debug.LogError("合并出错");
                    m_OnCompleted(MergeDiffResType.MergeFail, res);
                    goto Exit0;
                }

                //完成回调
                m_OnCompleted(MergeDiffResType.MergeSucc, res);
            }

            //将临时文件夹中新版资源文件覆盖到旧版文件
            foreach (FileDiffTool.Tools.DiffConfig fileSingle in GlobalVariable.g_FileInfoList)
            {
                if (DirectoryHelp.CopyFile(fileSingle.GetTargetPath(), fileSingle.GetLocalPath()) != 1)
                {
                    m_OnCompleted(MergeDiffResType.MergeFail, -6);
                    Debug.LogError("文件覆盖出错");
                    goto Exit0;
                }
            }
        Exit0:
            //DirectoryHelp.CleanDirectory(Application.temporaryCachePath);
            DirectoryHelp.CleanDirectory(GamePathConfig.LOCAL_ANDROID_TEMP_TARGET_1);
        }
    }
}
