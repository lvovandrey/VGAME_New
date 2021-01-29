using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using VanyaGame.Struct;
using VanyaGame.Struct.Components;
using System.Collections.ObjectModel;
using VanyaGame.GameCardsNewDB.Tools;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using VanyaGame.PrevMenuNS;
using Model = CardsGameNewDBRepository.Model;
using CardsGameNewDBRepository.Model;
using CardsGameNewDBRepository;

namespace VanyaGame.GameCardsNewDB.Struct
{

    /// <summary>
    /// Данный класс переопределяет функционал стандартного класса. 
    /// По умолчанию ничего не меняется - просто вызываются одноименные методы базового класса
    /// </summary>
    public class CardsNewDBLevel : VanyaGame.Struct.Level, INotifyPropertyChanged
    {
        public Model.Level DbLevelRecord;

        public ObservableCollection<LevelPassing> LevelPassings { get 
            {
                if (DbLevelRecord._LevelPassings == null) DbLevelRecord._LevelPassings = new ObservableCollection<LevelPassing>();
                return DbLevelRecord._LevelPassings; 
            } 
            set { DbLevelRecord._LevelPassings = value; OnPropertyChanged("LevelPassings"); } }
        public int LevelPassingsCount { get { if (LevelPassings != null) return LevelPassings.Count; else return 0; } }
        public LevelPassing CurLevelPassing;
        public int? CardsCount { get { return DbLevelRecord?.Cards?.Count; } }
        public double AvgCardsErrorsPercentInLast3LevelPassings { get { return GetAvgCardsErrorsPercentInLast3LevelPassings(); } }

        private double GetAvgCardsErrorsPercentInLast3LevelPassings()
        {
            var AllCardPassings = LevelPassings.Select(lp => new
            {
                CardPassings = lp.CardPassings,
                DateAndTime = GetDateTime(lp.DateAndTime)
            });

            var Last3LevelPassings = AllCardPassings.OrderByDescending(lp => lp.DateAndTime).Take(3);

            List<double> CardsErrorPercents = new List<double>();
            foreach (var lp in Last3LevelPassings)
            {
                double cardsErrorsSumm = 0;
                double cardsPassesSumm = 0;
                foreach (var p in lp.CardPassings)
                {
                    cardsPassesSumm++;
                    if (p.AttemptsNumber > 1) cardsErrorsSumm++;
                }
                if (cardsPassesSumm != 0)
                    CardsErrorPercents.Add(cardsErrorsSumm / cardsPassesSumm);
            }
            if (CardsErrorPercents.Count > 0) return CardsErrorPercents.Average();
            else return 0;
        }

        private DateTime GetDateTime(string dateAndTime)
        {
            DateTime dt;
            if (DateTime.TryParse(dateAndTime, out dt)) return dt;
            else return new DateTime(2000, 1, 1);
        }

        public CardsNewDBLevel(Model.Level _DbLevelRecord) : base()
        {
            DbLevelRecord = DBTools.Context.Levels.Find(_DbLevelRecord.Id);
            if (DbLevelRecord == null) DbLevelRecord = _DbLevelRecord;
            Sets = new LevelSets(this);

            GetComponent<Loader>().LoadSets = LoadSets;
            GetComponent<Loader>().LoadContent = LoadContent;
            GetComponent<Starter>().StartElements.Add(Start);
        }

        private static async Task<bool> LoadDBAsync()
        {
            Console.WriteLine("StartLoading DB");

            Game.Owner.Dispatcher.Invoke(() => { Game.Owner.ProgressBarLoadDB.Visibility = Visibility.Visible; }); 
            bool result =  await Task.Run(() => DBTools.LoadDB(new ObservableCollection<Model.Card>(), 
                                                       new ObservableCollection<Model.Level>(), 
                                                       new ObservableCollection<Model.LevelPassing>(), 
                                                       Settings.GetInstance().AttachedDBCardsFilename));
            Game.Owner.Dispatcher.Invoke(() => { Game.Owner.ProgressBarLoadDB.Visibility = Visibility.Collapsed; });
            Console.WriteLine("End Loading DB");
            return result;
        }

        public static void AddLevelsInPrevMenu() 
        {
            Console.WriteLine("Add levels in PrevMenu");
            foreach (VanyaGame.Struct.Level Level in Game.Levels)
            {
                Level.GetComponent<Loader>().LoadSets();
                string filename = VanyaGame.Sets.Settings.GetInstance().DefaultImage;

                if (Game.gameType == GameType.CardsNewDB)
                    filename = ((CardsNewDBLevel)Level).Sets.PreviewURL;

                PrevMenuItem NewItem = new PrevMenuNS.PrevMenuItem(filename, Level, "youtube");
                Game.Owner.PreviewMenu.AddItem(NewItem, Game.ItemClick);
            }
        }

