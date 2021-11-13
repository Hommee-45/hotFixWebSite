using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using XLua;
namespace HotfixFrameWork
{


    [LuaCallCSharp]
    public class LuaScript : MonoBehaviour
    {
        public bool m_Root = false;
        public LuaTable Table {get{return m_ScriptEnv;}}
        public String LuaPath{get{return m_LuaPath;} set{m_LuaPath = value;}}
        private string m_LuaPath;
        private Action m_LuaOnEnable;
        private Action m_LuaAwake;
        private Action m_LuaStart;
        private Action m_LuaUpdate;
        private Action m_LuaOnDisable;
        private Action m_LuaOnDestroy;
        private LuaTable m_ScriptEnv = null;
        private LuaEnv m_LuaEnv;


        public void Awake() 
        {
            if (string.IsNullOrEmpty(LuaPath)) return;
            if (Table == null) Init();
            if (m_LuaAwake != null) m_LuaAwake();
        }

        private void OnEnable()
        {
            if (m_LuaOnEnable != null) m_LuaOnEnable();
        }

        private void OnDisable()
        {
            if (m_LuaOnDisable != null) m_LuaOnDisable();
        }

        void Start()
        {
            if (m_LuaStart != null) m_LuaStart();
        }

        void Update()
        {
            if (m_LuaUpdate != null) m_LuaUpdate();
        }

        void OnDestroy()
        {
            if (m_LuaOnDestroy != null) m_LuaOnDestroy();
            m_ScriptEnv.Dispose();
            m_LuaOnDestroy = null;
            m_LuaOnDisable = null;
            m_LuaUpdate = null;
            m_LuaStart = null;
            m_ScriptEnv = null;
            m_LuaEnv = null;
        }

        public void Init()
        {
            m_LuaEnv = LuaEnvMgr.Instance.LuaEnv;
            m_ScriptEnv = m_LuaEnv.NewTable();
            LuaTable meta = m_LuaEnv.NewTable();
            meta.Set("__index", m_LuaEnv.Global);           //到G表找数据
            m_ScriptEnv.SetMetaTable(meta);                 
            meta.Dispose();
            m_ScriptEnv.Set("self", this);

            Byte[] lua = LuaEnvMgr.Instance.GetLuaText(LuaPath);

            if (meta == null)
            {
                Debug.LogError("meta 为空");
            }
            if (lua == null)
            {
                Debug.LogError("byte lua 为空");
            }
            if (m_ScriptEnv == null)
            {
                Debug.LogError("m_ScriptEnv为空");
            }
            if (Table == null)
            {
                Debug.LogError("Table为空");
            }
            m_LuaEnv.DoString(lua, "LuaScript", m_ScriptEnv);
            m_ScriptEnv.Get("Awake", out m_LuaAwake);
            m_ScriptEnv.Get("Start", out m_LuaStart);
            m_ScriptEnv.Get("Update", out m_LuaUpdate);
            m_ScriptEnv.Get("OnEnable", out m_LuaOnEnable);
            m_ScriptEnv.Get("OnDisable", out m_LuaOnDisable);
            m_ScriptEnv.Get("OnDestroy", out m_LuaOnDestroy);
        }
        
    }
}

