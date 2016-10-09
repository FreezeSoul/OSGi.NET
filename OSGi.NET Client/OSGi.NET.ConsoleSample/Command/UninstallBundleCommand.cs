using System;
using System.Collections.Generic;
using System.Text;

using OSGi.NET.Core;
using OSGi.NET.Core.Root;

namespace OSGi.NET.ConsoleSample.Command
{
    class UninstallBundleCommand : ICommand
    {
        private IFramework framework;
        public UninstallBundleCommand(IFramework framework)
        {
            this.framework = framework;
        }

        public string GetCommandName()
        {
            return "uninstall";
        }

        public string GetHelpText()
        {
            return "卸载插件";
        }

        public string GetDetailHelpText()
        {
            return "卸载插件\r\n\r\n"
                + "uninstall [插件Index]  卸载插件Index为[插件Index]的插件";
        }

        public string ExecuteCommand(string commandLine)
        {
            String bundleIdStr = commandLine.Substring(GetCommandName().Length).Trim();
            var bundleId = int.Parse(bundleIdStr);
            IBundle bundle = framework.GetBundleContext().GetBundle(bundleId);
            if (bundle == null) return String.Format("未找到Index为[{0}]的Bundle", bundleId);
            bundle.Stop();
            bundle.UnInstall();
            return String.Format("插件[{0} ({1})]已卸载.", bundle.GetSymbolicName(), bundle.GetVersion());
        }
    }
}
