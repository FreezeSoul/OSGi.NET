using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using OSGi.NET.Core;
using OSGi.NET.Core.Root;

namespace OSGiClientAgent.Command
{
    class RestartCommand : ICommand
    {
        private IBundleContext context;

        public RestartCommand(IBundleContext context)
        {
            this.context = context;
        }

        public string GetCommandName()
        {
            return "restart";
        }

        public string GetHelpText()
        {
            return "重启当前客户机程序";
        }

        public string GetDetailHelpText()
        {
            return "restart 重启当前客户机程序";
        }

        public string ExecuteCommand(string commandLine)
        {
            try
            {
                var framework = context.GetBundles()[0] as IFramework;
                if (framework != null)
                    framework.Stop();
                Application.ExitThread();
                Application.Exit();
                Application.Restart();
                Process.GetCurrentProcess().Kill();
            }
            catch { }
            return "正在重启...";
        }
    }
}
