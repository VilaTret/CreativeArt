using System.Windows.Controls;
using System.Windows.Input;

namespace CreativeArt.Controls
{
    public class DragingTabItem : TabItem
    {
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            e.Handled = false;
        }
    }
}
