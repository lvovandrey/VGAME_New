using CefSharp.Wpf;
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

namespace InternetBrowser
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class Browser : UserControl
    {
        public WebBrowserVM WebBrowserVM;

        public event Action<string> ChoiceUrl;

        public Browser()
        {
            CefSettings settings = new CefSettings();
            settings.CachePath = @"C:\CEFcookies";
            CefSharp.Cef.Initialize(settings);

            InitializeComponent();



            WebBrowserVM = new WebBrowserVM(MyBrowser);
            LifespanHandler life = new LifespanHandler();
            MyBrowser.LifeSpanHandler = life;
            life.popup_request += life_popup_request;
            MyBrowser.DataContext = WebBrowserVM;
        }

        private void life_popup_request(string obj) 
        {
            Dispatcher.Invoke(() => { WebBrowserVM.CurURL = obj; });
             
        }

        private void ButtonGo_Click(object sender, RoutedEventArgs e)
        {
            WebBrowserVM.CurURL = TextURL.Text;
        }

        private void TextURL_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) WebBrowserVM.CurURL = TextURL.Text;
        }

        private void ButtonChoiceUrl_Click(object sender, RoutedEventArgs e)
        {
            ChoiceUrl(WebBrowserVM.CurURL);
        }
    }
}
