using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

using OSGi.NET.Core.Root;

namespace WpfDemo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IFramework Framework;

        public static SplashWindow Splash;

        public static MainWindow MainWin;

        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            var frameworkFactory = new FrameworkFactory();
            Framework = frameworkFactory.CreateFramework();
        }
    }
}
