namespace CreativeArt.ViewModels
{
    public class TabViewModel : BaseViewModel
    {
        private string _header;

        private object _content;

        public string Header
        {
            get { return _header; }
            set
            {
                _header = value;
                OnPropertyChanged(nameof(Header));
            }
        }

        public object Content
        {
            get { return _content; }
            set
            {
                _content = value;
                OnPropertyChanged(nameof(Content));
            }
        }

    }
}
