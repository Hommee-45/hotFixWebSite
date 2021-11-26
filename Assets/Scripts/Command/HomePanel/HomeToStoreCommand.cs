using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PureMVC.Patterns.Command;
using PureMVC.Interfaces;

namespace PureMVC.Tutorial
{
    public class HomeToStoreCommand : SimpleCommand
    {
        public override void Execute(INotification notification)
        {
            base.Execute(notification);
            Debug.Log("HomeToStore");
            GameObject canvasObj = GameObject.Find("Canvas");

            GameObject tempStorePanel = ResourcesManager.Instance.LoadPrefab("StorePanel");
            tempStorePanel.transform.SetParent(canvasObj.transform, false);
            tempStorePanel.name = "StorePanel";
            tempStorePanel.AddComponent<StorePanel>();
        }
    }

}
