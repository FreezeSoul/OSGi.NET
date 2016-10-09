using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

using OSGi.NET.Provider;

namespace OSGi.NET.ConsoleSample.Command
{
    class ListAssembliesCommand : ICommand
    {
        public string GetCommandName()
        {
            return "la";
        }

        public string GetHelpText()
        {
            return "列出当前AppDomain加载的程序集";
        }

        public string GetDetailHelpText()
        {
            return "la 列出当前AppDomain加载的程序集";
        }

        public string ExecuteCommand(string commandLine)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("当前Domain已加载的程序集");
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            for (int i = 0; i < assemblies.Length; i++)
            {
                Assembly assembly = assemblies[i];
                AssemblyName assemblyName = assembly.GetName();
                sb.Append(String.Format("[{0}]: {1}, Version:{2}", i, assemblyName.Name, assemblyName.Version));
                sb.AppendLine();
            }

            sb.AppendLine();

            sb.AppendLine("当前Framework管理的程序集");
            var assemblyList = BundleAssemblyProvider.GetManagedAssemblies();
            var index = 0;
            foreach (var assembly in assemblyList)
            {
                AssemblyName assemblyName = assembly.GetName();
                sb.Append(String.Format("[{0}]: {1}, Version:{2}", index++, assemblyName.Name, assemblyName.Version));
                sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}
