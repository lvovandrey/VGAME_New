﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using LevelSetsEditor.Model;

namespace LevelSetsEditor.ViewModel
{
    public class LevelVM : INotifyPropertyChanged
    {
        private Level _Level;
        private VideoInfo _VideoInfo { get { return _Level.VideoInfo; } set { _Level.VideoInfo = value; } }
        private VideoPlayerVM _videoPlayerVM;
        private TimeLineVM _timeLineVM;
        private ListView SceneListBox;

        public LevelVM(Level level, VideoPlayerVM videoPlayerVM, TimeLineVM timeLineVM)
        {
            _Level = level;
            _VideoInfoVM = new VideoInfoVM(_Level.VideoInfo, level);  //ВОт как надо - надо опираться на единую модель и не создавать новые представления в геттерах!!!
            _videoPlayerVM = videoPlayerVM;
            _timeLineVM = timeLineVM;
            SegregateTime = TimeSpan.FromSeconds(100);
            SegregateCount = 5;
            OverlapSegregateTime = TimeSpan.Zero;
           // SceneListBox = ((MainWindow)Application.Current.MainWindow).SceneListBox;

            //пробрасываем событие изменения коллекции сцен 
            try { _Level.Scenes.CollectionChanged -= SceneVMs_CollectionChanged; } 
            finally { _Level.Scenes.CollectionChanged += SceneVMs_CollectionChanged; }
        }



        #region Свое событие изменения коллекции сцен (collectionChanged) т.к. родное криво работает.
        public event Action SceneVMsCollectionChangedEvent; //событие изменения коллекции сцен  - родное снаружи не работает
        private void SceneVMs_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            SceneVMsCollectionChangedEvent?.Invoke();
        }
        public void SceneVMsCollectionChangedEventClear() { SceneVMsCollectionChangedEvent = null; }
        #endregion

        public string Name
        {
            get { return _Level.Name; }
            set { _Level.Name = value; OnPropertyChanged("Name"); }
        }

