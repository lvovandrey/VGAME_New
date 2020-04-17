using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

using VanyaGame.GameNumberDB.DB;
using VanyaGame.Struct;
using VanyaGame.Struct.Components;

namespace VanyaGame.GameNumberDB.Struct
{

    /// <summary>
    /// Данный класс переопределяет функционал стандартного класса. 
    /// По умолчанию ничего не меняется - просто вызываются одноименные методы базового класса
    /// </summary>
    public class NumberDBLevel : Level
    {
        private LevelSetsEditor.Model.Level DbLevelRecord;

        public NumberDBLevel(string _LevelDir = @"\Level1") : base()
        {
            Sets = new LevelSets(this);
            Sets.Directory = _LevelDir;
            GetComponent<Loader>().LoadSets = LoadSets;
            GetComponent<Loader>().LoadContent = LoadContent;

            GetComponent<Starter>().StartElements.Add(Start);
        }

        public NumberDBLevel(LevelSetsEditor.Model.Level DbLevelRecord) : base()
        {
            this.DbLevelRecord = DbLevelRecord;
            Sets = new LevelSets(this);
            
           // Sets.Directory = "NONE";

            GetComponent<Loader>().LoadSets = LoadSets;
            GetComponent<Loader>().LoadContent = LoadContent;

            GetComponent<Starter>().StartElements.Add(Start);
        }

        public static void LoadLevels(DBmainTools DB)
        {
            
            DB.LoadDB(new System.Collections.ObjectModel.ObservableCollection<LevelSetsEditor.Model.Level>(), DB.Context);


           
            Random random = new Random();

            //Перемешиваем уровни  //что за алгоритм - хз - раньше работал вроде
            for (int i = DB.Levels.Count - 1; i >= 1; i--)
            {
                int j = random.Next(i+1);
                var tmpLevel = DB.Levels[j];
                DB.Levels[j] = DB.Levels[i];
                DB.Levels[i] = tmpLevel;
            }

            foreach (var level in DB.Levels)
            {
                
                Level NewLevel = new NumberDBLevel(level);
                
                Game.Levels.Enqueue(NewLevel);
            }


       //     throw new NotImplementedException("NumberDBLevel.LoadLevels()");
        }


        private void LoadSets()
        {
           // CreateEmptyScenes();
            // string filenameXML = Game.Sets.MainDir + Sets.Directory + @"\LevelSets.xml";
            LoadSetsFromDBRecord(this);
        }


        public static void LoadSetsFromDBRecord(NumberDBLevel Level)
        {
            Level.Scenes.Clear();

          
            //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            //Level.Name = Level.DbLevelRecord.Name;
            //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            Level.Sets.Description = Level.DbLevelRecord.VideoInfo.Title;
            Level.Sets.Name = Level.DbLevelRecord.Name;
            Level.Sets.PreviewType = Level.DbLevelRecord.VideoInfo.Preview.Type.ToString();
            Level.Sets.BaseVideoFilename = Level.DbLevelRecord.VideoInfo.Address;


            foreach (var Scene in Level.DbLevelRecord.Scenes)
            {
                Scene NewScene = new NumberDBScene(Level, Scene, "Scene" + Scene.Id.ToString());
                NewScene.Sets.GetComponent<InnerVideoSets>().VideoFileName = Level.DbLevelRecord.VideoInfo.Address;
                NewScene.Sets.GetComponent<InnerVideoSets>().VideoFileType = Tools.ConvertDBTools.VideoTypeConvertFromDB(Level.DbLevelRecord.VideoInfo.Type);
                NewScene.Sets.GetComponent<InnerVideoSets>().VideoFileNumber = Level.DbLevelRecord.Scenes.IndexOf(Scene).ToString();
                NewScene.Sets.GetComponent<InnerVideoSets>().VideoTimeBegin = Scene.VideoSegment.TimeBegin;
                NewScene.Sets.GetComponent<InnerVideoSets>().VideoTimeEnd = Scene.VideoSegment.TimeEnd;
                Level.Scenes.Add(NewScene.Name, NewScene);
            }


            
        }





        /// <summary>
        /// Создает в объекте уровня (TGameLevel) объекты субуровней из текущей директории (указана в Sets.Directory) и
        /// создает из них коллекцию Scenes.
        /// Название папок уровней должно начинаться со строки "Scene"
        /// </summary>
        private void CreateEmptyScenes()
        {
            //MessageBox.Show("Остановился тут - NumberDBLevel.CreateEmptyScenes!!!!!!");

            //Scenes.Clear();
            ////string filenameXML = Game.Sets.MainDir + this.Sets.Directory + @"\LevelSets.xml";

            ////  string dir = Game.Sets.MainDir + this.Sets.Directory + @"\";
            ////  Scene_dirs = Directory.GetDirectories(dir, "Scene*");

            //foreach (var scene in DbLevelRecord.Scenes)
            //{
            //    //string Scene_dir_short = @"\" + Scene_dir.Replace(dir, "");

            //    NumberDBScene NewScene = new NumberDBScene(this, scene, scene.Name);
            //    Scenes.Add(NewScene.Name, NewScene);
            //}


        }

        private void LoadContent()
        {
            LoadBackground();
            LoadPreview();            
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
           // Game.CurVideo.LoadMediaFilesFromDir(Game.Sets.MainDir + Sets.Directory + Sets.VideoDir + @"\");
        }

        /// <summary>
        /// Дает команду объекту игры Game загрузить фон из файла. 
        /// Папку в которой искать файл определяет самостоятельно по объектам Game.Sets и Level.Sets 
        /// </summary>
        /// <param name="fileshortname">Имя файла фона. По умолч."back.jpg"</param>
        public void LoadBackground(string fileshortname = @"back.jpg")
        {
            string BackGrndFilepath = Game.Sets.MainDir +  Sets.InterfaceDir + @"\" + fileshortname;
            try
            {
                Game.LoadBackGround(BackGrndFilepath);
            }
            catch
            {
                MessageBox.Show("Не могу загрузить файл фона");
            }
        }

        /// <summary>
        /// Дает команду объекту игры Game загрузить превью уровня из файла. 
        /// Папку в которой искать файл определяет самостоятельно по объектам Game.Sets и Level.Sets 
        /// </summary>
        /// <param name="fileshortname">Имя файла фона. По умолч."back.jpg"</param>
        public void LoadPreview(string fileshortname = @"preview.jpg")
        {
            if (Sets.PreviewType == VideoType.youtube.ToString())
            {
                string filename;
                try { filename = YouTubeUrlSupplier.YoutubeGet.GetImage(Sets.BaseVideoFilename); }
                catch { filename = Game.Sets.MainDir + @"\default.jpg"; Sets.PreviewType = "local"; }
                Game.LoadPreview(filename);
                return;
            }

            string Filepath = Game.Sets.MainDir + Sets.Directory + Sets.InterfaceDir + @"\" + fileshortname;
            try
            {
                Game.LoadPreview(Filepath);
            }
            catch
            {
                MessageBox.Show("Не могу загрузить файл превью");
            }
        }

        public int SceneNomer { get; private set; }
       
        public static object DBMainTools { get; private set; }

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
