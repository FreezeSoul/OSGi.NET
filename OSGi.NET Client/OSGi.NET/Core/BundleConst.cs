using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OSGi.NET.Core
{
    /// <summary>
    /// Bunble模块状态
    /// </summary>
    public class BundleStateConst
    {
        /// <summary>
        /// 已卸载
        /// </summary>
        public const int UNINSTALLED = 0x00000001;
        /// <summary>
        /// 已安装
        /// </summary>
        public const int INSTALLED = 0x00000002;
        /// <summary>
        /// 已装载
        /// </summary>
        public const int RESOLVED = 0x00000004;
        /// <summary>
        /// 正在启动
        /// </summary>
        public const int STARTING = 0x00000008;
        /// <summary>
        /// 正在停止
        /// </summary>
        public const int STOPPING = 0x00000010;
        /// <summary>
        /// 已激活
        /// </summary>
        public const int ACTIVE = 0x00000020;
    }

    /// <summary>
    /// Bunde常量
    /// </summary>
    internal class BundleConst
    {
        /// <summary>
        /// 依赖库目录
        /// </summary>
        public const string BUNDLE_LIBS_DIRECTORY_NAME = "Libs";

        /// <summary>
        /// bundle版本
        /// </summary>
        public const string BUNDLE_MANIFEST_REQUIRED_BUNDLE_VERSION = "bundle-version";

        /// <summary>
        /// bundle激活器
        /// </summary>
        public const string BUNDLE_MANIFEST_ACTIVATOR = "Activator";

        /// <summary>
        /// bundle符号名称
        /// </summary>
        public const string BUNDLE_MANIFEST_SYMBOLIC_NAME = "SymbolicName";

        /// <summary>
        /// bundle版本
        /// </summary>
        public const string BUNDLE_MANIFEST_VERSION = "Version";

        /// <summary>
        /// bundle名称
        /// </summary>
        public const string BUNDLE_MANIFEST_NAME = "Name";

        /// <summary>
        /// bundle厂商信息
        /// </summary>
        public const string BUNDLE_MANIFEST_VENDOR = "Vendor";

        /// <summary>
        /// bundle依赖bundle
        /// </summary>
        public const string BUNDLE_MANIFEST_REQUIRE_BUNDLE = "Require-Bundle";
    }
}
