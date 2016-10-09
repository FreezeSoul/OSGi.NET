using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Xml;

using DevExpress.Xpf.NavBar;

using OSGi.NET.Core;
using OSGi.NET.Event;
using OSGi.NET.Service;

using WpfServiceContract;

namespace WpfLeftMenu
{
    public class BundleActivator : IBundleActivator
    {

        private IServiceRegistration serviceRegistration;
        public static UserControl1 LeftMenu;

        public static IBundleContext BundleContext;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void Start(IBundleContext context)
        {

            BundleContext = context;

            LeftMenu = new UserControl1();
            context.ExtensionChanged += ContextOnExtensionChanged;

            serviceRegistration = context.RegisterService<ILoadMenuView>(new LoadMenuView());
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void Stop(IBundleContext context)
        {
            context.ExtensionChanged -= ContextOnExtensionChanged;
            serviceRegistration.Unregister();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ContextOnExtensionChanged(object sender, ExtensionEventArgs e)
        {
            if (e.GetState() == ExtensionEventArgs.LOAD)
            {
                if (e.GetExtensionData().Name == "WpfLeftMenu.TopMenuExtension")
                {
                    LeftMenu.TopGroup.Items.Clear();
                    foreach (XmlNode xmlNode in e.GetExtensionData().ExtensionList)
                    {
                        foreach (XmlNode childNode in xmlNode.ChildNodes)
                        {
                            if (childNode.Attributes != null && childNode.Attributes["Name"] != null)
                            {
                                var item = new NavBarItem { Content = childNode.Attributes["Name"].Value };
                                item.Click += ItemOnClick;
                                LeftMenu.TopGroup.Items.Add(item);
                            }
                        }
                    }
                }
            }
            else if (e.GetState() == ExtensionEventArgs.UNLOAD)
            {
                if (e.GetExtensionData().Name == "WpfLeftMenu.TopMenuExtension")
                {
                    LeftMenu.TopGroup.Items.Clear();
                }
            }

        }

        private void ItemOnClick(object sender, EventArgs eventArgs)
        {

            var serviceRef = BundleContext.GetServiceReference<IMenuItemEvent>();
            var servcie = BundleContext.GetService<IMenuItemEvent>(serviceRef);
            servcie.FireEvent(((NavBarItem)sender).Content.ToString());
        }
    }
}
