using System;
namespace PureMVC.Interfaces
{
    /// <summary>
    /// Facade接口 三大核心类 持有者需要继承的接口
    /// </summary>
    public interface IFacade : INotifier
    {
        /// <summary>
        /// 注册 代理类
        /// </summary>
        /// <param name="proxy"></param>
        void RegisterProxy(IProxy proxy);
        /// <summary>
        /// 检索查找对应的代理类
        /// </summary>
        /// <param name="proxyName"></param>
        /// <returns></returns>
        IProxy RetrieveProxy(string proxyName);
        /// <summary>
        /// 移除代理类
        /// </summary>
        /// <param name="proxyName">代理类名称</param>
        /// <returns></returns>
        IProxy RemoveProxy(string proxyName);
        /// <summary>
        /// 判断 是否有指定的代理类
        /// </summary>
        /// <param name="proxyName">代理类名称</param>
        /// <returns></returns>
        bool HasProxy(string proxyName);
        /// <summary>
        /// 注册命令类
        /// </summary>
        /// <param name="notificationName">消息的名称</param>
        /// <param name="commandFunc">此委托返回需要执行的命令类</param>
        void RegisterCommand(string notificationName, Func<ICommand> commandFunc);
        /// <summary>
        /// 移除命令
        /// </summary>
        /// <param name="notificationName">执行此命令的消息名称</param>
        void RemoveCommand(string notificationName);
        /// <summary>
        /// 是否有命令
        /// </summary>
        /// <param name="notificationName"></param>
        /// <returns></returns>
        bool HasCommand(string notificationName);
        /// <summary>
        /// 注册对应视图中介
        /// </summary>
        /// <param name="mediator"></param>
        void RegisterMediator(IMediator mediator);
        /// <summary>
        /// 检索对应中介类
        /// </summary>
        /// <param name="mediatorName">中介者名称</param>
        /// <returns></returns>
        IMediator RetrieveMediator(string mediatorName);
        /// <summary>
        /// 移除对应视图中介
        /// </summary>
        /// <param name="mediatorName">中介者名称</param>
        /// <returns></returns>
        IMediator RemoveMediator(string mediatorName);
        /// <summary>
        /// 判断是否 含有该中介者
        /// </summary>
        /// <param name="mediatorName">中介者名称</param>
        /// <returns></returns>
        bool HasMediator(string mediatorName);
        /// <summary>
        /// 执行某消息
        /// </summary>
        /// <param name="notification"></param>
        void NotifyObservers(INotification notification);
    }
}
