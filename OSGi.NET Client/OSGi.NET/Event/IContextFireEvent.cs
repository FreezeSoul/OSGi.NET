using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OSGi.NET.Core;

namespace OSGi.NET.Event
{
    /// <summary>
    /// 模块上下文触发事件接口
    /// </summary>
    interface IContextFireEvent
    {
        /// <summary>
        /// 触发扩展变更事件
        /// </summary>
        /// <param name="bundle">引发的模块</param>
        /// <param name="extensionEventArgs">扩展事件参数</param>
        void FireExtensionChanged(IBundle bundle, ExtensionEventArgs extensionEventArgs);

        /// <summary>
        /// 触发服务变更事件
        /// </summary>
        /// <param name="bundle">引发的模块</param>
        /// <param name="serviceEventArgs">服务事件参数</param>
        void FireServiceChanged(IBundle bundle, ServiceEventArgs serviceEventArgs);

    }
}
