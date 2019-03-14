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
using System.Drawing;
using CefSharp;
using CefSharp.Wpf;
using YouTubeUrlSupplier;
using LevelSetsEditor.Model;
using System.ComponentModel;

namespace LevelSetsEditor
{
    public class Test:INotifyPropertyChanged
    {
        string _address;
        public string Address
        {
            get
            {
                return _address;
            }
            set
            {
                _address = value;
                OnPropertyChanged("Address");
            }
        }

        #region mvvm
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }

    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        WebBrowserVM WebBrowserVM;

        MainVM MainVM;

        Test test;

        


        //   LevelSetVM LevelSetVMDataContext;
        public MainWindow()
        {
            CefSettings settings = new CefSettings();
            settings.CachePath = @"C:\CEFcookies"; // Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\CEF";
            CefSharp.Cef.Initialize(settings);

            InitializeComponent();
            //WebBrowserVM Browser = new WebBrowserVM(new Model.WebBrowserModel());
            WebBrowserVM = new WebBrowserVM(Browser);
            GridBrowser.DataContext = WebBrowserVM;




            test = new Test();

            test.Address = "АДРЕС ТЕСТА";
            DataContext = test;
            //  MainVM = new MainVM();
            //DataContext = MainVM;



        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            WebBrowserVM.CurURL = TextURL.Text;
        }

        private void TextURL_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) WebBrowserVM.CurURL = TextURL.Text;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            WebBrowserVM.CurURL = "Youtube.com";
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            TabItemEditor.Focus();
            
            YoutubeVidInfo vidInfo = new YoutubeVidInfo(TextURL.Text);
            if (vidInfo.DirectURL == "") return;

            MainVM.VideoInfoVM.Address = TextURL.Text;
            MainVM.VideoInfoVM.Title = vidInfo.Title;
        }



    }
}
