using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CefSharp.Wpf;

namespace InternetBrowser
{

    public class WebBrowserVM : INotifyPropertyChanged
    {
        WebBrowserModel WebBrowserModel;
        ChromiumWebBrowser browser;

        public WebBrowserVM(ChromiumWebBrowser _browser)
        {
            WebBrowserModel = new WebBrowserModel();
            browser = _browser;
            //      browser.U
        }

        public string CurURL
        {
            get
            {
                return WebBrowserModel.CurURL;
            }
            set
            {
                WebBrowserModel.CurURL = value;
                browser.Address = value;
                OnPropertyChanged("CurURL");
            }
        }

        public ChromiumWebBrowser Browser
        {
            get
            {
                return browser;
            }
            set
            {
                Browser = value;
                OnPropertyChanged("Browser");
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
}

