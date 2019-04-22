using LevelSetsEditor.DB;
using LevelSetsEditor.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LevelSetsEditor.ViewModel
{
    public class VM : INotifyPropertyChanged
    {

        Context context;


        private ObservableCollection<Level> _levels { get; set; }
        private ObservableCollection<LevelVM> _levelsvm { get; set; }

        public ObservableCollection<LevelVM> LevelVMs
        {
            get
            {
                _levelsvm = new ObservableCollection<LevelVM>(from l in _levels select new LevelVM(l));
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

        public VM() 
        {

            context = new Context();

            _levels = new ObservableCollection<Level>();
            using (Context context = new Context())
            {


                var levels = context.Levels.Join(context.VideoInfos, // второй набор
                p => p.VideoInfoId, // свойство-селектор объекта из первого набора
                c => c.Id, // свойство-селектор объекта из второго набора
                (p, c) => new // результат
                {
                    levelId = p.Id,
                    vidId = p.VideoInfoId,
                    Name = p.Name,
                    Title = c.Title,
                });

                foreach (var p in levels)
                {
                    Level l = new Level()
                    {
                        Name = p.Name,
                        Id = p.levelId,
                        VideoInfoId = p.vidId,
                        VideoInfo = new VideoInfo { Title = p.Title, Id = p.vidId }
                    };
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
                      Level l = new Level() { Id = _levels.Count + 1, Name = "Level "+ (_levels.Count+1).ToString() };
                      l.VideoInfo = new VideoInfo() { Title = (_levels.Count+1).ToString(), Id = l.Id };
                      l.VideoInfoId = l.VideoInfo.Id;
                      _levels.Add(l);


                      //context.Levels.Add(l);
                      //context.VideoInfos.Add(l.VideoInfo);
                      //context.SaveChanges();

                      foreach (Level vl in context.Levels)
                      { }

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
                      foreach (Level vl in context.Levels)
                      { }
                      context.SaveChanges();
                      foreach (Level l in _levels)
                      {
                          context.Levels.Add(l);
                          context.Entry(l).State = EntityState.Modified;
                          context.Entry(l.VideoInfo).State = EntityState.Modified;
                      }

                      context.SaveChanges();
                      
                      foreach (Level vl in _levels)
                      { }
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
