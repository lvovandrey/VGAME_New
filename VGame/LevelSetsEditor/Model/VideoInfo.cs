using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;

namespace LevelSetsEditor.Model
{
    public enum VideoType {local, youtube, net}
	public class VideoInfo: INotifyPropertyChanged
	{
		public string Title { get; set; }
		
		public TimeSpan Duration { get; set; }

        public string Description { get; set; }

        public System.Drawing.Size Resolution { get; set; }

        public Uri Source { get; set; }

        string _address;
        public string Address
        {
            get
            {
                return _address;
            }
            set
            {
                _address = value;
                OnPropertyChanged("Address");
            }
        }

public VideoType Type { get; set; }

        public Preview Preview { get; set; }

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
