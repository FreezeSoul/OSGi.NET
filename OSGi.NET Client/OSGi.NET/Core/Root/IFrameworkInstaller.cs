using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OSGi.NET.Core.Root
{

    /// <summary>
    /// 框架内核安装器接口
    /// </summary>
    interface IFrameworkInstaller
    {

        /// <summary>
        /// 安装指定Bundle模块
        /// </summary>
        /// <param name="zipFile">Bundle文件全路径</param>
        /// <returns>已安装的Bundle对象实例</returns>
        IBundle Install(string zipFile);

        /// <summary>
        /// 卸载指定Bundle模块
        /// </summary>
        /// <param name="bundle">Bundle实例对象</param>
        void UnInstall(IBundle bundle);

        /// <summary>
        /// 删除指定Bundle模块
        /// </summary>
        /// <param name="bundle">Bundle实例对象</param>
        void Delete(IBundle bundle);
    }
}
