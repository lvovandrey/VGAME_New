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
                        {
                            string filename = openFileDialog.FileName;
                            MessageBox.Show(filename);
                        }
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
