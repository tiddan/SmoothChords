using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using SmoothChords.ViewModel;

namespace SmoothChords
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            MainWindowModel mainView = new MainWindowModel();
            mainWindow.DataContext = mainView;
            mainWindow.Show();
        }
    }
}
