using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OSGi.NET.Core;
using OSGi.NET.Event;

using ReferenceLibrary;

using ServiceContract;

namespace BundleDemoB
{
    public class BundleActivator : IBundleActivator
    {
        /// <summary>
        /// 
        /// </summary>
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void Start(IBundleContext context)
        {
            context.ServiceChanged += ContextOnServiceChanged;
            var serviceReference = context.GetServiceReference<IHelloWord>();
            var service = context.GetService<IHelloWord>(serviceReference);
            log.Info(service.SayHi("BundleDemoB" + ReferenceClass.GetName()));

            var serviceRef1 = context.GetServiceReferences<IMutilImplement>(new Dictionary<string, object>()
                                                                            {
                                                                                {"id","1"}
                                                                            }).FirstOrDefault();
            var service1 = context.GetService<IMutilImplement>(serviceRef1);
            log.Info(service1.TestMethod());

            var serviceRef2 = context.GetServiceReferences<IMutilImplement>(new Dictionary<string, object>()
                                                                            {
                                                                                {"id","2"}
                                                                            }).FirstOrDefault();
            var service2 = context.GetService<IMutilImplement>(serviceRef2);
            log.Info(service2.TestMethod());


            var serviceRef3 = context.GetServiceReferences<IMutilImplement>(new Dictionary<string, object>()
                                                                            {
                                                                                {"id","3"}
                                                                            }).FirstOrDefault();
            var service3 = context.GetService<IMutilImplement>(serviceRef3);
            log.Info(service3.TestMethod());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ContextOnServiceChanged(object sender, ServiceEventArgs e)
        {
            log.Info(string.Format("{0} {1} Service {2}", ((IBundle)sender).GetSymbolicName(), e.GetState(), string.Join(",", e.GetSercieContracts())));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void Stop(IBundleContext context)
        {
            context.ServiceChanged -= ContextOnServiceChanged;
        }
    }
}
