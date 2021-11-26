using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PureMVC.Tutorial
{


    public class Singleton<T> : MonoBehaviour
    {
        static Singleton() { }
        protected  Singleton() { }
        private static volatile Singleton<T> m_Instance = null;
        protected readonly object syncRoot = new object();
        protected static readonly object staticSyncRoot = new object();

        protected static Singleton<T> Instance
        {
            get
            {
                return m_Instance;
            }
        }

        
        protected virtual void Awake()
        {
            m_Instance = this;
        }
        private void OnDestroy()
        {
            m_Instance = null;
        }
    }

}

