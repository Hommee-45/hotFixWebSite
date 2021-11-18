
using System;

namespace PureMVC.Interfaces
{
    /// <summary>
    /// 消息观察者
    /// </summary>
    public interface IObserver
    {
        /// <summary>
        /// 接到消息后需要出发的函数
        /// </summary>
        Action<INotification> NotifyMethod { set; }
        /// <summary>
        /// 需要触发函数所在的类
        /// </summary>
        object NotifyContext { set; }
        /// <summary>
        /// 执行消息
        /// </summary>
        /// <param name="notification"></param>
        void NotifyObserver(INotification notification);
        /// <summary>
        /// 比较对应触发函数所在的类是否一致
        /// </summary>
        /// <param name="obj"></param>
        bool CompareNotifyContext(object obj);


    }
}
