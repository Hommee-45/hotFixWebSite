using System;
using System.Collections.Generic;
using PureMVC.Interfaces;
using PureMVC.Patterns.Mediator;

namespace PureMVC.Tutorial
{
    public class StorePanelMediator : Mediator
    {
        public new static string NAME = "StorePanelMediator";
        public StorePanelMediator(string mediatorName, Object viewComponent = null) : base(mediatorName, viewComponent)
        {

        }

        protected StorePanel GetStorePanel
        {
            get
            {
                return ViewComponet as StorePanel;
            }
        }
        public override void OnRegister()
        {
            base.OnRegister();
            GetStorePanel.GoldBuyAction = GoldBuyActionHandle;
            GetStorePanel.SilverBuyAction = SilverBuyActionHandle;
            GetStorePanel.BronzeBuyAction = BronzeBuyActionHandle;
            GetStorePanel.BackButtonAction = BackButtonActionHandle;
        }

        #region ActionHandle
        protected void GoldBuyActionHandle()
        {
            SoundManager.Instance.PlayMusic("Button");
            SendNotification(NotificationConfig.CostCupCommand, new CostCupCommand.Data(CurrencyType.Gold, 100));
        }
        protected void SilverBuyActionHandle()
        {
            SoundManager.Instance.PlayMusic("Button");
            SendNotification(NotificationConfig.CostCupCommand, new CostCupCommand.Data(CurrencyType.Silver, 100));
        }
        protected void BronzeBuyActionHandle()
        {
            SoundManager.Instance.PlayMusic("Button");
            ApplicationFacade.Instance.SendNotification(NotificationConfig.CostCupCommand, new CostCupCommand.Data(CurrencyType.Bronze, 100));
        }
        protected void BackButtonActionHandle()
        {
            SoundManager.Instance.PlayMusic("Button");
            SendNotification(NotificationConfig.StoreToHomePanel, null, null);
        }
        #endregion

        public override string[] ListNotificationInterests()
        {
            List<string> listNotificationInterests = new List<string>();
            listNotificationInterests.Add(NotificationConfig.ChangeGoldCup);
            listNotificationInterests.Add(NotificationConfig.ChangeSilverCup);
            listNotificationInterests.Add(NotificationConfig.ChangeBronzeCup);
            listNotificationInterests.Add(NotificationConfig.CloseStorePanel);
            return listNotificationInterests.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            int tempNumber = -1;
            if (notification.Body is int)
            {
                tempNumber = (int)notification.Body;
            }
            switch (notification.Name)
            {
                case NotificationConfig.ChangeGoldCup:
                    {
                        GetStorePanel.ChangeCup(CurrencyType.Gold, tempNumber);
                        break;
                    }
                case NotificationConfig.ChangeSilverCup:
                    {
                        GetStorePanel.ChangeCup(CurrencyType.Silver, tempNumber);
                        break;
                    }
                case NotificationConfig.ChangeBronzeCup:
                    {
                        GetStorePanel.ChangeCup(CurrencyType.Bronze, tempNumber);
                        break;
                    }
                case NotificationConfig.CloseStorePanel:
                    {
                        GetStorePanel.CloseCurrencyPanel();
                        break;
                    }
                default:
                    break;
            }
        }
    }
}
