using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Xml;
using OsgiDllTool.Model;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using ListBox = System.Windows.Controls.ListBox;
using MessageBox = System.Windows.MessageBox;

namespace OsgiDllTool.View
{
    /// <summary>
    /// BundleInfoSetView.xaml 的交互逻辑
    /// </summary>
    public partial class BundleInfoSetView
    {
        private BundleInfo _bundleInfo;

        public BundleInfo BundleInfo
        {
            get { return _bundleInfo; }
            set
            {
                _bundleInfo = value;
                txtAssemblyNamePath.Text = _bundleInfo.AssemblyNamePath != null ? _bundleInfo.AssemblyNamePath.Path : string.Empty;
                checkBoxIsDebug.IsChecked = _bundleInfo.IsDebug;
                var listLibs = new ObservableCollection<FileDll>();
                foreach (var libPath in _bundleInfo.LibsPath)
                {
                    listLibs.Add(libPath);
                }
                listLibsPath.ItemsSource = listLibs;
                var shareLibs = new ObservableCollection<FileDll>();
                foreach (var libPath in _bundleInfo.ShareLibsPath)
                {
                    shareLibs.Add(libPath);
                }
                listShareLibsPath.ItemsSource = shareLibs;
                btnSave.IsEnabled = false;
                if (_bundleInfo.Bundle != null)
                    txtStartLevel.Text = _bundleInfo.Bundle.StartLevel;
                this.txtStartLevel.TextChanged -= TxtStartLevel_OnTextChanged;
                this.txtStartLevel.TextChanged += TxtStartLevel_OnTextChanged;
            }
        }

        public BundleInfoSetView()
        {
            InitializeComponent();
        }

        private void BtnSave_OnClick(object sender, RoutedEventArgs e)
        {
            if (_bundleInfo.AssemblyNamePath == null)
                _bundleInfo.AssemblyNamePath = new FileDll();
            _bundleInfo.AssemblyNamePath.Name = Helper.FromPathGetName(txtAssemblyNamePath.Text);
            _bundleInfo.AssemblyNamePath.Path = txtAssemblyNamePath.Text;
            _bundleInfo.IsDebug = checkBoxIsDebug.IsChecked.Value;
            _bundleInfo.LibsPath.Clear();
            foreach (FileDll item in listLibsPath.Items)
            {
                _bundleInfo.LibsPath.Add(item);
            }
            _bundleInfo.ShareLibsPath.Clear();
            foreach (FileDll item in listShareLibsPath.Items)
            {
                _bundleInfo.ShareLibsPath.Add(item);
            }
            btnSave.IsEnabled = false;
            if (_bundleInfo.Bundle == null)
                _bundleInfo.Bundle = new Bundle();
            _bundleInfo.Bundle.AssemblyName = _bundleInfo.AssemblyNamePath.Name;
            _bundleInfo.Bundle.StartLevel = txtStartLevel.Text;
            MainWindow.Instance.Save();
        }

        private List<string> Dialog(bool multiselect)
        {
            var dialog = new OpenFileDialog { Filter = "dll文件|*.dll", Multiselect = multiselect };
            DialogResult result = dialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.Cancel)
            {
                return null;
            }
            return dialog.FileNames.ToList().Select(source => Helper.Transform(source)).ToList();
        }

        private void BtnAssemblyNamePath_OnClick(object sender, RoutedEventArgs e)
        {
            var list = Dialog(false);
            if (list == null || list.Count == 0)
                return;
            if (list[0] != this.txtAssemblyNamePath.Text)
            {
                var bundleName = Helper.FromPathGetName(list[0]);
                var isBundle = _bundleInfo.Parent.Bundles.FirstOrDefault(s => s != null && s.AssemblyNamePath != null && s.AssemblyNamePath.Name == bundleName);
                if (isBundle != null)
                {
                    MessageBox.Show("【" + bundleName + "】已经存在【" + _bundleInfo.Parent.Name + "】项目中。", "提示");
                    return;
                }
                this.txtAssemblyNamePath.Text = list[0];
                var manifestFromPath = Path.GetDirectoryName(this.txtAssemblyNamePath.Text) + "\\Manifest.xml";
                if (File.Exists(manifestFromPath))
                {
                    var doc = new XmlDocument();
                    doc.Load(manifestFromPath);
                    var selectSingleNode = doc.SelectSingleNode("Bundle");
                    if (selectSingleNode != null)
                    {
                        var nodeList = selectSingleNode.Attributes;
                        if (nodeList != null)
                        {
                            foreach (XmlAttribute xn in nodeList)
                            {
                                if (xn.Name == "StartLevel")
                                {
                                    txtStartLevel.Text = xn.Value;
                                    break;
                                }
                            }
                        }
                    }
                }
                btnSave.IsEnabled = true;
            }
        }

        private void BtnLibsPath_OnClick(object sender, RoutedEventArgs e)
        {
            var files = Dialog(true);
            if (files == null || files.Count == 0)
                return;
            var list = listLibsPath.ItemsSource as ObservableCollection<FileDll>;
            AddFile(files, list);
            btnSave.IsEnabled = true;
        }

        private void BtnShareLibsPath_OnClick(object sender, RoutedEventArgs e)
        {
            var files = Dialog(true);
            if (files == null || files.Count == 0)
                return;
            var list = listShareLibsPath.ItemsSource as ObservableCollection<FileDll>;
            AddFile(files, list);
            btnSave.IsEnabled = true;
        }

        private static void AddFile(IEnumerable<string> files, ObservableCollection<FileDll> list)
        {
            foreach (var file in files)
            {
                var item = new FileDll { Path = file };
                item.Name = Helper.FromPathGetName(item.Path);
                list.Add(item);
            }
        }

        private void CheckBoxIsDebug_OnClick(object sender, RoutedEventArgs e)
        {
            btnSave.IsEnabled = true;
        }

        private void TxtStartLevel_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            btnSave.IsEnabled = true;
        }

        private void List_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                var list = sender as ListBox;
                if (list != null && list.SelectedItems.Count > 0)
                {
                    var all = list.ItemsSource as ObservableCollection<FileDll>;
                    if (all == null) return;
                    for (int i = list.SelectedItems.Count - 1; i >= 0; i--)
                    {
                        all.Remove(list.SelectedItems[i] as FileDll);
                    }
                    btnSave.IsEnabled = true;
                }
            }
        }
    }
}
