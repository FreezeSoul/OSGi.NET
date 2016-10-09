using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OSGi.NET.Listener;

namespace OSGi.NET.Core.Root
{
    /// <summary>
    /// 框架内核监听器接口
    /// </summary>
    interface IFrameworkListener
    {
        /// <summary>
        /// 添加一个Bundle监听器
        /// </summary>
        /// <param name="listener">Bundle监听器实例</param>
        void AddBundleListener(IBundleListener listener);

        /// <summary>
        /// 移除一个Bundle监听器
        /// </summary>
        /// <param name="listener">Bundle监听器实例</param>
        void RemoveBundleListener(IBundleListener listener);

        /// <summary>
        /// 添加一个Extension监听器
        /// </summary>
        /// <param name="listener">Extension监听器实例</param>
        void AddExtensionListener(IExtensionListener listener);

        /// <summary>
        /// 移除一个Extension监听器
        /// </summary>
        /// <param name="listener">Extension监听器实例</param>
        void RemoveExtensionListener(IExtensionListener listener);

        /// <summary>
        /// 添加一个服务监听器
        /// </summary>
        /// <param name="listener">服务监听器实例</param>
        void AddServiceListener(IServiceListener listener);

        /// <summary>
        /// 移除一个服务监听器
        /// </summary>
        /// <param name="listener">服务监听器</param>
        void RemoveServiceListener(IServiceListener listener);

    }
}
