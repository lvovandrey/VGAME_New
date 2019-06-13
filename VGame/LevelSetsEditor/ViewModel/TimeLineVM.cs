using LevelSetsEditor.Model;
using LevelSetsEditor.View.TimeLine;
using LevelSetsEditor.View.VideoPlayerMVVM;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LevelSetsEditor.ViewModel
{
    public class TimeLineVM : INotifyPropertyChanged
    {
        TimeLine TimeLine;
        //public ObservableCollection<Interval> Intervals;

        public TimeLineVM(VideoPlayer _videoPlayer, TimeLine timeLine)
        {
            TimeLine = timeLine;
            TimeLine.videoPlayer = _videoPlayer;

        //    _SelectedLevelVM.SceneVMs.CollectionChanged += SceneVMs_CollectionChanged;
        }

        private void _SelectedLevelVM_SceneVMsCollectionChangedEvent()
        {
            TimeLine.ClearIntervals();
            foreach (SceneVM sceneVM in _SelectedLevelVM.SceneVMs)
            {
                TimeLine.AddInterval(sceneVM.VideoSegment_TimeBegin, sceneVM.VideoSegment_TimeEnd);
            }
           // throw new NotImplementedException();
        }

        private void SceneVMs_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {

        }

        private LevelVM _SelectedLevelVM;
        public LevelVM SelectedLevelVM
        {
            get
            { return _SelectedLevelVM; }
            set
            {
                //if (_SelectedLevelVM!=null && _SelectedLevelVM.SceneVMs!=null)
                // _SelectedLevelVM.SceneVMs.CollectionChanged -= SceneVMs_CollectionChanged;


                _SelectedLevelVM = value;
          //      _SelectedLevelVM.SceneVMs.CollectionChanged += SceneVMs_CollectionChanged;
                OnPropertyChanged("SelectedLevelVM");
            //    SceneVMs_CollectionChanged(null, null);
                TimeLine.FullTime = _SelectedLevelVM.VideoInfoVM.Duration;

                _SelectedLevelVM.SceneVMsCollectionChangedEventClear();
                _SelectedLevelVM.SceneVMsCollectionChangedEvent += _SelectedLevelVM_SceneVMsCollectionChangedEvent;
            }

        }

        private Level Level { get { return SelectedLevelVM._level; } }



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
