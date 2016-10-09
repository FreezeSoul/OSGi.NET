using System.Windows;
using OsgiDllTool.Model;

namespace OsgiDllTool.View
{
    /// <summary>
    /// AddProjectView.xaml 的交互逻辑
    /// </summary>
    public partial class AddProjectView
    {
        private Project _project;

        public Project Project
        {
            get { return _project; }
            set
            {
                _project = value;
            }
        }

        public AddProjectView()
        {
            InitializeComponent();
            if (Application.Current.MainWindow != null)
                this.Owner = Application.Current.MainWindow;
            this.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            this.WindowStyle = WindowStyle.ToolWindow;
            this.Loaded += AddProjectView_Loaded;
        }

        private void AddProjectView_Loaded(object sender, RoutedEventArgs e)
        {
            txtName.Focus();
        }

        private void BtnEnter_OnClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("请输入项目名称。");
                return;
            }
            _project = new Project { Name = txtName.Text.Trim() };
            this.DialogResult = true;
        }

        private void BtnEsc_OnClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
