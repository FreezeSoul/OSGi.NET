using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.IO;
using System.Runtime.Remoting.Messaging;
using System.Xml;

using Mono.Cecil;

using log4net;
using OSGi.NET.Event;
using OSGi.NET.Extension;
using OSGi.NET.Provider;

using OSGi.NET.Listener;
using OSGi.NET.Service;
using OSGi.NET.Utils;

namespace OSGi.NET.Core.Root
{

    /// <summary>
    /// 框架内核，内核也作为一个Bundle，用来启动其他Bundle组件
    /// </summary>
    internal class Framework : IFramework, IFrameworkFireEvent, IFrameworkService, IFrameworkListener, IFrameworkInstaller
    {

        #region Property & Field

        /// <summary>
        /// 框架锁,避免多实例启动
        /// </summary>
        private const string FRAMEWORK_LOCK_FILE = "lock";

        /// <summary>
        /// 日志对象
        /// </summary>
        private static ILog log = LogManager.GetLogger(typeof(Framework));

        /// <summary>
        /// Bundle上下文对象
        /// </summary>
        private IBundleContext bundleContext;

        /// <summary>
        /// Bundle目录
        /// </summary>
        private string bundlesDirectoryPath;

        /// <summary>
        /// 共享程序集目录
        /// </summary>
        private string shareLibsDirectoryPath;

        /// <summary>
        /// Bundle卸载存储目录
        /// </summary>
        private string bundlesUninstallPath;

        /// <summary>
        /// 所有插件包
        /// </summary>
        private List<IBundle> bundleList = new List<IBundle>();

        /// <summary>
        /// 扩展点
        /// </summary>
        private IList<ExtensionPoint> extensionPoints;

        /// <summary>
        /// 扩展数据
        /// </summary>
        private IList<ExtensionData> extensionDatas;

        /// <summary>
        /// bundle监听器集合
        /// </summary>
        private IList<IBundleListener> bundleListenerList = new List<IBundleListener>();

        /// <summary>
        /// Extension监听器集合
        /// </summary>
        private IList<IExtensionListener> extensionListenerList = new List<IExtensionListener>();

        /// <summary>
        /// 服务监听器集合
        /// </summary>
        private IList<IServiceListener> serviceListenerList = new List<IServiceListener>();

        /// <summary>
        /// 已注册的契约服务
        /// </summary>
        private IDictionary<string, IList<IServiceReference>> serviceReferenceDictionary = new Dictionary<string, IList<IServiceReference>>();

        /// <summary>
        /// 引用服务的模块
        /// </summary>
        private IDictionary<IServiceReference, IList<IBundle>> usingServiceBundleDict = new Dictionary<IServiceReference, IList<IBundle>>();

        /// <summary>
        /// Framework元数据字典
        /// </summary>
        private IDictionary<string, string> metaDataDictionary;

        /// <summary>
        /// framework程序集全名
        /// </summary>
        private string frameworkAssemblyFullName;

        /// <summary>
        /// framework符号名称
        /// </summary>
        private string frameworkSymbolicName = "<未加载>";

        /// <summary>
        /// framework版本
        /// </summary>
        private Version frameworkVersion;

        /// <summary>
        /// 框架锁文件流
        /// </summary>
        private FileStream lockFileStream;

        /// <summary>
        /// 状态，内核默认状态已装载
        /// </summary>
        private int state = BundleStateConst.RESOLVED;

        #endregion

        #region Constructor

        /// <summary>
        /// 内核构造
        /// </summary>
        public Framework()
        {
            this.bundleContext = new BundleContext(this, this);

            //读取元数据信息
            this.LoadMetaData();
        }

        /// <summary>
        /// 扩展点构造重载
        /// </summary>
        /// <param name="extensionPoints"></param>
        public Framework(IList<ExtensionPoint> extensionPoints)
            : this()
        {
            extensionPoints.ToList().ForEach(point => point.Owner = this);
            this.extensionPoints = extensionPoints;
        }

