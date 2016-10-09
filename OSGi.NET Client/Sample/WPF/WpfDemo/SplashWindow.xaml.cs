using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using System.Threading;
using System.Windows.Threading;

using WpfServiceContract;

namespace WpfDemo
{
    /// <summary>
    /// Interaction logic for splash.xaml
    /// </summary>
    public partial class SplashWindow : Window
    {
        private delegate void ShowDelegate(string txt);
        ShowDelegate showDelegate;

        public Queue<String> MessageStack;

        public static object lockObj = new object();

        private bool flag = false;

        public DispatcherTimer timer = new DispatcherTimer();
        public SplashWindow()
        {
            InitializeComponent();
            showDelegate = new ShowDelegate(this.ShowText);
            MessageStack = new Queue<string>();
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {

            timer.Tick += TimerOnTick;
            timer.Interval = new TimeSpan(0, 0, 0, 0, 50);
            timer.Start();

            App.Framework.Init();

            App.Framework.GetBundleContext().RegisterService<IAttachContent>(new AttachContent());

            App.Framework.Start();

            flag = true;
        }

        private void TimerOnTick(object sender, EventArgs eventArgs)
        {
            lock (lockObj)
            {
                if (MessageStack.Count > 0)
                {
                    var message = MessageStack.Dequeue();
                    if (message != null)
                    {
                        this.Dispatcher.BeginInvoke(showDelegate, message);
                    }
                }
                else
                {
                    if (flag)
                    {
                        timer.Stop();
                        this.Close();
                    }
                }
            }
        }

        private void ShowText(string txt)
        {
            txtLoading.Text = txt;
        }

    }
}
