using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WWWMgr : MonoBehaviour
{
    private static WWWMgr m_Instance;


    public static WWWMgr Instance
    {
        get { return HotfixFrameWork.Util.GetInstance(ref m_Instance, "_WWWMgr"); }
    }

    public void Download(string url, Action<WWW>done, float delay = 0)
    {
        if (done == null) done = (WWW www) => { };
        CoroutineManager.Instance.StartCoroutine(IEDownload(url, done, delay));

    }

    public void DownloadCorotine(IEnumerator downloader)
    {
        if (downloader != null)
        {
            CoroutineManager.Instance.StartCoroutine(downloader);
        }
    }

    

    private IEnumerator IEDownload(string url, Action<WWW> done, float delay)
    {
        yield return new WaitForSeconds(delay);
        using (WWW www = new WWW(url))
        {
            if (www == null)
            {
                yield break;
            }

            if (www.error != null)
            {
                Debug.LogError(string.Format("{0}: {1}", url, www.error));
                done(null);
            }

            //等到www下载完成再执行下面的代码
            //如果没有这一局，则无法成功下载任何东西
            yield return www;

            if (www.isDone)
            {
                done(www);
            }
        }
        
    }


}