        /// <summary>
        /// 扩展数据构造重载
        /// </summary>
        /// <param name="extensionDatas"></param>
        public Framework(IList<ExtensionData> extensionDatas)
            : this()
        {
            extensionDatas.ToList().ForEach(data => data.Owner = this);
            this.extensionDatas = extensionDatas;
        }
        #endregion

        #region Method

        #region Init
        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
            log.Debug("框架初始化开始！");

            Assembly selfAssembly = typeof(Framework).Assembly;

            //将自身Bundle程序集提供程序
            BundleAssemblyProvider.AddAssembly(selfAssembly.GetName().Name, selfAssembly);

            //读取配置文件
            this.ReadConfig();

            //将自身加入Bundle集合
            this.bundleList.Add(this);

            //加载共享程序集目录dll
            this.LoadShareAssemblys();

            //发现目录中的Bundle集合
            this.DiscoverBundles();

            log.Debug("框架初始化结束！");

            this.state = BundleStateConst.RESOLVED;

            this.FireBundleEvent(new BundleEventArgs(BundleEventArgs.RESOLVED, this));

        }


        /// <summary>
        /// 读取元数据信息
        /// </summary>
        private void LoadMetaData()
        {
            log.Debug("框架读取元数据信息！");

            metaDataDictionary = new Dictionary<string, string>();
            var assemblyResolver = new AssemblyResolver();
            assemblyResolver.Init(typeof(Framework).Assembly);
            this.frameworkSymbolicName = assemblyResolver.GetAssemblyName();
            this.frameworkAssemblyFullName = assemblyResolver.GetAssemblyFullName();
            metaDataDictionary.Add(BundleConst.BUNDLE_MANIFEST_SYMBOLIC_NAME, frameworkSymbolicName);
            this.frameworkVersion = assemblyResolver.GetVersion();
            metaDataDictionary.Add(BundleConst.BUNDLE_MANIFEST_VERSION, frameworkVersion.ToString());
            metaDataDictionary.Add(BundleConst.BUNDLE_MANIFEST_NAME, assemblyResolver.GetAssemblyTitle());
            metaDataDictionary.Add(BundleConst.BUNDLE_MANIFEST_VENDOR, assemblyResolver.GetVendor());
            metaDataDictionary.Add(BundleConst.BUNDLE_MANIFEST_REQUIRE_BUNDLE, assemblyResolver.GetAssemblyRequiredAssembly());
            assemblyResolver = null;
        }

        /// <summary>
        /// 读取配置信息
        /// </summary>
        private void ReadConfig()
        {
            log.Debug("框架读取配置！");


            bundlesDirectoryPath = BundleConfigProvider.GetBundlesDirectory();

            shareLibsDirectoryPath = BundleConfigProvider.GetShareLibsDirectory();

            bundlesUninstallPath = BundleConfigProvider.GetBundlesUninstallDirectory();
        }



        /// <summary>
        /// 从共享程序集LIB目录和根目录中配置项加载程序集
        /// </summary>
        /// <returns></returns>
        private void LoadShareAssemblys()
        {
            log.Debug("框架加载共享程序集！");

            if (Directory.Exists(shareLibsDirectoryPath))
            {
                var files = Directory.GetFiles(shareLibsDirectoryPath, "*.*", SearchOption.AllDirectories)
                            .Where(s => s.ToLower().EndsWith(".dll") || s.ToLower().EndsWith(".exe")).ToList();
                foreach (var file in files)
                {
                    LoadShareAssemblyByReflect(file);
                }
            }


            var shareLibs = BundleConfigProvider.GetConfigRootShareDll();
            foreach (var shareLib in shareLibs)
            {
                var shareLibFile = Path.Combine(Environment.CurrentDirectory, shareLib);
                if (File.Exists(shareLibFile))
                    LoadShareAssemblyByReflect(shareLibFile);
            }

            DomainAssemblyInsertShareLib();
        }

