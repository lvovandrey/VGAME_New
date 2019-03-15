using System;
using System.ComponentModel;
using LevelSetsEditor.Model;

namespace LevelSetsEditor.ViewModel
{
    public class VideoInfoVM : INotifyPropertyChanged
    {
        private LevelSet levelSet;

        public PreviewVM previewVM;

        public VideoInfoVM(LevelSet levelSet)
        {
            this.levelSet = levelSet;
            previewVM = new PreviewVM(levelSet.VideoInfo.Preview);
        }


        public string Title
        {
            get
            {
                return levelSet.VideoInfo.Title;
            }
            set
            {
                levelSet.VideoInfo.Title = value;
                OnPropertyChanged("Title");
            }
        }

        public TimeSpan Duration
        {
            get
            {
                return levelSet.VideoInfo.Duration;
            }
            set
            {
                levelSet.VideoInfo.Duration = value;
                OnPropertyChanged("Duration");
            }
        }

        public string Description
        {
            get
            {
                return levelSet.VideoInfo.Description;
            }
            set
            {
                levelSet.VideoInfo.Description = value;
                OnPropertyChanged("Description");
            }
        }

        public System.Drawing.Size Resolution
        {
            get
            {
                return levelSet.VideoInfo.Resolution;
            }
            set
            {
                levelSet.VideoInfo.Resolution = value;
                OnPropertyChanged("Resolution");
            }
        }

        public Uri Source
        {
            get
            {
                return levelSet.VideoInfo.Source;
            }
            set
            {
                levelSet.VideoInfo.Source = value;
                OnPropertyChanged("Source");
            }
        }

        public string Address
        {
            get
            {
                return levelSet.VideoInfo.Address;
            }
            set
            {
                levelSet.VideoInfo.Address = value;
                OnPropertyChanged("Address");
            }
        }

        public VideoType Type
        {
            get
            {
                return levelSet.VideoInfo.Type;
            }
            set
            {
                levelSet.VideoInfo.Type = value;
                OnPropertyChanged("Type");
            }
        }


        //ПОчему закомментирован Preview: а зачем он нужен отсюда? чтобы внешние слои могли получить доступ к модели? Спасибо большое....не стоит...
        public PreviewVM PreviewVM
        {
            get
            {
                return previewVM;
            }
            set
            {
                previewVM = value;
                OnPropertyChanged("PreviewVM");
            }
        }

        public Uri Preview_Source
        {
            get
            {
                return levelSet.VideoInfo.Preview.Source;
            }
            set
            {
                levelSet.VideoInfo.Preview.Source = value;
                OnPropertyChanged("Preview_Source");
            }
        }

        public System.Drawing.Size Preview_Size
        {
            get
            {
                return levelSet.VideoInfo.Preview.Size;
            }
            set
            {
                levelSet.VideoInfo.Preview.Size = value;
                OnPropertyChanged("Preview_Size");
            }
        }

        public PreviewType Preview_Type
        {
            get
            {
                return levelSet.VideoInfo.Preview.Type;
            }
            set
            {
                levelSet.VideoInfo.Preview.Type = value;
                OnPropertyChanged("Preview_Type");
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