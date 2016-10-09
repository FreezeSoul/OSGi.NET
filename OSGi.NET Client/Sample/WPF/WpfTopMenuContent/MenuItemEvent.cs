using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using WpfServiceContract;

namespace WpfTopMenuContent
{
    class MenuItemEvent : IMenuItemEvent
    {
        private UserControl1 userControl;

        public void FireEvent(string name)
        {
            if (userControl == null)
            {
                userControl = new UserControl1();
                BundleActivator.AttachContentService.AttachUcContent(userControl);
            }
            userControl.MenuInfo.Text = name + Environment.NewLine + string.Join(Environment.NewLine, AppDomain.CurrentDomain.GetAssemblies().Select(asm => asm.FullName));
        }
    }
}
