using System;
using System.Collections.Generic;
using System.Text;

namespace OSGiClientAgent.Command
{
    public interface ICommand
    {
        /// <summary>
        /// 得到命令名称
        /// </summary>
        /// <returns></returns>
        String GetCommandName();
        /// <summary>
        /// 得到帮助文本
        /// </summary>
        /// <returns></returns>
        String GetHelpText();
        /// <summary>
        /// 得到详细帮助文本
        /// </summary>
        /// <returns></returns>
        String GetDetailHelpText();
        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="commandLine"></param>
        /// <returns></returns>
        String ExecuteCommand(String commandLine);
    }
}
