using System;
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
using VanyaGame.GameNumberDB.Struct;
using VanyaGame.GameCardsEasyDB.Struct;
using VanyaGame.DB;
using VanyaGame.GameCardsNewDB.Struct;

namespace VanyaGame
{
    public delegate void VoidDelegate();
    public delegate bool BoolDelegate();
    public delegate object FullFreeDelegate(params object[] vs);
    public delegate void TUserDoSomething(MouseEventArgs mouse, MouseButtonEventArgs mousebutton, KeyEventArgs key);
    public delegate bool BoolUserDoSomething(MouseEventArgs mouse, MouseButtonEventArgs mousebutton, KeyEventArgs key);


    public delegate void SenderDelegate(object sender);
    public enum GameType
    {
        Fish,
        Number,      
        NumberDB,
        CardsEasy,
        CardsNewDB,
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
        public static Window SettingsWindow;
        public static bool IsPlaying = false;
        public static TDrawEffects DrawEffects = new TDrawEffects();
        public static TUserActivity UserActivity = new TUserActivity();

        public static DBmainTools DBTools = new DBmainTools();

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


        internal static void CreateSettingsWindow()
        {

            if (Game.Sets.gameType == GameType.CardsEasy)
            {
                SettingsWindow = new GameCardsEasyDB.Interface.SettingsWindow();
                var SW = SettingsWindow as GameCardsEasyDB.Interface.SettingsWindow;
                SW.Owner = Owner;
                new GameCardsEasyDB.Interface.SettingsWindowVM(SW);
            }

            if (Game.Sets.gameType == GameType.CardsNewDB)
            {
                SettingsWindow = new GameCardsNewDB.Interface.SettingsWindow();
                var SW = SettingsWindow as GameCardsNewDB.Interface.SettingsWindow;
                SW.Owner = Owner;
                new GameCardsNewDB.Interface.SettingsWindowVM(SW, SW.MusicSettings.MusicFilenamesListView);
            }
        }

        internal static void ShowSettingsWindow()
        {
                SettingsWindow?.Show();
        }

        public static void LoadLevels(string dir) 
        {
            if (Game.Sets.gameType == GameType.NumberDB)
            {
                DBTools = new DBmainTools();
                NumberDBLevel.LoadLevels(DBTools);
                return;
            }

            if (Game.Sets.gameType == GameType.CardsEasy)
            {
                DBTools = new DBmainTools();
                CardsEasyDBLevel.LoadLevels(DBTools);
                

                return;
            }

            if (Game.Sets.gameType == GameType.CardsNewDB)
            {
                CardsNewDBLevel.LoadLevels();
                return;
            }



            string dir_ = dir + @"\";
            string[] Level_dirs = Directory.GetDirectories(dir_, "Level*");
            
            //Random random = new Random();

            ////Перемешиваем уровни
            //for (int i = Level_dirs.Length - 1; i >= 1; i--)
            //{
            //    int j = random.Next(i + 1);
            //    // обменять значения data[j] и data[i]
            //    string temp = Level_dirs[j];
            //    Level_dirs[j] = Level_dirs[i];
            //    Level_dirs[i] = temp;
            //}
            
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

            VanyaGame.Sets.Settings.RestoreAllSettings();
            Owner = WND_Owner;
            RandomGenerator = new Random();
            Sets = new TGameSets();

            if (Owner.StartButton.GetType() == typeof(VanyaGame.GameNumber.Interface.BeautyButtonNumber))
                Sets.gameType = GameType.Number;

            if (Owner.StartButton.GetType() == typeof(VanyaGame.GameNumberDB.Interface.BeautyButtonNumberDB))
                Sets.gameType = GameType.NumberDB;

            if (Owner.StartButton.GetType() == typeof(VanyaGame.GameCardsEasyDB.Interface.BeautyButtonCardsEasyDB))
                Sets.gameType = GameType.CardsEasy;

            if (Owner.StartButton.GetType() == typeof(VanyaGame.GameCardsNewDB.Interface.BeautyButtonCardsNewDB))
                Sets.gameType = GameType.CardsNewDB;

            CreateSettingsWindow();
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
//            System.Drawing.Bitmap myBitmap = new System.Drawing.Bitmap(@Filename);
            Owner.PrevImg.Source = new BitmapImage(new Uri(Filename));// Imaging.CreateBitmapSourceFromBitmap(myBitmap);
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
            LoadBackGround(VanyaGame.Sets.Settings.BackgroundGameOverFilename);
        }
        public static void PreviewStart()
        {
           
            LoadBackGround(VanyaGame.Sets.Settings.BackgroundStartFilename);

            Owner.StartButton.Visibility = Visibility.Visible; Owner.StartButton.Opacity = 1;

            /// ??? двойной вызов с ImageBegin_MouseUp   
            Owner.StartButton.MouseUp += StartButton_MouseUp;
            Owner.StartButton2.MouseUp += StartButton_MouseUp;
            Owner.StartButton3.MouseUp += StartButton_MouseUp;
        }


