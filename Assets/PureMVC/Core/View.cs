using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using PureMVC.Interfaces;
using PureMVC.Patterns.Observer;

namespace PureMVC.Core
{
    public class View : IView
    {
        protected readonly ConcurrentDictionary<string, IMediator> m_MediatorMap;

        protected readonly ConcurrentDictionary<string, IList<IObserver>> m_ObserverMap;

        protected static IView m_Instance;

        protected const string SINGLETON_MSG = "View Singleton already constructed!";

        public View()
        {
            if (m_Instance != null) throw new Exception(SINGLETON_MSG);
            m_Instance = this;
            m_MediatorMap = new ConcurrentDictionary<string, IMediator>();
            m_ObserverMap = new ConcurrentDictionary<string, IList<IObserver>>();
            InitializeView();
        }

        public static IView GetInstance(Func<IView> viewFunc)
        {
            if (m_Instance == null)
            {
                m_Instance = viewFunc();
            }
            return m_Instance;
        }

        public virtual void RegisterObserver(string notificationName, IObserver observer)
        {
            if (m_ObserverMap.TryGetValue(notificationName, out IList<IObserver> observers))
            {
                observers.Add(observer);
            }
            else
            {
                m_ObserverMap.TryAdd(notificationName, new List<IObserver> { observer });
            }
        }

        public virtual void NotifyObservers(INotification notification)
        {
            if (m_ObserverMap.TryGetValue(notification.Name, out IList<IObserver> observers_ref))
            {
                // Copy observers from reference array to working array, 
                // since the reference array may change during the notification loop
                var observers = new List<IObserver>(observers_ref);

                // Notify Observers from the working array
                foreach (IObserver observer in observers)
                {
                    observer.NotifyObserver(notification);
                }
            }
        }

        public virtual void RemoveObserver(string notificationName, object notifyContext)
        {
            // the observer list for the notification under inspection
            if (m_ObserverMap.TryGetValue(notificationName, out IList<IObserver> observers))
            {
                // find the observer for the notifyContext
                for (int i = 0; i < observers.Count; i++)
                {
                    // there can only be one Observer for a given notifyContext 
                    // in any given Observer list, so remove it and break
                    if (observers[i].CompareNotifyContext(notifyContext))
                    {
                        observers.RemoveAt(i);
                        break;
                    }
                }
                // Also, when a Notification's Observer list length falls to
                // zero, delete the notification key from the observer map
                if (observers.Count == 0)
                    m_ObserverMap.TryRemove(notificationName, out IList<IObserver> _);
            }
        }

        public virtual void RegisterMediator(IMediator mediator)
        {
            //不允许重复注册，以中介名称为Key
            if (m_MediatorMap.TryAdd(mediator.MediatorName, mediator))
            {
                // 获得此Mediator中 视图需要关注的消息列表
                string[] interests = mediator.ListNotificationInterests();
                // 判断是否有消息需要注册
                if (interests.Length > 0)
                {
                    // 获取对应Mediator中HandleNotification函数的引用，实例化一个Observer
                    IObserver observer = new Observer(mediator.HandleNotification, mediator);

                    // 根据消息列表的长度创建对应数量的消息观察者
                    for (int i = 0; i < interests.Length; i++)
                    {
                        RegisterObserver(interests[i], observer);
                    }
                }
                // 注册对应Mediator后的回调
                mediator.OnRegister();
            }
        }

        public virtual IMediator RetrieveMediator(string mediatorName)
        {
            return m_MediatorMap.TryGetValue(mediatorName, out IMediator mediator) ? mediator : null;
        }

        public virtual IMediator RemoveMediator(string mediatorName)
        {
            //Retrieve the named mediator
            if (m_MediatorMap.TryRemove(mediatorName, out IMediator mediator))
            {
                // for every notification this mediator is interested in...
                string[] interests = mediator.ListNotificationInterests();
                for (int i = 0; i < interests.Length; i++)
                {
                    // remove the observer linking the mediator 
                    // to the notification interest
                    RemoveObserver(interests[i], mediator);
                }
                // remove the mediator from the map
                mediator.OnRemove();
            }
            return mediator;
        }

        public virtual bool HasMediator(string mediatorName)
        {
            return m_MediatorMap.ContainsKey(mediatorName);
        }

        protected virtual void InitializeView()
        {

        }



    }
}
