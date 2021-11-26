using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PureMVC.Tutorial
{
    public abstract class Panel : MonoBehaviour
    {
        protected virtual void Start()
        {
            InitPanel();
            InitDataAndSetComponetState();
            RegisterComponent();
            RegisterCommand();
            ReigsterMediator();
        }

        protected abstract void InitPanel();
        protected abstract void InitDataAndSetComponetState();
        protected abstract void RegisterComponent();
        protected abstract void RegisterCommand();
        protected abstract void ReigsterMediator();

        protected abstract void UnRegisterComponet();
        protected abstract void UnRegisterCommand();
        protected abstract void UnRegisterMediator();

        public virtual void OnDestroy()
        {
            UnRegisterComponet();
            UnRegisterCommand();
            UnRegisterMediator();
        }
    }

}

