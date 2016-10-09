using System;
using System.Collections.Generic;
using System.Text;

using OSGi.NET.Service;

namespace OSGi.NET.Event
{
    /// <summary>
    /// 服务事件参数
    /// </summary>
    public class ServiceEventArgs : EventArgs
    {
        /// <summary>
        /// 注册服务
        /// </summary>
        public const int REGISTERED = 0x00000001;
        //public const int MODIFIED = 0x00000002;
        /// <summary>
        /// 取消注册
        /// </summary>
        public const int UNREGISTERING = 0x00000004;
        //public const int MODIFIED_ENDMATCH = 0x00000008;

        /// <summary>
        /// 状态
        /// </summary>
        private int state;

        /// <summary>
        /// 服务约束
        /// </summary>
        private string[] contracts;

        /// <summary>
        /// 服务引用
        /// </summary>
        private IServiceReference reference;

        /// <summary>
        /// 服务事件参数
        /// </summary>
        /// <param name="state">服务状态</param>
        /// <param name="contracts">服务约束</param>
        /// <param name="reference">服务引用</param>
        internal ServiceEventArgs(int state, string[] contracts, IServiceReference reference)
        {
            this.state = state;
            this.contracts = contracts;
            this.reference = reference;
        }

        /// <summary>
        /// 获取服务引用
        /// </summary>
        /// <returns></returns>
        public IServiceReference GetServiceReference()
        {
            return reference;
        }

        /// <summary>
        /// 获取服务约束数组
        /// </summary>
        /// <returns></returns>
        public string[] GetSercieContracts()
        {
            var tmpContracts = new string[contracts.Length];
            contracts.CopyTo(tmpContracts, 0);
            return tmpContracts;
        }

        /// <summary>
        /// 获取服务变更状态
        /// </summary>
        /// <returns></returns>
        public int GetState()
        {
            return state;
        }

    }
}
