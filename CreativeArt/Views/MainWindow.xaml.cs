using CreativeArt.Views;
using System.Windows;

namespace CreativeArt
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            mainFrame.Navigate(new TabControlPage());
        }
    }
}
