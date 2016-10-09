using System;
using System.Collections.Generic;
using System.Text;
using OSGi.NET.Core;
using OSGi.NET.Core.Root;

namespace OSGi.NET.ConsoleSample.Command
{
    class InstallBundleCommand : ICommand
    {
        private IFramework framework;
        public InstallBundleCommand(IFramework framework)
        {
            this.framework = framework;
        }

        public string GetCommandName()
        {
            return "install";
        }

        public string GetHelpText()
        {
            return "安装插件";
        }

        public string GetDetailHelpText()
        {
            return "安装插件\r\n\r\n"
                    + "install [路径]  从[路径]安装插件";
        }

        public string ExecuteCommand(string commandLine)
        {
            String location = commandLine.Substring(GetCommandName().Length).Trim();
            IBundle bundle = null;
            try
            {
                bundle = framework.GetBundleContext().InstallBundle(location);
            }
            catch (Exception ex)
            {
                return String.Format("安装插件出错，原因：{0}", ex.Message);
            }
            return String.Format("插件[{0} ({1})]已安装.", bundle.GetSymbolicName(), bundle.GetVersion());
        }
    }
}
