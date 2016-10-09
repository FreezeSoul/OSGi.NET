using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Policy;

using Mono.Cecil;

using OSGi.NET.Core;
using OSGi.NET.Core.Root;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

using OSGi.NET.Provider;
using OSGi.NET.Utils;

namespace OSGi.NET.TEST
{
    using System.Diagnostics;
    using System.Reflection;

    /// <summary>
    ///This is a test class for FrameworkImplTest and is intended
    ///to contain all FrameworkImplTest Unit Tests
    ///</summary>
    [TestClass()]
    public class FrameworkImplTest
    {

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion

        #region AssemblyResolver测试
        /// <summary>
        ///
        ///</summary>
        [TestMethod()]
        public void AssemblyResolverTest()
        {
            var assemblyFileName = @"F:\Work\启明星\OSGi.NET\Sample\WPF\WpfDemo\bin\Debug\Libs\DevExpress.Images.v13.2.dll";

            var readerParameter = new ReaderParameters()
            {
                AssemblyResolver = new CustomAssemblyResolver(
                    filename =>
                    {
                        var requestfilename = string.Format(@"F:\Work\启明星\OSGi.NET\Sample\WPF\WpfDemo\bin\Debug\Libs\{0}.dll", filename);
                        return AssemblyDefinition.ReadAssembly(requestfilename);
                    })
            };

            AssemblyDefinition assemblyDefinition = AssemblyDefinition.ReadAssembly(assemblyFileName, readerParameter);
            AssemblyNameDefinition assemblyNameDefinition = assemblyDefinition.Name;
            //修改名称
            //assemblyNameDefinition.Name = assemblyNameDefinition.Name + Guid.NewGuid();
            //去掉强签名
            assemblyNameDefinition.Hash = new Byte[0];
            assemblyNameDefinition.PublicKey = new Byte[0];
            assemblyNameDefinition.PublicKeyToken = new Byte[0];
            assemblyNameDefinition.HasPublicKey = false;
            assemblyNameDefinition.HashAlgorithm = AssemblyHashAlgorithm.None;
            assemblyNameDefinition.Culture = "neutral";
            foreach (ModuleDefinition moduleDefinition in assemblyDefinition.Modules)
            {
                foreach (AssemblyNameReference assemblyNameReference in moduleDefinition.AssemblyReferences)
                {
                    Console.WriteLine(assemblyNameReference.FullName);
                }
                moduleDefinition.Attributes &= ~ModuleAttributes.StrongNameSigned;
            }
            assemblyDefinition.MainModule.Attributes &= ~ModuleAttributes.StrongNameSigned;

            MemoryStream ms = new MemoryStream();
            assemblyDefinition.Write(ms);
            //assemblyDefinition.Write(@"c:\log4net.dll");
            //Assembly.LoadFrom(@"c:\log4net.dll");
            Assembly.Load(ms.ToArray());

        }
        /// <summary>
        /// 
        /// </summary>
        internal class CustomAssemblyResolver : BaseAssemblyResolver
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="assemblyName"></param>
            /// <returns></returns>
            public delegate AssemblyDefinition ResolveAssemblyDefinitionDelegate(string assemblyName);

            /// <summary>
            /// 
            /// </summary>
            private readonly ResolveAssemblyDefinitionDelegate resolveAssemblyDefinition;

            /// <summary>
            /// 
            /// </summary>
            public CustomAssemblyResolver(ResolveAssemblyDefinitionDelegate resolveAssemblyDefinition)
            {
                this.resolveAssemblyDefinition = resolveAssemblyDefinition;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="assemblyNameReference"></param>
            /// <returns></returns>
            public override AssemblyDefinition Resolve(AssemblyNameReference assemblyNameReference)
            {
                if (resolveAssemblyDefinition != null)
                    return resolveAssemblyDefinition(assemblyNameReference.Name);
                return null;
            }
        }
        #endregion

        #region AssemblyVersion测试

        private string CurrentVersion;

        private string CurrentPath;

        [TestMethod()]
        public void AssemblyResoveTest()
        {
            CurrentPath = @"F:\Work\启明星\OSGi.NET\OSGi.NET.TEST\bin\Debug\Test";
            CurrentVersion = "Version1";
            //调用版本1
            AssemblyLoadTest(CurrentVersion);
            //重复调用测试
            AssemblyLoadTest(CurrentVersion);
            CurrentVersion = "Version2";
            //调用版本2
            AssemblyLoadTest(CurrentVersion);
        }

