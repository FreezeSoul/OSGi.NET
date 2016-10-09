using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

using OSGi.NET.Core;
using OSGi.NET.Core.Root;

namespace OSGi.NET.ConsoleSample.Command
{
    public class ListBundleCommand : ICommand
    {
        private IFramework framework;

        public ListBundleCommand(IFramework framework)
        {
            this.framework = framework;
        }

        public string GetCommandName()
        {
            return "lb";
        }

        public string GetDetailHelpText()
        {
            return "列出所有已安装插件，状态和名称信息\r\n\r\nlb";
        }

        public string ExecuteCommand(string commandLine)
        {
            var bundles = framework.GetBundleContext().GetBundles();
            StringBuilder sb = new StringBuilder();
            sb.Append("Index".PadLeft(5));
            sb.Append("|");
            sb.Append("State".PadRight(12));
            sb.Append("|");
            sb.Append("Name");
            sb.AppendLine();
            var index = 0;
            foreach (IBundle bundle in bundles)
            {
                sb.Append(index.ToString().PadLeft(5));
                sb.Append("|");
                sb.Append(BundleUtils.GetBundleStateString(bundle.GetState()).PadRight(12));
                sb.Append("|");
                sb.Append(String.Format("{0} ({1}) \"{2}\"", bundle.GetSymbolicName(), bundle.GetVersion(),bundle.GetManifest()["Name"]));
                sb.AppendLine();
                index++;
            }
            return sb.ToString();
        }


        public string GetHelpText()
        {
            return "列出所有插件及状态";
        }
    }
}
