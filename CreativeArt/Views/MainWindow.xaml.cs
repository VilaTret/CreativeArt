using CreativeArt.Controls;
using CreativeArt.Utilities;
using CreativeArt.ViewModels;
using CreativeArt.Views;
using System;
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
            TabControlPage tabControlPage = new TabControlPage();
            TabControlPageViewModel tabControlPageViewModel = tabControlPage.DataContext as TabControlPageViewModel;
            tabControlPageViewModel.AddTabCommand.Execute(new TabViewModel() { Header = "Hello", Content = "Hello world!" });
            tabControlPageViewModel.AddTabCommand.Execute(new TabViewModel() { Header = "Hello2", Content = "Hello world222222222222!" });
            tabControlPageViewModel.AddTabCommand.Execute(new TabViewModel() { Header = "Hello2", Content = "Hello world222222222222!" });
            tabControlPageViewModel.AddTabCommand.Execute(new TabViewModel() { Header = "Hello4", Content = "Hello world222222222222!" });
            tabControlPageViewModel.AddTabCommand.Execute(new TabViewModel() { Header = "Hello3", Content = "Hello world333333222!" });
            mainFrame.Content = tabControlPage;
        }
    }
}
