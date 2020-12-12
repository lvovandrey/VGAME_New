using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using VanyaGame.DB;
using VanyaGame.Struct;
using VanyaGame.Struct.Components;
using DBModel = VanyaGame.DB.DBLevelsRepositoryModel;
using DBCardsModel = VanyaGame.DB.DBCardsRepositoryModel;
using System.Collections.ObjectModel;
using VanyaGame.GameCardsNewDB.Tools;
using VanyaGame.GameCardsNewDB.DB;


namespace VanyaGame.GameCardsNewDB.Struct
{

    /// <summary>
    /// Данный класс переопределяет функционал стандартного класса. 
    /// По умолчанию ничего не меняется - просто вызываются одноименные методы базового класса
    /// </summary>
    public class CardsNewDBLevel : Level
    {
        public DB.RepositoryModel.Level DbLevelRecord;


        public CardsNewDBLevel(DB.RepositoryModel.Level DbLevelRecord) : base()
        {
            this.DbLevelRecord = DbLevelRecord;
            Sets = new LevelSets(this);
            
           // Sets.Directory = "NONE";

            GetComponent<Loader>().LoadSets = LoadSets;
            GetComponent<Loader>().LoadContent = LoadContent;
            GetComponent<Starter>().StartElements.Add(Start);
        }

        public static void LoadLevels()
        {
            Settings.RestoreAllSettings();

            DBTools.LoadDB(new ObservableCollection<DB.RepositoryModel.Card>(), new ObservableCollection<DB.RepositoryModel.Level>(),  Settings.AttachedDBCardsFilename);
           
           
            Random random = new Random();

            //Перемешиваем уровни  //что за алгоритм - хз - раньше работал вроде
            //for (int i = DB.Levels.Count - 1; i >= 1; i--)
            //{
            //    int j = random.Next(i+1);
            //    var tmpLevel = DB.Levels[j];
            //    DB.Levels[j] = DB.Levels[i];
            //    DB.Levels[i] = tmpLevel;
            //}

            foreach (var level in DB.DBTools.Levels)
            {
                Level NewLevel = new CardsNewDBLevel(level);
                Game.Levels.Enqueue(NewLevel);
            }

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
            LoadMedia();
            LoadScenes();
            Game.Music.PlayRandom();
            Game.Music.Pause();

            CurScene.GetComponent<Starter>().Start();
        }




        public void LoadMedia()
        {
            Game.Sound.LoadMediaFilesFromDir(Game.Sets.MainDir +  Sets.SoundDir + @"\");
            Game.Music.LoadMediaFilesFromDir(Game.Sets.MainDir +  Sets.MusicDir + @"\");
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
                Game.LoadBackGround(Settings.BackgroundFilename);
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
            if ((SceneNomer <= ScenesKeys.Count-1) && (SceneNomer >= 0))
            {
                CurScene = Scenes[ScenesKeys[SceneNomer]];
                CurScene.GetComponent<Starter>().Start();
            }
            else if (SceneNomer > ScenesKeys.Count - 1)
            {
                End();
            }
        }

        private void End()
        {
            this.Scenes.Clear();
            Game.PrevMenuShow();
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



    }
}
