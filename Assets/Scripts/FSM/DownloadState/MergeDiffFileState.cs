using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HotfixFrameWork
{


    public class MergeDiffFileState : FSMState
    {
        protected enum MergeState
        {
            Merging,
            MergeSucc,
            MergeFail,
        }

        private MergeState m_MergeState;

        //下载是否有回应
        private bool m_IsCallback = false;
        //是否可以下载
        private bool m_IsCanMerge = true;
        //合并器
        private MergeDiffFile m_MergeFiler;
        public MergeDiffFileState(FSMSystem fsmSystem) : base(fsmSystem)
        {
            m_StateID = StateID.MergeDiffFile;
        }

        public override void DoBeforeEnter()
        {
            if (m_MergeFiler == null)
                m_MergeFiler = new MergeDiffFile(MergeFileCompletedd);

            m_IsCallback = false;
            m_IsCanMerge = true;

        }
        public override void Act(Object person = null)
        {
            if (m_IsCanMerge)
            {
                if (m_MergeFiler == null)
                    m_MergeFiler = new MergeDiffFile(MergeFileCompletedd);
                m_MergeFiler.MergeDiffToTarget();
                m_IsCanMerge = false;
            }
        }

        public override void Reason(Object person = null)
        {
            if (m_IsCallback)
            {
                if (m_MergeState == MergeState.MergeSucc)
                    m_FSMSystem.PerformTransition(Transition.MergeDiffFileSuccess);
                else if (m_MergeState == MergeState.MergeFail)
                    m_FSMSystem.PerformTransition(Transition.MergeDiffFileFailed);

                m_IsCallback = false;
            }
        }

        public override void DoAfterLeave()
        {
        }




        private void MergeFileCompletedd(MergeDiffResType type)
        {
            m_IsCallback = true;
            switch (type)
            {
                case MergeDiffResType.MergeSucc:
                    Debug.Log("===============差分文件合并成功");
                    m_MergeState = MergeState.MergeSucc;
                    //合并流程结束了， 更新本地版本号
                    DownloadVersionFile.UpdateWriteLocalVersionFile();
                    break;
                case MergeDiffResType.MergeFail:
                    Debug.Log("===============差分文件合并失败");
                    m_MergeState = MergeState.MergeFail;
                    break;
               
            }
        }
    }
}

