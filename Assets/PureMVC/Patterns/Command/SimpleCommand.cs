using PureMVC.Interfaces;
using PureMVC.Patterns.Observer;

namespace PureMVC.Patterns.Command
{
    /// <summary>
    /// 单命令类
    /// </summary>
    public class SimpleCommand : Notifier, ICommand, INotifier
    {
        /// <summary>
        /// 执行对应命令的具体操作
        /// </summary>
        /// <param name="notification"></param>
        public virtual void Execute(INotification notification)
        {
        }
    }
}
