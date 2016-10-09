using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

using OSGi.NET.Core;
using OSGi.NET.Event;
using OSGi.NET.Service;
using WpfServiceContract;

namespace WpfTopMenuContent
{
    public class BundleActivator : IBundleActivator
    {

        public static IAttachContent AttachContentService;
        private IServiceRegistration serviceRegistration;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void Start(IBundleContext context)
        {
            var serviceRef = context.GetServiceReference<IAttachContent>();
            AttachContentService = context.GetService<IAttachContent>(serviceRef);

            serviceRegistration = context.RegisterService<IMenuItemEvent>(new MenuItemEvent());
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void Stop(IBundleContext context)
        {
            serviceRegistration.Unregister();
        }
    }
}
