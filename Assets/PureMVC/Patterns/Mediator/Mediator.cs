using PureMVC.Interfaces;
using PureMVC.Patterns.Observer;

namespace PureMVC.Patterns.Mediator
{
    /// <summary>
    /// 视图对应的中阶层
    /// </summary>
    public class Mediator : Notifier, IMediator, INotifier
    {
        public static string NAME = "Mediator";
        /// <summary>
        /// 中介层名称
        /// </summary>
        public string MediatorName { get; protected set; }
        /// <summary>
        /// 对应的视图UI吗，一般是MonoBehaviour
        /// </summary>
        public object ViewComponet { get; set; }
        public Mediator(string mediatorName, object viewComponent = null)
        {
            MediatorName = mediatorName ?? Mediator.NAME;
            ViewComponet = viewComponent;
        }
        /// <summary>
        /// 此视图层需要关注的消息列表
        /// </summary>
        /// <returns></returns>
        public virtual string[] ListNotificationInterests()
        {
            return new string[0];
        }
        public virtual void HandleNotification(INotification notification)
        {

        }
        /// <summary>
        /// 当此视图中介注册后触发
        /// </summary>
        public virtual void OnRegister() { }
        /// <summary>
        /// 当此视图中介注销后立即触发
        /// </summary>
        public virtual void OnRemove() { }
    }
}

