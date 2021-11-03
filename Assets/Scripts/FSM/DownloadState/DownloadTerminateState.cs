using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HotfixFrameWork
{


    public class DownloadTerminateState : FSMState
    {
        public DownloadTerminateState(FSMSystemManager fsmSystem) : base(fsmSystem)
        {
            m_StateID = StateID.DownloadTerminate;
            m_FSMSystem.UnRegisterFSM();
        }

        public override void DoBeforeEnter()
        {
            Debug.Log("===============终止下载");
        }
        public override void Act(Object person = null)
        {
            Debug.Log("===============终止下载");
        }

        public override void Reason(Object person = null)
        {
            
        }
    }

}
