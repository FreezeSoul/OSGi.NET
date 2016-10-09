using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OSGi.NET.Core;
using OSGi.NET.Event;
using OSGi.NET.Service;

using ServiceContract;

namespace BundleDemoA
{
    public class BundleActivator : IBundleActivator
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private IServiceRegistration serviceRegistration;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void Start(IBundleContext context)
        {
            context.ServiceChanged += contextServiceChanged;
            context.ExtensionChanged += ContextOnExtensionChanged;
            serviceRegistration = context.RegisterService<IHelloWord>(new BundleAHelloWord());

            context.RegisterService<IMutilImplement>(new MutilImplementA(),
               new Dictionary<string, object>
                {
                    {"id","1"}
                });

            context.RegisterService<IMutilImplement>(new MutilImplementB(),
            new Dictionary<string, object>
                {
                    {"id","2"}
                });

            context.RegisterService<IMutilImplement>(new MutilImplementC(),
            new Dictionary<string, object>
                {
                    {"id","3"}
                });

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ContextOnExtensionChanged(object sender, ExtensionEventArgs e)
        {
            var stateStr = string.Empty;
            if (e.GetState() == ExtensionEventArgs.LOAD)
            {
                stateStr = "Load";
            }
            else if (e.GetState() == ExtensionEventArgs.UNLOAD)
            {
                stateStr = "UnLoad";
            }
            var extensionStr = new StringBuilder();
            foreach (var xmlNode in e.GetExtensionData().ExtensionList)
            {
                extensionStr.Append(xmlNode.InnerXml);
            }
            log.Info(string.Format("{0} {1} {2} Extension {3}", ((IBundle)sender).GetSymbolicName(), stateStr, e.GetExtensionPoint().Name, extensionStr));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void contextServiceChanged(object sender, OSGi.NET.Event.ServiceEventArgs e)
        {
            log.Info(string.Format("{0} {1} Service {2}", ((IBundle)sender).GetSymbolicName(), e.GetState(), string.Join(",", e.GetSercieContracts())));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void Stop(IBundleContext context)
        {
            serviceRegistration.Unregister();
            context.ServiceChanged -= contextServiceChanged;
            context.ExtensionChanged -= ContextOnExtensionChanged;
        }
    }
}
