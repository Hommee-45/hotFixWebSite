

namespace PureMVC.Interfaces
{
    /// <summary>
    /// 数据代理类接口
    /// </summary>
    public interface IProxy : INotifier
    {
        /// <summary>
        /// 代理类名称
        /// </summary>
        string ProxyName { get; }
        /// <summary>
        /// 代理类所持有的数据
        /// </summary>
        object Data { get; set; }
        /// <summary>
        /// 注册数据代理类后触发
        /// </summary>
        void OnRegister();
        /// <summary>
        /// 注销数据代理后触发
        /// </summary>
        void OnRemove();
    }
}
