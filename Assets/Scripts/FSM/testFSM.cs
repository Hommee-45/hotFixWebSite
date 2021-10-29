using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HotfixFrameWork;

public class testFSM : MonoBehaviour
{
    private FSMSystem m_FSMSystem;
    // Start is called before the first frame update
    void Start()
    {
        InitFSM();
    }

    private void InitFSM()
    {
        m_FSMSystem = new FSMSystem();
        DownloadVerState downloadVerState = new DownloadVerState(m_FSMSystem);
        downloadVerState.AddTransition(Transition.Download_Success, StateID.DownloadFinished);
        downloadVerState.AddTransition(Transition.Download_Failed, StateID.DownloadVersionFile);
        

        m_FSMSystem.AddState(downloadVerState);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
