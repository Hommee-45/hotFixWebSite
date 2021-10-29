using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HotfixFrameWork
{
    public class Version
    {
        public string version;
    }

    public enum VersionResType
    {
        DownloadFail,

        DownloadSuccess,

        Different,

        Unusual,
    }

    public class DownloadVersionFile
    {
        private string m_Url;

        private int m_FailRetryCount;

        private float m_FailRetryDelay;

        private Action<VersionResType, Version> m_OnCompleted;

        //服务器版本文件信息
        private Version remoteVersion;
        //本地版本信息文件
        private Version localVersion;

        public DownloadVersionFile(string url, int failRetryCount, float failRetryDelay, Action<VersionResType, Version> callback)
        {
            m_Url = url;
            m_FailRetryCount = failRetryCount;
            m_FailRetryDelay = failRetryDelay;
            m_OnCompleted = callback != null ? callback : (VersionResType t, Version v) => { };

            //StartDownload(0);
        }

        public void StartDownload(float delay)
        {
            WWWMgr.Instance.Download(m_Url, DownloadCompleted, delay);
        }

        private void DownloadCompleted(WWW www)
        {
            if (www == null)
            {
                if (m_FailRetryCount <= 0)
                {
                    m_OnCompleted(VersionResType.DownloadFail, localVersion);
                    return;
                }
                m_FailRetryCount--;
                StartDownload(m_FailRetryDelay);
                return;
            }

            CheckVersion(www.text);
        }

        private void CheckVersion(string text)
        {
            //RemoteVersion.txt
            remoteVersion = VersionHelp.JsonForVersion(text);
            if (remoteVersion == null)
            {
                m_OnCompleted(VersionResType.Unusual, localVersion);
                return;
            }

            //获取本地版本文件
            localVersion = VersionHelp.GetLocalVersionForApp();
            //更改下载版本文件配置
            GamePathConfig.VERISION_DIFF_FILEDICT = localVersion.version + "-" + remoteVersion.version;
            //版本是否一致, 版本不一致的时候 的处理
            if (localVersion != null && localVersion.version != remoteVersion.version)
            {
                m_OnCompleted(VersionResType.Different, remoteVersion);
                //TODO:
                //应该提示UI ： 检查到新版本， 是否更新，现在是默认更新
                goto Exit;
                //return;
            }
            else
            {
                m_OnCompleted(VersionResType.DownloadFail, localVersion);
            }


        Exit:
            DownloadNewVersionFile();
        }   


        public void DownloadNewVersionFile()
        {
            if (remoteVersion == null || remoteVersion.version.Equals(localVersion.version))
            {
                return;
            }

            //更新本地版本文件
            //VersionHelp.WriteLocalVersionFile(remoteVersion);
            //m_OnCompleted(VersionResType.DownloadSuccess, remoteVersion);
        }


        /// <summary>
        /// 更新本地版本文件
        /// </summary>
        public void UpdateWriteLocalVersionFile()
        {
            Debug.Log("覆盖写入版本文件");
            //更新当前版本
            VersionHelp.WriteLocalVersionFile(remoteVersion);
            localVersion = VersionHelp.GetLocalVersionForApp();
            m_OnCompleted(VersionResType.DownloadSuccess, localVersion);
        }
    }
}

