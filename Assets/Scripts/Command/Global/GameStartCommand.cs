using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PureMVC.Patterns.Command;
using PureMVC.Interfaces;


namespace PureMVC.Tutorial
{
    public class GameStartCommand : SimpleCommand
    {
        public override void Execute(INotification notification)
        {
            base.Execute(notification);
            Debug.Log("游戏启动成功");

            GameObject canvas = GameObject.Find("Canvas");
            GameObject tempObj = UnityEngine.Object.Instantiate(ResourcesManager.Instance.LoadPrefab("HomePanel"));
            tempObj.name = "HomePanel";
            tempObj.transform.SetParent(canvas.transform, false);
            tempObj.AddComponent<HomePanel>();
            //获取数据
            GlobalDataProxy globalDataProxy = ApplicationFacade.Instance.RetrieveProxy(GlobalDataProxy.NAME) as GlobalDataProxy;
            GlobalData globalData = globalDataProxy.GetGlobalData;
            //初始化声音
            SoundManager.Instance.SetMusicVolume((float)globalData.MusicVolume);
            SoundManager.Instance.SetSoundVolume((float)globalData.SoundVolume);
            SoundManager.Instance.PlayMusic("Background Music", SoundManager.SoundType.BackGround);

        }
    }

}

