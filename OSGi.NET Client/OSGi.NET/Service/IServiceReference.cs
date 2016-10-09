using System;
using System.Collections.Generic;
using System.Text;

using OSGi.NET.Core;

namespace OSGi.NET.Service
{
    /// <summary>
    /// 服务引用，指向服务的引用，它们并不是真正的服务对象
    /// </summary>
    public interface IServiceReference : IComparable
    {
        /// <summary>
        /// 返回公布服务的模块
        /// </summary>
        /// <returns>Bundle模块</returns>
        IBundle GetBundle();

        /// <summary>
        /// 获取服务属性值
        /// </summary>
        /// <param name="key">属性Key</param>
        /// <returns>属性值</returns>
        object GetProperty(string key);

        /// <summary>
        /// 获取服务属性Key数组
        /// </summary>
        /// <returns>Key数组</returns>
        string[] GetPropertyKeys();

        /// <summary>
        /// 设置服务属性
        /// </summary>
        /// <param name="propertyDictionary">服务属性字典</param>
        void SetProperties(IDictionary<string, object> propertyDictionary);

        /// <summary>
        /// 获取服务实例
        /// </summary>
        /// <returns>服务实例</returns>
        object GetService();

        /// <summary>
        /// 获取服务约束数组
        /// </summary>
        /// <returns>约束数组</returns>
        string[] GetSercieContracts();

        /// <summary>
        /// 获取正在使用指定服务的所有Bundle模块
        /// </summary>
        /// <returns>Bundle模块列表</returns>
        IList<IBundle> GetUsingBundles();

    }
}
