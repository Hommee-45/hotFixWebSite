using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PureMVC.Tutorial
{

    public static class NotificationConfig
    {
        //**********************【Framework】********************
        public const string StartUp = "StartUp";
        public const string GameStart = "GameStart";

        //**********************【Global】**********************
        public const string CostCupCommand = "CostCupCommand";

        //***********************【HomePanel】******************
        public const string OpenHomePanel = "OpenHomePanel";
        public const string CloseHomePanel = "CloseHomePanel";
        public const string HomeToStoreCommand = "HomeToStoreCommand";

        //**********************【StorePanel】***************
        public const string ChangeGoldCup = "ChangeClodCup";
        public const string ChangeSilverCup = "ChangeSilverCup";
        public const string ChangeBronzeCup = "ChangeBronzeCup";
        public const string ChangeCurrencyData = "ChangeCurrencyData";
        public const string StoreToHomePanel = "StoreToHomePanel";
        public const string CloseStorePanel = "CloseStorePanel";
    }

}
