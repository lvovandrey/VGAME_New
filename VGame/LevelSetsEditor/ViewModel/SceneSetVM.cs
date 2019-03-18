using LevelSetsEditor.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LevelSetsEditor.ViewModel
{
    public class SceneSetVM : INotifyPropertyChanged
    {
        private SceneSet sceneSet;

        public SceneSetVM(SceneSet _sceneSet)
        {
            this.sceneSet = _sceneSet;
        }


        public TimeSpan VideoSegment_TimeBegin
        {
            get
            {
                return sceneSet.VideoSegment.TimeBegin;
            }
            set
            {
                sceneSet.VideoSegment.TimeBegin = value;
                OnPropertyChanged("VideoSegment_TimeBegin");
            }
        }

        public TimeSpan VideoSegment_TimeEnd
        {
            get
            {
                return sceneSet.VideoSegment.TimeEnd;
            }
            set
            {
                sceneSet.VideoSegment.TimeEnd = value;
                OnPropertyChanged("VideoSegment_TimeEnd");
            }
        }

        public Uri VideoSegment_Source
        {
            get
            {
                return sceneSet.VideoSegment.Source;
            }
            set
            {
                sceneSet.VideoSegment.Source = value;
                OnPropertyChanged("VideoSegment_Source");
            }
        }

        public int UnitsCount
        {
            get
            {
                return sceneSet.UnitsCount;
            }
            set
            {
                sceneSet.UnitsCount = value;
                OnPropertyChanged("UnitsCount");
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
