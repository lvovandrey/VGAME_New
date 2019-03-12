using LevelSetsEditor.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace LevelSetsEditor.ViewModel
{

    public class WebBrowserVM : INotifyPropertyChanged
    {
        WebBrowserModel WebBrowserModel;

        public WebBrowserVM()
        {
            WebBrowserModel = new WebBrowserModel();
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
                OnPropertyChanged("CurURL");
            }
        }

        public WebBrowser Body
        {
            get
            {
                return WebBrowserModel.Body;
            }
            set
            {
                WebBrowserModel.Body = value;
                OnPropertyChanged("Body");
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
