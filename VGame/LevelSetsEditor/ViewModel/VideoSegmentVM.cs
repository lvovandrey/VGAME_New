using LevelSetsEditor.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LevelSetsEditor.ViewModel
{
    public class VideoSegmentVM: INotifyPropertyChanged
    {
        VideoSegment VideoSegment;
        public VideoSegmentVM(VideoSegment videoSegment)
        {
            VideoSegment = videoSegment;
        }

        public TimeSpan TimeBegin
        {
            get
            {
                return VideoSegment.TimeBegin;
            }
            set
            {
                VideoSegment.TimeBegin= value;
                OnPropertyChanged("TimeBegin");
            }
        }

        public TimeSpan TimeEnd
        {
            get
            {
                return VideoSegment.TimeEnd;
            }
            set
            {
                VideoSegment.TimeEnd = value;
                OnPropertyChanged("TimeEnd");
            }
        }

        public Uri Source
        {
            get
            {
                return VideoSegment.Source;
            }
            set
            {
                VideoSegment.Source = value;
                OnPropertyChanged("Source");
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

