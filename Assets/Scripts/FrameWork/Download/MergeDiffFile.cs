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

        public MergeDiffFile(Action<MergeDiffResType> done) 
        {
            m_OnCompleted = done != null ? done : (MergeDiffResType v) => { };

        }


        private Action<MergeDiffResType> m_OnCompleted;
        /// <summary>
        /// 下载完成之后的回调，合并VCDiff与本地生成新版本文件
        /// </summary>
        /// <param name="isSuc">是否进行合并</param>
        /// <param name="complete">完成回调</param>
        public void MergeDiffToTarget()
        {
            foreach (DiffFileInfo fileSingle in GlobalVariable.g_FileInfoList)
            {
                //本来资源路径
                string localFilePath = Path.Combine(GamePathConfig.LOCAL_ANDROID_TEMP_TARGET, fileSingle.RelativePath);
                //目标资源路径
                string targetFilePath = DirectoryHelp.CreateDirectoryRecursiveInclude(fileSingle.RelativePath, GamePathConfig.LOCAL_ANDROID_TEMP_TARGET_1);
                //差分文件路径
                string diffFilePath = Path.Combine(DirectoryHelp.CreateDirectoryRecursive(fileSingle.RelativePath, Application.temporaryCachePath), fileSingle.FileName);
                //Debug.Log("diffFile url: " + diffFilePath);
                //Debug.Log("localFilePath: " + localFilePath);
                //Debug.Log("targetFilePath: " + targetFilePath);

                if (!VCDiffHelp.DoDecode(localFilePath, diffFilePath, targetFilePath))
                {
                    Debug.LogError("合并出错");
                    m_OnCompleted(MergeDiffResType.MergeFail);
                    goto Exit0;
                }
                DirectoryHelp.CopyFile(targetFilePath, localFilePath);

            }
        //完成回调
        m_OnCompleted(MergeDiffResType.MergeSucc);
        Exit0:
            DirectoryHelp.CleanDirectory(Application.temporaryCachePath);
        }
    }
}
