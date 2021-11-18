using System;
using PureMVC.Interfaces;

namespace PureMVC.Patterns.Observer
{


    /// <summary>
    ///  通知观察者 内嵌执行对应消息的回调函数 HandleNotification 或 ExecuteCommand
    /// </summary>
    public class Observer : IObserver
    {
        public Action<INotification> NotifyMethod { get; set; }

        public object NotifyContext { get; set; }

        /// <summary>
        /// 构造通知观察者
        /// </summary>
        /// <param name="notifyMethod">此通知需要执行的函数</param>
        /// <param name="notifyContext">需要执行通知函数所在的类</param>
        public Observer(Action<INotification> notifyMethod, object notifyContext)
        {
            NotifyMethod = notifyMethod;
            NotifyContext = notifyContext;
        }
        /// <summary>
        /// 执行对应的通知
        /// </summary>
        /// <param name="notification">消息体</param>
        public virtual void NotifyObserver(INotification notification)
        {
            NotifyMethod(notification);
        }

        public virtual bool CompareNotifyContext(object obj)
        {
            return NotifyContext.Equals(obj);
        }
    }
}
