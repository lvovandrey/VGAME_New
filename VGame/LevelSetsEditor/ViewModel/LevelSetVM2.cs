using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using LevelSetsEditor.DB;
using LevelSetsEditor.Model;

namespace LevelSetsEditor.ViewModel
{
    public class LevelSetVM : INotifyPropertyChanged
    {
 

        private LevelSet _LevelSet;


        private VideoInfoVM _VideoInfoVM { get; set; }
        public VideoInfoVM VideoInfoVM { get { return _VideoInfoVM; }}




        private VideoPlayerVM videoPlayerVM;

        private ObservableCollection<SceneSetVM> sceneSetVMs;
        private SceneSetVM selectedSceneSet;
        

        
        public LevelSetVM(LevelSet levelSet) // В этом конструкторе заполняем тестовыми данными свойства ойства...
        {
            _LevelSet = levelSet;
            _VideoInfoVM = new VideoInfoVM(_LevelSet);
            sceneSetVMs = new ObservableCollection<SceneSetVM>();
            VideoPlayerVM = new VideoPlayerVM(this);
            PropertyChanged += LevelSetVM_PropertyChanged;
        }

        private void LevelSetVM_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedSceneSetVM") // Надо бы как-то иначе это реализовать идиотизм какой-то...
            {
                selectedSceneSet.UnitsCount = selectedSceneSet.UnitsCount;
                selectedSceneSet.VideoSegment_Source = selectedSceneSet.VideoSegment_Source;
                selectedSceneSet.VideoSegment_TimeBegin = selectedSceneSet.VideoSegment_TimeBegin;
                selectedSceneSet.VideoSegment_TimeEnd = selectedSceneSet.VideoSegment_TimeEnd;
            }
        }

        public ObservableCollection<SceneSetVM> SceneSetVMs
        {
            get { return sceneSetVMs; }
            set { sceneSetVMs = value; OnPropertyChanged("SceneSetVMs"); }
        }

        public SceneSetVM SelectedSceneSetVM
        {
            get { return selectedSceneSet; }
            set
            {
                selectedSceneSet = value;
                OnPropertyChanged("SelectedSceneSetVM");
            }
        }

        public VideoPlayerVM VideoPlayerVM
        {
            get { return videoPlayerVM; }
            set { videoPlayerVM = value; OnPropertyChanged("VideoPlayerVM"); }
        }

 
        public string Name
        {
            get { return _LevelSet.Name; }
            set
            {
                _LevelSet.Name = value;
                OnPropertyChanged("Name");
            }
        }



        public void SegregateScenes()
        {
            _LevelSet.SegregateScenes();
            sceneSetVMs.Clear();
            foreach (SceneSet s in _LevelSet.SceneSets)
                sceneSetVMs.Add(new SceneSetVM(s));
            
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
