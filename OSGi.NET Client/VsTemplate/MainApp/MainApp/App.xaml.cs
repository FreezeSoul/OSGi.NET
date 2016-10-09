using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

using OSGi.NET.Core.Root;

namespace MainApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            //创建框架工厂
            var frameworkFactory = new FrameworkFactory();
            //创建框架内核
            var framework = frameworkFactory.CreateFramework();
            //初始化框架
            framework.Init();
            //启动框架
            framework.Start();
        }
    }
}
