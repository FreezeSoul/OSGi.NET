using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

using WpfServiceContract;

namespace WpfLeftMenu
{
    class LoadMenuView : ILoadMenuView
    {
        public UserControl LoadMenuControl()
        {
            return BundleActivator.LeftMenu;
        }
    }
}
