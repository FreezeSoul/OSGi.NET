using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfLeftMenu
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class UserControl1 : UserControl
    {
        public UserControl1()
        {
            InitializeComponent();
        }

        private void NavBarItemInstall_OnClick(object sender, EventArgs e)
        {
            try
            {
                var bundle = BundleActivator.BundleContext.InstallBundle(System.IO.Path.Combine(Environment.CurrentDirectory, "Install", "WpfTopMenuContent.zip"));
                bundle.Start();
                MessageBox.Show("安装成功，请查看Group1");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }

        }

        private void NavBarItemUninstall_OnClick(object sender, EventArgs e)
        {
            try
            {
                var bundle = BundleActivator.BundleContext.GetBundle("WpfTopMenuContent");
                if (bundle==null)
                    throw new Exception("指定插件不存在！");
                bundle.Stop();
                bundle.UnInstall();
                MessageBox.Show("卸载成功，请查看Group1");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }

        }

        private void NavBarItemUpdate_OnClick(object sender, EventArgs e)
        {
            var bundle = BundleActivator.BundleContext.GetBundle("WpfRightContent");
            bundle.Update(System.IO.Path.Combine(Environment.CurrentDirectory, "Version1", "WpfRightContent.zip"));
            MessageBox.Show("更新成功");
        }

        private void NavBarItemRestore_OnClick(object sender, EventArgs e)
        {
            var bundle = BundleActivator.BundleContext.GetBundle("WpfRightContent");
            bundle.Update(System.IO.Path.Combine(Environment.CurrentDirectory, "Version2", "WpfRightContent.zip"));
            MessageBox.Show("更新成功");
        }
    }
}