        public string Tag
        {
            get { return _Level.Tag; }
            set { _Level.Tag = value; OnPropertyChanged("Tag"); }
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
                _scenesvm = new ObservableCollection<SceneVM>(from l in _scenes select new SceneVM(l, _videoPlayerVM, this));
                //_scenesvm = _scenesvm.OrderBy
                return _scenesvm;

            }
        }



        private SceneVM _SelectedSceneVM;
        public SceneVM SelectedSceneVM
        {
            get
            {
                return _SelectedSceneVM;
            }
            set
            {
                _SelectedSceneVM = value;
                OnPropertyChanged("SelectedSceneVM");

                Console.WriteLine("LevelVM.SelectedSceneVM Change");
            }

        }


        public void RepaintTimeLineIntervals()
        {
            TimeLineRepaintCommand.Execute(null);
        }


        public TimeSpan _SegregateTime { get; set; }
        public TimeSpan _OverlapSegregateTime { get; set; }
        public int _SegregateCount { get; set; }
        public TimeSpan SegregateTime { get { return _SegregateTime; } set { _SegregateTime = value; OnPropertyChanged("SegregateTime"); } }
        public TimeSpan OverlapSegregateTime { get { return _OverlapSegregateTime; } set { _OverlapSegregateTime = value; OnPropertyChanged("OverlapSegregateTime"); } }
        public int SegregateCount { get { return _SegregateCount; } set { _SegregateCount = value; OnPropertyChanged("SegregateCount"); } }

        public int id { get { return _Level.Id; } }
        public Level _level { get { return _Level; } }



     


        //SegregateScenesCommand


        private RelayCommand addSceneCommand;
        public RelayCommand AddSceneCommand
        {
            get
            {
                return addSceneCommand ??
                  (addSceneCommand = new RelayCommand(obj =>
                  {
                      int pos;
                      if (_Level.Scenes.Count == 0) return; //TODO: Тут потенциальная ошибка "Последовательность не содержит элементов"
                      Scene scene = new Scene();

                      int nextid = 0;
                      if (_Level.Scenes.Count > 0)
                          nextid = _Level.Scenes.Max(a => a.Id);
                      scene.Id = ++nextid;
                      pos = _Level.Scenes.Max(a => a.Id);
                      scene.Position = ++pos;
                      scene.VideoSegment.TimeEnd = _Level.VideoInfo.Duration;
                      scene.VideoSegment.Source = _Level.VideoInfo.Source;
                      _Level.Scenes.Add(scene);
                      OnPropertyChanged("SceneVMs");
                  }));
            }
        }


        private RelayCommand removeSceneCommand;
        public RelayCommand RemoveSceneCommand
        {
            get
            {
                return removeSceneCommand ??
                  (removeSceneCommand = new RelayCommand(obj =>
                  {
                      _Level.Scenes.Remove(SelectedSceneVM.scene);
                      OnPropertyChanged("SceneVMs");
                  }));
            }
        }


        private RelayCommand downSceneCommand;
        public RelayCommand DownSceneCommand
        {
            get
            {
                return downSceneCommand ??
                  (downSceneCommand = new RelayCommand(obj =>
                  {
                      int oldPos = SceneListBox.Items.CurrentPosition;
                      if (oldPos >= SceneListBox.Items.Count-1) return;

                      if (SelectedSceneVM == null) return;
                      int pos = SelectedSceneVM.scene.Position;

                      Scene PostScene = _scenes.Where(i => i.Position == pos + 1).FirstOrDefault();
                      PostScene.Position = pos;
                      SelectedSceneVM.scene.Position = pos + 1;

                      var newscenes = _scenes.OrderBy(a => a.Position).OfType<Scene>();

                      ObservableCollection<Scene> Tmp = new ObservableCollection<Scene>();
                      foreach (var item in newscenes)
                      {
                          Tmp.Add((Scene)item);
                      }
                      _scenes.Clear();
                      _scenes = Tmp;

                    //  SelectedSceneVM = SceneVMs.Last();

                      SceneListBox.SelectedIndex = oldPos + 1;
                      OnPropertyChanged("SceneVMs");
                      SceneListBox.SelectedIndex = oldPos + 1;
                      OnPropertyChanged("SelectedSceneVM");
                  }));
            }
        }

        private RelayCommand upSceneCommand;
        public RelayCommand UpSceneCommand
        {
            get
            {
                return upSceneCommand ??
                  (upSceneCommand = new RelayCommand(obj =>
                  {

                      int oldPos = SceneListBox.Items.CurrentPosition;
                      if (oldPos < 1) return;
                   //   MessageBox.Show(SceneListBox.Items.CurrentPosition.ToString() ); //SceneListBox.SelectedItem

                      if (SelectedSceneVM == null) return;
                      int pos = SelectedSceneVM.scene.Position;

                      Scene PrevScene = _scenes.Where(i => i.Position == pos - 1).FirstOrDefault();
                      PrevScene.Position = pos;
                      SelectedSceneVM.scene.Position = pos - 1;

                     var newscenes = _scenes.OrderBy(a => a.Position).OfType<Scene>();
                      
                      ObservableCollection<Scene> Tmp = new ObservableCollection<Scene>();
                      foreach (var item in newscenes)
                      {
                          Tmp.Add((Scene)item);
                      }
                      _scenes.Clear();
                      _scenes = Tmp;
                      SceneListBox.SelectedIndex = oldPos - 1;
                      OnPropertyChanged("SceneVMs");
                      SceneListBox.SelectedIndex = oldPos - 1;
                      OnPropertyChanged("SelectedSceneVM");

                  }));
            }
        }

        private RelayCommand clearScenesListCommand;
        public RelayCommand ClearScenesListCommand
        {
            get
            {
                return clearScenesListCommand ??
                  (clearScenesListCommand = new RelayCommand(obj =>
                  {
                      // if (SelectedSceneVM == null) return;
                      if (MessageBox.Show("Точно стереть все сцены?", "Удаление всех сцен", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.No) return;
                      _Level.Scenes = new ObservableCollection<Scene>();
                      OnPropertyChanged("SceneVMs");
                  }));
            }
        }

        private RelayCommand segregateScenesCommand;
        public RelayCommand SegregateScenesCommand
        {
            get
            {
                return segregateScenesCommand ??
                  (segregateScenesCommand = new RelayCommand(obj =>
                  {

                      if (MessageBox.Show("Точно заменить все сцены?", "Замена всех сцен", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.No) return;
                      SegregateScenes(obj);
                      OnPropertyChanged("SceneVMs");
                  }));
            }
        }




        public void SegregateScenes(object parameter)
        {
            if (parameter == null)
                _Level.SegregateScenes();
            else if (parameter is TimeSpan) 
                _Level.SegregateScenes(SegregateTime, OverlapSegregateTime);
            else if (parameter is int)
                _Level.SegregateScenes(SegregateCount, OverlapSegregateTime);

            OnPropertyChanged("SceneVMs");

        }

        private RelayCommand splitScenesCommand;
        public RelayCommand SplitScenesCommand
        {
            get
            {
                return splitScenesCommand ??
                  (splitScenesCommand = new RelayCommand(obj =>
                  {

                      if (MessageBox.Show("Разделить сцену?", "Разделение сцены", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.No) return;
                      SplitScene(obj);
                      OnPropertyChanged("SceneVMs");
                  }));
            }
        }
        private void SplitScene(object parameter)
        {
            if (parameter is TimeSpan)
                _Level.SplitScene((TimeSpan)parameter);
        }



        private RelayCommand timeLineRefreshCommand;
        public RelayCommand TimeLineRefreshCommand
        {
            get
            {
                return timeLineRefreshCommand ?? (timeLineRefreshCommand = new RelayCommand(obj =>
                {
                    _timeLineVM.Body.RefreshAll();
                }));
            }
        }

        private RelayCommand timeLineRepaintCommand;
        public RelayCommand TimeLineRepaintCommand
        {
            get
            {
                return timeLineRepaintCommand ?? (timeLineRepaintCommand = new RelayCommand(obj =>
                {
                    _timeLineVM.Body.RepaintAll();
                }));
            }
        }



        public void JoinYoutubeVideoInLevel(string YoutubeAddress)
        {
            _Level.JoinYoutubeVideoInLevel(YoutubeAddress);
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
