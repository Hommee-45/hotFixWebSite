using UnityEngine;
using PureMVC.Patterns.Proxy;
using System.IO;
using System;
using Newtonsoft.Json;

namespace PureMVC.Tutorial
{
    public class GlobalDataProxy : Proxy
    {
        public new static string NAME = "GloabalDataProxy";

        public GlobalDataProxy(string proxyName, object data = null) : base(proxyName, data) { }

        public GlobalData GetGlobalData
        {
            get { return Data as GlobalData; }
        }

        public override void OnRegister()
        {
            base.OnRegister();
            DeserializeData();
        }

        public override void OnRemove()
        {
            base.OnRemove();
            SerializeData();
        }
        public void SerializeData()
        {
            string jsonStr = JsonConvert.SerializeObject(GetGlobalData);
            if (!Directory.Exists(Application.streamingAssetsPath))
            {
                Directory.CreateDirectory(Application.streamingAssetsPath);
            }
            File.WriteAllText(Application.streamingAssetsPath + "/" + "GlobalData.json", jsonStr);
        }

        public void DeserializeData()
        {
            if (!Directory.Exists(Application.streamingAssetsPath))
            {
                Directory.CreateDirectory(Application.streamingAssetsPath);
            }
            string jsonStr = File.ReadAllText(Application.streamingAssetsPath + "/" + "GlobalData.json");
            Data = (GlobalData)JsonConvert.DeserializeObject(jsonStr);
        }
        public void CostCup(CurrencyType currencyType, int costCupNumber)
        {
            switch(currencyType)
            {
                case CurrencyType.Gold:
                    {
                        GetGlobalData.GoldCup -= costCupNumber;
                    }
                    break;
                case CurrencyType.Silver:
                    {
                        GetGlobalData.SilverCup -= costCupNumber;
                    }
                    break;
                case CurrencyType.Bronze:
                    {
                        GetGlobalData.BronzeCup -= costCupNumber;
                    }
                    break;
            }
        }

    }

    [Serializable]
    public class GlobalData
    {
        public int BoyOrGirl { get; set; }
        public double MusicVolume { get; set; }
        public double SoundVolume { get; set; }
        public int ThemeIndex { get; set; }
        public int ItemCount { get; set; }
        public int GoldCup { get; set; }
        public int SilverCup { get; set; }
        public int BronzeCup { get; set; }

    }
}

