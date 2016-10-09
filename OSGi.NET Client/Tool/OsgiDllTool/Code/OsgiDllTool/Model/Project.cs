using System.Collections.ObjectModel;

namespace OsgiDllTool.Model
{
    public class Project : EntityBase
    {
        private string _id;

        private string _name;

        private string _boundlePath;

        private string _shareLibPath;

        private ObservableCollection<BundleInfo> _bundles;


        public string Id
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged(() => Id);
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged(() => Name);
            }
        }


        public string BoundlePath
        {
            get { return _boundlePath; }
            set
            {
                _boundlePath = value;
                OnPropertyChanged(() => BoundlePath);
            }
        }


        public string ShareLibPath
        {
            get { return _shareLibPath; }
            set
            {
                _shareLibPath = value;
                OnPropertyChanged(() => ShareLibPath);
            }
        }


        public ObservableCollection<BundleInfo> Bundles
        {
            get { return _bundles ?? (_bundles = new ObservableCollection<BundleInfo>()); }
            set
            {
                _bundles = value;
                OnPropertyChanged(() => Bundles);
            }
        }
    }
}
