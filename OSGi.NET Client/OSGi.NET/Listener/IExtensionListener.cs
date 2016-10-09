using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OSGi.NET.Event;

namespace OSGi.NET.Listener
{
    /// <summary>
    /// Bundle扩展点监听器
    /// </summary>
    public interface IExtensionListener
    {
        /// <summary>
        /// Bundle扩展点变更
        /// </summary>
        /// <param name="e">扩展点事件参数</param>
        void ExtensionChanged(ExtensionEventArgs e);
    }
}
