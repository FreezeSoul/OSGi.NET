using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;

namespace OSGi.NET.Provider
{

    /// <summary>
    /// 配置信息提供程序
    /// </summary>
    internal static class BundleConfigProvider
    {
        #region Property

        /// <summary>
        /// OSGi配置文件名称常量
        /// </summary>
        public static string OSGi_NET_CONFIG = "OSGi.NET.properties";

        /// <summary>
        /// Bundle存储目录常量
        /// </summary>
        public const string OSGi_NET_BUNDLE_STORAGE = "OSGi.NET.BoundlePath";

        /// <summary>
        /// SHARE共享程序集目录常量
        /// </summary>
        public const string OSGi_NET_SHARELIB_STORAGE = "OSGi.NET.ShareLibPath";

        /// <summary>
        /// 依赖库或共享程序集是否全类型加载常量
        /// </summary>
        public const string OSGi_NET_ALL_TYPES_LOAD = "OSGi.NET.ALLTYPESLOAD";

        /// <summary>
        /// 是否独立运行
        /// </summary>
        public const string OSGi_NET_SINGLE_RUNNING = "OSGi.NET.SINGLERUNNING";

        /// <summary>
        /// FCL程序清单常量(框架程序集清单)
        /// </summary>
        public const string OSGi_NET_FCL_LIST = "OSGi.NET.FCL";

        /// <summary>
        /// SCL程序清单常量(共享程序集清单)
        /// </summary>
        public const string OSGi_NET_SCL_LIST = "OSGi.NET.SCL";

        /// <summary>
        /// 是否Debug模式
        /// </summary>
        public const string OSGi_NET_DEBUG_MODE = "OSGi.NET.DEBUG";

        /// <summary>
        /// Bundle卸载目录常量
        /// </summary>
        public const string OSGi_NET_UNINSTALL_STORAGE = "OSGi.NET.UnInstallPath";

        /// <summary>
        /// Bundle缺省目录
        /// </summary>
        public const string OSGi_NET_BUNDLE_DIRECTORY_NAME = "Bundles";

        /// <summary>
        /// SHARE共享程序集缺省目录
        /// </summary>
        public const string OSGi_NET_SHARELIB_DIRECTORY_NAME = "Libs";

        /// <summary>
        /// Bundle配置文件名称常量
        /// </summary>
        public const string OSGi_NET_BUNDLE_CONFIG = "Manifest.xml";

        /// <summary>
        /// Bundle卸载目录
        /// </summary>
        public const string OSGi_NET_UNINSTALL_DIRECTORY_NAME = "Temp";


        /// <summary>
        /// 对于依赖库或共享程序集是否全类型加载
        /// </summary>
        public static bool OSGi_NET_ALLTYPES_LOAD = false;


        /// <summary>
        /// 是否独立运行
        /// </summary>
        public static bool OSGi_NET_SINGLERUNNING = false;

        /// <summary>
        /// 是否是Debug模式，差异主要在加载DLL方式上，Debug模式只是通过反射加载，不能做到
        /// </summary>
        public static bool OSGi_NET_IS_DEBUG_MODE = false;

        /// <summary>
        /// 框架配置信息
        /// </summary>
        public static IDictionary<string, string> FrameworkConfiguration;

        /// <summary>
        /// Bundles配置信息
        /// </summary>
        public static IDictionary<string, XmlNode> BundlesConfiguration;

        /// <summary>
        /// 启动级别
        /// </summary>
        private const string StartLevel = "StartLevel";

        /// <summary>
        /// 程序集名称
        /// </summary>
        private const string AssemblyName = "AssemblyName";

        #endregion

        #region Constructor