         //??? двойной вызов с ImageBegin_MouseUp   
        static void StartButton_MouseUp(object sender, RoutedEventArgs e)
        {
            if (sender.GetType() == typeof(VanyaGame.GameNumber.Interface.BeautyButtonNumber))
                Sets.gameType = GameType.Number;

            if (sender.GetType() == typeof(VanyaGame.GameNumberDB.Interface.BeautyButtonNumberDB))
                Sets.gameType = GameType.NumberDB;

            if (sender.GetType() == typeof(VanyaGame.GameCardsEasyDB.Interface.BeautyButtonCardsEasyDB))
                Sets.gameType = GameType.CardsEasy;

            if (sender.GetType() == typeof(VanyaGame.GameCardsNewDB.Interface.BeautyButtonCardsNewDB))
                Sets.gameType = GameType.CardsNewDB;

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
                    string filename = Game.Sets.MainDir + @"\default.jpg";
                    if (Level.Sets.PreviewType == "local")
                    {
                        //                        filename = Game.Sets.MainDir + Level.Sets.Directory + Level.Sets.InterfaceDir + @"\preview.jpg";
                        if (Sets.gameType == GameType.CardsEasy)
                            filename = ((CardsEasyDBLevel)Level).Sets.PreviewURL;
                        if (Sets.gameType == GameType.CardsNewDB)
                            filename = ((CardsNewDBLevel)Level).Sets.PreviewURL;
                    }
                    if (Level.Sets.PreviewType == "youtube")
                    {
                        try { filename = YouTubeUrlSupplier.YoutubeGet.GetImage(Level.Sets.BaseVideoFilename); }
                        catch { filename = Game.Sets.MainDir + @"\default.jpg"; Level.Sets.PreviewType = "local"; }
                    }

                    PrevMenuItem NewItem = new PrevMenuNS.PrevMenuItem(filename, Level, "youtube");
                    Owner.PreviewMenu.AddItem(NewItem, ItemClick);
                }
            }, TimeSpan.FromSeconds(1.5));

        }

        public static void PrevMenuShow()
        {
            Game.LoadBackGround(VanyaGame.Sets.Settings.BackgroundMenuFilename);
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
        public string InterfaceBackgroundDir;
        public string InterfaceControlsDir;
        public string InterfaceUnitsDir;
        public string DefaultVideo = @"\default.wmv";

        public int LevelsCount; //Количество уровней
        public TGameSets():base()
        {
            MainDir = XMLTools.LoadFromXML(Directory.GetCurrentDirectory() + @"\GameSets.xml", "maindir");  
            VideoPlayer = XMLTools.LoadFromXML(Directory.GetCurrentDirectory() + @"\GameSets.xml", "videoplayer");
            InterfaceDir = XMLTools.LoadFromXML(Directory.GetCurrentDirectory() + @"\GameSets.xml", "InterfaceDir"); 
            InterfaceDirCurVersion = XMLTools.LoadFromXML(Directory.GetCurrentDirectory() + @"\GameSets.xml", "InterfaceDirCurVersion"); 
            InterfaceBackgroundDir= XMLTools.LoadFromXML(Directory.GetCurrentDirectory() + @"\GameSets.xml", "InterfaceBackgroundDir");
            InterfaceControlsDir = XMLTools.LoadFromXML(Directory.GetCurrentDirectory() + @"\GameSets.xml", "InterfaceControlsDir");
            InterfaceUnitsDir = XMLTools.LoadFromXML(Directory.GetCurrentDirectory() + @"\GameSets.xml", "InterfaceUnitsDir");
            DefaultVideo = XMLTools.LoadFromXML(Directory.GetCurrentDirectory() + @"\GameSets.xml", "DefaultVideo");
            LevelsCount = 0;
            DebugMode = true;
        }
        public double InterfaceDefaultOpacity = 0;
    }


}
