namespace OsgiDllTool.Model
{

    public class Bundle : EntityBase
    {
        private string _assemblyName;
        private string _startLevel;
        private ExtensionPoint _extensionPoint;
        private Extension _extension;

        public string AssemblyName
        {
            get
            {
                return _assemblyName;
            }
            set
            {
                _assemblyName = value;
                OnPropertyChanged(() => AssemblyName);
            }
        }

        public string StartLevel
        {
            get
            {
                return _startLevel;
            }
            set
            {
                _startLevel = value;
                OnPropertyChanged(() => StartLevel);
            }
        }

        public ExtensionPoint ExtensionPoint
        {
            get
            {
                return _extensionPoint;
            }
            set
            {
                _extensionPoint = value;
                OnPropertyChanged(() => ExtensionPoint);
            }
        }

        public Extension Extension
        {
            get
            {
                return _extension;
            }
            set
            {
                _extension = value;
                OnPropertyChanged(() => Extension);
            }
        }
    }
}
