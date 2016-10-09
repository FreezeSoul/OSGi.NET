using System;
using System.Collections.Generic;
using System.Text;
using OSGi.NET.Core;
using OSGi.NET.Core.Root;

namespace OSGiClientAgent.Command
{
    class StopBundleCommand : ICommand
    {
        private IBundleContext context;
        public StopBundleCommand(IBundleContext context)
        {
            this.context = context;
        }

        public string GetCommandName()
        {
            return "stop";
        }

        public string GetHelpText()
        {
            return "停止插件";
        }

        public string GetDetailHelpText()
        {
            return "停止插件\r\n\r\n"
            + "stop [插件Index]  停止插件Index为[插件Index]的插件";
        }

        public string ExecuteCommand(string commandLine)
        {
            String bundleIdStr = commandLine.Substring(GetCommandName().Length).Trim();
            var bundleId = int.Parse(bundleIdStr);
            IBundle bundle = context.GetBundle(bundleId);
            if (bundle == null) return String.Format("未找到ID为[{0}]的Bundle", bundleId);
            bundle.Stop();
            return String.Format("插件[{0} ({1})]已停止.当前状态为:{2}", bundle.GetSymbolicName(), bundle.GetVersion(), BundleUtils.GetBundleStateString(bundle.GetState()));
        }
    }
}
