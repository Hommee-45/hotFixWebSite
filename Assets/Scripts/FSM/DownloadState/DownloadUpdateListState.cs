using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


namespace HotfixFrameWork
{

    public class DownloadUpdateListState : FSMState
    {

        //是否成功下载版本文件
        private FSMDownloadState m_DownloadState;
        //下载是否有回应
        private bool m_IsCallback = false;
        //是否可以下载
        private bool m_IsCanDownload = true;
        //版本文件下载
        private DownloadUpdateListFile m_DownLoadUpdateListFile;

        public DownloadUpdateListState(FSMSystemManager fsmSystem) : base(fsmSystem)
        {
            m_StateID = StateID.DownloadUpdateListFile;
        }

        public override void DoBeforeEnter()
        {
            if (m_DownLoadUpdateListFile == null)
            {
                m_DownLoadUpdateListFile = new DownloadUpdateListFile(
                    Path.Combine(DownLoadUrlConfig.ANDROID_XIAOMI_ASSETSPATH, GlobalVariable.VERISION_DIFF_FILENAME, GamePathConfig.ANDROID_UPDATELIST_NAME),
                    GameConfig.DOWNLOAD_FAIL_COUNT,
                    GameConfig.DOWNLOAD_FAIL_RETRY_DELAY,
                    DownLoadUpdateListCompleted);
            }
            //初始化
            m_IsCanDownload = true;
            m_IsCallback = false;
            m_DownloadState = FSMDownloadState.Downloading;
        }

        public override void Act(Object person = null)
        {
            if (m_IsCanDownload)
            {
                if (m_DownLoadUpdateListFile == null)
                {
                    m_DownLoadUpdateListFile = new DownloadUpdateListFile(
                        Path.Combine(DownLoadUrlConfig.ANDROID_XIAOMI_ASSETSPATH, GlobalVariable.VERISION_DIFF_FILENAME, GamePathConfig.ANDROID_UPDATELIST_NAME), 
                        GameConfig.DOWNLOAD_FAIL_COUNT, 
                        GameConfig.DOWNLOAD_FAIL_RETRY_DELAY,
                        DownLoadUpdateListCompleted);
                }
                m_DownLoadUpdateListFile.StartDownload();
                m_IsCanDownload = false;
            }
        }

        public override void Reason(Object person = null)
        {
            if (m_IsCallback)
            {
                if (m_DownloadState == FSMDownloadState.DownSuccess)
                {
                    m_FSMSystem.PerformTransition(Transition.Download_Success);
                }
                else if (m_DownloadState == FSMDownloadState.DownloadFail)
                {
                    m_FSMSystem.PerformTransition(Transition.Download_Failed);
                }
                m_IsCallback = false;

            }
        }







        private void DownLoadUpdateListCompleted(DownloadResType type)
        {
            m_IsCallback = true;
            switch (type)
            {
                case DownloadResType.DownloadFail:
                    Debug.Log("===============updateList拉取失败  ");
                    m_DownloadState = FSMDownloadState.DownloadFail;
                    break;
                case DownloadResType.DownloadSuccess:
                    Debug.Log("===============updateList拉取成功");
                    m_DownloadState = FSMDownloadState.DownSuccess;
                    break;

            }
        }
    }

}

