using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

using OSGi.NET.Core;
using OSGi.NET.Core.Root;
using OSGi.NET.Event;
using OSGi.NET.Service;

using OSGiClientAgent.Helper;
using OSGiClientAgent.Model;

using RestSharp;

namespace OSGiClientAgent
{
    public class BundleActivator : IBundleActivator
    {
        /// <summary>
        /// 
        /// </summary>
        private ConfigHelper configHelper;

        /// <summary>
        /// 
        /// </summary>
        private RestClient restClient;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void Start(IBundleContext context)
        {
            try
            {
                InitClient(context);
                InitCommandProxy(context);
                new Task(PostClientInfo).Start();
            }
            catch (Exception ex) { }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void Stop(IBundleContext context)
        {
            try
            {
                this.PostClientUnline();
                this.StopWebSocketServer();
            }
            catch (Exception ex) { }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        private void InitClient(IBundleContext context)
        {
            configHelper = new ConfigHelper(context.GetBundle());
            var serverAddr = configHelper.GetManifestConfigValue("ServerAddr");
            restClient = new RestClient(serverAddr);
            var monitorPort = configHelper.GetManifestConfigValue("MonitorPort");
            InitWebSocketServer(int.Parse(monitorPort));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        private void InitCommandProxy(IBundleContext context)
        {
            CommandHelper.Init(context);
        }


        /// <summary>
        /// 
        /// </summary>
        private void InitWebSocketServer(int port)
        {
            WebsocketHelper.Start(port);
        }

        /// <summary>
        /// 
        /// </summary>
        private void StopWebSocketServer()
        {
            WebsocketHelper.Stop();
        }

        /// <summary>
        /// 
        /// </summary>
        private void PostClientInfo()
        {
            var status = "在线";
            var request = new RestRequest("v1/client/", Method.POST);
            var ip = ClientDeviceHelper.GetIpAddress();
            var mac = ClientDeviceHelper.GetMacAddress();
            var appName = ClientDeviceHelper.GetApplicationName();
            var systemInfo = string.Empty;
            var loadSystemInfo = configHelper.GetManifestConfigValue("LoadSystemInfo");
            if (loadSystemInfo == "1")
                systemInfo = ClientDeviceHelper.GetSystemInfo();
            var monitorPort = configHelper.GetManifestConfigValue("MonitorPort");
            request.RequestFormat = DataFormat.Json;
            request.AddBody(new Client
            {
                ClientMac = mac,
                ClientIp = ip,
                LaunchTime = DateTime.Now,
                Status = status,
                SystemName = appName,
                SystemInfo = systemInfo,
                MonitorPort = monitorPort
            });
            restClient.Execute(request);
        }

        /// <summary>
        /// 
        /// </summary>
        private void PostClientUnline()
        {
            var mac = ClientDeviceHelper.GetMacAddress();
            var request = new RestRequest(string.Format("v1/client/{0}/{1}", mac, "离线"), Method.GET);
            request.RequestFormat = DataFormat.Json;
            restClient.Get(request);
        }

    }
}
