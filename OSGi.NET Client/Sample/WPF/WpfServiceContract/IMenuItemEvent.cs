using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WpfServiceContract
{
    public interface IMenuItemEvent
    {
        void FireEvent(string name);
    }
}
