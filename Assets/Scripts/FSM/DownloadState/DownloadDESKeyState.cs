using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


namespace HotfixFrameWork
{
    public class DownloadDESKeyState : FSMState
    {
        //是否成功下载版本文件
        private FSMDownloadState m_DownloadState;
        //下载是否有回应
        private bool m_IsCallback = false;
        //是否可以下载
        private bool m_IsCanDownload = true;
        //DESKey下载器
        private DownloadDESKey m_DownloadDESKey;

        public DownloadDESKeyState(FSMSystemManager fsmSystem) : base(fsmSystem)
        {
            m_StateID = StateID.DownloadDESKey;
        }

        public override void DoBeforeEnter()
        {
            if (m_DownloadDESKey == null)
            {
                m_DownloadDESKey = new DownloadDESKey(
                    Path.Combine(DownLoadUrlConfig.ANDROID_XIAOMI_ASSETSPATH, GlobalVariable.VERISION_DIFF_FILENAME, GamePathConfig.ANDROID_DESKEY_FILENAME),
                    GameConfig.DOWNLOAD_FAIL_COUNT,
                    GameConfig.DOWNLOAD_FAIL_RETRY_DELAY,
                    DownLoadDESKeyCompleted);
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
                if (m_DownloadDESKey == null)
                {
                    m_DownloadDESKey = new DownloadDESKey(
                        Path.Combine(DownLoadUrlConfig.ANDROID_XIAOMI_ASSETSPATH, GlobalVariable.VERISION_DIFF_FILENAME, GamePathConfig.ANDROID_DESKEY_FILENAME),
                        GameConfig.DOWNLOAD_FAIL_COUNT,
                        GameConfig.DOWNLOAD_FAIL_RETRY_DELAY,
                        DownLoadDESKeyCompleted);
                }
                m_DownloadDESKey.StartDownload();
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

        public override void DoAfterLeave()
        {
            base.DoAfterLeave();
        }


        private void DownLoadDESKeyCompleted(DownloadResType type, string key)
        {
            m_IsCallback = true;
            switch (type)
            {
                case DownloadResType.DownloadSuccess:
                    Debug.Log("==============DESKey: 拉取成功" + key);
                    m_DownloadState = FSMDownloadState.DownSuccess;
                    break;
                case DownloadResType.DownloadFail:
                    Debug.Log("==============DESKey拉取失败失败");
                    m_DownloadState = FSMDownloadState.DownloadFail;
                    break;
            }
        }
    }

}
