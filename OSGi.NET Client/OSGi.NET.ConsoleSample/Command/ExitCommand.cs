using System;
using System.Collections.Generic;
using System.Text;

using OSGi.NET.Core.Root;

namespace OSGi.NET.ConsoleSample.Command
{
    class ExitCommand : ICommand
    {
        private IFramework framework;
        public ExitCommand(IFramework framework)
        {
            this.framework = framework;
        }

        public string GetCommandName()
        {
            return "exit";
        }

        public string GetHelpText()
        {
            return "退出程序";
        }

        public string GetDetailHelpText()
        {
            return "退出程序\r\n\r\nexit";
        }

        public string ExecuteCommand(string commandLine)
        {
            framework.Stop();
            Environment.Exit(0);
            return "";
        }
    }
}
