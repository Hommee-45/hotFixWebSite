using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PureMVC.Patterns.Facade;


namespace PureMVC.Tutorial
{

    public class ApplicationFacade : Facade
    {
        public static ApplicationFacade Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = GetInstance(() => new ApplicationFacade());
                }
                return m_Instance as ApplicationFacade;
            }
        }

        protected override void InitializeController()
        {
            base.InitializeController();
            RegisterCommand(NotificationConfig.StartUp, () => new StartupCommand());
            RegisterCommand(NotificationConfig.GameStart, () => new GameStartCommand());
            RegisterCommand(NotificationConfig.CostCupCommand, () => new CostCupCommand());
        }

        protected override void InitializeModel()
        {
            base.InitializeModel();
            RegisterProxy(new GlobalDataProxy(GlobalDataProxy.NAME, new GlobalData()));
        }

        public void StartUpHandle()
        {
            SendNotification(NotificationConfig.StartUp);
        }

        public void GameStartHandle()
        {
            SendNotification(NotificationConfig.GameStart);
        }

    }

}

