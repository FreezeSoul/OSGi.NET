using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OSGi.NET.Service;

namespace OSGi.NET.Core.Root
{
    /// <summary>
    /// 框架内核服务接口
    /// </summary>
    interface IFrameworkService
    {
        /// <summary>
        /// 注册一个公开的服务对象
        /// </summary>
        /// <param name="bundleContext">Bundle上下文</param>
        /// <param name="contracts">服务约束</param>
        /// <param name="service">服务对象</param>
        /// <param name="properties">服务属性</param>
        /// <returns>服务注册信息</returns>
        IServiceRegistration RegisterService(IBundleContext bundleContext, string[] contracts, object service, IDictionary<string, object> properties);

        /// <summary>
        /// 取消注册公开的服务对象
        /// </summary>
        /// <param name="serviceReference">服务引用</param>
        void UnRegisterService(IServiceReference serviceReference);

        /// <summary>
        /// 根据服务约束获取服务引用
        /// </summary>
        /// <param name="contract">服务约束</param>
        /// <returns>服务引用</returns>
        IServiceReference GetServiceReference(string contract);

        /// <summary>
        /// 获取正在使用指定服务的所有Bundle模块
        /// </summary>
        /// <param name="reference">服务引用</param>
        /// <returns>正在使用服务的Bundle列表</returns>
        IList<IBundle> GetUsingBundles(IServiceReference reference);

        /// <summary>
        /// 根据服务引用获取对应的服务实例
        /// </summary>
        /// <param name="reference">服务引用</param>
        /// <param name="bundle">调用服务的Bundle</param>
        /// <returns>服务实例</returns>
        object GetService(IServiceReference reference, IBundle bundle);

        /// <summary>
        /// 取消调用指定服务引用的服务实例
        /// </summary>
        /// <param name="reference">服务引用</param>
        /// <param name="bundle">调用服务的Bundle</param>
        /// <returns>是否成功</returns>
        bool UnGetService(IServiceReference reference, IBundle bundle);

        /// <summary>
        /// 通过服务约束获取服务引用列表
        /// </summary>
        /// <param name="contract">服务约束</param>
        /// <returns>服务引用列表</returns>
        IList<IServiceReference> GetServiceReferences(string contract);


        /// <summary>
        /// 通过服务约束和服务属性获取服务引用列表
        /// </summary>
        /// <param name="contract">服务约束</param>
        /// <param name="properties">服务属性</param>
        /// <returns>服务引用列表</returns>
        IList<IServiceReference> GetServiceReferences(string contract, IDictionary<string, object> properties);

    }
}
