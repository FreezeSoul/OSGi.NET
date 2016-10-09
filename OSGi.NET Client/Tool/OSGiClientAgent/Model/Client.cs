using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OSGiClientAgent.Model
{
    public class Client
    {
        /// <summary>
        /// 
        /// </summary>
        public string ClientMac { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public string ClientIp { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public string SystemName { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public string SystemInfo { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public string MonitorPort { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime LaunchTime { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public string Status { set; get; }
    }
}
