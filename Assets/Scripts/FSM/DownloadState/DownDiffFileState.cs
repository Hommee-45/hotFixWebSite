using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


namespace HotfixFrameWork
{




    public class DownDiffFileState : FSMState
    {
        //是否成功下载版本文件
        private FSMDownloadState downloadState;
        //下载器
        private DownloadDiffFile m_PathFileDownloader;
        //下载是否有回应
        private bool m_IsCallback = false;
        //是否可以下载
        private bool m_IsCanDownload = true;
        public DownDiffFileState(FSMSystemManager fsmSystem) : base(fsmSystem)
        {
            m_StateID = StateID.DownloadDiffFile;
        }

        public override void DoBeforeEnter()
        {
            if (m_PathFileDownloader == null)
            {
                m_PathFileDownloader = new DownloadDiffFile(
                    Path.Combine(DownLoadUrlConfig.ANDROID_TAPTAP_ASSETSPATH, GlobalVariable.VERISION_DIFF_FILENAME),
                    GameConfig.DOWNLOAD_FAIL_COUNT,
                    GameConfig.DOWNLOAD_FAIL_RETRY_DELAY,
                    DownloadDiffFileCompleted);
            }
            m_IsCanDownload = true;
            m_IsCallback = false;
            downloadState = FSMDownloadState.Downloading;
        }

        public override void Act(Object person = null)
        {
            if (m_IsCanDownload)
            {
                if (m_PathFileDownloader == null)
                {
                    m_PathFileDownloader = new DownloadDiffFile(
                    Path.Combine(DownLoadUrlConfig.ANDROID_TAPTAP_ASSETSPATH, GlobalVariable.VERISION_DIFF_FILENAME),
                    GameConfig.DOWNLOAD_FAIL_COUNT,
                    GameConfig.DOWNLOAD_FAIL_RETRY_DELAY,
                    DownloadDiffFileCompleted);
                }
                m_PathFileDownloader.Download();
                m_IsCanDownload = false;
            }
        }

        public override void Reason(Object person = null)
        {
            if (m_IsCallback)
            {
                if (downloadState == FSMDownloadState.DownSuccess)
                {
                    m_FSMSystem.PerformTransition(Transition.Download_Success);
                }
                else if (downloadState == FSMDownloadState.DownloadFail)
                {
                    m_FSMSystem.PerformTransition(Transition.Download_Failed);
                }
                m_IsCallback = false;
            }
        }

        private void DownloadDiffFileCompleted(DownloadResType type)
        {
            m_IsCallback = true;
            switch (type)
            {
                case DownloadResType.DownloadFail:
                    Debug.Log("===============下载差分文件失败  ");
                    downloadState = FSMDownloadState.DownloadFail;
                    break;
                case DownloadResType.DownloadSuccess:
                    Debug.Log("===============下载差分文件成功");
                    downloadState = FSMDownloadState.DownSuccess;
                    break;

            }
        }
    }
}
