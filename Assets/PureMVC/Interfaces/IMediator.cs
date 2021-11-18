

namespace PureMVC.Interfaces
{
    /// <summary>
    /// View视图对应的中介者接口
    /// </summary>
    public interface IMediator : INotifier
    {
        /// <summary>
        /// 对应视图中介者名称
        /// </summary>
        string MediatorName { get; }
        /// <summary>
        /// 对应的视图UI， 一般指MonoBehaviour
        /// </summary>
        object ViewComponet { get; set; }
        /// <summary>
        /// 此视图层中需要关注的消息列表
        /// </summary>
        /// <returns></returns>
        string[] ListNotificationInterests();
        /// <summary>
        /// 对此视图中关注消息对应的处理
        /// </summary>
        /// <param name="notification"></param>
        void HandleNotification(INotification notification);
        /// <summary>
        /// 当此视图层中介注册时触发
        /// </summary>
        void OnRegister();
        /// <summary>
        /// 当此视图层中介移除时触发
        /// </summary>
        void OnRemove();
    }
}
