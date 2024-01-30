using System.Windows;

namespace CreativeArt.Controls
{
    public class TabDraggedOutsideBondsEventArgs : RoutedEventArgs
    {
        private object _dragingtab;

        public object DragingTab
        {
            get { return _dragingtab; }
            set { _dragingtab = value; }
        }

        public TabDraggedOutsideBondsEventArgs(RoutedEvent routedEvent, object source, object dragingTab) : base(routedEvent, source)
        {
            _dragingtab = dragingTab;
        }
    }
}
