﻿using LevelSetsEditor.Model;
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
        VideoInfo _videoInfo;
        public VideoInfoVM(VideoInfo videoInfo)
        {
            _videoInfo = videoInfo;
        }



        public string Title
        {
            get
            {
                return _videoInfo.Title;
            }
            set
            {
                _videoInfo.Title = value;
                OnPropertyChanged("Title");
            }
        }



        public string Address
        {
            get
            {
                return _videoInfo.Address;
            }
            set
            {
                _videoInfo.Address = value;
                OnPropertyChanged("Address");
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
