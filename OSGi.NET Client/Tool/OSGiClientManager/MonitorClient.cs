using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace OSGiClientManager
{
    public class MonitorClient : INotifyPropertyChanged
    {
        /// <summary>
        /// 
        /// </summary>
        private string clientID;
        /// <summary>
        /// 
        /// </summary>
        public string ClientID
        {
            get
            {
                return this.clientID;
            }
            set
            {
                if (this.clientID != value)
                {
                    this.clientID = value;
                    OnPropertyChanged("ClientID");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private string clientIP;
        /// <summary>
        /// 
        /// </summary>
        public string ClientIP
        {
            get
            {
                return this.clientIP;
            }
            set
            {
                if (this.clientIP != value)
                {
                    this.clientIP = value;
                    OnPropertyChanged("ClientIP");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private string startTime;
        /// <summary>
        /// 
        /// </summary>
        public string StartTime
        {
            get
            {
                return this.startTime;
            }
            set
            {
                if (this.startTime != value)
                {
                    this.startTime = value;
                    OnPropertyChanged("StartTime");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private string runningState;
        /// <summary>
        /// 
        /// </summary>
        public string RunningState
        {
            get
            {
                return this.runningState;
            }
            set
            {
                if (this.runningState != value)
                {
                    this.runningState = value;
                    OnPropertyChanged("RunningState");
                }
            }
        }

        public string View = "管理";

        /// <summary>
        /// 
        /// </summary>

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        /// <summary>
        /// 属性发生改变时执行的操作
        /// </summary>
        /// <param name="propertyName">改变的属性名称</param>
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
