using CreativeArt.Utilities;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace CreativeArt.Controls
{
    public class DragingTabPanel : TabPanel
    {
        private DragingTabControl _parentTabControl;

        private Point _startCursorPosition;

        protected override void OnVisualParentChanged(DependencyObject oldParent)
        {
            base.OnVisualParentChanged(oldParent);
            _parentTabControl = VisualTreeFinding.FindParent<DragingTabControl>(this);
        }

        protected virtual void OnTabMouseLeftButtonDown(object sender, MouseEventArgs e)
        {
            _startCursorPosition = e.GetPosition(this);
        }

        protected virtual void OnTabMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragingTabItem dragingTab = sender as DragingTabItem;
                if (dragingTab == _parentTabControl?.CapturedTab)
                {
                    Point currentCursorPosition = e.GetPosition(this);
                    Point deff = currentCursorPosition - (Vector)_startCursorPosition;
                    DoDragTab(dragingTab, deff.X);
                    _startCursorPosition = currentCursorPosition;
                    int indexFirstTab = Children.IndexOf(dragingTab);
                    if (deff.X < 0 && indexFirstTab > 0)
                    {
                        int indexSecoondTab = indexFirstTab - 1;
                        if (Children[indexSecoondTab] is DragingTabItem leftTab
                            && dragingTab.Margin.Left < -leftTab.Margin.Right - leftTab.Width / 2)
                        {
                            _parentTabControl.ReorderTabs(indexFirstTab, indexSecoondTab);
                            DoDragTab(dragingTab, dragingTab.Width);
                        }
                    }
                    else if (deff.X > 0 && indexFirstTab < Children.Count - 1)
                    {
                        int indexSecoondTab = indexFirstTab + 1;
                        if (Children[indexSecoondTab] is DragingTabItem rightTab
                            && dragingTab.Margin.Right < -rightTab.Margin.Left - rightTab.Width / 2)
                        {
                            _parentTabControl.ReorderTabs(indexFirstTab, indexSecoondTab);
                            DoDragTab(dragingTab, -dragingTab.Width);
                        }
                    }
                }
            }
        }

        protected virtual void OnTabMouseLeftButtonUp(object sender, MouseEventArgs e)
        {
            (sender as DragingTabItem).Margin = new Thickness(0);
        }

        protected virtual void OnTabMouseLeave(object sender, MouseEventArgs e)
        {
            (sender as DragingTabItem).Margin = new Thickness(0);
        }

        private static void DoDragTab(DragingTabItem tab, double distance)
        {
            tab.Margin = new Thickness(tab.Margin.Left + distance, tab.Margin.Top, tab.Margin.Right - distance, tab.Margin.Bottom);
        }

        protected override void OnVisualChildrenChanged(DependencyObject visualAdded, DependencyObject visualRemoved)
        {
            base.OnVisualChildrenChanged(visualAdded, visualRemoved);
            if (visualAdded is DragingTabItem tabAdded)
            {
                tabAdded.MouseLeftButtonDown += OnTabMouseLeftButtonDown;
                tabAdded.MouseMove += OnTabMouseMove;
                tabAdded.MouseLeftButtonUp += OnTabMouseLeftButtonUp;
                tabAdded.MouseLeave += OnTabMouseLeave;
            }
            if (visualRemoved is DragingTabItem tabRemoved)
            {
                tabRemoved.MouseLeftButtonDown -= OnTabMouseLeftButtonDown;
                tabRemoved.MouseMove -= OnTabMouseMove;
                tabRemoved.MouseLeftButtonUp -= OnTabMouseLeftButtonUp;
                tabRemoved.MouseLeave -= OnTabMouseLeave;
            }
        }
    }
}
