using LevelSetsEditor.DB;
using LevelSetsEditor.Model;
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


        private ObservableCollection<Level> _levels { get; set; }
        private ObservableCollection<LevelVM> _levelsvm { get; set; }
        private VideoPlayerVM _videoPlayerVM;

        public ObservableCollection<LevelVM> LevelVMs
        {
            get
            {
                _levelsvm = new ObservableCollection<LevelVM>(from l in _levels select new LevelVM(l,_videoPlayerVM));
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
            }

        }













        public Context db;

        public VM(VideoPlayerVM videoPlayerVM)
        {
            _videoPlayerVM = videoPlayerVM;

            _levels = new ObservableCollection<Level>();
            {
                context = new Context();

                IEnumerable<VideoInfo> VI = context.VideoInfoes.OfType<VideoInfo>().Where(n => n.Id < 1000);
                List<VideoInfo> VList = VI.ToList();

                IEnumerable<Preview> Pr = context.Previews.OfType<Preview>().Where(n => n.Id < 1000);
                List<Preview> PList = Pr.ToList();

                IEnumerable<Scene> Sc = context.Scenes.OfType<Scene>().Where(n => n.Id < 1000);
                List<Scene> ScList = Sc.ToList();

                IEnumerable<VideoSegment> Vss = context.VideoSegments.OfType<VideoSegment>().Where(n => n.Id < 1000);
                List<VideoSegment> VssList = Vss.ToList();

                ////То же самое с помощью операции OfType
                IEnumerable<Level> LLL = context.Levels.OfType<Level>().Where(n => n.Id < 1000);
                foreach (Level l in LLL)
                {
                    l.VideoInfo = VList.Where(n => n.Id == l.VideoInfoId).FirstOrDefault();
                    l.VideoInfo.Preview = PList.Where(n => n.Id == l.VideoInfo.PreviewId).FirstOrDefault();
                    var newScenes = l.Scenes.OrderBy(i => i.Id);
                    l.Scenes = new ObservableCollection<Scene>();
                    foreach (Scene s in newScenes)
                    {
                        l.Scenes.Add(s);
                    }
                    foreach (Scene s in l.Scenes)
                    {
                        foreach (VideoSegment v in VssList)
                        {
                            if (s.VideoSegmentId == v.Id)
                            {
                                s.VideoSegment = v;
                            }
                        }
                    }

                    l.RefreshYoutubeLink();
                    _levels.Add(l);
                }

            }
        }


        

        private RelayCommand addCommand;
        public RelayCommand AddCommand
        {
            get
            {
                return addCommand ??
                  (addCommand = new RelayCommand(obj =>
                  {
                      Random random = new Random();
                      Level l = new Level() { Id = _levels.Count + 1, Name = "Level "+ (_levels.Count+1).ToString() };
                      l.VideoInfo = new VideoInfo() { Title = (_levels.Count+1).ToString(), Id = l.Id };
                      l.VideoInfoId = l.VideoInfo.Id;
                      l.VideoInfo.Preview = new Preview() { Source = new Uri(@"C:\Program Files\FuckWinActivator\wallpapers\1.jpg") , Id = l.VideoInfo.Id };
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
                          //context.SaveChanges();
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
                      //    using (Context context = new Context())
                      {
                          if (SelectedLevelVM == null) return;

                          Level level = SelectedLevelVM._level;
                          context.Entry(level).State = EntityState.Deleted;
                        //  context.Levels.Remove(level);
                          //foreach (Level l in _levels)
                          //{
                          //    context.Entry(l).State = EntityState.Modified;
                          //    context.Entry(l.VideoInfo).State = EntityState.Modified;
                          //}
                          context.SaveChanges();

                          _levels.Remove(level);

                          //context.SaveChanges();
                          OnPropertyChanged("LevelVMs");
                          SelectedLevelVM = LevelVMs.FirstOrDefault();
                      }
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
