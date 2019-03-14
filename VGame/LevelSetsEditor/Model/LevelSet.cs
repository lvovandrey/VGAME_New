using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace LevelSetsEditor.Model
{
	public class LevelSet:INotifyPropertyChanged
	{
        public LevelSet()
        {
           
        }
        VideoInfo _videoInfo;
        public VideoInfo VideoInfo
        {
            get
            {
                return _videoInfo;
            }
            set
            {
                _videoInfo = value;
                OnPropertyChanged("VideoInfo");
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
