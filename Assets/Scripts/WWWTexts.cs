using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class WWWTexts : MonoBehaviour
{

    private string urlPath = @"http://localhost:81/Xlua/ChangeSelf.lua";

    private string file_SaveUrl;

    private FileInfo file;

    // Start is called before the first frame update
    [System.Obsolete]
    void Start()
    {
        file_SaveUrl = Application.dataPath + "/ChangeSelf.lua";
        file = new FileInfo(file_SaveUrl);
        Debug.Log("Application.persistentDataPath: " + file_SaveUrl);


        StartCoroutine(DownFile(urlPath));
    }

    [System.Obsolete]
    IEnumerator DownFile(string url)
    {
        WWW www = new WWW(url);
        yield return www;

        if (www.isDone)
        {
            Debug.Log("下载完成");

            byte[] bytes = www.bytes;
            CreateFile(bytes);
        }
    }


     private void CreateFile(byte[] bytes)
    {

        Stream stream;
        stream = file.Create(); ;
        stream.Write(bytes, 0, bytes.Length);
        stream.Close();
        stream.Dispose();
    }


}
