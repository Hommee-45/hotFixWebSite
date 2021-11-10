using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HotfixFrameWork;

public class testFSM : MonoBehaviour
{
    private FSMSystemManager m_FSMSystem;
    private LuaEnvMgr m_LuaEnvMgr;

    private void Awake() 
    {    
        m_LuaEnvMgr = LuaEnvMgr.Instance;
        m_LuaEnvMgr.CallLua(string.Format("{0}/Main", "LuaHotfix"));    
    }
    void Start()
    {
        m_FSMSystem = FSMSystemManager.Instance;

    }



    // Update is called once per frame
    void Update()
    {
        m_FSMSystem.Update();
        CoroutineManager.Instance.Tick();
    }
}
