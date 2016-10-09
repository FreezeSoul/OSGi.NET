using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using OSGi.NET.Core.Root;

using WpfServiceContract;

namespace WpfDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public MainWindow()
        {
            InitializeComponent();

            App.MainWin = this;
            App.Splash = new SplashWindow();
            CustomAppender.Context = App.Splash;
            App.Splash.ShowDialog();
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var context = App.Framework.GetBundleContext();
                var serviceRef = context.GetServiceReference<ILoadMenuView>();
                var service = context.GetService<ILoadMenuView>(serviceRef);
                var leftUc = service.LoadMenuControl();

                this.LeftMenu.Content = leftUc;
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }

        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            App.Framework.Stop();
        }
    }
}
