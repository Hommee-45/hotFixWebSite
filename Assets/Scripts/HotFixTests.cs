using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using XLua;

public class HotFixTests : MonoBehaviour
{
    private LuaEnv m_XLuaEnv;

    // Start is called before the first frame update
    [System.Obsolete]
    void Start()
    {
        m_XLuaEnv = new LuaEnv();

        string path = Application.persistentDataPath + "/ChangeSelf.lua";

        StartCoroutine(DownLoadFile(path));
    }

    [System.Obsolete]
    public IEnumerator DownLoadFile(string path)
    {
        WWW www = new WWW(path);
        yield return www;

        if (www.isDone)
        {
            System.IO.StreamReader sr = new System.IO.StreamReader(path, Encoding.UTF8);
            if (sr != null)
            {
                m_XLuaEnv.DoString(sr.ReadToEnd());
            }
        }
    }
}
