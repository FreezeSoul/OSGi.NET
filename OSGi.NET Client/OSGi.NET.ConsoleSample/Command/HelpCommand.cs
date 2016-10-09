using System;
using System.Collections.Generic;
using System.Text;

namespace OSGi.NET.ConsoleSample.Command
{
    class HelpCommand : ICommand
    {
        private IDictionary<String, ICommand> commandDict;
        public HelpCommand(IDictionary<String, ICommand> commandDict)
        {
            this.commandDict = commandDict;
        }

        public string GetCommandName()
        {
            return "help";
        }

        public string GetHelpText()
        {
            return "显示帮助，help [命令]显示命令详细帮助";
        }

        public string GetDetailHelpText()
        {
            return "显示帮助\r\n\r\n"
                + "help         显示所有可用的命令\r\n"
                + "help [命令]  显示[命令]的详细帮助";
        }

        public string ExecuteCommand(string commandLine)
        {
            String arg_CommandName = commandLine.Substring(GetCommandName().Length).Trim();
            if (String.IsNullOrEmpty(arg_CommandName))
            {
                StringBuilder sb = new StringBuilder();
                foreach (String commandName in commandDict.Keys)
                {
                    ICommand command = commandDict[commandName];
                    sb.Append(commandName.PadRight(12));
                    sb.Append(command.GetHelpText());
                    sb.AppendLine();
                }
                return sb.ToString();
            }
            else
            {
                if (commandDict.ContainsKey(arg_CommandName))
                {
                    return commandDict[arg_CommandName].GetDetailHelpText();
                }
                else
                {
                    return String.Format("未找到命令:{0}", arg_CommandName);
                }
            }
        }
    }
}
