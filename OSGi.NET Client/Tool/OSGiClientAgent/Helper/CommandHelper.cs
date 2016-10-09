using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OSGi.NET.Core;
using OSGi.NET.Core.Root;

using OSGiClientAgent.Command;

namespace OSGiClientAgent.Helper
{
    class CommandHelper
    {
        static IList<ICommand> commandList = new List<ICommand>();
        static IDictionary<String, ICommand> commandDict = new Dictionary<String, ICommand>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public static void Init(IBundleContext context)
        {
            commandList.Add(new HelpCommand(commandDict));
            commandList.Add(new ListBundleCommand(context));
            commandList.Add(new ListAssembliesCommand());
            commandList.Add(new StartBundleCommand(context));
            commandList.Add(new StopBundleCommand(context));
            commandList.Add(new SysInfoCommand());
            commandList.Add(new RestartCommand(context));
            

            foreach (ICommand command in commandList)
            {
                commandDict.Add(command.GetCommandName(), command);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandLine"></param>
        /// <returns></returns>
        public static string Exc(string commandLine)
        {
            if (String.IsNullOrEmpty(commandLine)) return string.Empty;

            String commandName = commandLine.Split(new Char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[0];
            if (commandDict.ContainsKey(commandName))
            {
                try
                {
                    return commandDict[commandName].ExecuteCommand(commandLine);
                }
                catch (Exception ex)
                {
                    return String.Format("命令在执行过程中出现异常，信息：{0}", ex.Message);
                }
            }
            else
            {
                return String.Format("未知命令：{0}", commandName);
            }
        }
    }
}
