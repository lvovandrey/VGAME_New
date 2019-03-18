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
        LevelSetVM levelSetVM;
        public VideoPlayerVM(LevelSetVM _levelSetVM)
        {
            levelSetVM = _levelSetVM;
            levelSetVM.PropertyChanged += LevelSetVM_PropertyChanged;
        }

        private void LevelSetVM_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName== "SelectedSceneSetVM")
            {
          //     levelSetVM
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
