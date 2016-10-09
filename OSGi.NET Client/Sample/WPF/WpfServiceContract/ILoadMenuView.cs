using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace WpfServiceContract
{
    public interface ILoadMenuView
    {
        UserControl LoadMenuControl();
    }
}
