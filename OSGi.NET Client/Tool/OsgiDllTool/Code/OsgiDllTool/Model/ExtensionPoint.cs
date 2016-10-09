namespace OsgiDllTool.Model
{
    public class ExtensionPoint : EntityBase
    {
        private string _point;

        public string Point
        {
            get { return _point; }
            set
            {
                _point = value;
                OnPropertyChanged(() => Point);
            }
        }
    }
}
