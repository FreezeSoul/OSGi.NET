using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using OSGi.NET.Core;

namespace OSGi.NET.Extension
{
    /// <summary>
    /// 扩展数据实体
    /// </summary>
    public class ExtensionData
    {
        /// <summary>
        /// 扩展点名称
        /// </summary>
        public string Name { set; get; }

        /// <summary>
        /// 扩展数据节点集合
        /// </summary>
        public IList<XmlNode> ExtensionList { set; get; }

        /// <summary>
        /// 拥有Bundle
        /// </summary>
        public IBundle Owner { set; get; }
    }
}