        /// <summary>
        /// 静态构造
        /// </summary>
        static BundleConfigProvider()
        {
            OSGi_NET_ALLTYPES_LOAD = false;
            OSGi_NET_IS_DEBUG_MODE = false;
            OSGi_NET_SINGLERUNNING = false;

            LoadFrameworkConfig();

            if (FrameworkConfiguration.ContainsKey(OSGi_NET_ALL_TYPES_LOAD))
                OSGi_NET_ALLTYPES_LOAD = FrameworkConfiguration[OSGi_NET_ALL_TYPES_LOAD].Trim() == "1";

            if (FrameworkConfiguration.ContainsKey(OSGi_NET_DEBUG_MODE))
                OSGi_NET_IS_DEBUG_MODE = FrameworkConfiguration[OSGi_NET_DEBUG_MODE].Trim() == "1";

            if (FrameworkConfiguration.ContainsKey(OSGi_NET_SINGLE_RUNNING))
                OSGi_NET_SINGLERUNNING = FrameworkConfiguration[OSGi_NET_SINGLE_RUNNING].Trim() == "1";
        }

        #endregion

        #region Method

        /// <summary>
        /// 读取框架配置
        /// </summary>
        static void LoadFrameworkConfig()
        {
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);

            var fileNameInConfig = ConfigurationManager.AppSettings["OSGiConfigFile"];
            if (false == string.IsNullOrWhiteSpace(fileNameInConfig))
                OSGi_NET_CONFIG = fileNameInConfig;

            FrameworkConfiguration = new Dictionary<string, string>();
            string configFileName = Path.Combine(Environment.CurrentDirectory, OSGi_NET_CONFIG);
            if (File.Exists(configFileName))
            {
                //读取properties文件
                string[] lines = File.ReadAllLines(configFileName);
                foreach (string line in lines)
                {
                    string templine = line.Trim();
                    if (string.IsNullOrEmpty(line) || templine.StartsWith("#")) continue;
                    int equalIndex = templine.IndexOf("=", StringComparison.Ordinal);
                    if (equalIndex <= 0) continue;
                    string key = line.Substring(0, equalIndex);
                    string value = line.Substring(equalIndex + 1);
                    FrameworkConfiguration.Add(key, value.Trim());
                }
            }
        }

        /// <summary>
        /// 读取Bundle配置
        /// </summary>
        internal static void LoadBundlesConfig(string bundlesPath)
        {
            BundlesConfiguration = new Dictionary<string, XmlNode>();
            string bundlesPathName = Path.Combine(Environment.CurrentDirectory, bundlesPath);
            if (Directory.Exists(bundlesPathName))
            {
                var directory = new DirectoryInfo(bundlesPathName);
                var tempConfiguration = new Dictionary<string, XmlNode>();
                foreach (var directoryInfo in directory.GetDirectories())
                {
                    var bundleConfigFilePath = Path.Combine(directoryInfo.FullName, OSGi_NET_BUNDLE_CONFIG);
                    if (File.Exists(bundleConfigFilePath))
                    {
                        var xmlDoc = new XmlDocument();
                        xmlDoc.Load(bundleConfigFilePath);
                        //Key为文件目录，Value为配置节点
                        tempConfiguration.Add(directoryInfo.Name, xmlDoc.DocumentElement);
                    }
                }

                //按加载级别顺序读取Bundles配置
                var sorted = tempConfiguration.OrderBy(
                    d =>
                    {
                        var index = 0;
                        if (d.Value.Attributes != null)
                        {
                            //根据启动级别设置加载Bundle
                            var bundleIndex = d.Value.Attributes[StartLevel];
                            if (bundleIndex != null) int.TryParse(bundleIndex.Value, out index);
                        }
                        return index;
                    });

                foreach (var keyValuePair in sorted)
                {
                    BundlesConfiguration.Add(keyValuePair.Key, keyValuePair.Value);
                }
            }
            else
            {
                Directory.CreateDirectory(bundlesPathName);
            }
        }

