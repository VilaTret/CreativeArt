using CreativeArt.Controls;
using CreativeArt.Utilities;
using CreativeArt.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;

namespace CreativeArt.Views
{
    /// <summary>
    /// Логика взаимодействия для TabControlPage.xaml
    /// </summary>
    public partial class TabControlPage : Page
    {
        public TabControlPage()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, EventArgs e)
        {
            VisualTreeFinding.FindParent<Window>(this).LocationChanged += OnParentWindowLocationChanged;
            Loaded -= OnLoaded;
        }

        private void TabDraggedOutsideBonds(object sender, TabDraggedOutsideBondsEventArgs e)
        {
            (DataContext as TabControlPageViewModel).TabCollection.Remove(e.DragingTab as TabViewModel);
            TabControlPage tabControlPage = new TabControlPage();
            (tabControlPage.DataContext as TabControlPageViewModel).TabCollection.Add(e.DragingTab as TabViewModel);
            Point mousePosition = Win32Helper.GetMousePosition(this);
            ChildWindow windowForTab = new ChildWindow(tabControlPage);
            windowForTab.Left = mousePosition.X - windowForTab.Width / 2;
            windowForTab.Top = mousePosition.Y - 16;
            windowForTab.Show();
            windowForTab.DragMove();
        }

        protected virtual void OnParentWindowLocationChanged(object sender, EventArgs e)
        {
            Window parentWindow = sender as Window;
            if (parentWindow.IsLoaded)
            {
                Point mousePosition = Win32Helper.GetMousePosition(this);
                Window windowUnder = FindWindowUnderThisAt(mousePosition, parentWindow);
                if (windowUnder != null)
                {
                    if (VisualTreeFinding.FindChild<TabControlPage>(windowUnder).DataContext is TabControlPageViewModel viewModelTabControlPageUnder
                        && DataContext is TabControlPageViewModel viewModelThis)
                    {
                        TabViewModel selectedTab = viewModelThis.SelectedTab;
                        ObservableCollection<TabViewModel> tempCollection = new ObservableCollection<TabViewModel>();
                        foreach (TabViewModel item in viewModelThis.TabCollection)
                        {
                            tempCollection.Add(item);
                        }
                        viewModelThis.TabCollection.Clear();
                        foreach (TabViewModel item in tempCollection)
                        {
                            viewModelTabControlPageUnder.TabCollection.Add(item);
                        }
                        viewModelTabControlPageUnder.SelectedTab = selectedTab;
                        parentWindow.LocationChanged -= OnParentWindowLocationChanged;
                        parentWindow.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Used P/Invoke to find and return the top window under the cursor position
        /// </summary>
        /// <param name="source"></param>
        /// <param name="mousePosition"></param>
        /// <returns></returns>
        private static Window FindWindowUnderThisAt(Point mousePosition, Window parentWindow)  // WPF units (96dpi), not device units
        {
            //This prevents "UI debugging tools for XAML" from interfering when debugging.
            var allWindows = from window in SortWindowsTopToBottom(Application.Current.Windows.OfType<Window>().Where(x => x.GetType().ToString() != "Microsoft.VisualStudio.DesignTools.WpfTap.WpfVisualTreeService.Adorners.AdornerWindow"
                                                                                                         && x.GetType().ToString() != "Microsoft.VisualStudio.DesignTools.WpfTap.WpfVisualTreeService.Adorners.AdornerLayerWindow"))
                             where !Equals(window, parentWindow)
                                    && (window.WindowState == WindowState.Maximized || window.WindowState == WindowState.Normal)
                             select window;
            var windowsUnderCurrent = from window in allWindows
                                      where CheckMouseOverTabPanel(mousePosition, window)
                                            && !CheckOverlapAnotherWindow(mousePosition, window, allWindows)
                                      select window;
            return windowsUnderCurrent.FirstOrDefault();
        }

        private static bool CheckMouseOverTabPanel(Point mousePoint, Window parentWindow)
        {
            DragingTabPanel tabPanel = VisualTreeFinding.FindChild<DragingTabPanel>(parentWindow);
            if (tabPanel != null)
            {
                return GetTabPanelRectRelativeToScreen(tabPanel, parentWindow).Contains(mousePoint);
            }
            return false;
        }

        private static bool CheckOverlapAnotherWindow(Point mousePosition, Window targetWindow, IEnumerable<Window> allWindows)
        {
            for (int i = 0; i < allWindows.ToList().IndexOf(targetWindow); i++)
            {
                if (Win32Helper.GetWindowRect(allWindows.ElementAt(i)).Contains(mousePosition))
                {
                    return true;
                }
            }
            return false;
        }

        private static Rect GetTabPanelRectRelativeToScreen(DragingTabPanel tabPanel, Window window)
        {
            Rect tabPanelRect = Win32Helper.GetWindowRect(window);
            Rect tabPanelBounds = tabPanel.TransformToAncestor(window).TransformBounds(new Rect(tabPanel.Margin.Left, tabPanel.Margin.Top, tabPanel.ActualWidth, tabPanel.ActualHeight));
            tabPanelRect.X += tabPanelBounds.X;
            tabPanelRect.Y += tabPanelBounds.Y;
            tabPanelRect.Width = tabPanelBounds.Width;
            tabPanelRect.Height = tabPanelBounds.Height;
            return tabPanelRect;
        }

        /// <summary>
        /// We need to do some P/Invoke magic to get the windows on screen
        /// </summary>
        /// <param name="unsorted"></param>
        /// <returns></returns>
        private static IEnumerable<Window> SortWindowsTopToBottom(IEnumerable<Window> unsorted)
        {
            var byHandle = unsorted.ToDictionary(window =>
                ((HwndSource)PresentationSource.FromVisual(window)).Handle);
            for (IntPtr hWnd = Win32Helper.GetTopWindow(IntPtr.Zero); hWnd != IntPtr.Zero; hWnd = Win32Helper.GetWindow(hWnd, Win32Helper.GW_HWNDNEXT))
            {
                if (byHandle.ContainsKey(hWnd))
                {
                    yield return byHandle[hWnd];
                }
            }
        }
    }
}
