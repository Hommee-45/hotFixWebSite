
using System;

namespace PureMVC.Interfaces
{
    /// <summary>
    /// Controller接口 三大核心类Controller需要继承的接口
    /// </summary>
    public interface IController
    {
        /// <summary>
        /// 注册命令
        /// </summary>
        /// <param name="notificationName">执行命令的消息名称</param>
        /// <param name="commandFunc">此委托返回需要执行的命令类</param>
        void RegisterCommand(string notificationName, Func<ICommand> commandFunc);
        /// <summary>
        /// 执行对应的命令
        /// </summary>
        /// <param name="notification">消息体</param>
        void ExecuteCommand(INotification notification);
        /// <summary>
        /// 移除对应的命令
        /// </summary>
        /// <param name="notificationName">执行命令的消息名称</param>
        void RemoveCommand(string notificationName);
        /// <summary>
        /// 是否有对应的命令
        /// </summary>
        /// <param name="notificationName">执行命令的消息名称</param>
        /// <returns></returns>
        bool HasCommand(string notificationName);
    }
}
