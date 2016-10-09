using System;

using ICSharpCode.SharpZipLib.Zip;

using Mono.Cecil;

using OSGi.NET.Provider;

namespace OSGi.NET.Utils
{

    /// <summary>
    /// Bundle工具对象
    /// </summary>
    internal class BundleUtils
    {

        #region Public Method
        /// <summary>
        /// 程序集是否属于Framework class dll
        /// </summary>
        /// <param name="assemblyName">程序集名称</param>
        /// <returns>是否属于</returns>
        public static Boolean IsAssemblyBelongsFcl(string assemblyName)
        {
            return assemblyName.StartsWith("System.")
                || assemblyName.Equals("System")
                || assemblyName.Equals("mscorlib")
                || assemblyName.Equals("OSGi.NET")
                || BundleConfigProvider.CheckDllInWhiteList(assemblyName);
        }

        /// <summary>
        /// 去掉强签名
        /// </summary>
        /// <param name="reference">程序集引用</param>
        public static void RemoveAssemblyStrongName(AssemblyNameReference reference)
        {
            reference.Hash = new Byte[0];
            reference.PublicKey = new Byte[0];
            reference.PublicKeyToken = new Byte[0];
            reference.HasPublicKey = false;
        }

        /// <summary>
        /// 解压文件
        /// </summary>
        /// <param name="outFolder">输出目录</param>
        /// <param name="zipFile">压缩包文件路径</param>
        public static void ExtractBundleFile(string outFolder, string zipFile)
        {
            var fastZip = new FastZip();
            fastZip.ExtractZip(zipFile, outFolder, string.Empty);
        }

        #endregion

    }
}
