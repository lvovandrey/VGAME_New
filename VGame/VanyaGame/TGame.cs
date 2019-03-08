﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using VanyaGame.PrevMenuNS;
using VanyaGame.GameNumber.Struct;
using VanyaGame.Struct;
using VanyaGame.Struct.Components;
using VanyaGame.Media;
using VanyaGame.Media.Abstract;
using VanyaGame.Abstract;

namespace VanyaGame
{
    public delegate void VoidDelegate ();
    public delegate bool BoolDelegate();
    public delegate object FullFreeDelegate(params object[] vs);
    public delegate void TUserDoSomething(MouseEventArgs mouse, MouseButtonEventArgs mousebutton, KeyEventArgs key);
    public delegate bool BoolUserDoSomething(MouseEventArgs mouse, MouseButtonEventArgs mousebutton, KeyEventArgs key);


    public delegate void SenderDelegate(object sender);
    public enum GameType
    {
        Fish,
        Number,
        None
    }

    public static class Game
    {
        public static Random RandomGenerator;
        public static TGameSets Sets;
        public static Queue<Level> Levels = new Queue<Level>();
        public static Level Level;
        public static GameSound Sound;
        public static GameMusic Music;
        public static GameVideo CurVideo;
        public static GameVideo VideoVlc;
        public static GameVideo VideoWpf;
        public static  MainWindow Owner;
        public static bool IsPlaying = false;
        public static TDrawEffects DrawEffects = new TDrawEffects();
        public static TUserActivity UserActivity = new TUserActivity();

        private static ComponentContainer mediaContainer;


        private static bool hasCursor = true;
        /// <summary>
        /// Какое-то мутное свойство по поводу курсора у игры как таковой...
        /// </summary>
        public static bool HasCursor
        {
            get
            {
                return hasCursor;
            }

            set
            {
                hasCursor = value;
                if (value)
                {

                    //это альтернативная затычка
                    Game.Owner.Cursor = Cursors.Hand;
                }
                   // ПРИСВОИТЬ КУРСОР!!!!
                   // Game.Owner.Cursor = Level.CurScene.Sets.Cursor;
                else Game.Owner.Cursor = Cursors.None;
            }
        }

        

        public static void Msg(string s)
        {
          Owner.TxtBlock.Text += "\r\n" + s; 
        }
        public static void MsgClear()
        {
            Owner.TxtBlock.Text = "";
        }
        
        public static double TimeDiagnostic;



        public static void LoadLevels(string dir) 
        {
            string dir_ = dir + @"\";
            string[] Level_dirs = Directory.GetDirectories(dir_, "Level*");
            
            Random random = new Random();

            //Перемешиваем уровни
            for (int i = Level_dirs.Length - 1; i >= 1; i--)
            {
                int j = random.Next(i + 1);
                // обменять значения data[j] и data[i]
                string temp = Level_dirs[j];
                Level_dirs[j] = Level_dirs[i];
                Level_dirs[i] = temp;
            }
            
            foreach (string Level_dir in Level_dirs)
            {
                string Level_dir_short = Level_dir.Replace(dir, "");
                Level NewLevel = null;
                if (Game.Sets.gameType == GameType.Number)
                {
                    NewLevel = new NumberLevel(Level_dir_short);
                }
                Levels.Enqueue(NewLevel);
            }
        }

        public static void VideoPlayerSet(GameVideo gameVideo)
        {
            if (CurVideo == null) return;
            if (CurVideo.IsPlaying || CurVideo.IsPaused) {
                MessageBox.Show("Cannt change videoplayer while it play");
                return; }
            gameVideo.MediaGUI.Clear();


            gameVideo.MediaGUI.Add(Owner.BackImgButton);
            gameVideo.MediaGUI.Add(Owner.NextImgButton);
            gameVideo.MediaGUI.Add(Owner.VideoVolumeSlider);
            gameVideo.MediaGUI.Add(Owner.VideoTimeSlider);
            gameVideo.MediaGUI.Add(Owner.PlayImgButton);

            gameVideo.SliderTimer = Owner.VideoTimeSlider;
            gameVideo.PlayBtn = Owner.PlayImgButton;
            Game.Owner.VideoVolumeSlider.DataContext = gameVideo.volume;
            Game.Owner.VideoTimeSlider.DataContext = gameVideo.timer;
            
            CurVideo = gameVideo;
            CurVideo.MediaGUI.UIMediaHide();
            Game.Msg("VideoPlayer change");
        }

        public static void Create(MainWindow WND_Owner)
        {
            Owner = WND_Owner;
            RandomGenerator = new Random();
            Sets = new TGameSets();
            mediaContainer = new GameMediaContainer();

            Player Snd = new PlayerWpf("PlayerSound", mediaContainer, Owner.MediaElementSound);
            Player Msc = new PlayerWpf("PlayerMusic", mediaContainer, Owner.MediaElementSound);
            
            Sound = new GameSound(ref Snd);
            Music = new GameMusic(ref Msc);

            Player Vdovlc = new PlayerVlc("PlayerVideoVlc", mediaContainer, Owner.Vlc);
            VideoVlc = new GameVideo(ref Vdovlc);

            Player Vdowpf = new PlayerWpf("PlayerVideoWpf", mediaContainer, Owner.MediaElementVideo);
            VideoWpf = new GameVideo(ref Vdowpf);

            CurVideo = new GameVideo(ref Vdowpf);


            Music.MediaGUI.Add(Owner.MusicVolumeSlider);
            Game.Owner.MusicVolumeSlider.DataContext = Music.volume;
            Music.MediaGUI.UIMediaHide();

            if (Sets.VideoPlayer == "vlc")
            {
                VideoPlayerSet(VideoVlc);
            }
            if (Sets.VideoPlayer == "wpf")
            {
                VideoPlayerSet(VideoWpf);
            }

        }

        
        /// <summary>
        /// Грузит в главный Grid окна владельца Owner фон из картинки
        /// </summary>
        /// <param name="BackgroundFilename">Путь к файлу картинки на диске</param>
        public static void LoadBackGround(string BackgroundFilename)
        {
            ImageBrush im = new ImageBrush();
            im.ImageSource =  new BitmapImage(new Uri(BackgroundFilename));
            Owner.GridMain.Background = im;
        }

