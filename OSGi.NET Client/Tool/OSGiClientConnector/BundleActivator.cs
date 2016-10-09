using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using Hik.Communication.Scs.Client;
using Hik.Communication.Scs.Communication;
using Hik.Communication.Scs.Communication.EndPoints.Tcp;
using Hik.Communication.Scs.Communication.Messages;
using Hik.Communication.ScsServices.Client;
using Hik.Communication.ScsServices.Communication.Messages;

using OSGi.NET.Core;
using OSGi.NET.Event;
using OSGi.NET.Service;

namespace OSGiClientConnector
{
    public class BundleActivator : IBundleActivator
    {
        /// <summary>
        /// 
        /// </summary>
        private IScsClient _scsClient;

        /// <summary>
        /// 
        /// </summary>
        private IBundle _bundle;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void Start(IBundleContext context)
        {
            _bundle = context.GetBundle();
            Connet();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void Stop(IBundleContext context)
        {
            this.Disconnect();
        }

        /// <summary>
        /// 
        /// </summary>
        private void Connet()
        {
            try
            {
                var manifestData = _bundle.GetBundleManifestData();
                var serverIp = string.Empty;
                var serverPort = int.MinValue;
                foreach (XmlNode childNode in manifestData.ChildNodes)
                {
                    if (childNode.Name == "ServerIP")
                        if (childNode.Attributes != null)
                        {
                            serverIp = childNode.Attributes["Value"].Value;
                        }
                    if (childNode.Name == "ServerPort")
                        if (childNode.Attributes != null)
                        {
                            serverPort = int.Parse(childNode.Attributes["Value"].Value);
                        }
                }
                this.Connect(serverIp, serverPort);
            }
            catch (Exception)
            {
                this.Disconnect();
            }

        }

        /// <summary>
        /// Connects to the server.
        /// It automatically Logins to server if connection success.
        /// </summary>
        private void Connect(string serverIpAddress, int serverTcpPort)
        {
            //Disconnect if currently connected
            Disconnect();

            //Create a SCS client to connect to SCS server
            _scsClient = ScsClientFactory.CreateClient(new ScsTcpEndPoint(serverIpAddress, serverTcpPort));

            _scsClient.ConnectTimeout = 3;

            //Register events of SCS client
            _scsClient.Connected += this.ScsClientConnected;
            _scsClient.Disconnected += this.ScsClientDisconnected;
            _scsClient.MessageReceived += this.ScsClientMessageReceived;


            //Connect to the server
            _scsClient.Connect();
        }

        /// <summary>
        /// Disconnects from server if it is connected.
        /// </summary>
        private void Disconnect()
        {
            if (_scsClient != null && _scsClient.CommunicationState == CommunicationStates.Connected)
            {
                try
                {
                    _scsClient.Disconnect();
                }
                catch (Exception) { }

                _scsClient = null;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScsClientMessageReceived(object sender, MessageEventArgs e)
        {

        }


        /// <summary>
        /// This method handles Connected event of _scsClient.
        /// </summary>
        /// <param name="sender">Source of event</param>
        /// <param name="e">Event arguments</param>
        private void ScsClientConnected(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// This method handles Disconnected event of _scsClient.
        /// </summary>
        /// <param name="sender">Source of event</param>
        /// <param name="e">Event arguments</param>
        private void ScsClientDisconnected(object sender, EventArgs e)
        {
        }
    }
}
