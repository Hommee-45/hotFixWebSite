using System;
using PureMVC.Interfaces;
using PureMVC.Core;
using PureMVC.Patterns.Observer;

namespace PureMVC.Patterns.Facade
{
    public class Facade : IFacade
    {
        protected const string SINGLETON_MSG = "Facade Singleton already construsted!";

        protected static IFacade m_Instance;

        protected IView m_View;

        protected IModel m_Model;

        protected IController m_Controller;
        public Facade()
        {
            if (m_Instance != null) throw new Exception(SINGLETON_MSG);
            m_Instance = this;
            InitializeFacade();
        }
        public static IFacade GetInstance(Func<IFacade> facadeFunc)
        {
            if (m_Instance == null)
            {
                m_Instance = facadeFunc();
            }
            return m_Instance;
        }
        /// <summary>
        /// 初始化Facade类
        /// </summary>
        protected virtual void InitializeFacade()
        {
            InitializeModel();
            InitializeController();
            InitializeView();
        }
        /// <summary>
        /// 初始化Controller类
        /// </summary>
        protected virtual void InitializeController()
        {
            m_Controller = Controller.GetInstance(() => new Controller());
        }
        /// <summary>
        /// 初始化Model类
        /// </summary>
        protected virtual void InitializeModel()
        {
            m_Model = Model.GetInstance(() => new Model());
        }
        /// <summary>
        /// 初始化View类
        /// </summary>
        protected virtual void InitializeView()
        {
            m_View = View.GetInstance(() => new View());
        }

        /// <summary>
        /// 注册命令
        /// </summary>
        /// <param name="notificationName">消息名称</param>
        /// <param name="commandFunc">此委托返回需要执行的命令类</param>
        public virtual void RegisterCommand(string notificationName, Func<ICommand> commandFunc)
        {
            m_Controller.RegisterCommand(notificationName, commandFunc);
        }
        /// <summary>
        /// 移除命令类 
        /// </summary>
        /// <param name="notificationName"></param>
        public virtual void RemoveCommand(string notificationName)
        {
            m_Controller.RemoveCommand(notificationName);
        }
        /// <summary>
        /// 是否含有命令
        /// </summary>
        /// <param name="noticicationName">执行此命令消息的名称</param>
        /// <returns></returns>
        public virtual bool HasCommand(string noticicationName)
        {
            return m_Controller.HasCommand(noticicationName);
        }
        /// <summary>
        /// 注册数据代理类
        /// </summary>
        /// <param name="proxy">对应代理类</param>
        public virtual void RegisterProxy(IProxy proxy)
        {
            m_Model.RegisterProxy(proxy);
        }

        /// <summary>
        /// 检索数据代理类
        /// </summary>
        /// <param name="proxyName">数据代理类名称</param>
        /// <returns></returns>
        public virtual IProxy RetrieveProxy(string proxyName)
        {
            return m_Model.RetrieveProxy(proxyName);
        }

        /// <summary>
        /// 注销数据代理类
        /// </summary>
        /// <param name="proxyName">数据代理类名称</param>
        /// <returns></returns>
        public virtual IProxy RemoveProxy(string proxyName)
        {
            return m_Model.RemoveProxy(proxyName);
        }

        public virtual bool HasProxy(string proxyName)
        {
            return m_Model.HasProxy(proxyName);
        }

        /// <summary>
        /// 注册视图中介类
        /// </summary>
        /// <param name="mediator">中介者名称</param>
        public virtual void RegisterMediator(IMediator mediator)
        {
            m_View.RegisterMediator(mediator);
        }

        /// <summary>
        /// 检索视图中介类
        /// </summary>
        /// <param name="mediatorName">中介者名称</param>
        public virtual IMediator RetrieveMediator(string mediatorName)
        {
            return m_View.RetrieveMediator(mediatorName);
        }

        /// <summary>
        /// 注销视图中介类
        /// </summary>
        /// <param name="mediatorName"></param>
        /// <returns></returns>
        public virtual IMediator RemoveMediator(string mediatorName)
        {
            return m_View.RemoveMediator(mediatorName);
        }

        /// <summary>
        /// 判断是否含有视图中介类
        /// </summary>
        /// <param name="mediatorName"></param>
        /// <returns></returns>
        public virtual bool HasMediator(string mediatorName)
        {
            return m_View.HasMediator(mediatorName);
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="notificationName">消息名称</param>
        /// <param name="body">消息所带的数据</param>
        /// <param name="type">消息的类别</param>
        public virtual void SendNotification(string notificationName, object body = null, string type = null)
        {
            NotifyObservers(new Notification(notificationName, body, type));
        }

        /// <summary>
        /// 执行对应消息
        /// </summary>
        /// <param name="notification">消息体</param>
        public virtual void NotifyObservers(INotification notification)
        {
            m_View.NotifyObservers(notification);
        }
    }

}
