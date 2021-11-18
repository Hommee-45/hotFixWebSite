using PureMVC.Interfaces;

namespace PureMVC.Patterns.Observer
{
    /// <summary>
    /// 消息体具体实现
    /// </summary>
    public class Notification : INotification
    {
        /// <summary>
        /// 消息所带的数据
        /// </summary>
        public string Name {get;}
        /// <summary>
        /// 消息所带的数据
        /// </summary>
        public object Body { get; set; }
        /// <summary>
        /// 消息的类型
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 初始化通知消息
        /// </summary>
        /// <param name="name">消息名称</param>
        /// <param name="body">消息所带的数据</param>
        /// <param name="type">消息的类型</param>
        public Notification(string name, object body = null, string type = null)
        {
            Name = name;
            Body = body;
            Type = type;
        }
        public override string ToString()
        {
            string msg = "Notification Name" + Name;
            msg += "\nBody:" + ((Body == null) ? "null" : Body.ToString());
            msg += "\nType:" + ((Type == null) ? "null" : Type);
            return msg;
        }
    }

}
