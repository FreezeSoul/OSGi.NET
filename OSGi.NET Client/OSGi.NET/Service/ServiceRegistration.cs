using System;
using System.Collections.Generic;
using System.Text;

using OSGi.NET.Core;
using OSGi.NET.Core.Root;

namespace OSGi.NET.Service
{
  
    /// <summary>
    /// 服务注册信息，用于更新服务属性，反注册服务
    /// </summary>
    internal class ServiceRegistration : IServiceRegistration
    {
        /// <summary>
        /// 内核框架
        /// </summary>
        private IFramework framework;
        /// <summary>
        /// 暴露服务的Bundle上下文
        /// </summary>
        private IBundleContext bundleContext;
        /// <summary>
        /// 服务引用
        /// </summary>
        private ServiceReference reference;

        /// <summary>
        /// 服务注册信息
        /// </summary>
        /// <param name="framework">内核框架</param>
        /// <param name="bundleContext">暴露服务的Bundle上下文</param>
        /// <param name="reference">服务引用</param>
        public ServiceRegistration(IFramework framework, IBundleContext bundleContext, ServiceReference reference)
        {
            this.framework = framework;
            this.bundleContext = bundleContext;
            this.reference = reference;
        }

        /// <summary>
        /// 获取服务引用
        /// </summary>
        /// <returns>服务引用</returns>
        public IServiceReference GetReference()
        {
            return reference;
        }

        /// <summary>
        /// 设置服务属性
        /// </summary>
        /// <param name="properties">服务属性字典</param>
        public void SetProperties(IDictionary<string, object> properties)
        {
            reference.SetProperties(properties);
        }

        /// <summary>
        /// 取消注册公开的服务对象
        /// </summary>
        public void Unregister()
        {
            bundleContext.UnRegisterService(reference);
        }
    }
}
