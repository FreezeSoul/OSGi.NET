using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace OSGi.NET.Core.Root
{
    /// <summary>
    /// 框架内核构造工厂
    /// </summary>
    public class FrameworkFactory : IFrameworkFactory
    {
        /// <summary>
        /// 创建一个框架内核实例
        /// </summary>
        /// <returns>框架内核实例</returns>
        public IFramework CreateFramework()
        {
            return new Framework();
        }

        /// <summary>
        /// 创建一个框架内核实例，扩展点支持
        /// </summary>
        /// <returns>框架内核实例</returns>
        public IFramework CreateFramework(IList<Extension.ExtensionPoint> extensionPoints)
        {
            return new Framework(extensionPoints);
        }

        /// <summary>
        /// 创建一个框架内核实例，扩展数据支持
        /// </summary>
        /// <returns>框架内核实例</returns>
        public IFramework CreateFramework(IList<Extension.ExtensionData> extensionDatas)
        {
            return new Framework(extensionDatas);
        }
    }
}
