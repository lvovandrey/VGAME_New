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
    public class LevelSetVM: INotifyPropertyChanged
    {
        private LevelSet LevelSet;
        public VideoInfoVM VideoInfoVM { get; set; }


        public LevelSetVM() // В этом конструкторе заполняем тестовыми данными свойства ойства...
        {
            LevelSet = new Model.LevelSet();
            VideoInfoVM = new VideoInfoVM(LevelSet);
        }


        public VideoInfo VideoInfo
        {
            get { return LevelSet.VideoInfo; }
            set
            {
                LevelSet.VideoInfo = value;
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