        private void AssemblyLoadTest(string version)
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomainOnAssemblyResolve;
            AssemblyDefinition assemblyDefinition = AssemblyDefinition.ReadAssembly(string.Format(@"{1}\{0}\BundleDemoB.dll", version, CurrentPath));
            MemoryStream ms = new MemoryStream();
            assemblyDefinition.Write(ms);
            var assembly = Assembly.Load(ms.ToArray());


            foreach (Type type in assembly.GetTypes())
            {
                if (type.Name == "AssemblyResoveTest")
                {
                    var obj = Activator.CreateInstance(type);
                    var method = type.GetMethod("Test");
                    method.Invoke(obj, null);
                }
            }
        }

        private Assembly CurrentDomainOnAssemblyResolve(object sender, ResolveEventArgs args)
        {
            var assmblyName = new AssemblyName(args.Name);
            Console.WriteLine(assmblyName.FullName);
            AssemblyDefinition assemblyDefinition = AssemblyDefinition.ReadAssembly(string.Format(@"{2}\{1}\{0}.dll", assmblyName.Name, CurrentVersion, CurrentPath));
            MemoryStream ms = new MemoryStream();
            assemblyDefinition.Write(ms);
            var assembly = Assembly.Load(ms.ToArray());
            Console.WriteLine(assembly.FullName);
            return assembly;
        }


        #endregion

        #region MonoAssemblyLoad测试
        /// <summary>
        ///
        ///</summary>
        [TestMethod()]
        public void MonoAssemblyLoadTest()
        {

            string[] files = Directory.GetFiles(@"F:\Work\启明星\visualdesktop\融合平台客户端\output", "Techstar.Framework.dll", SearchOption.AllDirectories);
            var readerParameter = new ReaderParameters()
            {
                AssemblyResolver = new CustomAssemblyResolver(
                    filename =>
                    {
                        string[] findFiles = Directory.GetFiles(@"F:\Work\启明星\visualdesktop\融合平台客户端\output", string.Format("{0}.dll", filename), SearchOption.AllDirectories);
                        if (findFiles.Length > 0)
                        {
                            try
                            {
                                return AssemblyDefinition.ReadAssembly(findFiles[0]);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("失败：" + findFiles[0]);
                            }
                        }
                        return null;
                    })
            };
            foreach (var assemblyFileName in files)
            {
                try
                {
                    AssemblyDefinition assemblyDefinition = AssemblyDefinition.ReadAssembly(assemblyFileName, readerParameter);
                    AssemblyNameDefinition assemblyNameDefinition = assemblyDefinition.Name;
                    assemblyNameDefinition.Hash = new Byte[0];
                    assemblyNameDefinition.PublicKey = new Byte[0];
                    assemblyNameDefinition.PublicKeyToken = new Byte[0];
                    assemblyNameDefinition.HasPublicKey = false;
                    assemblyNameDefinition.HashAlgorithm = AssemblyHashAlgorithm.None;
                    foreach (ModuleDefinition moduleDefinition in assemblyDefinition.Modules)
                    {
                        foreach (AssemblyNameReference assemblyNameReference in moduleDefinition.AssemblyReferences)
                        {
                            //Console.WriteLine(assemblyNameReference.FullName);
                        }
                        moduleDefinition.Attributes &= ~ModuleAttributes.StrongNameSigned;
                    }
                    assemblyDefinition.MainModule.Attributes &= ~ModuleAttributes.StrongNameSigned;

                    MemoryStream ms = new MemoryStream();
                    assemblyDefinition.Write(ms);
                    Assembly.Load(ms.ToArray());

                    Console.WriteLine(assemblyFileName);
                }
                catch (Exception ex )
                {
                    Console.WriteLine("失败：" + assemblyFileName);
                }
            
            }
      


        }
        #endregion

        #region PathConvert测试

        /// <summary>
        ///
        ///</summary>
        [TestMethod()]
        public void PathConvertTest()
        {
            var path1 = @"..\..\..\Sample\WPF\WpfLeftMenu\bin\Debug\WpfLeftMenu.dll";
            var path2 = @"F:\Work\启明星\OSGi.NET\Tool\OsgiDllTool\Release\";

            Uri u1 = new Uri(path2);
            Uri u2 = new Uri(u1, path1);
            string absolutePath = u2.LocalPath;

            Console.WriteLine(absolutePath);
        }

        #endregion

        #region LoadGAC测试

        /// <summary>
        ///
        ///</summary>
        [TestMethod()]
        public void LoadGacTest()
        {
            string assemblyName = @"System.EnterpriseServices, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";
            Assembly assembly = Assembly.Load(assemblyName);
            foreach (var t in assembly.GetExportedTypes())
            {
                Console.WriteLine(t.FullName);
            }
        }

        #endregion
    }
}
