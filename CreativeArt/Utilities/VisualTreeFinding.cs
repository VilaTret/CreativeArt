using System.Windows.Media;
using System.Windows;

namespace CreativeArt.Utilities
{
    public static class VisualTreeFinding
    {
        public static T FindParent<T>(Visual visual) where T : Visual
        {
            DependencyObject parent = VisualTreeHelper.GetParent(visual);
            while (parent != null && !(parent is T))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }
            return parent as T;
        }

        public static T FindChild<T>(DependencyObject parent) where T : DependencyObject
        {
            if (parent == null)
            {
                return null;
            }
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                if (child is T typedChild)
                {
                    return typedChild;
                }
                T childOfChild = FindChild<T>(child);
                if (childOfChild != null)
                {
                    return childOfChild;
                }
            }
            return null;
        }
    }
}
