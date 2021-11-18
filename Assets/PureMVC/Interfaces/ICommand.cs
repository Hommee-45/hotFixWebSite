
namespace PureMVC.Interfaces
{
    /// <summary>
    /// 命令接口 含有此接口具有执行命令功能
    /// </summary>
    public interface ICommand : INotifier
    {
        void Execute(INotification notification);
    }

}
