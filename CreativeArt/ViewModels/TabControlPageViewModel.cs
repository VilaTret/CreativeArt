using CreativeArt.Controls;
using DevExpress.Mvvm;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CreativeArt.ViewModels
{
    public class TabControlPageViewModel : BaseViewModel
    {
        private ObservableCollection<TabViewModel> _tabCollection;

        private TabViewModel _selectedTab;

        public ObservableCollection<TabViewModel> TabCollection
        {
            get { return _tabCollection; }
            set
            {
                _tabCollection = value;
                OnPropertyChanged(nameof(TabCollection));
            }
        }

        public TabViewModel SelectedTab
        {
            get { return _selectedTab; }
            set
            {
                _selectedTab = value;
                OnPropertyChanged(nameof(SelectedTab));
            }
        }

        public ICommand RemoveTabCommand
        {
            get
            {
                return new DelegateCommand<TabViewModel>((tab) =>
                {
                    TabCollection.Remove(tab);
                });
            }
        }

        public ICommand AddTabCommand
        {
            get
            {
                return new DelegateCommand<TabViewModel>((tab) =>
                {
                    TabCollection.Add(tab);
                });
            }
        }

        public ICommand ReorderTabsCommand
        {
            get
            {
                return new DelegateCommand<TabReorder>((tabReoder) =>
                {
                    TabCollection.Move(tabReoder.FromIndex, tabReoder.ToIndex);
                });
            }
        }

        public TabControlPageViewModel()
        {
            TabCollection = new ObservableCollection<TabViewModel>();
            AddTabCommand.Execute(new TabViewModel() { Header = "Hello", Content = "Hello world!" });
            AddTabCommand.Execute(new TabViewModel() { Header = "Hello2", Content = "Hello world222222222222!" });
            AddTabCommand.Execute(new TabViewModel() { Header = "Hello2", Content = "Hello world222222222222!" });
            AddTabCommand.Execute(new TabViewModel() { Header = "Hello4", Content = "Hello world222222222222!" });
            AddTabCommand.Execute(new TabViewModel() { Header = "Hello3", Content = "Hello world333333222!" });

        }
    }
}
