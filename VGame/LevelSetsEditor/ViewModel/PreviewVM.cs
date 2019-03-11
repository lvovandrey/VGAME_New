using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using LevelSetsEditor.Model;

namespace LevelSetsEditor.ViewModel
{
    public class PreviewVM:INotifyPropertyChanged
    {
        Preview Preview;
        public PreviewVM(Preview preview)
        {
            Preview = preview;
        }

        public Size Size
        {
            get
            {
                return Preview.Size;
            }
            set
            {
                Preview.Size = value;
                OnPropertyChanged("Preview");
            }
        }

        public Uri Source
        {
            get
            {
                return Preview.Source;
            }
            set
            {
                Preview.Source = value;
                OnPropertyChanged("Source");
            }
        }

        public PreviewType Type
        {
            get
            {
                return Preview.Type;
            }
            set
            {
                Preview.Type = value;
                OnPropertyChanged("Type");
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
