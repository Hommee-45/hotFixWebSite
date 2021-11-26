using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PureMVC.Interfaces;
using PureMVC.Patterns.Mediator;

namespace PureMVC.Tutorial
{

    public class HomePanelMediator : Mediator
    {
        public new static string NAME = "HomePanelMediator";
        public HomePanelMediator(string mediatorName, object viewComponent = null) : base(mediatorName, viewComponent)
        {

        }
        protected HomePanel GetHomePanel
        {
            get
            {
                return ViewComponet as HomePanel;
            }
        }

        public override void OnRegister()
        {
            base.OnRegister();
            GetHomePanel.m_PlayAction = PlayActionHandle;
        }
        public override void OnRemove()
        {
            base.OnRemove();
            GetHomePanel.m_PlayAction = null;
        }

        protected void PlayActionHandle()
        {
            SoundManager.Instance.PlayMusic("Button");
            SendNotification(NotificationConfig.HomeToStoreCommand, null, null);
            SendNotification(NotificationConfig.CloseHomePanel, null, null);
        }

        public override string[] ListNotificationInterests()
        {
            List<string> listNotificationInterests = new List<string>();
            listNotificationInterests.Add(NotificationConfig.CloseHomePanel);
            listNotificationInterests.Add(NotificationConfig.OpenHomePanel);

            return listNotificationInterests.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch(notification.Name)
            {
                case NotificationConfig.OpenHomePanel:
                    {
                        GetHomePanel.OpenHomePanel();
                        break;
                    }
                case NotificationConfig.CloseHomePanel:
                    {
                        GetHomePanel.CloseHomePanel();
                        break;
                    }
                default:
                    break;
            }
        }
    }

}
