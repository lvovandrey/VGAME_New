using LevelSetsEditor.Model;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LevelSetsEditor.ViewModel
{
    public class PreviewVM : INotifyPropertyChanged
    {
        private Preview preview;

        public PreviewVM(Preview _preview)
        {
            this.preview = _preview;
        }

        public PreviewType Type
        {
            get
            {
                return preview.Type;
            }
            set
            {
                preview.Type = value;
                OnPropertyChanged("Type");
            }
        }

        private int curnum = 0;

        public Uri CurPreSources
        {
            get
            {
                if (preview.MultiplePrevSources.Count == 0) return null; 
                Uri t = preview.MultiplePrevSources[curnum];
                curnum++;
                if (curnum > 2) curnum = 0;
                if (RefreshPrev)
                    Tools.ToolsTimer.Delay(() =>
                    {
                        OnPropertyChanged("CurPreSources");
                    },TimeSpan.FromSeconds(2));
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

        public Uri Source
        {
            get
            {
                return preview.Source;
            }
            set
            {
                preview.Source = value;
                OnPropertyChanged("Source");
            }
        }



        public System.Drawing.Size Size
        {
            get
            {
                return preview.Size;
            }
            set
            {
                preview.Size = value;
                OnPropertyChanged("Size");
            }
        }

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
