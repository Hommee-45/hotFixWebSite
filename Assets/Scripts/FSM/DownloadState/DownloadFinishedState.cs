using System.Collections;
using System.Collections.Generic;
using UnityEngine;




namespace HotfixFrameWork
{

    public class DownloadFinishedState : FSMState
    {

        public DownloadFinishedState(FSMSystem fsmSystem): base(fsmSystem)
        {
            m_StateID = StateID.DownloadFinished;
            m_FSMSystem.UnRegisterFSM();
        }

        public override void DoBeforeEnter()
        {
            Debug.Log("===============退出下载");
        }
        public override void Act(Object person = null)
        {
            Debug.Log("===============退出下载");
        }

        public override void Reason(Object person = null)
        {
            
        }

    }

}
