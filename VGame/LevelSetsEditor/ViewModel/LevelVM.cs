﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using LevelSetsEditor.Model;

namespace LevelSetsEditor.ViewModel
{
    public class LevelVM : INotifyPropertyChanged
    {
        private Level _Level;
        private VideoInfo _VideoInfo { get { return _Level.VideoInfo; } set { _Level.VideoInfo = value; } }


        public LevelVM(Level level)
        {
            _Level = level;
            _VideoInfoVM = new VideoInfoVM(_Level.VideoInfo);  //ВОт как надо - надо опираться на единую модель и не создавать новые представления в геттерах!!!
        }

        public string Name
        {
            get { return _Level.Name; }
            set { _Level.Name = value; OnPropertyChanged("Name"); }
        }

        /// <summary>
        /// ВОТ !!! Вот как нада! 
        /// </summary>
        private VideoInfoVM _VideoInfoVM { get; set; }
        public VideoInfoVM VideoInfoVM
        {
            get
            {
                return _VideoInfoVM;
            } // ЕСЛИ СЛОЖНАЯ ВЛОЖЕННАЯ СТРУКТУРА - то ее не присваиваем! А?
        }




        private ObservableCollection<Scene> _scenes { get {return _Level.Scenes; } set { _Level.Scenes = value; OnPropertyChanged("SceneVMs"); } }
        private ObservableCollection<SceneVM> _scenesvm { get; set; }

        public ObservableCollection<SceneVM> SceneVMs
        {
            get
            {
                _scenesvm = new ObservableCollection<SceneVM>(from l in _scenes select new SceneVM(l));
                return _scenesvm;
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







        public int id { get { return _Level.Id; } }

        public Level _level { get { return _Level; } }








        //public ObservableCollection<SceneVM> SceneVMs
        //{
        //    get { return sceneVMs; }
        //    set { sceneVMs = value; OnPropertyChanged("SceneVMs"); }
        //}


        //public VideoInfoVM VideoInfoVM { get; set; }
        //private VideoPlayerVM videoPlayerVM;

        //private ObservableCollection<SceneVM> sceneVMs;
        //private SceneVM selectedScene;


        ////public LevelSetVM() // В этом конструкторе заполняем тестовыми данными свойства ойства...
        ////{
        ////    LevelSet = new Model.LevelSet();
        ////    VideoInfoVM = new VideoInfoVM(LevelSet);
        ////    sceneVMs = new ObservableCollection<SceneVM>();
        ////    VideoPlayerVM = new VideoPlayerVM(this);
        ////    PropertyChanged += LevelSetVM_PropertyChanged;
        ////}

        //public LevelVM(Level levelSet) // В этом конструкторе заполняем тестовыми данными свойства ойства...
        //{
        //    LevelSet = levelSet;
        //    VideoInfoVM = new VideoInfoVM(LevelSet);
        //    sceneVMs = new ObservableCollection<SceneVM>();
        //    VideoPlayerVM = new VideoPlayerVM(this);
        //    PropertyChanged += LevelSetVM_PropertyChanged;
        //}

        //private void LevelSetVM_PropertyChanged(object sender, PropertyChangedEventArgs e)
        //{
        //    if (e.PropertyName == "SelectedSceneVM") // Надо бы как-то иначе это реализовать идиотизм какой-то...
        //    {
        //        selectedScene.UnitsCount = selectedScene.UnitsCount;
        //        selectedScene.VideoSegment_Source = selectedScene.VideoSegment_Source;
        //        selectedScene.VideoSegment_TimeBegin = selectedScene.VideoSegment_TimeBegin;
        //        selectedScene.VideoSegment_TimeEnd = selectedScene.VideoSegment_TimeEnd;
        //    }
        //}



        //public SceneVM SelectedSceneVM
        //{
        //    get { return selectedScene; }
        //    set
        //    {
        //        selectedScene = value;
        //        OnPropertyChanged("SelectedSceneVM");
        //    }
        //}

        //public VideoPlayerVM VideoPlayerVM
        //{
        //    get { return videoPlayerVM; }
        //    set { videoPlayerVM = value; OnPropertyChanged("VideoPlayerVM"); }
        //}

        //public VideoInfo VideoInfo
        //{
        //    get { return LevelSet.VideoInfo; }
        //    set
        //    {
        //        LevelSet.VideoInfo = value;
        //        OnPropertyChanged("VideoInfo");
        //    }
        //}



        //public string Name
        //{
        //    get { return LevelSet.Name; }
        //    set
        //    {
        //        LevelSet.Name = value;
        //        OnPropertyChanged("Name");
        //    }
        //}



        public void SegregateScenes()
        {
            _Level.SegregateScenes();
            OnPropertyChanged("SceneVMs");

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
