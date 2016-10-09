using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OSGi.NET.Event
{

    /// <summary>
    /// 内核触发事件接口
    /// </summary>
    interface IFrameworkFireEvent
    {
        /// <summary>
        /// 触发服务变更事件
        /// </summary>
        /// <param name="serviceEvent">服务事件参数</param>
        void FireServiceEvent(ServiceEventArgs serviceEvent);

        /// <summary>
        /// 触发Bundle状态变更事件
        /// </summary>
        /// <param name="bundleEvent">Bundle事件参数</param>
        void FireBundleEvent(BundleEventArgs bundleEvent);

        /// <summary>
        /// 触发Extension变更事件
        /// </summary>
        /// <param name="extensionEvent">Extension事件参数</param>
        void FireExtensionEvent(ExtensionEventArgs extensionEvent);

    }
}
