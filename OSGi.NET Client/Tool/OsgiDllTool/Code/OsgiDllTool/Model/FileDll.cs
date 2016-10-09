namespace OsgiDllTool.Model
{
    public class FileDll : EntityBase
    {
        private string _name;
        private string _path;

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged(() => Name);
            }
        }

        public string Path
        {
            get { return _path; }
            set
            {
                _path = value;
                OnPropertyChanged(() => Path);
            }
        }
        public override string ToString()
        {
            return Name;
        }
    }
}
