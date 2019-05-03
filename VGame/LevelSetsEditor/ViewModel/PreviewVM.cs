using LevelSetsEditor.Model;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LevelSetsEditor.ViewModel
{
    public class PreviewVM : INotifyPropertyChanged
    {
        private Preview _Preview { get; set; }

        public PreviewVM(Preview _preview)
        {
            this._Preview= _preview;
        }

       

        public Uri Source
        {
            get { return _Preview.Source; }
            set { _Preview.Source = value; OnPropertyChanged("Source"); }
        }

        public System.Drawing.Size Size
        {
            get { return _Preview.Size; }
            set { _Preview.Size = value; OnPropertyChanged("Size"); }
        }
        public PreviewType Type
        {
            get { return _Preview.Type; }
            set { _Preview.Type = value; OnPropertyChanged("Type"); }
        }

        public ObservableCollection<Uri> MultiplePrevSources
        {
            get { return _Preview.MultiplePrevSources; }
            set { _Preview.MultiplePrevSources = value; OnPropertyChanged("MultiplePrevSources"); }
        }


        #region old

        //public PreviewType Type
        //{
        //    get
        //    {
        //        return preview.Type;
        //    }
        //    set
        //    {
        //        preview.Type = value;
        //        OnPropertyChanged("Type");
        //    }
        //}

        private int curnum = 0;

        public Uri CurPreSources
        {
            get
            {
                if (_Preview.MultiplePrevSources.Count == 0) return null;
                Uri t = _Preview.MultiplePrevSources[curnum];
                curnum++;
                if (curnum > 2) curnum = 0;
                if (RefreshPrev)
                    Tools.ToolsTimer.Delay(() =>
                    {
                        OnPropertyChanged("CurPreSources");
                    }, TimeSpan.FromSeconds(2));
                return t;
            }
            set
            {
                OnPropertyChanged("CurPreSources");
            }
        }

        private bool refreshPrev = true;
        public bool RefreshPrev
        {
            get
            {
                return refreshPrev;
            }
            set
            {
                if (value)
                    Tools.ToolsTimer.Delay(() =>
                    {
                        OnPropertyChanged("CurPreSources");
                    }, TimeSpan.FromSeconds(2));

                refreshPrev = value;
                OnPropertyChanged("RefreshPrev");
            }
        }


        //public System.Drawing.Size Size
        //{
        //    get
        //    {
        //        return preview.Size;
        //    }
        //    set
        //    {
        //        preview.Size = value;
        //        OnPropertyChanged("Size");
        //    }
        //}
        #endregion
        private RelayCommand openPreviewFileCommand;
        public RelayCommand OpenPreviewFileCommand
        {
            get
            {
                return openPreviewFileCommand ??
                    (openPreviewFileCommand = new RelayCommand(obj =>
                    {
                        OpenFileDialog openFileDialog = new OpenFileDialog();
                        openFileDialog.Filter = "Файлы изображений (*.bmp, *.jpg, *.png)|*.bmp;*.jpg;*.png";
                        openFileDialog.Title = "Открыть свое изображение для превью видео";
                        if (openFileDialog.ShowDialog() == true)
                            Source = new Uri(@openFileDialog.FileName);
                    }));
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
