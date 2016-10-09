using System;
using System.Collections.Generic;
using System.Text;

namespace OSGi.NET.Service
{

    /// <summary>
    /// 服务注册信息，用于更新服务属性，反注册服务
    /// </summary>
    public interface IServiceRegistration
    {
        /// <summary>
        /// 获取服务引用
        /// </summary>
        /// <returns>服务引用</returns>
        IServiceReference GetReference();

        /// <summary>
        /// 设置服务属性
        /// </summary>
        /// <param name="properties">服务属性字典</param>
        void SetProperties(IDictionary<string, object> properties);


        /// <summary>
        /// 取消注册公开的服务对象
        /// </summary>
        void Unregister();
    }
}
