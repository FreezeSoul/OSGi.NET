using System.Windows;
using System.Windows.Forms;
using OsgiDllTool.Model;
using MessageBox = System.Windows.MessageBox;

namespace OsgiDllTool.View
{
    /// <summary>
    /// ProjectSetView.xaml 的交互逻辑
    /// </summary>
    public partial class ProjectSetView
    {
        private Project _project;

        public Project Project
        {
            get { return _project; }
            set
            {
                _project = value;
                txtBoundlePath.Text = _project.BoundlePath;
                txtShareLibPath.Text = _project.ShareLibPath;
                btnSave.IsEnabled = false;
            }
        }

        public ProjectSetView()
        {
            InitializeComponent();
        }

        private void BtnSave_OnClick(object sender, RoutedEventArgs e)
        {
            if (_project.BoundlePath == _project.ShareLibPath && !string.IsNullOrEmpty(_project.ShareLibPath))
            {
                MessageBox.Show("Boundle目录和ShareLib目录不能是同一个目录。");
                return;
            }
            _project.BoundlePath = txtBoundlePath.Text;
            _project.ShareLibPath = txtShareLibPath.Text;
            btnSave.IsEnabled = false;
            MainWindow.Instance.Save();
        }

        private void BtnShareLibPath_OnClick(object sender, RoutedEventArgs e)
        {
            var path = Dialog();
            if (string.IsNullOrEmpty(path))
                return;
            if (path != this.txtShareLibPath.Text)
            {
                this.txtShareLibPath.Text = path;
                btnSave.IsEnabled = true;
            }
        }

        private void BtnBoundlePath_OnClick(object sender, RoutedEventArgs e)
        {
            var path = Dialog();
            if (string.IsNullOrEmpty(path))
                return;
            if (path != this.txtBoundlePath.Text)
            {
                this.txtBoundlePath.Text = path;
                btnSave.IsEnabled = true;
            }
        }

        private string Dialog()
        {
            var dialog = new FolderBrowserDialog();
            DialogResult result = dialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.Cancel)
            {
                return string.Empty;
            }
            return Helper.Transform(dialog.SelectedPath.Trim());
        }
    }
}
