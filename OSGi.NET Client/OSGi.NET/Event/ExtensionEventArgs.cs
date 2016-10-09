using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OSGi.NET.Extension;

namespace OSGi.NET.Event
{

    /// <summary>
    /// 扩展事件参数
    /// </summary>
    public class ExtensionEventArgs : EventArgs
    {
        /// <summary>
        /// 加载扩展
        /// </summary>
        public const int LOAD = 0x00000001;
        /// <summary>
        /// 卸载扩展
        /// </summary>
        public const int UNLOAD = 0x00000002;

        /// <summary>
        /// 状态
        /// </summary>
        private int state;
        /// <summary>
        /// 扩展点
        /// </summary>
        private ExtensionPoint extensionPoint;
        /// <summary>
        /// 扩展点数据
        /// </summary>
        private ExtensionData extensionData;

        /// <summary>
        /// 服务事件参数
        /// </summary>
        /// <param name="state">状态</param>
        /// <param name="extensionPoint">扩展点</param>
        /// <param name="extensionData">扩展点数据</param>
        internal ExtensionEventArgs(int state, ExtensionPoint extensionPoint, ExtensionData extensionData)
        {
            this.state = state;
            this.extensionPoint = extensionPoint;
            this.extensionData = extensionData;
        }

        /// <summary>
        /// 扩展点数据加载状态
        /// </summary>
        /// <returns></returns>
        public int GetState()
        {
            return state;
        }

        /// <summary>
        /// 获取扩展点
        /// </summary>
        /// <returns></returns>
        public ExtensionPoint GetExtensionPoint()
        {
            return extensionPoint;
        }

        /// <summary>
        /// 获取扩展点数据
        /// </summary>
        /// <returns></returns>
        public ExtensionData GetExtensionData()
        {
            return extensionData;
        }

    }
}
