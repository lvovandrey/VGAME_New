using LevelSetsEditor.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Drawing;

namespace LevelSetsEditor.ViewModel
{
    public class VideoInfoVM: INotifyPropertyChanged
    {
        VideoInfo VideoInfo;
        public VideoInfoVM(VideoInfo videoInfo)
        {
            VideoInfo = videoInfo;
        }



        public string Title
        {
            get
            {
                return VideoInfo.Title;
            }
            set
            {
                VideoInfo.Title = value;
                OnPropertyChanged("Title");
            }
        }

        public TimeSpan Duration
        {
            get
            {
                return VideoInfo.Duration;
            }
            set
            {
                VideoInfo.Duration = value;
                OnPropertyChanged("Duration");
            }
        }

        public string Description
        {
            get
            {
                return VideoInfo.Description;
            }
            set
            {
                VideoInfo.Description = value;
                OnPropertyChanged("Description");
            }
        }

        public System.Drawing.Size Resolution
        {
            get
            {
                return VideoInfo.Resolution;
            }
            set
            {
                VideoInfo.Resolution = value;
                OnPropertyChanged("Resolution");
            }
        }

        public Uri Source
        {
            get
            {
                return VideoInfo.Source;
            }
            set
            {
                VideoInfo.Source = value;
                OnPropertyChanged("Source");
            }
        }

        public string Address
        {
            get
            {
                return VideoInfo.Address;
            }
            set
            {
                VideoInfo.Address = value;
                OnPropertyChanged("Address");
            }
        }

        public VideoType Type
        {
            get
            {
                return VideoInfo.Type;
            }
            set
            {
                VideoInfo.Type = value;
                OnPropertyChanged("Type");
            }
        }

        public Preview Preview
        {
            get
            {
                return VideoInfo.Preview;
            }
            set
            {
                VideoInfo.Preview = value;
                OnPropertyChanged("Preview");
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
