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
        Debug.Log("ASDSAD");
        InitFSM();
    }

    private void InitFSM()
    {
        m_FSMSystem = new FSMSystem();
        //拉取版本文件
        DownloadVerState downloadVerState = new DownloadVerState(m_FSMSystem);
        downloadVerState.AddTransition(Transition.Download_Success, StateID.DownloadUpdateListFile);
        downloadVerState.AddTransition(Transition.Download_Failed, StateID.DownloadVersionFile);
        //拉取更新配置表文件
        DownloadUpdateListState downloadUpdateListState = new DownloadUpdateListState(m_FSMSystem);
        downloadUpdateListState.AddTransition(Transition.Download_Success, StateID.DownloadDiffFile);
        downloadUpdateListState.AddTransition(Transition.Download_Failed, StateID.DownloadUpdateListFile);
        //下载差分文件
        DownDiffFileState downDiffFileState = new DownDiffFileState(m_FSMSystem);
        downDiffFileState.AddTransition(Transition.Download_Success, StateID.DownloadTerminate);
        downDiffFileState.AddTransition(Transition.Download_Failed, StateID.DownloadDiffFile);

        //下载终止状态
        DownloadTerminateState downloadTerminateState = new DownloadTerminateState(m_FSMSystem);
        m_FSMSystem.AddState(downloadVerState);
        m_FSMSystem.AddState(downloadUpdateListState);
        m_FSMSystem.AddState(downloadTerminateState);
        m_FSMSystem.AddState(downDiffFileState);


    }

    // Update is called once per frame
    void Update()
    {
        m_FSMSystem.Update();
        CoroutineManager.Instance.Tick();
    }
}
