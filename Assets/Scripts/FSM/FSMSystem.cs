using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HotfixFrameWork
{

    public class FSMSystem
    {
        private Dictionary<StateID, FSMState> m_StateDic = new Dictionary<StateID, FSMState>();
        private StateID m_CurrentStateID;
        private FSMState m_CurrentState;


        /// <summary>
        /// 更新npc的动作
        /// </summary>
        /// <param name="person"></param>
        public void Update(Object person)
        {
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
    }

}

