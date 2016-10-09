using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

using log4net;

namespace OSGi.NET.Utils
{
    /// <summary>
    /// 跨域程序集分析器
    /// </summary>
    class AssemblyResolver : MarshalByRefObject
    {

        /// <summary>
        /// 程序集
        /// </summary>
        private Assembly assembly;

        /// <summary>
        /// BundleLib目录
        /// </summary>
        private string bundleLib;

        /// <summary>
        /// ShareLib目录
        /// </summary>
        private string shareLib;

        /// <summary>
        /// 程序集描述特性信息
        /// </summary>
        private IList<CustomAttributeData> attributeDataList = new List<CustomAttributeData>();

        /// <summary>
        /// 程序集版本关键字
        /// </summary>
        private const string BUNDLE_MANIFEST_REQUIRED_BUNDLE_VERSION = "bundle-version";


        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="assemblyByteArray">程序集二进制</param>
        /// <param name="bundleLibPath">BundleLib目录</param>
        /// <param name="shareLibPath">ShareLib目录</param>
        public void Init(Byte[] assemblyByteArray, string bundleLibPath, string shareLibPath)
        {
            this.bundleLib = bundleLibPath;
            this.shareLib = shareLibPath;
            AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve += CurrentDomainOnReflectionOnlyAssemblyResolve;
            try
            {
                this.assembly = Assembly.ReflectionOnlyLoad(assemblyByteArray);
                this.attributeDataList = CustomAttributeData.GetCustomAttributes(assembly);
            }
            catch (Exception) {/*对于程序集特性中如存在其他程序集定义的特性，则简化处理，无法读取厂商信息内容*/}
            AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve -= CurrentDomainOnReflectionOnlyAssemblyResolve;
        }


        /// <summary>
        /// 当前域关联程序集发现逻辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        private Assembly CurrentDomainOnReflectionOnlyAssemblyResolve(object sender, ResolveEventArgs args)
        {
            try
            {
                var resovleAssemblyName = new AssemblyName(args.Name);
                if (Directory.Exists(this.shareLib))
                {
                    string[] assemblyFiles = Directory.GetFiles(this.shareLib, string.Format("{0}.dll", resovleAssemblyName.Name), SearchOption.AllDirectories);
                    if (assemblyFiles.Length > 0)
                    {
                        var assemblyFile = assemblyFiles[0];
                        if (File.Exists(assemblyFile)) return Assembly.ReflectionOnlyLoadFrom(assemblyFile);
                    }
                }
                if (Directory.Exists(this.bundleLib))
                {
                    string[] bundleLibFles = Directory.GetFiles(this.bundleLib, string.Format("{0}.dll", resovleAssemblyName.Name), SearchOption.AllDirectories);
                    if (bundleLibFles.Length > 0)
                    {
                        var assemblyFile = bundleLibFles[0];
                        if (File.Exists(assemblyFile)) return Assembly.ReflectionOnlyLoadFrom(assemblyFile);
                    }
                }
                if (Directory.Exists(Environment.CurrentDirectory))
                {
                    string[] rootFiles = Directory.GetFiles(Environment.CurrentDirectory, string.Format("{0}.dll", resovleAssemblyName.Name), SearchOption.AllDirectories);
                    if (rootFiles.Length > 0)
                    {
                        var assemblyFile = rootFiles[0];
                        if (File.Exists(assemblyFile)) return Assembly.ReflectionOnlyLoadFrom(assemblyFile);
                    }
                }
                if (BundleUtils.IsAssemblyBelongsFcl(resovleAssemblyName.Name))
                {
                    return Assembly.ReflectionOnlyLoad(resovleAssemblyName.FullName);
                }
            }
            catch (Exception) { }
            return null;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="assemblyObj">程序集实例</param>
        public void Init(Assembly assemblyObj)
        {
            this.assembly = assemblyObj;
            attributeDataList = CustomAttributeData.GetCustomAttributes(assemblyObj);
        }

        /// <summary>
        /// 获取程序集全名
        /// </summary>
        /// <returns>程序集全名</returns>
        public string GetAssemblyFullName()
        {
            return assembly.FullName;
        }

        /// <summary>
        /// 获取程序集名称
        /// </summary>
        /// <returns>程序集名称</returns>
        public string GetAssemblyName()
        {
            return assembly.GetName().Name;
        }

        /// <summary>
        /// 获取程序集版本
        /// </summary>
        /// <returns>程序集版本</returns>
        public Version GetVersion()
        {
            return assembly.GetName().Version;
        }

        /// <summary>
        /// 获取程序集标题
        /// </summary>
        /// <returns>程序集标题</returns>
        public string GetAssemblyTitle()
        {
            string title = this.GetCustomAttributeData(typeof(AssemblyTitleAttribute)).ToString();
            if (string.IsNullOrEmpty(title))
                title = GetAssemblyName();
            return title;
        }

        /// <summary>
        /// 获取程序集厂商信息
        /// </summary>
        /// <returns>程序集厂商信息</returns>
        public string GetVendor()
        {
            return this.GetCustomAttributeData(typeof(AssemblyCompanyAttribute)).ToString();
        }

        /// <summary>
        /// 获取程序集自定义特性数据
        /// </summary>
        /// <param name="type">关键字</param>
        /// <returns>自定义特性数据</returns>
        private Object GetCustomAttributeData(Type type)
        {
            foreach (CustomAttributeData cad in attributeDataList)
            {
                if (cad.Constructor.DeclaringType != null && cad.Constructor.DeclaringType == type)
                {
                    return cad.ConstructorArguments[0].Value;
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// 获取程序集依赖程序集
        /// </summary>
        /// <returns>依赖程序集信息</returns>
        public string GetAssemblyRequiredAssembly()
        {
            var sb = new StringBuilder();
            foreach (AssemblyName assemblyName in assembly.GetReferencedAssemblies())
            {
                string referenceAssemblyName = assemblyName.Name;
                if (referenceAssemblyName == "mscorlib") continue;
                if (referenceAssemblyName.StartsWith("System.")) continue;
                if (referenceAssemblyName.Equals(typeof(AssemblyResolver).Assembly.GetName().Name)) continue;

                sb.Append(string.Format("{0};{1}=\"{2}\"", referenceAssemblyName, BUNDLE_MANIFEST_REQUIRED_BUNDLE_VERSION, assemblyName.Version));
                sb.Append(",");
            }
            if (sb.Length == 0) return "";
            return sb.ToString(0, sb.Length - 1);
        }
    }
}
