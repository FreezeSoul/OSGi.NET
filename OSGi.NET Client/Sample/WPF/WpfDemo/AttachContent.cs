using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

using WpfServiceContract;

namespace WpfDemo
{
    public class AttachContent : IAttachContent
    {
        public void AttachUcContent(UserControl userControl)
        {
            App.MainWin.RightContent.Content = userControl;
        }
    }
}
