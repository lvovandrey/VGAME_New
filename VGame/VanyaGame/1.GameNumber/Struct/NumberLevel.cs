using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using VanyaGame.Struct;
using VanyaGame.Struct.Components;

namespace VanyaGame.GameNumber.Struct
{

    /// <summary>
    /// Данный класс переопределяет функционал стандартного класса. 
    /// По умолчанию ничего не меняется - просто вызываются одноименные методы базового класса
    /// </summary>
    public class NumberLevel : Level
    {



        public NumberLevel(string _LevelDir = @"\Level1") : base()
        {
            Sets = new LevelSets(this);
            Sets.Directory = _LevelDir;
            GetComponent<Loader>().LoadSets = LoadSets;
            GetComponent<Loader>().LoadContent = LoadContent;

            GetComponent<Starter>().StartElements.Add(Start);
        }




        private void LoadSets()
        {
            CreateEmptyScenes();
            string filenameXML = Game.Sets.MainDir + Sets.Directory + @"\LevelSets.xml";
            VanyaGame.XMLTools.LoadSetsFromXML(this, filenameXML);
        }


        /// <summary>
        /// Создает в объекте уровня (TGameLevel) объекты субуровней из текущей директории (указана в Sets.Directory) и
        /// создает из них коллекцию Scenes.
        /// Название папок уровней должно начинаться со строки "Scene"
        /// </summary>
        private void CreateEmptyScenes()
        {
            Scenes.Clear();
            string filenameXML = Game.Sets.MainDir + this.Sets.Directory + @"\LevelSets.xml";

            string dir = Game.Sets.MainDir + this.Sets.Directory + @"\";
            Scene_dirs = Directory.GetDirectories(dir, "Scene*");
            foreach (string Scene_dir in Scene_dirs)
            {
                string Scene_dir_short = @"\" + Scene_dir.Replace(dir, "");
                NumberScene NewScene = new NumberScene(this, Scene_dir_short, Scene_dir.Replace(dir, ""));
                Scenes.Add(Scene_dir_short, NewScene);
            }


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
            Game.Music.PlayRandom(true);
            Game.Music.Pause();

            CurScene.GetComponent<Starter>().Start();

        }




        public void LoadMedia()
        {
            Game.Sound.LoadMediaFilesFromDir(Game.Sets.MainDir + Sets.Directory + Sets.SoundDir + @"\");
            Game.Music.LoadMediaFilesFromDir(Game.Sets.MainDir + Sets.Directory + Sets.MusicDir + @"\");
            Game.CurVideo.LoadMediaFilesFromDir(Game.Sets.MainDir + Sets.Directory + Sets.VideoDir + @"\");
        }

        /// <summary>
        /// Дает команду объекту игры Game загрузить фон из файла. 
        /// Папку в которой искать файл определяет самостоятельно по объектам Game.Sets и Level.Sets 
        /// </summary>
        /// <param name="fileshortname">Имя файла фона. По умолч."back.jpg"</param>
        public void LoadBackground(string fileshortname = @"back.jpg")
        {
            string BackGrndFilepath = Game.Sets.MainDir + Sets.Directory + Sets.InterfaceDir + @"\" + fileshortname;
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
        public string[] Scene_dirs { get; private set; }

        /// <summary>
        /// Переключение на следующую сцену
        /// </summary>
        internal void NextScene()
        {
            SceneNomer++;
            if ((SceneNomer <= Scene_dirs.GetUpperBound(0)) && (SceneNomer >= Scene_dirs.GetLowerBound(0)))
            {
                string dir = Game.Sets.MainDir + this.Sets.Directory + @"\";
                string SceneKey = @"\" + Scene_dirs[SceneNomer].Replace(dir, "");
                CurScene = Scenes[SceneKey];
                CurScene.GetComponent<Starter>().Start();
            }
            else if (SceneNomer > Scene_dirs.GetUpperBound(0))
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
            string dir = Game.Sets.MainDir + this.Sets.Directory + @"\";
            string SceneKey = @"\" + Scene_dirs[SceneNomer].Replace(dir, "");
            CurScene = Scenes[SceneKey];
        }



    }
}
