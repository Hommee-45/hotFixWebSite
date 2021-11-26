using System;
using System.Collections.Generic;
using PureMVC.Interfaces;
using PureMVC.Patterns.Command;

namespace PureMVC.Tutorial
{
    public class CostCupCommand : SimpleCommand
    {
        public class Data
        {
            public CurrencyType currencyType = CurrencyType.None;
            public int costCupNumber = 0;
            public Data (CurrencyType tempCurrencyType, int tempCostCupNumer)
            {
                currencyType = tempCurrencyType;
                costCupNumber = tempCostCupNumer;
            }
        }

        public override void Execute(INotification notification)
        {
            base.Execute(notification);
            Data data = notification.Body as Data;
            if (data == null)
            {
                return;
            }
            GlobalDataProxy globalDataProxy = (GlobalDataProxy)ApplicationFacade.Instance.RetrieveProxy(GlobalDataProxy.NAME);
            globalDataProxy.CostCup(data.currencyType, data.costCupNumber);
            GlobalData globalData = globalDataProxy.GetGlobalData;
            switch (data.currencyType)
            {
                case CurrencyType.Gold:
                    ApplicationFacade.Instance.SendNotification(NotificationConfig.ChangeGoldCup, globalData.GoldCup);
                    break;
                case CurrencyType.Silver:
                    ApplicationFacade.Instance.SendNotification(NotificationConfig.ChangeSilverCup, globalData.SilverCup);
                    break;
                case CurrencyType.Bronze:
                    ApplicationFacade.Instance.SendNotification(NotificationConfig.ChangeBronzeCup, globalData.BronzeCup);
                    break;
                default:
                    break;
            }

        }
    }
}
