using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


namespace PureMVC.Tutorial
{

    public class StorePanel : Panel
    {
        public const string StorePanelMediatorName = "StorePanelMediator";

        #region Component
        [SerializeField]
        public Text m_GoldText = null;
        [SerializeField]
        public Text m_SilverText = null;
        [SerializeField]
        public Text m_BronzeText = null;
        [SerializeField]
        public Button m_GoldBtn = null;
        [SerializeField]
        public Button m_SilverBtn = null;
        [SerializeField]
        public Button m_BronzeBtn = null;
        [SerializeField]
        public Button m_BackBtn = null;

        public Action GoldBuyAction = null;
        public Action SilverBuyAction = null;
        public Action BronzeBuyAction = null;
        public Action BackButtonAction = null;
        #endregion

        #region 初始化相关
        protected override void InitPanel()
        {
            m_GoldText = transform.Find("GoldText").GetComponent<Text>();
            m_SilverText = transform.Find("SilverText").GetComponent<Text>();
            m_BronzeText = transform.Find("BronzeText").GetComponent<Text>();
            m_GoldBtn = transform.Find("GoldButton").GetComponent<Button>();
            m_SilverBtn = transform.Find("SilverButton").GetComponent<Button>();
            m_BronzeBtn = transform.Find("BronzeButton").GetComponent<Button>();
            m_BackBtn = transform.Find("BackButton").GetComponent<Button>();
        }
        protected override void InitDataAndSetComponetState()
        {
            GlobalDataProxy globalDataProxy = ApplicationFacade.Instance.RetrieveProxy(GlobalDataProxy.NAME) as GlobalDataProxy;
            GlobalData globalData = globalDataProxy.GetGlobalData;
            m_GoldText.text = "Gold: " + globalData.GoldCup.ToString();
            m_SilverText.text = "Silver: " + globalData.SilverCup.ToString();
            m_BronzeText.text = "Bronze: " + globalData.BronzeCup.ToString();

        }
        protected override void RegisterComponent()
        {
            m_GoldBtn.onClick.AddListener(GoldButtonOnClick);
            m_SilverBtn.onClick.AddListener(SilverButtonOnClick);
            m_BronzeBtn.onClick.AddListener(BronzeButtonOnClick);
            m_BackBtn.onClick.AddListener(BackButtonOnClick);
        }
        protected override void UnRegisterComponet()
        {
            m_GoldBtn.onClick.RemoveAllListeners();
            m_SilverBtn.onClick.RemoveAllListeners();
            m_BronzeBtn.onClick.RemoveAllListeners();
            m_BackBtn.onClick.RemoveAllListeners();
        }
        protected override void RegisterCommand()
        {
            ApplicationFacade.Instance.RegisterCommand(NotificationConfig.StoreToHomePanel, () => new StoreToHomeCommand());

        }
        protected override void UnRegisterCommand()
        {
            ApplicationFacade.Instance.RemoveCommand(NotificationConfig.StoreToHomePanel);

        }
        protected override void ReigsterMediator()
        {
            ApplicationFacade.Instance.RegisterMediator(new StorePanelMediator(StorePanelMediatorName, this));
        }
        protected override void UnRegisterMediator()
        {
            ApplicationFacade.Instance.RemoveMediator(StorePanelMediatorName);
        }
        #endregion


        #region Event
        public void GoldButtonOnClick()
        {
            GoldBuyAction?.Invoke();
        }
        public void SilverButtonOnClick()
        {
            SilverBuyAction?.Invoke();
        }
        public void BronzeButtonOnClick()
        {
            BronzeBuyAction?.Invoke();
        }
        public void BackButtonOnClick()
        {
            BackButtonAction?.Invoke();
        }

        #endregion


        #region ComponentHandle
        public void ChangeCup(CurrencyType type, int number)
        {
            switch (type)
            {
                case CurrencyType.Gold:
                    {
                        m_GoldText.text = "Gold: " + number.ToString();
                    }
                    break;
                case CurrencyType.Silver:
                    {
                        m_SilverText.text = "Silver: " + number.ToString();
                    }
                    break;
                case CurrencyType.Bronze:
                    {
                        m_BronzeText.text = "Bronze: " + number.ToString();
                    }
                    break;
                default:
                    break;
            }
        }

        public void CloseCurrencyPanel()
        {
            Destroy(gameObject);
        }

        #endregion
    }

}
