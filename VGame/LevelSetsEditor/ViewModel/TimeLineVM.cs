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
using System.Windows;

namespace LevelSetsEditor.ViewModel
{
    public class TimeLineVM : INotifyPropertyChanged
    {
        TimeLine TimeLine;
       

        public TimeLineVM(VideoPlayer _videoPlayer, TimeLine timeLine)
        {
            TimeLine = timeLine;
            TimeLine.videoPlayer = _videoPlayer;

            TimeLine.SceneSelect += TimeLine_SceneSelect;
        }

        private void TimeLine_SceneSelect()
        {
            SetSelectedSceneFormTimeline();
        }

        private void _SelectedLevelVM_SceneVMsCollectionChangedEvent()
        {
            TimeLine.ClearIntervals();
            foreach (SceneVM sceneVM in _SelectedLevelVM.SceneVMs)
            {
                TimeLine.AddInterval(sceneVM.VideoSegment_TimeBegin, sceneVM.VideoSegment_TimeEnd);
            }
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
                _SelectedLevelVM = value;
                OnPropertyChanged("SelectedLevelVM");
                TimeLine.FullTime = _SelectedLevelVM.VideoInfoVM.Duration;
                _SelectedLevelVM.SceneVMsCollectionChangedEventClear();
                _SelectedLevelVM.SceneVMsCollectionChangedEvent += _SelectedLevelVM_SceneVMsCollectionChangedEvent;
                _SelectedLevelVM_SceneVMsCollectionChangedEvent();
            }
        }

        private SceneVM _SelectedSceneVM;
        public SceneVM SelectedSceneVM
        {
            get
            { return _SelectedSceneVM; }
            set
            {
                _SelectedSceneVM = value;
                OnPropertyChanged("SelectedSceneVM");
                _SelectedSceneVM.SceneVMsCollectionChangedEventClear();
                _SelectedSceneVM.SceneVMsCollectionChangedEvent += _SelectedSceneVM_SceneVMsCollectionChangedEvent;
                _SelectedSceneVM_SceneVMsCollectionChangedEvent();
            }
        }

        private void _SelectedSceneVM_SceneVMsCollectionChangedEvent()
        {
            MessageBox.Show("Сцена изменилась выбранная");
        }

        internal void SelectedSceneChange(SceneVM selectedSceneVM)
        {
            SelectedSceneVM = selectedSceneVM;
        }

        private void SetSelectedSceneFormTimeline()
        {
            SceneVM sceneVM = _SelectedLevelVM.SceneVMs[2];
            _SelectedLevelVM.SelectedSceneVM = sceneVM;
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
