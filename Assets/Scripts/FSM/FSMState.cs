using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HotfixFrameWork
{


    /// <summary>
    /// 转换条件
    /// </summary>
    public enum Transition
    {
        NullTransition = 0,             //空的转换条件

        Download_Failed,                //下载失败
        Download_Success,               //下载成功

        DownloadVersionSuccess,         //下载版本成功
        DownloadUpdateListSuccess,      //下载更新列表成功
        DownloadDiffFileSuccess,        //下载差分文件成功


        VeriftFileIntegSuccess,         //校验文件成功
        VeriftFileIntegFail,            //校验文件失败

        MergeDiffFileSuccess,           //合并差分文件成功
        MergeDiffFileFailed,            //合并差分文件失败


    }

    /// <summary>
    /// 当前状态
    /// </summary>
    public enum StateID
    {
        NullState,      //空的状态
        DownloadVersionFile,        //下载版本文件
        DownloadUpdateListFile,     //下载更新列表文件
        DownloadDiffFile,           //下载差分文件
    

        VerifyFileIntegrity,        //校验文件完整
        MergeDiffFile,              //合并差分文件

        DownloadFinished,           //所有下载完成
    }


    public abstract class FSMState
    {
        public StateID StateID { get { return m_StateID; } }

        protected StateID m_StateID;
        protected Dictionary<Transition, StateID> m_TransitionStateDict = new Dictionary<Transition, StateID>();
        protected FSMSystem m_FSMSystem;

        public FSMState(FSMSystem fsmSystem)
        {
            m_FSMSystem = fsmSystem;
        }


        /// <summary>
        /// 添加转换条件
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="id"></param>
        public void AddTransition(Transition trans, StateID id)
        {
            if (trans == Transition.NullTransition)
            {
                Debug.LogError("Not allow NullTransition");
                return;
            }
            if (id == StateID.NullState)
            {
                Debug.LogError("Not allow NullStateID");
                return;
            }
            if (m_TransitionStateDict.ContainsKey(trans))
            {
                Debug.LogError("this key has already in dict");
                return;
            }
        }

        /// <summary>
        /// 删除转换条件
        /// </summary>
        /// <param name="trans"></param>
        public void DeleteTransition(Transition trans)
        {
            if (trans == Transition.NullTransition)
            {
                Debug.LogError("Not allow NullTransition");
                return;
            }
            if (!m_TransitionStateDict.ContainsKey(trans))
            {
                Debug.LogError("Not have such " + trans + " key");
                return;
            }
            m_TransitionStateDict.Remove(trans);
        }

        /// <summary>
        /// 获取当前转换条件下的状态
        /// </summary>
        /// <param name="trans"></param>
        /// <returns></returns>
        public StateID GetOutputState(Transition trans)
        {
            if (m_TransitionStateDict.ContainsKey(trans))
            {
                return m_TransitionStateDict[trans];
            }
            return StateID.NullState;
        }

        /// <summary>
        /// 进入新状态之前做的事情
        /// </summary>
        public virtual void DoBeforeEnter() { }

        /// <summary>
        /// 离开当前状态时做的事
        /// </summary>
        public virtual void DoAfterLeave() { }

        /// <summary>
        /// 当前状态所做的事情
        /// </summary>
        /// <param name="person"></param>
        public abstract void Act(Object person = null);
        /// <summary>
        /// 在某一状态执行过程中， 新的转换条件满足时做的事情
        /// </summary>
        /// <param name="person"></param>
        public abstract void Reason(Object person = null);     //判断转换条件
    }
}
