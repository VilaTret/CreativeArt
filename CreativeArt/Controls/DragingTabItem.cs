using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace CreativeArt.Controls
{
    public class DragingTabItem : TabItem
    {
        private static readonly Type _typeofThis;

        public static readonly DependencyProperty CloseTabCommandProperty;

        public static readonly DependencyProperty CloseTabCommandParameterProperty;

        public ICommand CloseTabCommand
        {
            get { return (ICommand)GetValue(CloseTabCommandProperty); }
            set { SetValue(CloseTabCommandProperty, value); }
        }

        public object CloseTabCommandParameter
        {
            get { return GetValue(CloseTabCommandParameterProperty); }
            set { SetValue(CloseTabCommandParameterProperty, value); }
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            e.Handled = false;
        }

        static DragingTabItem()
        {
            _typeofThis = typeof(DragingTabItem);
            CloseTabCommandProperty = DependencyProperty.Register("CloseTabCommand", typeof(ICommand), _typeofThis);
            CloseTabCommandParameterProperty = DependencyProperty.Register("CloseTabCommandParameter", typeof(object), _typeofThis, new FrameworkPropertyMetadata((object)null));
        }
    }
}