        /// <summary>
        /// 读取指定目录的Bundle配置信息
        /// </summary>
        /// <param name="bundlePath"></param>
        /// <returns></returns>
        internal static XmlNode ReadBundleConfig(string bundlePath)
        {
            var bundleConfigFilePath = Path.Combine(bundlePath, OSGi_NET_BUNDLE_CONFIG);
            if (File.Exists(bundleConfigFilePath))
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(bundleConfigFilePath);
                return xmlDoc.DocumentElement;
            }
            return null;
        }

        /// <summary>
        /// 同步Bundle的配置信息
        /// </summary>
        /// <param name="bundleKey"></param>
        /// <param name="xmlNode"></param>
        internal static void SyncBundleConfig(string bundleKey, XmlNode xmlNode)
        {
            var tempConfiguration = new Dictionary<string, XmlNode>(BundlesConfiguration);

            if (!tempConfiguration.ContainsKey(bundleKey))
                tempConfiguration.Add(bundleKey, xmlNode);
            else
                tempConfiguration[bundleKey] = xmlNode;

            //按加载级别顺序读取Bundles配置
            var sorted = tempConfiguration.OrderBy(
                d =>
                {
                    var index = 0;
                    if (d.Value.Attributes != null)
                    {
                        //根据启动级别设置加载Bundle
                        var bundleIndex = d.Value.Attributes[StartLevel];
                        if (bundleIndex != null) int.TryParse(bundleIndex.Value, out index);
                    }
                    return index;
                });

            BundlesConfiguration = new Dictionary<string, XmlNode>();
            foreach (var keyValuePair in sorted)
            {
                BundlesConfiguration.Add(keyValuePair.Key, keyValuePair.Value);
            }
        }

        /// <summary>
        /// 移除指定Key的Bundle配置信息
        /// </summary>
        /// <param name="bundleKey"></param>
        internal static void RemoveBundleConfig(string bundleKey)
        {
            if (BundlesConfiguration.ContainsKey(bundleKey))
                BundlesConfiguration.Remove(bundleKey);

        }


        /// <summary>
        /// 检测引用程序集是否属于Dll白名单
        /// </summary>
        /// <param name="dllName"></param>
        /// <returns></returns>
        internal static bool CheckDllInWhiteList(string dllName)
        {
            if (FrameworkConfiguration.ContainsKey(OSGi_NET_FCL_LIST))
            {
                var list = FrameworkConfiguration[OSGi_NET_FCL_LIST].Split(',');
                return list.ToList().Any(item => item.Trim().Equals(dllName));
            }
            return false;
        }

        /// <summary>
        /// 返回根目录的共享程序集
        /// </summary>
        /// <returns></returns>
        internal static IList<string> GetConfigRootShareDll()
        {
            var list = new List<string>();
            if (FrameworkConfiguration.ContainsKey(OSGi_NET_SCL_LIST))
            {
                FrameworkConfiguration[OSGi_NET_SCL_LIST].Split(',').ToList().ForEach(list.Add);
            }
            return list;
        }

        /// <summary>
        /// 获取模块存储目录
        /// </summary>
        /// <returns></returns>
        internal static string GetBundlesDirectory()
        {

            if (FrameworkConfiguration == null) return string.Empty;

            var bundlesDirectoryPath = string.Empty;
            // 插件目录
            if (FrameworkConfiguration.ContainsKey(OSGi_NET_BUNDLE_STORAGE))
                bundlesDirectoryPath = Path.Combine(Environment.CurrentDirectory, FrameworkConfiguration[OSGi_NET_BUNDLE_STORAGE]);

            // 缺省目录
            if (string.IsNullOrEmpty(bundlesDirectoryPath))
                bundlesDirectoryPath = Path.Combine(Environment.CurrentDirectory, OSGi_NET_BUNDLE_DIRECTORY_NAME);

            return bundlesDirectoryPath;
        }

        /// <summary>
        /// 获取共享库目录
        /// </summary>
        /// <returns></returns>
        internal static string GetShareLibsDirectory()
        {
            if (FrameworkConfiguration == null) return string.Empty;

            var shareLibsDirectoryPath = string.Empty;

            // 共享程序集目录
            if (FrameworkConfiguration.ContainsKey(OSGi_NET_SHARELIB_STORAGE))
                shareLibsDirectoryPath = Path.Combine(Environment.CurrentDirectory, FrameworkConfiguration[OSGi_NET_SHARELIB_STORAGE]);

            // 共享程序集目录
            if (string.IsNullOrEmpty(shareLibsDirectoryPath))
                shareLibsDirectoryPath = Path.Combine(Environment.CurrentDirectory, OSGi_NET_SHARELIB_DIRECTORY_NAME);

            return shareLibsDirectoryPath;
        }

        /// <summary>
        /// 获取模块卸载目录
        /// </summary>
        /// <returns></returns>
        internal static string GetBundlesUninstallDirectory()
        {
            if (FrameworkConfiguration == null) return string.Empty;

            var bundlesUninstallPath = string.Empty;

            // 程序集卸载缓存目录
            if (FrameworkConfiguration.ContainsKey(OSGi_NET_UNINSTALL_STORAGE))
                bundlesUninstallPath = Path.Combine(Environment.CurrentDirectory, FrameworkConfiguration[OSGi_NET_UNINSTALL_STORAGE]);

            // 程序集卸载缓存目录
            if (string.IsNullOrEmpty(bundlesUninstallPath))
                bundlesUninstallPath = Path.Combine(Environment.CurrentDirectory, OSGi_NET_UNINSTALL_DIRECTORY_NAME);

            return bundlesUninstallPath;
        }


        /// <summary>
        /// 返回Bundle配置的程序集名称
        /// </summary>
        /// <param name="xmlNode">模块配置节点</param>
        /// <returns></returns>
        internal static string GetBundleConfigAssemblyName(XmlNode xmlNode)
        {
            if (xmlNode.Attributes == null) return string.Empty;
            var assemblyNameNode = xmlNode.Attributes[AssemblyName];
            var tmpassemblyName = assemblyNameNode == null ? string.Empty : assemblyNameNode.Value;
            return tmpassemblyName;
        }

        /// <summary>
        /// 返回Bundle配置的启动级别
        /// </summary>
        /// <param name="xmlNode">模块配置节点</param>
        /// <returns></returns>
        internal static int GetBundleConfigStartLevel(XmlNode xmlNode)
        {
            var bundleIndex = 0;
            if (xmlNode.Attributes == null) return bundleIndex;
            var startLevelNode = xmlNode.Attributes[StartLevel];
            var startLevel = startLevelNode == null ? string.Empty : startLevelNode.Value;
            int.TryParse(startLevel, out bundleIndex);
            return bundleIndex;
        }


        /// <summary>
        /// 根据模块配置节点返回扩展点列表
        /// </summary>
        /// <param name="xmlNode">模块配置节点</param>
        /// <returns>扩展点列表</returns>
        internal static IList<string> GetBundleConfigExtensionPoints(XmlNode xmlNode)
        {
            var extensionPoints = new List<string>();
            foreach (XmlNode childNode in xmlNode.ChildNodes)
            {
                if (childNode.Name.ToLower() == "ExtensionPoint".ToLower())
                {
                    if (childNode.Attributes == null) continue;
                    var extensionPointAttr = childNode.Attributes["Point"];
                    var extensionPoint = extensionPointAttr == null ? string.Empty : extensionPointAttr.Value;
                    if (!extensionPoints.Contains(extensionPoint))
                        extensionPoints.Add(extensionPoint);
                }
            }
            return extensionPoints;
        }

        /// <summary>
        /// 根据模块配置节点返回扩展数据字典
        /// </summary>
        /// <param name="xmlNode">模块配置节点</param>
        /// <returns>扩展数据字典</returns>
        internal static IDictionary<string, IList<XmlNode>> GetBundleConfigExtensionDatas(XmlNode xmlNode)
        {
            var extensionDatas = new Dictionary<string, IList<XmlNode>>();
            foreach (XmlNode childNode in xmlNode.ChildNodes)
            {
                if (childNode.Name.ToLower() == "Extension".ToLower())
                {
                    if (childNode.Attributes == null) continue;
                    var extensionPointAttr = childNode.Attributes["Point"];
                    var extensionPoint = extensionPointAttr == null ? string.Empty : extensionPointAttr.Value;
                    if (!extensionDatas.ContainsKey(extensionPoint))
                    {
                        var childNodes = new List<XmlNode>();
                        childNodes.Add(childNode);
                        extensionDatas.Add(extensionPoint, childNodes);
                    }
                    else
                    {
                        extensionDatas[extensionPoint].Add(childNode);
                    }
                }
            }
            return extensionDatas;
        }

        #endregion
    }
}
