using CardsEditor.Tools;
using CardsEditor.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CardsEditor
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        VM vm;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            vm = new VM(this);
            RefreshRecentlyFilesMenu();
        }
        public void RefreshRecentlyFilesMenu() 
        {
            RecentlyFilesMenuItem.Items.Clear();
            RecentlyFilesMenuItem.Items.Add(new MenuItem() { Header = "Очистить", Command = vm.ClearRecentlyBDCommand});
            RecentlyFilesMenuItem.Items.Add(new Separator());
            int i =0;
            var filenames =new List<string>(Settings.GetInstance().RecentlyOpenFilenames);
            filenames.Reverse();
            foreach (var filename in filenames)
            {
                i++;
                var icon = new TextBlock() { Text = i.ToString() };
                RecentlyFilesMenuItem.Items.Add(new MenuItem() { Header =  filename, Icon = icon, Command = vm.OpenRecentlyBDCommand, CommandParameter = filename }) ;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            vm.OnWindowClosing();
        }
    }
}