        /// <summary>
        /// 通过反射加载程序集
        /// </summary>
        /// <param name="assemblyFileName"></param>
        /// <returns></returns>
        private void LoadShareAssemblyByReflect(string assemblyFileName)
        {
            try
            {
                AssemblyDefinition assemblyDefinition = AssemblyDefinition.ReadAssembly(assemblyFileName);
                if (!BundleAssemblyProvider.CheckHasShareLib(assemblyDefinition.FullName))
                {
                    Assembly assembly = AppDomain.CurrentDomain.GetAssemblies().ToList()
                        .FirstOrDefault(item => item.FullName == assemblyDefinition.FullName);

                    if (assembly == null)
                    {
                        if (BundleConfigProvider.OSGi_NET_IS_DEBUG_MODE)
                            assembly = Assembly.LoadFrom(assemblyFileName);
                        else
                            assembly = Assembly.Load(File.ReadAllBytes(assemblyFileName));
                    }

                    if (BundleConfigProvider.OSGi_NET_ALLTYPES_LOAD)
                        assembly.GetTypes();

                    log.Debug(string.Format("框架加载共享程序集[{0}]！", assembly.GetName().Name));

                    BundleAssemblyProvider.AddShareAssembly(assembly.FullName, assembly);
                }
            }
            catch (Exception ex)
            {
                log.Error(string.Format("加载共享程序集[{0}]时出现异常！", assemblyFileName), ex);
            }
        }


