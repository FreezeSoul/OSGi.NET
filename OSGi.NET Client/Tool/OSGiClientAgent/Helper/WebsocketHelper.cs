using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

using Fleck;

namespace OSGiClientAgent.Helper
{
    class WebsocketHelper
    {
        private static WebSocketServer webSocketServer;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="port"></param>
        public static void Start(int port)
        {
            try
            {
                if (webSocketServer == null)
                {
                    webSocketServer = new WebSocketServer(string.Format("ws://0.0.0.0:{0}", port));
                }
                webSocketServer.Start(socket =>
                {
                    socket.OnOpen = () => socket.Send("服务器已连接！");
                    socket.OnClose = () => { };
                    socket.OnMessage = message =>
                    {
                        if (string.IsNullOrEmpty(message.Trim()) == false)
                        {
                            var result = CommandHelper.Exc(message);
                            socket.Send(message + Environment.NewLine + result);
                        }
                    };
                });
            }
            catch { }

        }

        /// <summary>
        /// 
        /// </summary>
        public static void Stop()
        {
            try
            {
                if (webSocketServer != null)
                {
                    webSocketServer.Dispose();
                }
            }
            catch { }
        }
    }
}
