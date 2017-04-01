using System;
using System.Collections.Generic;
using System.Text;
using OSGi.NET.Extension;

namespace OSGi.NET.Core.Root
{
    /// <summary>
    /// 框架内核构造工厂
    /// </summary>
    public interface IFrameworkFactory
    {
        /// <summary>
        /// 创建一个框架内核实例
        /// </summary>
        /// <returns>框架内核实例</returns>
        IFramework CreateFramework();

        /// <summary>
        /// 创建一个框架内核实例,扩展点支持
        /// </summary>
        /// <returns>框架内核实例</returns>
        IFramework CreateFramework(IList<ExtensionPoint> extensionPoints);

        /// <summary>
        /// 创建一个框架内核实例,扩展数据支持
        /// </summary>
        /// <returns>框架内核实例</returns>
        IFramework CreateFramework(IList<ExtensionData> extensionDatas);

    }
}
