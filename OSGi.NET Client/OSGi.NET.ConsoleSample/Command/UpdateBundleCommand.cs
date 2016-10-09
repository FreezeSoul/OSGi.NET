using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using OSGi.NET.Core;
using OSGi.NET.Core.Root;

namespace OSGi.NET.ConsoleSample.Command
{
    public class UpdateBundleCommand : ICommand
    {
        private IFramework framework;
        public UpdateBundleCommand(IFramework framework)
        {
            this.framework = framework;
        }

        public string GetCommandName()
        {
            return "update";
        }

        public string GetHelpText()
        {
            return "更新插件";
        }

        public string GetDetailHelpText()
        {
            return "更新插件\r\n\r\n"
            + "update [插件编号] [更新文件路径] 从[更新文件路径]更新插件编号为[插件编号]的插件\r\n";
        }

        public string ExecuteCommand(string commandLine)
        {
            String args = commandLine.Substring(GetCommandName().Length).Trim();

            int bundleId = 0;
            string location = null;

            if (args.Contains(" "))
            {
                Int32 spaceIndex = args.IndexOf(" ");
                bundleId = int.Parse(args.Substring(0, spaceIndex));
                location = args.Substring(spaceIndex).Trim();
            }
            else
            {
                return "输入内容有误！";
            }

            IBundle bundle = null;
            try
            {
                bundle = framework.GetBundleContext().GetBundle(bundleId);
                if (bundle == null) 
                    return String.Format("未找到Index为[{0}]的插件", bundleId);
                if (!File.Exists(location))
                    return String.Format("无法找到路径[{0}]对应的文件", location);

                bundle.Update(location);
            }
            catch (Exception ex)
            {
                return String.Format("更新插件出错，原因：{0}", ex.Message);
            }
            return String.Format("插件[{0} ({1})]已更新.", bundle.GetSymbolicName(), bundle.GetVersion());
        }
    }
}
