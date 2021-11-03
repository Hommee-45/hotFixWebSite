using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HotfixFrameWork;

public class testFSM : MonoBehaviour
{
    private FSMSystemManager m_FSMSystem;
    private LuaEnvMgr m_LuaEnvMgr;
    // Start is called before the first frame update
    public void Awake() {
        Debug.Log("This is testFSM awake");
    }
    void Start()
    {
        Awake();
        Debug.Log("ASDSAD");
        m_FSMSystem = FSMSystemManager.Instance;
        m_LuaEnvMgr = LuaEnvMgr.Instance;
    }



    // Update is called once per frame
    void Update()
    {
        m_FSMSystem.Update();
        CoroutineManager.Instance.Tick();
    }
}
