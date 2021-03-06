using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace HotfixFrameWork
{


    public class DownloadVerState : FSMState
    {


        //是否成功下载版本文件
        private FSMDownloadState m_DownloadState;
        //下载是否有回应
        private bool m_IsCallback = false;
        //是否可以下载
        private bool m_IsCanDownload = true;
        //版本文件下载
        private DownloadVersionFile m_DownloadVersionFile;
        public DownloadVerState(FSMSystemManager fsmSystem) : base(fsmSystem)
        {
            m_StateID = StateID.DownloadVersionFile;
        }


        public override void DoBeforeEnter()
        {
            if (m_DownloadVersionFile == null)
            {
                m_DownloadVersionFile = new DownloadVersionFile(
                    Path.Combine(DownLoadUrlConfig.ANDROID_XIAOMI, GamePathConfig.ANDROID_VERSION_FILENAME), 
                    GameConfig.DOWNLOAD_FAIL_COUNT, 
                    GameConfig.DOWNLOAD_FAIL_RETRY_DELAY, 
                    DownLoadVersionCompleted);
            }
            //初始化
            m_IsCanDownload = true;
            m_IsCallback = false;
            m_DownloadState = FSMDownloadState.Downloading;
        }

        public override void DoAfterLeave()
        {
            
        }
        public override void Act(Object person = null)
        {
            if (m_IsCanDownload)
            {
                if (m_DownloadVersionFile == null)
                {
                    m_DownloadVersionFile = new DownloadVersionFile(
                        Path.Combine(DownLoadUrlConfig.ANDROID_XIAOMI, GamePathConfig.ANDROID_VERSION_FILENAME), 
                        GameConfig.DOWNLOAD_FAIL_COUNT, 
                        GameConfig.DOWNLOAD_FAIL_RETRY_DELAY, 
                        DownLoadVersionCompleted);
                }
                m_DownloadVersionFile.StartDownload(0);
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


        private void DownLoadVersionCompleted(DownloadResType type, Version localVersion, Version remoteVersion)
        {
            m_IsCallback = true;
            switch (type)
            {
                case DownloadResType.DownloadFail:
                    Debug.Log("===============Version 下载失败 当前版本为： " + localVersion.version);
                    m_DownloadState = FSMDownloadState.DownloadFail;
                    break;
                case DownloadResType.DownloadSuccess:
                    Debug.Log("===============Version 版本拉取成功 当前版本为： " + localVersion.version + " 最新版本为： " + remoteVersion.version);
                    m_DownloadState = FSMDownloadState.DownSuccess;
                    break;
                case DownloadResType.Different:
                    Debug.Log("===============Version 版本不同 最新版本为： " + localVersion.version);
                    break;
                case DownloadResType.Unusual:
                    if (localVersion != null && remoteVersion != null)
                    {
                        Debug.Log("===============Version 解析异常 当前版本为： " + localVersion.version);
                    }
                    else
                        Debug.Log("===============Version 解析异常 请重启");
                    break;
                case DownloadResType.LatestVersion:
                    Debug.Log("===============Version 无需下载 当前版本为： " + localVersion.version + " 最新版本为： " + remoteVersion.version);
                    m_DownloadState = FSMDownloadState.DownloadFail;
                    break;
            }
        }


    }
}
