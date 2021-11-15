using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

[LuaCallCSharp]
public struct CoroutineTask
{
    public IEnumerator m_Routine;
    public bool m_IsFinish;

    public CoroutineTask(IEnumerator routine)
    {
        m_Routine = routine;
        m_IsFinish = false;
    }

}

[LuaCallCSharp]
public class CoroutineManager
{
    private static CoroutineManager m_Instance;

    public static CoroutineManager Instance
    {
        get
        {
            if (m_Instance == null)
                m_Instance = new CoroutineManager();
            return m_Instance;
        }
    }

    private readonly List<CoroutineTask> m_ActiveTaskList = new List<CoroutineTask>();

    private CoroutineManager() { }

    public void Tick()
    {
        if (m_ActiveTaskList.Count > 0)
        {
            bool anyTaskFinish = false;
            for (int i = 0; i < m_ActiveTaskList.Count; i++)
            {
                CoroutineTask tempTask = m_ActiveTaskList[i];
                if (!CanMoveNext(tempTask.m_Routine))
                {
                    anyTaskFinish = true;
                    tempTask.m_IsFinish = true;
                    m_ActiveTaskList[i] = tempTask;
                }
            }
            if (anyTaskFinish)
            {
                m_ActiveTaskList.RemoveAll(x => x.m_IsFinish == true);
            }
        }
    }


    public  bool CanMoveNext(IEnumerator subTask)
    {
        var child = subTask.Current;
        //yield return另一个协程：递归MoveNext
        if (child != null && child is IEnumerator && CanMoveNext(child as IEnumerator))
            return true;
#if UNITY
　　//yield return www：等待www完成
　　if(child is UnityEngine.WWW && !(child as UnityEngine.WWW).isDone)
　　　　return true;
#endif
        if (subTask.MoveNext())
            return true;
        return false;
    }

    public CoroutineTask StartCoroutine(IEnumerator routine)
    {
        if (routine.MoveNext())
        {
            CoroutineTask tempTask = new CoroutineTask(routine);
            m_ActiveTaskList.Add(tempTask);
            return tempTask;
        }
        return new CoroutineTask();
    }

    public void StopCoroutine(CoroutineTask coroutineTask)
    {
        if (m_ActiveTaskList.Contains(coroutineTask))
        {
            m_ActiveTaskList.Remove(coroutineTask);
        }
    }
}
