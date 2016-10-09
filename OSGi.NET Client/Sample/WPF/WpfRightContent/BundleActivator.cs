using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

using OSGi.NET.Core;
using OSGi.NET.Event;
using OSGi.NET.Service;
using WpfServiceContract;

namespace WpfRightContent
{
    public class BundleActivator : IBundleActivator
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void Start(IBundleContext context)
        {
            var serviceRef = context.GetServiceReference<IAttachContent>();
            var service = context.GetService<IAttachContent>(serviceRef);

            service.AttachUcContent(new UserControl1());
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void Stop(IBundleContext context)
        {
        }
    }
}
