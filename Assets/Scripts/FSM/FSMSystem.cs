using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

namespace HotfixFrameWork
{
    [LuaCallCSharp]
    public class FSMSystemManager
    {

        public static FSMSystemManager Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = new FSMSystemManager();
                }
                return m_Instance;
            }
        }
        private static FSMSystemManager m_Instance;
        private Dictionary<StateID, FSMState> m_StateDic = new Dictionary<StateID, FSMState>();
        private StateID m_CurrentStateID;
        private FSMState m_CurrentState;

        private FSMSystemManager()
        {
            InitFSM();
        }

        private void InitFSM()
        {
            //拉取版本文件
            DownloadVerState downloadVerState = new DownloadVerState(this);
            downloadVerState.AddTransition(Transition.Download_Success, StateID.DownloadUpdateListFile);
            downloadVerState.AddTransition(Transition.Download_Failed, StateID.DownloadTerminate);
            //拉取更新配置表文件
            DownloadUpdateListState downloadUpdateListState = new DownloadUpdateListState(this);
            downloadUpdateListState.AddTransition(Transition.Download_Success, StateID.DownloadDESKey);
            downloadUpdateListState.AddTransition(Transition.Download_Failed, StateID.DownloadUpdateListFile);
            //拉取DESkey
            DownloadDESKeyState downloadDESKeyState = new DownloadDESKeyState(this);
            downloadDESKeyState.AddTransition(Transition.Download_Success, StateID.DownloadDiffFile);
            downloadDESKeyState.AddTransition(Transition.Download_Failed, StateID.DownloadDESKey);

            //下载差分文件
            DownDiffFileState downDiffFileState = new DownDiffFileState(this);
            downDiffFileState.AddTransition(Transition.Download_Success, StateID.MergeDiffFile);
            downDiffFileState.AddTransition(Transition.Download_Failed, StateID.DownloadTerminate);

            //合并差分文件
            MergeDiffFileState mergeDiffFileState = new MergeDiffFileState(this);
            mergeDiffFileState.AddTransition(Transition.MergeDiffFileSuccess, StateID.DownloadTerminate);
            mergeDiffFileState.AddTransition(Transition.MergeDiffFileFailed, StateID.MergeDiffFile);

            //下载终止状态
            DownloadTerminateState downloadTerminateState = new DownloadTerminateState(this);
            this.AddState(downloadVerState);
            this.AddState(downloadUpdateListState);
            this.AddState(downloadTerminateState);
            this.AddState(downDiffFileState);
            this.AddState(mergeDiffFileState);
            this.AddState(downloadDESKeyState);
        }

        /// <summary>
        /// 更新npc的动作
        /// </summary>
        /// <param name="person"></param>
        public void Update(Object person = null)
        {
            if (m_CurrentStateID == StateID.DownloadTerminate || m_CurrentStateID == StateID.DownloadFinished)
                return;
            m_CurrentState.Act(person);
            m_CurrentState.Reason(person);
        }


        /// <summary>
        /// 添加新状态
        /// </summary>
        /// <param name="state"></param>
        public void AddState(FSMState state)
        {
            if (state == null)
            {
                Debug.LogError("FSMState cannot be empty");
                return;
            }
            if (m_CurrentState == null)
            {//如果是第一次添加
                m_CurrentState = state;
                m_CurrentStateID = state.StateID;
            }

            if (m_StateDic.ContainsKey(state.StateID))
            {
                Debug.LogError("State " + state.StateID + " has already existed, no more add operation");
                return;
            }

            m_StateDic.Add(state.StateID, state);
        }

        /// <summary>
        /// 删除状态
        /// </summary>
        /// <param name="stateID"></param>
        public void DeleteState(StateID stateID)
        {
            if (stateID == StateID.NullState)
            {
                Debug.LogError("Can not Delete NullState");
                return;
            }
            if (!m_StateDic.ContainsKey(stateID))
            {
                Debug.LogError("Can not Delete state which is not exist");
                return;
            }
            m_StateDic.Remove(stateID);
        }


        /// <summary>
        /// 执行过渡条件满足时对应状态改做的事情
        /// </summary>
        /// <param name="transition"></param>
        public void PerformTransition(Transition transition)
        {
            if (transition == Transition.NullTransition)
            {
                Debug.LogError("Can not execute empty transition");
                return;
            }
            StateID id = m_CurrentState.GetOutputState(transition);
            if (id == StateID.NullState)
            {
                Debug.LogWarning("the current " + m_CurrentStateID + "cannot execute the transition event according to the transition " + transition);
                return;
            }
            if (!m_StateDic.ContainsKey(id))
            {
                Debug.LogError("thie FSM dont have such state" + id + " ,cannot execute state transition");
                return;
            }

            FSMState state = m_StateDic[id];
            m_CurrentState.DoAfterLeave();
            m_CurrentState = state;
            m_CurrentStateID = state.StateID;
            m_CurrentState.DoBeforeEnter();
        }


        public void UnRegisterFSM()
        {
            m_StateDic.Clear();
            m_CurrentStateID = StateID.NullState;
            m_CurrentState = null;
        }
    }

}

