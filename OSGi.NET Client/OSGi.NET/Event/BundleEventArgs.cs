using System;
using System.Collections.Generic;
using System.Text;
using OSGi.NET.Core;

namespace OSGi.NET.Event
{
    /// <summary>
    /// Bundle事件参数
    /// </summary>
    public class BundleEventArgs : EventArgs
    {
        /// <summary>
        /// 已安装
        /// </summary>
        public const int INSTALLED = 0x00000001;
        /// <summary>
        /// 已启动
        /// </summary>
        public const int STARTED = 0x00000002;
        /// <summary>
        /// 已停止
        /// </summary>
        public const int STOPPED = 0x00000004;
        //public const int UPDATED = 0x00000008;
        //public const int UNINSTALLED = 0x00000010;
        /// <summary>
        /// 已装载
        /// </summary>
        public const int RESOLVED = 0x00000020;
        //public const int UNRESOLVED = 0x00000040;
        /// <summary>
        /// 启动中
        /// </summary>
        public const int STARTING = 0x00000080;
        /// <summary>
        /// 暂停中
        /// </summary>
        public const int STOPPING = 0x00000100;
        //public const int LAZY_ACTIVATION = 0x00000200;

        /// <summary>
        /// 状态
        /// </summary>
        private int state;
        /// <summary>
        /// 模块
        /// </summary>
        private IBundle bundle;


        /// <summary>
        /// Bundle事件参数
        /// </summary>
        /// <param name="state">状态</param>
        /// <param name="bundle">模块</param>
        internal BundleEventArgs(int state, IBundle bundle)
        {
            this.bundle = bundle;
            this.state = state;
        }

        /// <summary>
        /// 获取Bundle对象
        /// </summary>
        /// <returns></returns>
        public IBundle GetBundle()
        {
            return bundle;
        }


        /// <summary>
        /// 获取Bundle状态
        /// </summary>
        /// <returns></returns>
        public int GetState()
        {
            return state;
        }
    }
}