        /// <summary>
        /// Грузит в ImgPrev окна владельца Owner превью из картинки
        /// </summary>
        /// <param name="BackgroundFilename">Путь к файлу картинки на диске</param>
        public static void LoadPreview(string Filename)
        {
            System.Drawing.Bitmap myBitmap = new System.Drawing.Bitmap(@Filename);
            Owner.PrevImg.Source = Imaging.CreateBitmapSourceFromBitmap(myBitmap);
        }


        /// <summary>
        /// Переключает игру на следующий уровень
        /// </summary>
        public static void NextLevel()
        {
            if (Levels.Count > 0)
            {
                Level = Levels.Dequeue();

                Level.GetComponent<Loader>().Load();
                Level.GetComponent<Starter>().Start();




                // Level.Start();
            }
            else
            {
                GameOver();
                Msg("GAME OVER");
            }
        }
        public static void GameOver() 
        {
            LoadBackGround(Sets.InterfaceDirCurVersion + @"\backgrounds\backEnd.jpg");

            //   TDrawEffects.BlurHide(Owner, 5, 10, () => { Owner.Close(); });
        }
        public static void PreviewStart()
        {
            LoadBackGround(Sets.InterfaceDirCurVersion + @"\backgrounds\backBegin.jpg");

            Owner.StartButton.Visibility = Visibility.Visible; Owner.StartButton.Opacity = 1;
            //            TDrawEffects.BlurShow(Owner.StartButton, 0.5);


            /// ??? двойной вызов с ImageBegin_MouseUp   
            Owner.StartButton.MouseUp += StartButton_MouseUp;
           
        }


         //??? двойной вызов с ImageBegin_MouseUp   
        static void StartButton_MouseUp(object sender, RoutedEventArgs e)
        {
            if (sender.GetType() == typeof(VanyaGame.GameNumber.Interface.BeautyButtonNumber))
                Sets.gameType = GameType.Number;


            ToolsTimer.Delay(() =>
            {
                Game.LoadLevels(Game.Sets.MainDir);

                TDrawEffects.BlurHide((FrameworkElement)Game.Owner.StartButton, 0.1, 0, () =>
                {
                    ((FrameworkElement)Game.Owner.StartButton).BeginAnimation(FrameworkElement.OpacityProperty, null);
                    Game.PrevMenuShow();
                });
                foreach (Level Level in Game.Levels)
                {
                    Level.GetComponent<Loader>().LoadSets();
                    string filename = Game.Sets.MainDir + Level.Sets.Directory + Level.Sets.InterfaceDir + @"\preview.jpg";
                    PrevMenuNS.PrevMenuItem NewItem = new PrevMenuNS.PrevMenuItem(filename, Level);
                    Owner.PreviewMenu.AddItem(NewItem, ItemClick);
                }
            }, TimeSpan.FromSeconds(1.5));

        }

        public static void PrevMenuShow()
        {
            Game.LoadBackGround(Game.Sets.InterfaceDirCurVersion + @"\backgrounds\sky.jpg");
            Game.Owner.PreviewMenu.Visibility = Visibility.Visible;
        }

        public static void ItemClick(object sender)
        {
            if (Game.Levels.Count > 0)
            {
                    Game.Level = ((PrevMenuItem)sender).Level;
                    ((PrevMenuItem)sender).Level.GetComponent<Loader>().Load();
                    Game.Owner.PreviewMenu.Visibility = Visibility.Collapsed;
                    Level.GetComponent<Starter>().Start();
            }
        }
    
    }

    public class TGameSets
    {
        public bool DebugMode;
        public GameType gameType = GameType.None;
        public string MainDir;
        public string VideoPlayer;
        public string InterfaceDir;
        public string InterfaceDirCurVersion;

        public int LevelsCount; //Количество уровней
        public TGameSets(string _MainDir = @"\VanjaGame"):base()
        {
            // MainDir = Directory.GetCurrentDirectory() + _MainDir;
            MainDir = XMLTools.LoadFromXML(Directory.GetCurrentDirectory() + @"\GameSets.xml", "maindir");  //(@"c:\1.VANYA GAME\LEVELS" + @"\GameSets.xml", "maindir");
            VideoPlayer = XMLTools.LoadFromXML(Directory.GetCurrentDirectory() + @"\GameSets.xml", "videoplayer");
            InterfaceDir = MainDir + @"\interface";
            InterfaceDirCurVersion = InterfaceDir + @"\Numbers";
            LevelsCount = 0;
            DebugMode = true;
        }
        public double InterfaceDefaultOpacity = 0;
    }


}
