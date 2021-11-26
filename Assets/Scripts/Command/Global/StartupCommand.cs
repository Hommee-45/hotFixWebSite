using System;
using System.Collections.Generic;
using UnityEngine;
using PureMVC.Patterns.Command;
using PureMVC.Interfaces;

namespace PureMVC.Tutorial
{
    public class StartupCommand : SimpleCommand
    {
        public override void Execute(INotification notification)
        {
            base.Execute(notification);
            GameObject gameStart = GameObject.Find("GameStart");
            if (gameStart == null)
            {
                Debug.LogError("GameStart is Null,Please check it");
                return;
            }
            Debug.Log("GameStart");
        }
    }
}
