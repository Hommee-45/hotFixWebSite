

namespace PureMVC.Interfaces
{
    /// <summary>
    /// 消息接口
    /// </summary>
    public interface INotification
    {
        /// <summary>
        /// 消息的名称
        /// </summary>
        string Name { get; }
        /// <summary>
        /// 消息所带的数据
        /// </summary>
        object Body { get; set; }
        /// <summary>
        /// 消息的种类 用于区分消息名称一样但是分类不同
        /// </summary>
        string Type { get; set; }
        /// <summary>
        /// 重写ToString()
        /// </summary>
        /// <returns></returns>
        string ToString();

    }
}
