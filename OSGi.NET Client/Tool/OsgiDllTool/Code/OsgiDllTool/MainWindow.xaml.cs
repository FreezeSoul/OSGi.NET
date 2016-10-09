using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Xml;
using System.Xml.Linq;
using OsgiDllTool.Control;
using OsgiDllTool.Model;
using OsgiDllTool.View;

namespace OsgiDllTool
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainWindow
    {
        private ObservableCollection<Project> _projects = new ObservableCollection<Project>();

        public static MainWindow Instance;

        public MainWindow()
        {
            Instance = this;
            InitializeComponent();
            Init();
        }

        #region Event
        private void BtnNewProject_OnClick(object sender, RoutedEventArgs e)
        {
            var view = new AddProjectView();
            var showDialog = view.ShowDialog();
            if (showDialog != null && showDialog.Value)
            {
                var isProject = _projects.FirstOrDefault(s => s.Name == view.Project.Name);
                if (isProject != null)
                {
                    MessageBox.Show("项目【" + view.Project.Name + "】已经存在，请重新添加项目。", "提示");
                    BtnNewProject_OnClick(null, null);
                    return;
                }
                _projects.Add(view.Project);
                Save();
            }
        }

        private void BtnNewBundles_OnClick(object sender, RoutedEventArgs e)
        {
            if (treeView.SelectedItem is Project)
            {
                var project = treeView.SelectedItem as Project;
                project.Bundles.Add(new BundleInfo { Parent = project });
            }
            else if (treeView.SelectedItem is BundleInfo)
            {
                var project = (treeView.SelectedItem as BundleInfo).Parent;
                project.Bundles.Add(new BundleInfo { Parent = project });
            }
            else
            {
                MessageBox.Show("请先选择项目或者Bundle再新建Bundle。", "提示");
            }
        }

        private void BtnDele_OnClick(object sender, RoutedEventArgs e)
        {
            Delete();
        }

        private void Control_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var etb = sender as EditableTextBlock;
            if (etb != null)
            {
                etb.IsInEditMode = true;
                etb.EditCompletedEvent += EditCompletedEvent;
            }
        }

        private void EditCompletedEvent(object sender, DependencyPropertyChangedEventArgs e)
        {
            var etb = sender as EditableTextBlock;
            if (etb != null)
            {
                etb.EditCompletedEvent -= EditCompletedEvent;

                var isProject = _projects.FirstOrDefault(s => s.Name == etb.Text);
                if (isProject != null)
                {
                    MessageBox.Show("项目【" + etb.Text + "】已经存在，请重新修改项目。", "提示");
                    var item = etb.Tag as Project;
                    if (item != null)
                        item.Name = e.OldValue.ToString();
                    etb.IsInEditMode = true;
                    return;
                }
                Save();
            }
        }

        private void TreeView_OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            projectSetView.Visibility = Visibility.Collapsed;
            bundleInfoSetView.Visibility = Visibility.Collapsed;
            if (treeView.SelectedItem is Project)
            {
                var project = treeView.SelectedItem as Project;
                projectSetView.Project = project;
                projectSetView.Visibility = Visibility.Visible;
            }
            else if (treeView.SelectedItem is BundleInfo)
            {
                var bundleInfo = treeView.SelectedItem as BundleInfo;
                bundleInfoSetView.BundleInfo = bundleInfo;
                bundleInfoSetView.Visibility = Visibility.Visible;
            }
        }

        private void treeView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
                Delete();
        }

        private void BtnExecution_OnClick(object sender, RoutedEventArgs e)
        {
            if (treeView.SelectedItem is Project)
            {
                var project = treeView.SelectedItem as Project;
                foreach (var bundleInfo in project.Bundles)
                {
                    CopyBundeInfo(project, bundleInfo);
                }
            }
            else if (treeView.SelectedItem is BundleInfo)
            {
                var bundleInfo = treeView.SelectedItem as BundleInfo;
                var project = bundleInfo.Parent;

                CopyBundeInfo(project, bundleInfo);
            }
            else
            {
                MessageBox.Show("请先选择要生成的项目或者Bundle。", "提示");
            }
        }
        #endregion

        #region Method
        private void Init()
        {
            var dic = System.AppDomain.CurrentDomain.BaseDirectory + "AppData";
            if (Directory.Exists(dic))
            {
                var file = dic + "\\project.xml";
                if (File.Exists(file))
                {
                    var sr = new StreamReader(file, Encoding.UTF8);
                    string strLine = sr.ReadLine();
                    string xmlStr = strLine;
                    while (strLine != null)
                    {
                        strLine = sr.ReadLine();
                        xmlStr = xmlStr + strLine;
                    }
                    sr.Close();
                    _projects = Helper.DeserializeXml(_projects.GetType(), xmlStr) as ObservableCollection<Project>;
                    if (_projects != null)
                        foreach (var project in _projects)
                        {
                            foreach (var bundleInfo in project.Bundles)
                            {
                                bundleInfo.Parent = project;
                            }
                        }
                    else
                    {
                        _projects = new ObservableCollection<Project>();
                    }
                }
            }
            treeView.ItemsSource = _projects;
        }

        private void CopyBundeInfo(Project project, BundleInfo bundleInfo)
        {
            var fromPath = new List<string>();
            var toPath = new List<string>();
            try
            {
                //AssemblyNamePath
                var bundleDic = project.BoundlePath + "\\" + bundleInfo.AssemblyNamePath.Name;
                var uriBundleDic = new Uri(new Uri(AppDomain.CurrentDomain.BaseDirectory), bundleDic);
                if (!Directory.Exists(uriBundleDic.LocalPath))
                {
                    Directory.CreateDirectory(uriBundleDic.LocalPath);
                }
                fromPath.Add(bundleInfo.AssemblyNamePath.Path);
                toPath.Add(bundleDic + "\\" + bundleInfo.AssemblyNamePath.Name);

                //Manifest
                var manifestFromPath = Path.GetDirectoryName(bundleInfo.AssemblyNamePath.Path) + "\\Manifest.xml";
                var uriManifestFromPath = new Uri(new Uri(AppDomain.CurrentDomain.BaseDirectory), manifestFromPath);
                var manifestToPath = bundleDic + "\\Manifest.xml";
                var uriManifestToPath = new Uri(new Uri(AppDomain.CurrentDomain.BaseDirectory), manifestToPath);
                if (File.Exists(uriManifestFromPath.LocalPath))
                {
                    var doc = new XmlDocument();
                    doc.Load(uriManifestFromPath.LocalPath);
                    var selectSingleNode = doc.SelectSingleNode("Bundle");
                    if (selectSingleNode != null)
                    {
                        var nodeList = selectSingleNode.Attributes;
                        if (nodeList != null)
                        {
                            foreach (XmlAttribute xn in nodeList) //遍历所有子节点
                            {
                                if (xn.Name == "AssemblyName")
                                {
                                    xn.Value = bundleInfo.Bundle.AssemblyName;
                                }
                                else if (xn.Name == "StartLevel")
                                {
                                    xn.Value = bundleInfo.Bundle.StartLevel;
                                }
                            }
                            doc.Save(uriManifestFromPath.LocalPath);//保存。
                        }
                    }
                    File.Copy(uriManifestFromPath.LocalPath, uriManifestToPath.LocalPath, true);
                }
                else
                {
                    var result = new XElement("Bundle");
                    result.Add(new XAttribute("AssemblyName", bundleInfo.Bundle.AssemblyName));
                    result.Add(new XAttribute("StartLevel", bundleInfo.Bundle.StartLevel));
                    Helper.CreatFile(manifestToPath, "<?xml version=\"1.0\" encoding=\"utf-8\"?>" + result);
                }

                //LibsPath
                var bundleLibs = bundleDic + "\\Libs";
                var uriBundleLibs = new Uri(new Uri(AppDomain.CurrentDomain.BaseDirectory), bundleLibs);
                if (!Directory.Exists(uriBundleLibs.LocalPath))
                {
                    Directory.CreateDirectory(uriBundleLibs.LocalPath);
                }
                foreach (var file in bundleInfo.LibsPath)
                {
                    fromPath.Add(file.Path);
                    toPath.Add(bundleLibs + "\\" + file.Name);
                }

                //ShareLibPath
                var shareLibPath = new Uri(new Uri(AppDomain.CurrentDomain.BaseDirectory), project.ShareLibPath);
                if (!Directory.Exists(shareLibPath.LocalPath))
                {
                    Directory.CreateDirectory(shareLibPath.LocalPath);
                }
                foreach (var file in bundleInfo.ShareLibsPath)
                {
                    fromPath.Add(file.Path);
                    toPath.Add(project.ShareLibPath + "\\" + file.Name);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "【" + bundleInfo.AssemblyNamePath.Name + "】失败");
                return;
            }
            var list = Helper.CopyDll(fromPath, toPath, bundleInfo.IsDebug);
            if (list.Count == 0)
            {
                if (this.IsVisible)
                    MessageBox.Show("执行成功！", "【" + bundleInfo.AssemblyNamePath.Name + "】提示");
            }
            else
            {
                string message = list.Aggregate("下面是未能复制到指定目录中的文件：", (current, str) => current + "\n" + str);
                MessageBox.Show(message, "【" + bundleInfo.AssemblyNamePath.Name + "】提示");
            }
        }

        private void Delete()
        {
            if (treeView.SelectedItem == null)
            {
                MessageBox.Show("请先选择要删除的项目或者Bundle。", "提示");
                return;
            }

            if (treeView.SelectedItem is Project)
            {
                var project = treeView.SelectedItem as Project;
                if (MessageBox.Show("确定要删除项目【" + project.Name + "】吗？", "确认删除", MessageBoxButton.YesNo,
                                    MessageBoxImage.Information) == MessageBoxResult.Yes)
                {
                    _projects.Remove(project);
                }
            }
            else if (treeView.SelectedItem is BundleInfo)
            {
                var bundleInfo = treeView.SelectedItem as BundleInfo;
                if (MessageBox.Show("确定要删除【" + bundleInfo.AssemblyNamePath + "】吗？", "确认删除", MessageBoxButton.YesNo,
                                    MessageBoxImage.Information) == MessageBoxResult.Yes)
                {
                    bundleInfo.Parent.Bundles.Remove(treeView.SelectedItem as BundleInfo);
                }
            }
            Save();
        }
        #endregion

        public void Save()
        {
            var str = Helper.SerializeXml(_projects);
            var dic = System.AppDomain.CurrentDomain.BaseDirectory + "AppData";
            if (!Directory.Exists(dic))
            {
                Directory.CreateDirectory(dic);
            }
            var file = dic + "\\project.xml";
            Helper.CreatFile(file, str);
        }

        public void Execution(string projectName, string bundleName)
        {
            Project project;
            if (string.IsNullOrEmpty(projectName))
            {
                if (_projects.Count > 0)
                    project = _projects[0];
                else
                {
                    MessageBox.Show("没有项目。", "提示");
                    return;
                }
            }
            else
            {
                project = _projects.FirstOrDefault(s => s.Name == projectName);
                if (project == null)
                {
                    MessageBox.Show("未找到项目。", "提示");
                    return;
                }
            }

            var bundle = project.Bundles.FirstOrDefault(s => s.AssemblyNamePath.Name == bundleName);
            if (bundle == null)
            {
                MessageBox.Show("未找到Bundle。", "提示");
                return;
            }
            CopyBundeInfo(project, bundle);
        }
    }
}
