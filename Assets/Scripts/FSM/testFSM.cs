﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HotfixFrameWork;

public class testFSM : MonoBehaviour
{
    private FSMSystemManager m_FSMSystem;
    private LuaEnvMgr m_LuaEnvMgr;

    void Start()
    {
        m_FSMSystem = FSMSystemManager.Instance;
        m_LuaEnvMgr = LuaEnvMgr.Instance;
        m_LuaEnvMgr.CallLua(string.Format("{0}/Main", "LuaHotfix"));
    }



    // Update is called once per frame
    void Update()
    {
        m_FSMSystem.Update();
        CoroutineManager.Instance.Tick();
    }
}
