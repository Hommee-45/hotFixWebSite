using PureMVC.Interfaces;
namespace PureMVC.Patterns.Observer
{
    /// <summary>
    /// 发送消息 具体实现
    /// </summary>
    public class Notifier : INotifier
    {
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="notificationName">通知的名称</param>
        /// <param name="body">此条消息所带数据</param>
        /// <param name="type">此条消息通知的类型</param>
        public virtual void SendNotification(string notificationName, object body = null, string type = null)
        {

        }

        protected IFacade Facade
        {
            get
            {
                return Patterns.Facade.Facade.GetInstance(() => new Facade.Facade());
            }
        }
    }
}
