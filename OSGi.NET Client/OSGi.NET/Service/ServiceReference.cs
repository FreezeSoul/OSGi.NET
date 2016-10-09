using System;
using System.Collections.Generic;
using System.Text;

using OSGi.NET.Core;

namespace OSGi.NET.Service
{
    /// <summary>
    /// 服务引用，指向服务的引用，它们并不是真正的服务对象
    /// </summary>
    internal class ServiceReference : IServiceReference
    {
        /// <summary>
        /// 服务对象
        /// </summary>
        private object service;
        /// <summary>
        /// 服务约束数组
        /// </summary>
        private string[] contracts;
        /// <summary>
        /// 暴露服务的Bundle上下文
        /// </summary>
        private IBundleContext bundleContext;
        /// <summary>
        /// 服务属性
        /// </summary>
        private IDictionary<string, object> properties;

        /// <summary>
        /// 服务引用
        /// </summary>
        /// <param name="bundleContext">暴露服务的Bundle上下文</param>
        /// <param name="contracts">服务约束数组</param>
        /// <param name="properties">服务属性</param>
        /// <param name="service">服务对象</param>
        public ServiceReference(IBundleContext bundleContext, string[] contracts, IDictionary<string, object> properties, object service)
        {
            this.bundleContext = bundleContext;
            this.contracts = contracts;
            this.properties = properties;
            this.service = service;
        }

        /// <summary>
        /// 返回公布服务的模块
        /// </summary>
        /// <returns>Bundle模块</returns>
        public IBundle GetBundle()
        {
            return bundleContext.GetBundle();
        }

        /// <summary>
        /// 获取服务属性值
        /// </summary>
        /// <param name="key">属性Key</param>
        /// <returns>属性值</returns>
        public object GetProperty(string key)
        {
            if (properties.ContainsKey(key))
                return properties[key];
            return null;
        }

        /// <summary>
        /// 获取服务属性Key数组
        /// </summary>
        /// <returns>Key数组</returns>
        public string[] GetPropertyKeys()
        {
            var propertyKeys = new string[properties.Count];
            properties.Keys.CopyTo(propertyKeys, 0);
            return propertyKeys;
        }

        /// <summary>
        /// 设置服务属性
        /// </summary>
        /// <param name="propertyDictionary">服务属性字典</param>
        public void SetProperties(IDictionary<string, object> propertyDictionary)
        {
            this.properties = propertyDictionary;
        }

        /// <summary>
        /// 获取服务实例
        /// </summary>
        /// <returns>服务实例</returns>
        public object GetService()
        {
            return service;
        }

        /// <summary>
        /// 获取服务约束数组
        /// </summary>
        /// <returns>约束数组</returns>
        public string[] GetSercieContracts()
        {
            var tmpContracts = new string[contracts.Length];
            contracts.CopyTo(tmpContracts, 0);
            return tmpContracts;
        }

        /// <summary>
        /// 获取正在使用指定服务的所有Bundle模块
        /// </summary>
        /// <returns>Bundle模块列表</returns>
        public IList<IBundle> GetUsingBundles()
        {
            return bundleContext.GetUsingBundles(this);
        }

        /// <summary>
        /// 对比服务是否相等
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int CompareTo(object obj)
        {
            return GetHashCode().CompareTo(obj.GetHashCode());
        }
    }
}
