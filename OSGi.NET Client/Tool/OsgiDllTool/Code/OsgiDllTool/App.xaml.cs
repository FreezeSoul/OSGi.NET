using System;
using System.Windows;

namespace OsgiDllTool
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            string project = string.Empty;
            string bundle = string.Empty;

            if (e != null && e.Args.Length > 0)
            {
                //..\..\..\..\Release\OsgiDllTool.exe project=测试 bundle=WpfRightContent
                foreach (var arg in e.Args)
                {
                    var str = arg.ToLower();
                    if (str.IndexOf("project=", StringComparison.Ordinal) == 0)
                    {
                        var strs = arg.Split(new[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                        if (strs.Length == 2)
                        {
                            project = strs[1];
                        }
                    }
                    else if (str.IndexOf("bundle=", StringComparison.Ordinal) == 0)
                    {
                        var strs = arg.Split(new[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                        if (strs.Length == 2)
                        {
                            bundle = strs[1];
                        }
                    }
                }
            }
            var view = new MainWindow();
            if (project == bundle && bundle == string.Empty)
            {
                view.Show();
            }
            else
            {
                view.Execution(project, bundle);
                Application.Current.Shutdown();
            }
        }
    }
}
