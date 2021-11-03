using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using XLua;

namespace HotfixFrameWork
{
    public class LuaEnvMgr : MonoBehaviour
    {
        
        public static LuaEnvMgr Instance
        {
            get{return Util.GetInstance(ref m_Instance, "_LuaEnvMgr");}
        }
        private static LuaEnvMgr m_Instance;
        internal static LuaEnv m_LuaEnv = new LuaEnv();     //all lua behaviour shared one luaEnv only!
        internal static float m_LastGCTime = 0;
        internal static float m_GCInterval = 1;             //1 second
        public LuaEnv LuaEnv
        {
            get{return m_LuaEnv;}
        }
        public LuaScript Create(GameObject go, string luaPath)
        {
            foreach (var b in go.GetComponents<LuaScript>())
            {
                if (b.LuaPath == luaPath)
                    return b;
            }
            var script = go.AddComponent<LuaScript>();
            script.LuaPath = luaPath;
            script.Awake();
            return script;
        }

        public LuaScript CreateScript(GameObject go, string luaPath)
        {
            var script = go.AddComponent<LuaScript>();
            script.LuaPath = luaPath;
            script.Init();
            if (script.gameObject.activeSelf == true)
                script.Awake();
            return script;
        }
        public LuaScript CreateSingle(GameObject go, string luaPath)
        {
            foreach (var b in go.GetComponents<LuaScript>())
            {
                if (b.LuaPath == luaPath)
                    return b;
            }
            return CreateScript(go, luaPath);
        }

        public byte[] GetLuaText(string path)
        {
            string url = Util.GetLuaPath(path);
            if (File.Exists(path))
            {
                return File.ReadAllBytes(path);
            }
            else
            {
                Debug.LogError("No such a path: " + path);
                return null;
            }
        }

        public void CallLua(string lua)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("require ('");
            sb.Append(lua);
            sb.Append("') Main()");
            m_LuaEnv.DoString(sb.ToString());
        }

        private void Awake()
        {
            m_LuaEnv.AddLoader((ref string filePath) =>
            {
                Debug.Log("LuaMgr: " + filePath);
                return GetLuaText(filePath);
            }
            );
        }

        private void Update() {
            if (Time.time - m_LastGCTime > m_GCInterval)
            {
                m_LuaEnv.Tick();
                m_LastGCTime = Time.time;
            }
        }

        public void FastTick()
        {
            m_GCInterval = 0;
        }
        public void RestoreTick()
        {
            m_GCInterval = 1;
        }

    }
}

