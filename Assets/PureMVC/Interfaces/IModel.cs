
namespace PureMVC.Interfaces
{
    /// <summary>
    /// Model接口 三大核心类Model需要继承的接口
    /// </summary>
    public interface IModel
    {
        /// <summary>
        /// 注册数据代理类
        /// </summary>
        /// <param name="proxy"></param>
        void RegisterProxy(IProxy proxy);
        /// <summary>
        /// 检索数据代理类
        /// </summary>
        /// <param name="proxyName">代理类名称</param>
        /// <returns></returns>
        IProxy RetrieveProxy(string proxyName);
        /// <summary>
        /// 移除代理类
        /// </summary>
        /// <param name="proxyName">代理类名称</param>
        /// <returns></returns>
        IProxy RemoveProxy(string proxyName);
        /// <summary>
        /// 判断是否含有对应数据代理类
        /// </summary>
        /// <param name="proxyName">代理类名称</param>
        /// <returns></returns>
        bool HasProxy(string proxyName);
    }
}
