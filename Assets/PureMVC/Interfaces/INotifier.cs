using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PureMVC.Interfaces
{
    /// <summary>
    /// 发送消息接口 继承此接口就具有发送消息功能
    /// </summary>
    public interface INotifier
    {
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="notificationName">消息名称</param>
        /// <param name="body">此消息所带的数据</param>
        /// <param name="type">此消息类型</param>
        void SendNotification(string notificationName, object body = null, string type = null);
    }
}
