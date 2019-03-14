using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using LevelSetsEditor.Model;

namespace LevelSetsEditor.ViewModel
{
    public class MainVM: INotifyPropertyChanged
    {
        
        public MainVM() // В этом конструкторе заполняем тестовыми данными свойства ойства...
        {
            _levelSet = new LevelSet();
            _levelSet.VideoInfo = new VideoInfo();

            _videoInfoVM = new VideoInfoVM(_levelSet.VideoInfo);
        }

        LevelSet _levelSet;

        VideoInfoVM _videoInfoVM;
        public VideoInfoVM VideoInfoVM
        {
            get
            {
                return _videoInfoVM;
            }
            set
            {
                _videoInfoVM = value;
                OnPropertyChanged("VideoInfoVM");
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
