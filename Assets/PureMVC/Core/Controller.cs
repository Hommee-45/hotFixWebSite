using System;
using System.Collections.Concurrent;
using PureMVC.Interfaces;
using PureMVC.Patterns.Observer;


namespace PureMVC.Core
{
    public class Controller  : IController
    {
        protected IView m_View;

        protected readonly ConcurrentDictionary<string, Func<ICommand>> m_CommandMap;

        protected static IController m_Instance;

        protected const string SINGLETON_MSG = "Controller Singleton already constructed!";
        public Controller()
        {
            if (m_Instance != null) throw new Exception(SINGLETON_MSG);
            m_Instance = this;
            m_CommandMap = new ConcurrentDictionary<string, Func<ICommand>>();
            InitializeController();
        }
        public static IController GetInstance(Func<IController> controllerFunc)
        {
            if (m_Instance == null)
            {
                m_Instance = controllerFunc();
            }
            return m_Instance;
        }
        public virtual void ExecuteCommand(INotification notification)
        {
            if (m_CommandMap.TryGetValue(notification.Name, out Func<ICommand> commandFunc))
            {
                ICommand commandInstance = commandFunc();
                commandInstance.Execute(notification);
            }
        }
        public virtual void RegisterCommand(string notificationName, Func<ICommand> commandFunc)
        {
            if (m_CommandMap.TryGetValue(notificationName, out Func<ICommand> _) == false)
            {
                m_View.RegisterObserver(notificationName, new Observer(ExecuteCommand, this));
            }
            m_CommandMap[notificationName] = commandFunc;
        }

        public virtual void RemoveCommand(string notificationName)
        {
            if (m_CommandMap.TryRemove(notificationName, out Func<ICommand> _))
            {
                m_View.RemoveObserver(notificationName, this);
            }
        }

        public virtual bool HasCommand(string notificationName)
        {
            return m_CommandMap.ContainsKey(notificationName);
        }
        protected virtual void InitializeController()
        {
            m_View = View.GetInstance(() => new View());
        }

    }

}
