using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OSGi.NET.ConsoleSample.Command;
using OSGi.NET.Core.Root;

namespace OSGi.NET.ConsoleSample
{

    /// <summary>
    /// 控制台演示程序
    /// </summary>
    class Program
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {

            IDictionary<String, ICommand> commandDict = new Dictionary<String, ICommand>();
            IList<ICommand> commandList = new List<ICommand>();

            var frameworkFactory = new FrameworkFactory();
            var framework = frameworkFactory.CreateFramework();


            AppDomain.CurrentDomain.AssemblyLoad += CurrentDomainAssemblyLoad;
            framework.GetBundleContext().AddBundleListener(new ConsoleBundleListener());

            framework.Init();
            framework.Start();

            commandList.Add(new HelpCommand(commandDict));
            commandList.Add(new ListBundleCommand(framework));
            commandList.Add(new ListAssembliesCommand());
            commandList.Add(new StartBundleCommand(framework));
            commandList.Add(new StopBundleCommand(framework));
            commandList.Add(new InstallBundleCommand(framework));
            commandList.Add(new UninstallBundleCommand(framework));
            commandList.Add(new UpdateBundleCommand(framework));
            commandList.Add(new ExitCommand(framework));

            foreach (ICommand command in commandList)
            {
                commandDict.Add(command.GetCommandName(), command);
            }
            Console.WriteLine("------------------------------");
            Console.WriteLine("欢迎使用，输入help以查看帮助");
            Console.WriteLine("------------------------------");
            Console.WriteLine();

            while (true)
            {
                Console.Write("OSGi.NET>");
                var readLine = Console.ReadLine();
                if (readLine != null)
                {
                    String commandLine = readLine.Trim();
                    if (String.IsNullOrEmpty(commandLine)) continue;

                    String commandName = commandLine.Split(new Char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[0];
                    if (commandDict.ContainsKey(commandName))
                    {
                        String result = null;
                        try
                        {
                            result = commandDict[commandName].ExecuteCommand(commandLine);
                            Console.WriteLine(result);
                        }
                        catch (Exception ex)
                        {
                            log.Error(String.Format("命令在执行过程中出现异常，信息：{0}", ex.Message), ex);
                        }
                    }
                    else
                    {
                        log.Error(String.Format("未知       命令：{0}", commandName));
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        static void CurrentDomainAssemblyLoad(object sender, AssemblyLoadEventArgs args)
        {
            log.Debug(String.Format("程序集[{0}]已加载到当前应用程序域", args.LoadedAssembly));
        }
    }
}
