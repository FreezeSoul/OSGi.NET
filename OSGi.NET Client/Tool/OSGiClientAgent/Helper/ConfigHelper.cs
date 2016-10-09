using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using OSGi.NET.Core;

namespace OSGiClientAgent.Helper
{
    class ConfigHelper
    {
        /// <summary>
        /// 
        /// </summary>
        private IBundle _bundle;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bundle"></param>
        public ConfigHelper(IBundle bundle)
        {
            this._bundle = bundle;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nodeName"></param>
        /// <returns></returns>
        public string GetManifestConfigValue(string nodeName)
        {
            var manifestData = _bundle.GetBundleManifestData();
            var rValue = string.Empty;
            foreach (XmlNode childNode in manifestData.ChildNodes)
            {
                if (childNode.Name == nodeName)
                    if (childNode.Attributes != null)
                    {
                        rValue = childNode.Attributes["Value"].Value;
                    }
            }
            return rValue.Trim();
        }
    }
}
