using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using OSGiClientAgent.Helper;

namespace OSGiClientAgent.Command
{
    class SysInfoCommand : ICommand
    {
        public string GetCommandName()
        {
            return "sysinfo";
        }

        public string GetHelpText()
        {
            return "查看当前客户机系统信息";
        }

        public string GetDetailHelpText()
        {
            return "sysinfo 查看当前客户机系统信息";
        }

        public string ExecuteCommand(string commandLine)
        {
            return this.GetInfo();
        }

        string GetInfo()
        {
            return ClientDeviceHelper.GetSystemInfo();
        }
    }
}