        public static async void LoadLevels()
        {
            Settings.GetInstance().RestoreAllSettings();

            await LoadDBAsync();
            

             Random random = new Random();

            //Перемешиваем уровни  //что за алгоритм - хз - раньше работал вроде
            //for (int i = DB.Levels.Count - 1; i >= 1; i--)
            //{
            //    int j = random.Next(i+1);
            //    var tmpLevel = DB.Levels[j];
            //    DB.Levels[j] = DB.Levels[i];
            //    DB.Levels[i] = tmpLevel;
            //}
            Console.WriteLine("Repack Levels");
            foreach (var level in DBTools.Context.Levels)
            {
                VanyaGame.Struct.Level NewLevel = new CardsNewDBLevel(level);
                Game.Levels.Enqueue(NewLevel);
            }
            Console.WriteLine("End repack Levels");

            AddLevelsInPrevMenu();
        }


        private void LoadSets()
        {
            LoadSetsFromDBRecord(this);
        }


        public static void LoadSetsFromDBRecord(CardsNewDBLevel Level)
        {
            Level.Scenes.Clear();
            Level.Sets.Name = Level.DbLevelRecord.Name;
            Level.Sets.Description = Level.Sets.Name;
            Level.Sets.PreviewURL = Level.DbLevelRecord.ImageAddress;
            Level.Sets.PreviewType = "local";
            Scene NewScene = new CardsNewDBScene(Level);
            Level.Scenes.Add(NewScene.Name, NewScene);
        }


        private void LoadContent()
        {
            LoadBackground();
        }

        private void Start()
        {
            CurLevelPassing = new LevelPassing() { DateAndTime = DateTime.Now.ToString() };
            LevelPassings.Add(CurLevelPassing);
            DBTools.Context.Entry(DbLevelRecord).State = System.Data.Entity.EntityState.Modified;
            DBTools.Context.SaveChanges();
            OnPropertyChanged("LevelPassingsCount");
            OnPropertyChanged("CardsCount");
            OnPropertyChanged("AvgCardsErrorsPercentInLast3LevelPassings");

            int id = DbLevelRecord.Id;
            var RefreshedDbLevelRecord = DBTools.Context.Levels.Find(id);
            if (RefreshedDbLevelRecord != null) DbLevelRecord = RefreshedDbLevelRecord;

            LoadMedia();
            LoadScenes();
            if (Settings.GetInstance().ShuffleMusic) Game.Music.PlayRandom(Settings.GetInstance().RepeatMusicPlaylist);
            else Game.Music.PlayInOrder(Settings.GetInstance().RepeatMusicPlaylist);
            Game.Music.Pause();

            CurScene.GetComponent<Starter>().Start();
            Game.Owner.AbortLevelButton.Visibility = Visibility.Visible;
        }




        public void LoadMedia()
        {
            Game.Music.LoadMediaFiles(new List<string>(Settings.GetInstance()._MusicFilenames));
        }

        /// <summary>
        /// Дает команду объекту игры Game загрузить фон из файла. 
        /// Папку в которой искать файл определяет самостоятельно по объектам Game.Sets и Level.Sets 
        /// </summary>
        /// <param name="fileshortname">Имя файла фона. По умолч."back.jpg"</param>
        public void LoadBackground(string fileshortname = @"backBegin.jpg")
        {
            try
            {
                Game.LoadBackGround(Settings.GetInstance().BackgroundFilename);
            }
            catch
            {
                MessageBox.Show("Не могу загрузить файл фона");
            }
        }


        public int SceneNomer { get; private set; }

        /// <summary>
        /// Переключение на следующую сцену
        /// </summary>
        internal void NextScene()
        {
            SceneNomer++;
            List<string> ScenesKeys = new List<string>();
            foreach (KeyValuePair<string, Scene> Sc in Scenes)
            {
                ScenesKeys.Add(Sc.Key);
            }
            if ((SceneNomer <= ScenesKeys.Count - 1) && (SceneNomer >= 0))
            {
                CurScene = Scenes[ScenesKeys[SceneNomer]];
                CurScene.GetComponent<Starter>().Start();
            }
            else if (SceneNomer > ScenesKeys.Count - 1)
            {
                End(true);
            }
        }

        private void End(bool IsLevelComplete)
        {
            CurLevelPassing.IsComplete = IsLevelComplete;
            DBTools.Context.Entry(DbLevelRecord).State = System.Data.Entity.EntityState.Modified;
            DBTools.Context.SaveChanges();

            OnPropertyChanged("LevelPassingsCount");
            OnPropertyChanged("CardsCount");
            OnPropertyChanged("AvgCardsErrorsPercentInLast3LevelPassings");

            this.Scenes.Clear();
            Game.PrevMenuShow();
            Game.Owner.AbortLevelButton.Visibility = Visibility.Collapsed;
        }





        /// <summary>
        /// Загружает сцены с помощью комонента Loader и 
        /// </summary>
        public void LoadScenes()
        {
            if (Scenes.Count == 0) Scenes.Add("Scene 1", new CardsNewDBScene(this));


            foreach (KeyValuePair<string, Scene> Sc in Scenes)
            {
                Sc.Value.GetComponent<Loader>().Load();
            }
            SceneNomer = 0;
            List<string> ScenesKeys = new List<string>();
            foreach (KeyValuePair<string, Scene> Sc in Scenes)
            {
                ScenesKeys.Add(Sc.Key);
            }

            CurScene = Scenes[ScenesKeys[0]];
        }


        public override void Abort()
        {
            SceneNomer = int.MaxValue;
            CurScene.Abort();
            End(false);
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
