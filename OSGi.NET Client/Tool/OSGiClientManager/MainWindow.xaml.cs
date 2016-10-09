using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
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

using Hik.Communication.Scs.Communication.EndPoints.Tcp;
using Hik.Communication.Scs.Communication.Messages;
using Hik.Communication.Scs.Server;

namespace OSGiClientManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        #region Property

        /// <summary>
        /// 
        /// </summary>
        private IScsServer scsServer;

        /// <summary>
        /// 
        /// </summary>
        private int serverPort;

        /// <summary>
        /// 
        /// </summary>
        private ObservableCollection<MonitorClient> scsServerClients;


        #endregion

        public MainWindow()
        {
            InitializeComponent();

            InitializeData();
        }


        #region Event
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            if (scsServer == null)
            {
                StartServer();
            }
            else
            {
                StopServer();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExitItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            if (scsServer != null)
            {
                StopServer();
            }
            Application.Current.Shutdown();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindowOnClosed(object sender, EventArgs e)
        {
            if (scsServer != null)
            {
                StopServer();
            }
            Application.Current.Shutdown();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ServerClientConnected(object sender, ServerClientEventArgs e)
        {
            UpdateClientList(e.Client, false);
            e.Client.MessageReceived += ClientMessageReceived;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ServerClientDisconnected(object sender, ServerClientEventArgs e)
        {
            UpdateClientList(e.Client, true);
            e.Client.MessageReceived -= ClientMessageReceived;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ClientMessageReceived(object sender, MessageEventArgs e)
        {
            var message = e.Message as ScsTextMessage;
            if (message == null)
            {
                return;
            }
            UpdateClientList((IScsServerClient)sender, false, message.Text);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="isDelete"></param>
        /// <param name="message"></param>
        void UpdateClientList(IScsServerClient client, bool isDelete, string message = null)
        {
            var endPoint = client.RemoteEndPoint as ScsTcpEndPoint;
            if (endPoint != null)
            {
                if (isDelete)
                {
                    var clientItem = scsServerClients.FirstOrDefault(item => item.ClientIP == endPoint.IpAddress);
                    Dispatcher.Invoke(new Action(() => this.scsServerClients.Remove(clientItem)));
                }
                else
                {
                    if (scsServerClients.All(item => item.ClientIP != endPoint.IpAddress))
                    {
                        var clientItem = new MonitorClient()
                                         {
                                             ClientIP = endPoint.IpAddress,
                                             ClientID = client.ClientId.ToString(),
                                             StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                                             RunningState = "Connected"
                                         };

                        Dispatcher.Invoke(new Action(() => this.scsServerClients.Add(clientItem)));
                    }
                    else
                    {
                        var clientItem = scsServerClients.First(item => item.ClientIP == endPoint.IpAddress);
                        clientItem.RunningState = message;
                    }
                }

            }
        }


        #endregion

        #region Method


        /// <summary>
        /// 
        /// </summary>
        private void InitializeData()
        {
            var serverPortStr = ConfigurationManager.AppSettings["ServerPort"];

            if (!int.TryParse(serverPortStr, out serverPort)) serverPort = 9999;

            scsServerClients = new ObservableCollection<MonitorClient>();

            this.GridControlClientList.ItemsSource = scsServerClients;

            for (int i = 0; i < 20; i++)
            {
                scsServerClients.Add(new MonitorClient(){
                    ClientID = "User" + i,
                    ClientIP = "192.168.1.1" + i,
                    RunningState = "Running",
                    StartTime = DateTime.Now.AddMinutes(new Random().Next(1,60)).ToShortTimeString()
                }); 
            }
            
          
        }

        /// <summary>
        /// 
        /// </summary>
        private void StartServer()
        {
            scsServer = ScsServerFactory.CreateServer(new ScsTcpEndPoint(serverPort));

            scsServer.ClientConnected += ServerClientConnected;
            scsServer.ClientDisconnected += ServerClientDisconnected;

            scsServer.Start();

            Start.LargeGlyph = new BitmapImage(new Uri("Images/Stop.png", UriKind.Relative));
        }

        /// <summary>
        /// 
        /// </summary>
        private void StopServer()
        {
            scsServer.ClientConnected -= ServerClientConnected;
            scsServer.ClientDisconnected -= ServerClientDisconnected;

            scsServer.Stop();
            scsServer = null;

            Start.LargeGlyph = new BitmapImage(new Uri("Images/Start.png", UriKind.Relative));
        }

        #endregion

    }
}
