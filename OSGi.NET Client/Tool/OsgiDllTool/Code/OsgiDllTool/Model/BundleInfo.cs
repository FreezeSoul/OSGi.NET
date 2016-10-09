using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace OsgiDllTool.Model
{

    public class BundleInfo : EntityBase
    {
        private Bundle _bundle;

        private FileDll _assemblyNamePath;
        private bool _isDebug;
        private ObservableCollection<FileDll> _libsPath;

        private ObservableCollection<FileDll> _shareLibsPath;

        private Project _parent;

        public Bundle Bundle
        {
            get
            {
                return _bundle;
            }
            set
            {
                _bundle = value;
                OnPropertyChanged(() => Bundle);
            }
        }



        public FileDll AssemblyNamePath
        {
            get
            {
                return _assemblyNamePath;
            }
            set
            {
                _assemblyNamePath = value;
                OnPropertyChanged(() => AssemblyNamePath);
            }
        }



        public bool IsDebug
        {
            get
            {
                return _isDebug;
            }
            set
            {
                _isDebug = value;
                OnPropertyChanged(() => IsDebug);
            }
        }

        public ObservableCollection<FileDll> LibsPath
        {
            get
            {
                return _libsPath ?? (_libsPath = new ObservableCollection<FileDll>());
            }
            set
            {
                _libsPath = value;
                OnPropertyChanged(() => LibsPath);
            }
        }

        public ObservableCollection<FileDll> ShareLibsPath
        {
            get
            {
                return _shareLibsPath ?? (_shareLibsPath = new ObservableCollection<FileDll>());
            }
            set
            {
                _shareLibsPath = value;
                OnPropertyChanged(() => ShareLibsPath);
            }
        }

        [XmlIgnore]
        public Project Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }

    }

}
