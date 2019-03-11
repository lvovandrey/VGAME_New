using LevelSetsEditor.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LevelSetsEditor.ViewModel
{
    public class SceneSetVM: INotifyPropertyChanged
    {
        public SceneSet SceneSet;

        public SceneSetVM(SceneSet sceneSet)
        {
            SceneSet = sceneSet;
        }

        public int UnitsCount
        {
            get { return SceneSet.UnitsCount; }
            set
            {
                SceneSet.UnitsCount = value;
                OnPropertyChanged("UnitsCount");
            }
        }

        public VideoSegment VideoSegment
        {
            get { return SceneSet.VideoSegment; }
            set
            {
                SceneSet.VideoSegment = value;
                OnPropertyChanged("VideoSegment");
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
