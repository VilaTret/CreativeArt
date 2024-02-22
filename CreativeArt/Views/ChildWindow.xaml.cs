using System.Windows;
using System.Windows.Controls;

namespace CreativeArt.Views
{
    /// <summary>
    /// Логика взаимодействия для TabControlWindow.xaml
    /// </summary>
    public partial class ChildWindow : Window
    {
        public ChildWindow()
        {
            InitializeComponent();
        }

        public ChildWindow(Page page) : this()
        {
            mainFrame.Content = page;
        }
    }
}
