using LevelSetsEditor.DB;
using LevelSetsEditor.Model;
using LevelSetsEditor.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LevelSetsEditor.ViewModel
{
    public class VM : INotifyPropertyChanged
    {

        Context context;
        MainWindow mainWindow;


        private ObservableCollection<Level> _levels { get; set; }
        private ObservableCollection<LevelVM> _levelsvm { get; set; }
        private VideoPlayerVM _videoPlayerVM;
        private TimeLineVM _timeLineVM;

        public ObservableCollection<LevelVM> LevelVMs
        {
            get
            {
                if (_levels == null) return null;
                _levelsvm = new ObservableCollection<LevelVM>(from l in _levels select new LevelVM(l,_videoPlayerVM,_timeLineVM));
                return _levelsvm;
            }
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
                _timeLineVM.SelectedLevelVM = SelectedLevelVM;
            }

        }



        public Context db;

        public VM(MainWindow _mainWindow)
        {
            mainWindow = _mainWindow;
        }

        public VM(VideoPlayerVM videoPlayerVM,TimeLineVM timeLineVM, MainWindow _mainWindow):this(_mainWindow)
        {
            _videoPlayerVM = videoPlayerVM;
            _timeLineVM = timeLineVM;
          //  mainWindow.TimeLine1.DataContext = _timeLineVM;
            mainWindow.DataContext = this;
        }


        

        private RelayCommand addCommand;
        public RelayCommand AddCommand
        {
            get
            {
                return addCommand ??
                  (addCommand = new RelayCommand(obj =>
                  {
                      Add(obj);
                  }));
            }
        }

        private Level Add(object obj)
        {
            Random random = new Random();
            Level l = new Level() { Id = _levels.Count + 1, Name = "Level " + (_levels.Count + 1).ToString() };
            l.VideoInfo = new VideoInfo() { Title = (_levels.Count + 1).ToString(), Id = l.Id };
            l.VideoInfoId = l.VideoInfo.Id;
            l.VideoInfo.Preview = new Preview() { Source = new Uri(@"C:\1.png"), Id = l.VideoInfo.Id };
            l.VideoInfo.PreviewId = l.VideoInfo.Preview.Id;
            _levels.Add(l);
            context.Levels.Add(l);
            context.VideoInfoes.Add(l.VideoInfo);
            context.Previews.Add(l.VideoInfo.Preview);
            context.SaveChanges();
            OnPropertyChanged("LevelVMs");
            return l;
        }



        private RelayCommand saveCommand;
        public RelayCommand SaveCommand
        {
            get
            {
                return saveCommand ??
                  (saveCommand = new RelayCommand(obj =>
                  {
                      {

                          foreach (Level l in _levels)
                          {
                              context.Entry(l).State = EntityState.Modified;
                              context.Entry(l.VideoInfo).State = EntityState.Modified;
                              context.Entry(l.VideoInfo.Preview).State = EntityState.Modified;
                          } 
                          context.SaveChanges();
                          OnPropertyChanged("LevelVMs");
                      }
                  }));
            }
        }

        private RelayCommand removeCommand;
        public RelayCommand RemoveCommand
        {
            get
            {
                return removeCommand ??
                  (removeCommand = new RelayCommand(obj =>
                  {
                      {
                          if (SelectedLevelVM == null) return;

                          Level level = SelectedLevelVM._level;
                          context.Entry(level).State = EntityState.Deleted;
                          context.SaveChanges();

                          _levels.Remove(level);
                          OnPropertyChanged("LevelVMs");
                          SelectedLevelVM = LevelVMs.FirstOrDefault();
                      }
                  }));
            }
        }

        private RelayCommand joinCommand;
        public RelayCommand JoinCommand
        {
            get
            {
                return joinCommand ??
                  (joinCommand = new RelayCommand(obj =>
                  {


                      Random random = new Random();
                      Level l = new Level() { Id = _levels.Count + 1, Name = "Level " + (_levels.Count + 1).ToString() };
                      l.VideoInfo = new VideoInfo() { Title = (_levels.Count + 1).ToString(), Id = l.Id };
                      l.VideoInfoId = l.VideoInfo.Id;
                      l.VideoInfo.Preview = new Preview() { Source = new Uri(@"C:\1.png"), Id = l.VideoInfo.Id };
                      l.VideoInfo.PreviewId = l.VideoInfo.Preview.Id;
                      _levels.Add(l);
                      context.Levels.Add(l);
                      context.VideoInfoes.Add(l.VideoInfo);
                      context.Previews.Add(l.VideoInfo.Preview);
                      context.SaveChanges();
                      OnPropertyChanged("LevelVMs");
                  }));
            }
        }



        private RelayCommand loadDBCommand;
        public RelayCommand LoadDBCommand
        {
            get
            {
                return loadDBCommand ??
                  (loadDBCommand = new RelayCommand(obj =>
                  {
                      bool res = DBTools.LoadDB(this, _levels, context,(IInfoUI)mainWindow.windowProgress);
                      if (!res)
                      {
                          MessageBox.Show("Ошибка загрузки базы данных", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                          return;
                      }
                      mainWindow.DataContext = this;
                      OnPropertyChanged("LevelVMs");
                  }));
            }
        }

        //позорный костыль для загрузки БД - так и не разобрался почему коллекция после выхода из статического метода не изменяется. а внутри меняется вроде.
        public void init(ObservableCollection<Level>levels, Context Context)
        {
            _levels = levels;
            context = Context;
            OnPropertyChanged("txt");
        }

        public string txt { get {return "sdfsdfsdf"; } set {} }

        private RelayCommand joinVideoCommand;
        public RelayCommand JoinVideoCommand
        {
            get
            {
                return joinVideoCommand ??
                  (joinVideoCommand = new RelayCommand(obj =>
                  {

                      mainWindow.TabItemEditor.Focus();
                      if (SelectedLevelVM == null)
                      {
                          MessageBox.Show("Не выбран целевой уровень для загрузки данных о видео. Выберите уровень во вкладке УРОВНИ или воспользуйтесь командой NEW LVL.", "Загрузка данных о видео",
                              MessageBoxButton.OK, MessageBoxImage.Exclamation);
                          return;
                      }
                      if ((SelectedLevelVM.VideoInfoVM.Source != null) || (SelectedLevelVM.VideoInfoVM.PreviewVM.Source != null) || (SelectedLevelVM.VideoInfoVM.Title != ""))
                      {
                          MessageBoxResult r = MessageBox.Show("В качестве целевого уровня указан уровень с данными. Уверены что хотите перезаписать этот уровень?", "Загрузка данных о видео",
                              MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                          if (r== MessageBoxResult.No) return;
                      }

                      SelectedLevelVM.JoinYoutubeVideoInLevel((string)obj);
                     
                      OnPropertyChanged("LevelVMs");
                  }));
            }
        }

        private RelayCommand addAndJoinVideoCommand;
        public RelayCommand AddAndJoinVideoCommand
        {
            get
            {
                return addAndJoinVideoCommand ??
                  (addAndJoinVideoCommand = new RelayCommand(obj =>
                  {
                      Level l;
                      if (AddCommand.CanExecute(null)) l = Add(null);
                      else return;
                      LevelVM LVM = LevelVMs.Where(lvlvm => lvlvm.id == l.Id).FirstOrDefault();

                     // mainWindow.TabItemEditor.Focus();
                     

                      LVM.JoinYoutubeVideoInLevel((string)obj);

                      OnPropertyChanged("LevelVMs");
                  }));
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
