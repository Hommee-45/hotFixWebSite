using PureMVC.Interfaces;
using PureMVC.Patterns.Observer;

namespace PureMVC.Patterns.Proxy
{
    /// <summary>
    /// 数据代理类
    /// </summary>
    public class Proxy : Notifier, IProxy, INotifier
    {
        /// <summary>
        /// 数据代理类名称
        /// </summary>
        public static string NAME = "Proxy";
        /// <summary>
        /// 数据代理类名称
        /// </summary>
        public string ProxyName { get; protected set; }
        /// <summary>
        /// 对应代理数据
        /// </summary>
        public object Data { get; set; }

        public Proxy(string proxyName, object data = null)
        {
            ProxyName = proxyName ?? Proxy.NAME;
            if (data != null) Data = data;
        }
        /// <summary>
        /// 注册数据代理类后立即触发
        /// </summary>
        public virtual void OnRegister() { }
        /// <summary>
        /// 注销数据代理类后立刻触发
        /// </summary>
        public virtual void OnRemove() { }
    }

}
