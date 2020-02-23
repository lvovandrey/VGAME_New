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

                _SelectedLevelVM.SceneVMsCollectionChangedEventClear();//Обрабатываем/подписываемся на событие изменения коллекции сцен
                _SelectedLevelVM.SceneVMsCollectionChangedEvent += _SelectedLevelVM_SceneVMsCollectionChangedEvent;
                _SelectedLevelVM_SceneVMsCollectionChangedEvent();
            }
        }
        //Обрабатываем событие изменения коллекции сцен
        public void _SelectedLevelVM_SceneVMsCollectionChangedEvent()
        {
            if (_SelectedLevelVM == null) return;

            TimeLine.ClearIntervals();
            foreach (SceneVM sceneVM in _SelectedLevelVM.SceneVMs)
            {
                TimeLine.AddInterval(sceneVM);
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
            }
        }

        internal void SelectedSceneChange(SceneVM selectedSceneVM)
        {
            SelectedSceneVM = selectedSceneVM;
        }

        internal void SelectedSceneVMRefresh()
        {
            OnPropertyChanged("SelectedSceneVM");
            //    _SelectedLevelVM_SceneVMsCollectionChangedEvent();
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
