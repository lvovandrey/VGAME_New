using System.ComponentModel;
using LevelSetsEditor.Model;

namespace LevelSetsEditor.ViewModel
{
    public class VideoInfoVM : INotifyPropertyChanged
    {
        private LevelSet levelSet;

        public VideoInfoVM(LevelSet levelSet)
        {
            this.levelSet = levelSet;
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