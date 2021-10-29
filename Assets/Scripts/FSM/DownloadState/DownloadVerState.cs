using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace HotfixFrameWork
{
    public enum DownloadState
    {
        DownloadFail = -1,
        Downloading = 0,
        DownSuccess = 1
    }

    public class DownloadVerState : FSMState
    {


        //是否成功下载版本文件
        private DownloadState downloadState;
        //下载是否有回应
        private bool m_IsCallback = false;
        //是否可以下载
        private bool m_IsCanDownload = true;
        //版本文件下载
        private DownloadVersionFile m_DownloadVersionFile;
        public DownloadVerState(FSMSystem fsmSystem) : base(fsmSystem)
        {
            m_StateID = StateID.DownloadVersionFile;
        }


        public override void DoBeforeEnter()
        {
            if (m_DownloadVersionFile == null)
            {
                m_DownloadVersionFile = new DownloadVersionFile(Path.Combine(DownLoadUrlConfig.ANDROID_TAPTAP, GamePathConfig.ANDROID_VERSION_FILENAME), GameConfig.DOWNLOAD_FAIL_COUNT, GameConfig.DOWNLOAD_FAIL_RETRY_DELAY, DownLoadVersionCompleted);
            }
            //初始化
            m_IsCanDownload = true;
            m_IsCallback = false;
            downloadState = DownloadState.Downloading;
        }

        public override void DoAfterLeave()
        {
            
        }
        public override void Act(Object person = null)
        {
            if (m_IsCanDownload)
            {
                m_DownloadVersionFile.StartDownload(0);
                m_IsCanDownload = false;
            }
        }

        public override void Reason(Object person = null)
        {
            if (m_IsCallback)
            {
                if (downloadState == DownloadState.DownSuccess)
                {
                    m_FSMSystem.PerformTransition(Transition.Download_Success);
                }
                else if (downloadState == DownloadState.DownloadFail)
                {
                    m_FSMSystem.PerformTransition(Transition.Download_Failed);
                }
                m_IsCallback = false;

            }
        }


        private void DownLoadVersionCompleted(VersionResType type, Version version)
        {
            m_IsCallback = true;
            switch (type)
            {
                case VersionResType.DownloadFail:
                    Debug.Log("Version 下载失败  可能是版本是最新版, 当前版本为： " + version.version);
                    downloadState = DownloadState.DownloadFail;
                    break;
                case VersionResType.DownloadSuccess:
                    Debug.Log("Version 更新成功 当前版本为：" + version.version);
                    downloadState = DownloadState.DownSuccess;
                    break;
                case VersionResType.Different:
                    Debug.Log("Version 版本不同 最新版本为： " + version.version);
                    break;
                case VersionResType.Unusual:
                    Debug.Log("Version 解析异常 当前版本为： " + version.version);
                    break;
            }
        }


    }
}
