using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace CreativeArt.Controls
{
    public class DragingTabControl : TabControl
    {
        private static readonly Type _typeofThis;

        private ConditionalWeakTable<object, DependencyObject> _objectToContainerMap;

        public static readonly DependencyProperty CapturedTabProperty;

        public static readonly DependencyProperty ReorderTabsCommandProperty;

        public static readonly RoutedEvent TabDraggedOutsideBondsEvent;

        private ConditionalWeakTable<object, DependencyObject> ObjectToContainer => _objectToContainerMap ??
                                                                                    (_objectToContainerMap = new ConditionalWeakTable<object, DependencyObject>());

        public DragingTabItem CapturedTab
        {
            get { return (DragingTabItem)GetValue(CapturedTabProperty); }
            private set { SetValue(CapturedTabProperty, value); }
        }

        public ICommand ReorderTabsCommand
        {
            get { return (ICommand)GetValue(ReorderTabsCommandProperty); }
            set { SetValue(ReorderTabsCommandProperty, value); }
        }

        public event TabDraggedOutsideBondsEventHandler TabDraggedOutsideBonds
        {
            add { AddHandler(TabDraggedOutsideBondsEvent, value); }
            remove { RemoveHandler(TabDraggedOutsideBondsEvent, value); }
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is DragingTabItem;
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new DragingTabItem();
        }

        private static void OnTabDraggedOutsideBondsThunk(object sender, TabDraggedOutsideBondsEventArgs e)
        {
            if (!e.Handled)
            {
                if (sender is DragingTabControl tabControl)
                {
                    tabControl.OnTabDraggedOutsideBonds(e);
                }
            }
        }

        protected virtual void OnTabDraggedOutsideBonds(TabDraggedOutsideBondsEventArgs e)
        {

        }

        protected virtual void OnTabMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            CapturedTab = sender as DragingTabItem;
        }

        protected virtual void OnTabMouseLeave(object sender, MouseEventArgs e)
        {
            DragingTabItem tab = sender as DragingTabItem;
            if (e.LeftButton == MouseButtonState.Pressed && tab == CapturedTab)
            {
                RaiseEvent(new TabDraggedOutsideBondsEventArgs(TabDraggedOutsideBondsEvent, this, tab.Content));
            }
            CapturedTab = null;
        }

        protected virtual void OnTabMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            CapturedTab = null;
        }

        protected DragingTabItem AsTabItem(object item)
        {
            DragingTabItem tabItem = item as DragingTabItem;
            if (tabItem == null && item != null)
            {
                ObjectToContainer.TryGetValue(item, out DependencyObject dp);
                tabItem = dp as DragingTabItem;
            }
            return tabItem;
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);
            if (element != item)
            {
                ObjectToContainer.Remove(item);
                ObjectToContainer.Add(item, element);
                DragingTabItem tab = AsTabItem(element);
                tab.MouseLeftButtonDown += OnTabMouseLeftButtonDown;
                tab.MouseLeave += OnTabMouseLeave;
                tab.MouseLeftButtonUp += OnTabMouseLeftButtonUp;
            }
        }

        protected override void ClearContainerForItemOverride(DependencyObject element, object item)
        {
            base.ClearContainerForItemOverride(element, item);
            if (element != item)
            {
                ObjectToContainer.Remove(item);
                DragingTabItem tab = AsTabItem(element);
                tab.MouseLeftButtonDown -= OnTabMouseLeftButtonDown;
                tab.MouseLeave -= OnTabMouseLeave;
                tab.MouseLeftButtonUp -= OnTabMouseLeftButtonUp;
            }
        }

        public void ReorderTabs(int fromIndex, int toIndex)
        {
            TabReorder tabReorder = new TabReorder(fromIndex, toIndex);
            if (ReorderTabsCommand != null && ReorderTabsCommand.CanExecute(tabReorder))
            {
                ReorderTabsCommand.Execute(tabReorder);
            }
            else
            {
                var sourceType = ItemsSource.GetType();
                if (sourceType.IsGenericType)
                {
                    var sourceDefinition = sourceType.GetGenericTypeDefinition();
                    if (sourceDefinition == typeof(ObservableCollection<>))
                    {
                        var method = sourceType.GetMethod("Move");
                        method.Invoke(ItemsSource, new object[] { fromIndex, toIndex });
                    }
                }
            }
        }

        internal static void RegisterEvents(Type type)
        {
            EventManager.RegisterClassHandler(type, TabDraggedOutsideBondsEvent, new TabDraggedOutsideBondsEventHandler(OnTabDraggedOutsideBondsThunk), handledEventsToo: false);
        }

        static DragingTabControl()
        {
            _typeofThis = typeof(DragingTabControl);
            CapturedTabProperty = DependencyProperty.Register("CapturedTab", typeof(DragingTabItem), _typeofThis);
            ReorderTabsCommandProperty = DependencyProperty.Register("ReorderTabsCommand", typeof(ICommand), _typeofThis);
            TabDraggedOutsideBondsEvent = EventManager.RegisterRoutedEvent("TabDraggedOutsideBonds", RoutingStrategy.Direct, typeof(TabDraggedOutsideBondsEventHandler), _typeofThis);
            RegisterEvents(typeof(DragingTabControl));
        }
    }
}
