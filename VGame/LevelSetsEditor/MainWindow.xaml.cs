using LevelSetsEditor.ViewModel;
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
using CefSharp.Wpf;
using CefSharp;

namespace LevelSetsEditor
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            InitializeComponent();
            WebBrowserVM Browser = new WebBrowserVM();
            GridBrowser.DataContext = Browser;
            Browser.Body = SomeBrowser;
           // DataContext = levelSet;

            

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SomeBrowser.Source = new Uri(((WebBrowserVM)GridBrowser.DataContext).CurURL);
        }

        private void SomeBrowser_SourceUpdated(object sender, DataTransferEventArgs e)
        {
            ((WebBrowserVM)GridBrowser.DataContext).CurURL = SomeBrowser.Source.AbsoluteUri;
        }
    }
}
