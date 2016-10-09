using System;
using System.Collections.Generic;
using System.Text;
using OSGi.NET.Core;
using OSGi.NET.Core.Root;


namespace OSGi.NET.ConsoleSample.Command
{
    class StartBundleCommand : ICommand
    {
        private IFramework framework;
        public StartBundleCommand(IFramework framework)
        {
            this.framework = framework;
        }

        public string GetCommandName()
        {
            return "start";
        }

        public string GetHelpText()
        {
            return "启动插件";
        }

        public string GetDetailHelpText()
        {
            return "启动插件\r\n\r\n"
                + "start [插件Index]  启动插件Index为[插件Index]的插件";
        }

        public string ExecuteCommand(string commandLine)
        {
            String bundleIdStr = commandLine.Substring(GetCommandName().Length).Trim();
            var bundleId = int.Parse(bundleIdStr);
            IBundle bundle = framework.GetBundleContext().GetBundle(bundleId);
            if (bundle == null) return String.Format("未找到Index为[{0}]的插件", bundleId);
            bundle.Start();
            return String.Format("启动插件[{0} ({1})]完成，当前状态为:{2}", bundle.GetSymbolicName(), bundle.GetVersion(), BundleUtils.GetBundleStateString(bundle.GetState()));
        }
    }
}
