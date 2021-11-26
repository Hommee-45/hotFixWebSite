using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PureMVC.Interfaces;
using PureMVC.Patterns.Command;


namespace PureMVC.Tutorial
{


    public class StoreToHomeCommand : SimpleCommand
    {
        public override void Execute(INotification notification)
        {
            base.Execute(notification);
            SendNotification(NotificationConfig.CloseStorePanel, null, null);
            SendNotification(NotificationConfig.OpenHomePanel, null, null);
        }
    }

}
