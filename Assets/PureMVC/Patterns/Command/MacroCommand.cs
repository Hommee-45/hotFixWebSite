using System;
using System.Collections.Generic;
using PureMVC.Interfaces;
using PureMVC.Patterns.Observer;

namespace PureMVC.Patterns.Command
{
    /// <summary>
    /// 多命令类 持有多个SimpleCommand
    /// </summary>
    public class MacroCommand : Notifier, ICommand, INotifier
    {

        public IList<Func<ICommand>> m_SubCommands;

        public MacroCommand()
        {
            m_SubCommands = new List<Func<ICommand>>();
            InitializeMacroCommand();
        }

        protected virtual void InitializeMacroCommand()
        {

        }
        /// <summary>
        /// 添加命令
        /// </summary>
        /// <param name="commandFunc"></param>
        protected void AddSubCommand(Func<ICommand> commandFunc)
        {
            m_SubCommands.Add(commandFunc);
        }

        public virtual void Execute(INotification notification)
        {
            while (m_SubCommands.Count > 0)
            {
                Func<ICommand> commandFunc = m_SubCommands[0];
                ICommand commandInstance = commandFunc();
                commandInstance.Execute(notification);
                m_SubCommands.RemoveAt(0);
            }
        }
    }
}