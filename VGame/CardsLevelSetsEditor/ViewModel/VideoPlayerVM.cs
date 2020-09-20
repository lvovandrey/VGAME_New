using LevelSetsEditor.View.VideoPlayerMVVM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LevelSetsEditor.ViewModel
{
    public class VideoPlayerVM: INotifyPropertyChanged
    {
        VideoPlayer videoPlayer;
        public VideoPlayerVM( VideoPlayer _videoPlayer )
        {
            videoPlayer = _videoPlayer;
        }

        private TimeSpan curTime;
        public TimeSpan CurTime
        {
            get
            {
                return curTime;
            }
            set
            {
                curTime = value;
                videoPlayer.SetCurTime(value);
                //videoPlayer.CurTime = curTime;
                OnPropertyChanged("CurTime");
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
