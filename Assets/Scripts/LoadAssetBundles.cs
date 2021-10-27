using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class LoadAssetBundles : MonoBehaviour
{
    // Start is called before the first frame update
    [System.Obsolete]
    IEnumerator Start()
    {

        while(Caching.ready == false)
        {
            Debug.Log("return null");
            yield return null;
        }
        //var www = WWW.LoadFromCacheOrDownload(@"http://localhost:81/AssetBundles/luatestcube.unity3d", 2)
        //var www = UnityWebRequest.Get(@"http://localhost:81/AssetBundles/luatestcube.unity3d")
        using (var www = UnityWebRequest.Get(@"http://localhost:81/AssetBundles/luatestcube.unity3d"))
        {
            yield return www.SendWebRequest();

            if (www.error != null)
            {
                throw new Exception("www download had an error" + www.error);
            }
            Debug.Log("Progress: " + www.downloadProgress);
            if (www.isDone)
            {
                byte[] results = www.downloadHandler.data;
                var bundle = AssetBundle.LoadFromMemory(www.downloadHandler.data);
                var bytes = www.downloadHandler.data;

                FileInfo fileInfo = new FileInfo(Application.streamingAssetsPath + "/" + bundle.name);
                FileStream fs = fileInfo.Create();

                fs.Write(www.downloadHandler.data, 0, www.downloadHandler.data.Length);
                fs.Flush();

                fs.Close();
                fs.Dispose();
            }


            //if (!string.IsNullOrEmpty(www.error))
            //{
            //    Debug.Log("WWW ERROR: "+ www.isDone);
            //    yield return null;
            //}
            //var assetBundle = www.assetBundle;
            //if (assetBundle == null)
            //{
            //    Debug.Log("assetBundle 为空");
            //}
            //GameObject cubePrefab = assetBundle.LoadAsset<GameObject>("luatestcube");
            //Instantiate(cubePrefab);
        }
    }
}