        /// <summary>
        /// 将domain程序集插入共享程序集
        /// </summary>
        private void DomainAssemblyInsertShareLib()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
            foreach (var assembly in assemblies)
            {
                if (!BundleUtils.IsAssemblyBelongsFcl(assembly.GetName().Name))
                {
                    if (!BundleAssemblyProvider.CheckHasShareLib(assembly.FullName))
                    {
                        log.Debug(string.Format("框架关联共享程序集[{0}]！", assembly.GetName().Name));

                        BundleAssemblyProvider.AddShareAssembly(assembly.FullName, assembly);
                    }
                }
            }
        }

        /// <summary>
        /// 搜索发现Bundles
        /// </summary>
        private void DiscoverBundles()
        {
            log.Debug("框架读取模块信息！");

            BundleConfigProvider.LoadBundlesConfig(bundlesDirectoryPath);

            foreach (var bundleConfig in BundleConfigProvider.BundlesConfiguration)
            {
                //Bundle目录
                var bundleKey = bundleConfig.Key;
                //Bundle配置节点
                var bundleConfigData = bundleConfig.Value;
                string bundleDirectoryName = Path.Combine(bundlesDirectoryPath, bundleKey);
                try
                {
                    IBundle bundle = new Bundle(this, bundleDirectoryName, bundleConfigData);
                    if (bundleList.Exists(b => b.Equals(bundle)))
                    {
                        log.Warn(string.Format("插件[{0})]存在多重安装，只加载第一个！", bundleKey));
                        continue;
                    }
                    bundleList.Add(bundle);
                }
                catch (Exception ex)
                {
                    log.Error("加载Bundle时出现异常！", ex);
                }
            }

        }


        #endregion

        #region Start

        /// <summary>
        /// 启动
        /// </summary>
        public void Start()
        {
            log.Debug("框架启动开始！");

            //锁定文件
            if (BundleConfigProvider.OSGi_NET_SINGLERUNNING)
            {
                try
                {
                    string lockFileName = Path.Combine(bundlesDirectoryPath, FRAMEWORK_LOCK_FILE);
                    lockFileStream = File.Open(lockFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
                }
                catch (Exception ex)
                {
                    throw new IOException("锁定Bundle缓存目录失败，请确认没有另外的OSGi.NET实例正在运行或者当前用户有访问此目录的权限。", ex);
                }

            }
            
            state = BundleStateConst.STARTING;

            this.FireBundleEvent(new BundleEventArgs(BundleEventArgs.STARTING, this));

            foreach (IBundle bundle in bundleList)
            {
                if (bundle.Equals(this)) continue;

                if (bundle.GetState() == BundleStateConst.INSTALLED
                    || bundle.GetState() == BundleStateConst.RESOLVED)
                {
                    try
                    {
                        bundle.Start();
                    }
                    catch (Exception ex)
                    {
                        log.Error(string.Format("启动插件[{0}]时出现异常", bundle), ex);
                    }
                }
            }

            state = BundleStateConst.ACTIVE;

            this.FireBundleEvent(new BundleEventArgs(BundleEventArgs.STARTED, this));

            log.Debug("框架启动完成！");
        }

        #endregion

        #region Stop

        /// <summary>
        /// 停止
        /// </summary>
        public void Stop()
        {
            log.Debug("框架停止开始！");

            if (lockFileStream != null) lockFileStream.Close();
            state = BundleStateConst.STOPPING;

            this.FireBundleEvent(new BundleEventArgs(BundleEventArgs.STOPPING, this));

            foreach (IBundle bundle in bundleList)
            {
                if (bundle.Equals(this)) continue;
                if (bundle.GetState() == BundleStateConst.ACTIVE)
                {
                    bundle.Stop();
                }
            }

            state = BundleStateConst.RESOLVED;

            this.FireBundleEvent(new BundleEventArgs(BundleEventArgs.RESOLVED, this));

            log.Debug("框架停止完成！");

        }



        #region Install

        /// <summary>
        /// 安装指定Bundle模块
        /// </summary>
        /// <param name="zipFile">Bundle文件全路径</param>
        /// <returns>已安装的Bundle对象实例</returns>
        public IBundle Install(string zipFile)
        {

            var folderName = Path.GetFileNameWithoutExtension(zipFile);
            var newBundlePath = Path.Combine(bundlesDirectoryPath, folderName ?? string.Empty);

            log.Debug(string.Format("模块[{0}]安装开始！", folderName));

            //如果存在同名改名
            if (Directory.Exists(newBundlePath))
            {
                folderName = string.Format("{0}_{1}", folderName, Guid.NewGuid().ToString());
                newBundlePath = Path.Combine(bundlesDirectoryPath, folderName);
            }

            BundleUtils.ExtractBundleFile(newBundlePath, zipFile);

            //读取bundle配置信息
            var xmlNode = BundleConfigProvider.ReadBundleConfig(newBundlePath);

            try
            {
                var assemblyNameNode = xmlNode.Attributes == null ? null : xmlNode.Attributes["AssemblyName"];
                var tmpassemblyName = assemblyNameNode == null ? string.Empty : assemblyNameNode.Value;
                foreach (IBundle installedBundle in bundleList)
                {
                    //如果此插件的相同版本已经安装
                    var assemblyName = installedBundle.GetBundleAssemblyFileName();
                    if (assemblyName.Equals(tmpassemblyName))
                    {
                        throw new Exception(string.Format("名称为[{0}]的插件已经存在，安装失败！", installedBundle.GetSymbolicName()));
                    }
                }

                //添加Bundle配置信息
                BundleConfigProvider.SyncBundleConfig(folderName, xmlNode);

                IBundle bundle = new Bundle(this, newBundlePath, xmlNode);
                bundleList.Add(bundle);

                log.Debug(string.Format("模块[{0}]安装完成！", folderName));

                return bundle;
            }
            catch (Exception ex)
            {
                log.Error("指定模块安装失败！", ex);
                Directory.Delete(newBundlePath, true);
                throw;
            }
        }

        #endregion

        #region Uninstall

        /// <summary>
        /// 卸载当前Bundle
        /// </summary>
        public void UnInstall()
        {
            throw new NotSupportedException("框架模块自身不支持卸载");
        }

        /// <summary>
        /// 卸载指定Bundle模块
        /// </summary>
        /// <param name="bundle">Bundle实例对象</param>
        public void UnInstall(IBundle bundle)
        {
            bundleList.Remove(bundle);
            this.Delete(bundle);
        }

        /// <summary>
        /// 删除指定Bundle模块，默认实现备份至配置目录
        /// </summary>
        /// <param name="bundle">Bundle实例对象</param>
        public void Delete(IBundle bundle)
        {
            log.Debug(string.Format("卸载模块备份至[{0}]目录！", bundlesUninstallPath));

            var bundlePath = bundle.GetBundleDirectoryPath();
            var newbundlePath = string.Format("{0}_{1}", new DirectoryInfo(bundlePath).Name, DateTime.Now.Ticks);
            if (!Directory.Exists(bundlesUninstallPath)) Directory.CreateDirectory(bundlesUninstallPath);
            Directory.Move(bundle.GetBundleDirectoryPath(), Path.Combine(bundlesUninstallPath, newbundlePath));
        }

        #endregion

        #region Update

        /// <summary>
        /// 指定路径更新当前Bundle
        /// </summary>
        /// <param name="zipFile">更新的Bundle路径</param>
        public void Update(string zipFile)
        {
            throw new NotSupportedException("框架模块自身不支持更新");
        }
        #endregion


        #endregion

        #region Public

        /// <summary>
        /// 获取当前Bundle上下文对象
        /// </summary>
        /// <returns>Bundle上下文对象</returns>
        public IBundleContext GetBundleContext()
        {
            return this.bundleContext;
        }


        /// <summary>
        /// 获取当前Bundle版本信息
        /// </summary>
        /// <returns>Bundle版本信息</returns>
        public Version GetVersion()
        {
            return this.frameworkVersion;
        }

        /// <summary>
        /// 获取当前Bundle符号名称
        /// </summary>
        /// <returns>Bundle符号名称</returns>
        public string GetSymbolicName()
        {
            return this.frameworkSymbolicName;
        }

        /// <summary>
        /// 获取当前Bundle程序集全名
        /// </summary>
        /// <returns>Bundle程序集全名</returns>
        public string GetBundleAssemblyFullName()
        {
            return this.frameworkAssemblyFullName;
        }

        /// <summary>
        /// 获取当前Bundle程序集清单数据
        /// </summary>
        /// <returns>Bundle程序清单数据</returns>
        public IDictionary<string, string> GetManifest()
        {
            return new Dictionary<string, string>(metaDataDictionary);
        }

        /// <summary>
        /// 获取所有Bundle模块
        /// </summary>
        /// <returns>所有已装载Bundle列表</returns>
        public IList<IBundle> GetBundles()
        {
            var bundles = new IBundle[bundleList.Count];
            bundleList.CopyTo(bundles, 0);
            return bundles.ToList();
        }

        /// <summary>
        /// 获取当前Bundle状态
        /// </summary>
        /// <returns>Bundle状态</returns>
        public int GetState()
        {
            return this.state;
        }

        /// <summary>
        /// 获取当前Bundle程序集文件名称
        /// </summary>
        /// <returns>Bundle程序集文件名称</returns>
        public string GetBundleAssemblyFileName()
        {
            return GetSymbolicName();
        }


        /// <summary>
        /// 获取当前Bundle启动级别
        /// </summary>
        /// <returns>Bundle启动级别</returns>
        public int GetBundleStartLevel()
        {
            return 0;
        }

        /// <summary>
        /// 获取当前Bundle目录
        /// </summary>
        /// <returns>Bundle目录</returns>
        public string GetBundleDirectoryPath()
        {
            return Environment.CurrentDirectory;
        }


        /// <summary>
        /// 获取当前Bundle扩展点
        /// </summary>
        /// <returns>扩展点列表</returns>
        public IList<ExtensionPoint> GetExtensionPoints()
        {
            if (extensionPoints == null)
                return new List<ExtensionPoint>();

            var tmpExtensionPoints = new ExtensionPoint[extensionPoints.Count];
            extensionPoints.CopyTo(tmpExtensionPoints, 0);
            return tmpExtensionPoints.ToList();
        }

        /// <summary>
        /// 获取当前Bundle扩展的扩展点数据
        /// </summary>
        /// <returns>扩展点数据列表</returns>
        public IList<ExtensionData> GetExtensionDatas()
        {
            if (extensionDatas == null)
                return new List<ExtensionData>();

            var tmpExtensionDatas = new ExtensionData[extensionDatas.Count];
            extensionDatas.CopyTo(tmpExtensionDatas, 0);
            return tmpExtensionDatas.ToList();
        }

        /// <summary>
        /// 获取模块清单数据
        /// </summary>
        /// <returns>清单数据节点</returns>
        public XmlNode GetBundleManifestData()
        {
            throw new NotSupportedException("框架模块自身不存在清单信息");
        }

        #endregion

        #region BundleListener

        /// <summary>
        /// 添加一个Bundle监听器
        /// </summary>
        /// <param name="listener">Bundle监听器实例</param>
        public void AddBundleListener(IBundleListener listener)
        {
            bundleListenerList.Add(listener);
        }

        /// <summary>
        /// 移除一个Bundle监听器
        /// </summary>
        /// <param name="listener">Bundle监听器实例</param>
        public void RemoveBundleListener(IBundleListener listener)
        {
            bundleListenerList.Remove(listener);
        }


        /// <summary>
        /// 触发Bundle状态变更事件
        /// </summary>
        /// <param name="bundleEvent">Bundle事件参数</param>
        public void FireBundleEvent(BundleEventArgs bundleEvent)
        {
            foreach (IBundleListener listener in bundleListenerList)
            {
                listener.BundleChanged(bundleEvent);
            }
        }

        #endregion

        #region ExtensionListener

        /// <summary>
        /// 添加一个Extension监听器
        /// </summary>
        /// <param name="listener">Extension监听器实例</param>
        public void AddExtensionListener(IExtensionListener listener)
        {
            extensionListenerList.Add(listener);
        }

        /// <summary>
        /// 移除一个Extension监听器
        /// </summary>
        /// <param name="listener">Extension监听器实例</param>
        public void RemoveExtensionListener(IExtensionListener listener)
        {
            extensionListenerList.Remove(listener);
        }


        /// <summary>
        /// 触发Extension变更事件
        /// </summary>
        /// <param name="extensionEvent">Extension事件参数</param>
        public void FireExtensionEvent(ExtensionEventArgs extensionEvent)
        {
            foreach (IExtensionListener listener in extensionListenerList)
            {
                listener.ExtensionChanged(extensionEvent);
            }
        }
        #endregion

        #region ServiceListener

        /// <summary>
        /// 添加一个服务监听器
        /// </summary>
        /// <param name="listener">服务监听器实例</param>
        public void AddServiceListener(IServiceListener listener)
        {
            serviceListenerList.Add(listener);
        }

        /// <summary>
        /// 移除一个服务监听器
        /// </summary>
        /// <param name="listener">服务监听器</param>
        public void RemoveServiceListener(IServiceListener listener)
        {
            serviceListenerList.Remove(listener);
        }


        /// <summary>
        /// 触发服务变更事件
        /// </summary>
        /// <param name="serviceEvent">服务事件参数</param>
        public void FireServiceEvent(ServiceEventArgs serviceEvent)
        {
            foreach (IServiceListener listener in serviceListenerList)
            {
                listener.ServiceChanged(serviceEvent);
            }
        }

        #endregion

        #region Service Operation

        /// <summary>
        /// 注册一个公开的服务对象
        /// </summary>
        /// <param name="context">Bundle上下文</param>
        /// <param name="contracts">服务约束</param>
        /// <param name="service">服务对象</param>
        /// <param name="properties">服务属性</param>
        /// <returns>服务注册信息</returns>
        public IServiceRegistration RegisterService(IBundleContext context, string[] contracts, object service, IDictionary<string, object> properties)
        {
            var bundleContextImpl = context as BundleContext;

            var reference = new ServiceReference(bundleContextImpl, contracts, properties, service);
            foreach (string contract in contracts)
            {
                IList<IServiceReference> serviceReferenceList = null;
                if (serviceReferenceDictionary.ContainsKey(contract))
                {
                    serviceReferenceList = serviceReferenceDictionary[contract];
                }
                else
                {
                    serviceReferenceList = new List<IServiceReference>();
                    serviceReferenceDictionary.Add(contract, serviceReferenceList);
                }
                serviceReferenceList.Add(reference);
            }

            FireServiceEvent(new ServiceEventArgs(ServiceEventArgs.REGISTERED, contracts, reference));

            return new ServiceRegistration(this, bundleContextImpl, reference);
        }

        /// <summary>
        /// 取消注册公开的服务对象
        /// </summary>
        /// <param name="serviceReference">服务引用</param>
        public void UnRegisterService(IServiceReference serviceReference)
        {
            FireServiceEvent(new ServiceEventArgs(ServiceEventArgs.UNREGISTERING, serviceReference.GetSercieContracts(), serviceReference));

            foreach (string contract in serviceReferenceDictionary.Keys)
            {
                IList<IServiceReference> serviceReferenceList = serviceReferenceDictionary[contract];
                if (serviceReferenceList.Contains(serviceReference))
                {
                    serviceReferenceList.Remove(serviceReference);
                }
            }
            if (usingServiceBundleDict.ContainsKey(serviceReference))
            {
                usingServiceBundleDict.Remove(serviceReference);
            }
        }


        /// <summary>
        /// 通过服务约束获取服务引用列表
        /// </summary>
        /// <param name="contract">服务约束</param>
        /// <returns>服务引用列表</returns>
        public IList<IServiceReference> GetServiceReferences(string contract)
        {
            IList<IServiceReference> serviceReferenceList = new List<IServiceReference>();
            foreach (string tempContract in serviceReferenceDictionary.Keys)
            {
                if (!tempContract.Equals(contract)) continue;
                serviceReferenceList = serviceReferenceDictionary[contract];
            }
            return serviceReferenceList;
        }


        /// <summary>
        /// 通过服务约束和服务属性获取服务引用列表
        /// </summary>
        /// <param name="contract">服务约束</param>
        /// <param name="properties">服务属性</param>
        /// <returns>服务引用列表</returns>
        public IList<IServiceReference> GetServiceReferences(string contract, IDictionary<string, object> properties)
        {
            IList<IServiceReference> serviceReferenceList = new List<IServiceReference>();
            foreach (string tempContract in serviceReferenceDictionary.Keys)
            {
                if (!tempContract.Equals(contract)) continue;

                var sameContractRefDic = serviceReferenceDictionary[contract];

                foreach (var serviceReference in sameContractRefDic)
                {
                    foreach (var property in properties)
                    {
                        var serProperty = serviceReference.GetProperty(property.Key);
                        if (serProperty != null && serProperty.Equals(property.Value))
                        {
                            serviceReferenceList.Add(serviceReference);
                        }
                    }
                }
            }
            return serviceReferenceList;
        }


        /// <summary>
        /// 根据服务约束获取服务引用
        /// </summary>
        /// <param name="contract">服务约束</param>
        /// <returns>服务引用</returns>
        public IServiceReference GetServiceReference(string contract)
        {
            return this.GetServiceReferences(contract).FirstOrDefault();
        }



        /// <summary>
        /// 获取正在使用指定服务的所有Bundle模块
        /// </summary>
        /// <param name="reference">服务引用</param>
        /// <returns>正在使用服务的Bundle列表</returns>
        public IList<IBundle> GetUsingBundles(IServiceReference reference)
        {
            if (usingServiceBundleDict.ContainsKey(reference))
            {
                IList<IBundle> usingBundleList = usingServiceBundleDict[reference];
                var usingBundles = new IBundle[usingBundleList.Count];
                usingBundleList.CopyTo(usingBundles, 0);
                return usingBundles;
            }
            else
            {
                return new IBundle[0];
            }
        }

        /// <summary>
        /// 根据服务引用获取对应的服务实例
        /// </summary>
        /// <param name="reference">服务引用</param>
        /// <param name="bundle">调用服务的Bundle</param>
        /// <returns>服务实例</returns>
        public object GetService(IServiceReference reference, IBundle bundle)
        {
            ServiceReference sri = reference as ServiceReference;
            if (sri == null) return null;

            if (!usingServiceBundleDict.ContainsKey(reference))
            {
                usingServiceBundleDict.Add(reference, new List<IBundle>());
            }

            usingServiceBundleDict[reference].Add(bundle);

            return sri.GetService();
        }

        /// <summary>
        /// 取消调用指定服务引用的服务实例
        /// </summary>
        /// <param name="reference">服务引用</param>
        /// <param name="bundle">调用服务的Bundle</param>
        /// <returns>是否成功</returns>
        public bool UnGetService(IServiceReference reference, IBundle bundle)
        {
            if (usingServiceBundleDict.ContainsKey(reference))
            {
                return usingServiceBundleDict[reference].Remove(bundle);
            }
            return false;
        }

        #endregion

        #endregion


    }
}
