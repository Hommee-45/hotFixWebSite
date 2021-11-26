using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PureMVC.Tutorial
{
    public class HomePanel : Panel
    {
        public const string HomePanelMediatorName = "HomePanelMediator";

        #region Compnent
        [SerializeField]
        private Button m_PlayButton = null;
        [SerializeField]
        private CanvasGroup m_CanvasGroup = null;

        public Action m_PlayAction = null;
        #endregion

        protected sealed override void InitPanel()
        {
            m_PlayButton = transform.Find("playButton").GetComponent<Button>();
            m_CanvasGroup = GetComponent<CanvasGroup>();
        }

        protected override void InitDataAndSetComponetState()
        {
            
        }

        protected sealed override void RegisterComponent()
        {
            m_PlayButton.onClick.AddListener(PlayButtonOnClick);
        }

        protected sealed override void UnRegisterComponet()
        {
            m_PlayButton.onClick.RemoveAllListeners();
        }

        protected sealed override void RegisterCommand()
        {
            ApplicationFacade.Instance.RegisterCommand(NotificationConfig.HomeToStoreCommand, () => new HomeToStoreCommand());
        }
        protected sealed override void UnRegisterCommand()
        {
            ApplicationFacade.Instance.RemoveCommand(NotificationConfig.HomeToStoreCommand);
        }

        protected sealed override void ReigsterMediator()
        {
            ApplicationFacade.Instance.RegisterMediator(new HomePanelMediator(HomePanelMediatorName, this));
        }
        protected sealed override void UnRegisterMediator()
        {
            ApplicationFacade.Instance.RemoveMediator(HomePanelMediatorName);
        }
        #region Event
        private void PlayButtonOnClick()
        {
            m_PlayAction?.Invoke();
        }

        #endregion

        #region ComponentHandle
        public void OpenHomePanel()
        {
            m_CanvasGroup.alpha = 1;
            m_CanvasGroup.interactable = true;
            m_CanvasGroup.blocksRaycasts = true;
        }

        public void CloseHomePanel()
        {
            m_CanvasGroup.alpha = 0;
            m_CanvasGroup.interactable = false;
            m_CanvasGroup.blocksRaycasts = false;
        }

        #endregion
    }

}

