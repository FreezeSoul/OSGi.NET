using System.Collections.ObjectModel;

namespace OsgiDllTool.Model
{
    public class Extension : EntityBase
    {
        private string _point;

        private ObservableCollection<Item> _items;

        public string Point
        {
            get { return _point; }
            set
            {
                _point = value;
                OnPropertyChanged(() => Point);
            }
        }

        public ObservableCollection<Item> Items
        {
            get
            {
                return _items ?? (_items = new ObservableCollection<Item>());
            }
            set
            {
                _items = value;
                OnPropertyChanged(() => Items);
            }
        }
    }

    public class Item : EntityBase
    {
        private string _name;

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                OnPropertyChanged(() => Name);
            }
        }
    }
}
