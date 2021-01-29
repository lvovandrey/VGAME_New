using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using VanyaGame.PrevMenuNS;
using VanyaGame.Struct;
using VanyaGame.Struct.Components;
using VanyaGame.Media;
using VanyaGame.Media.Abstract;
using VanyaGame.Abstract;
using VanyaGame.GameCardsNewDB.Struct;
using VanyaGame.Sets;
using VanyaGame.Tools;

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

        public static GameType gameType = GameType.None;

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
                else Game.Owner.Cursor = Cursors.None;
            }
        }

        

        
        public static double TimeDiagnostic;


        internal static void CreateSettingsWindow()
        {
            if (gameType == GameType.CardsNewDB)
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

        public static void LoadLevels() 
        {
            if (gameType == GameType.CardsNewDB)
            {
                CardsNewDBLevel.LoadLevels();
                return;
            }
        }

        public static void VideoPlayerSet(GameVideo gameVideo)
        {
            if (CurVideo == null) return;
            if (CurVideo.IsPlaying || CurVideo.IsPaused) {
                MessageBox.Show("Cannt change videoplayer while it play");
                return; }
            gameVideo.MediaGUI.Clear();

            CurVideo = gameVideo;
            CurVideo.MediaGUI.UIMediaHide();
        }

        internal static void ReloadGame()
        {
            Level?.Abort();
            Owner.PreviewMenu.RemoveAllItems();
            Levels.Clear();
            LoadBackGround(VanyaGame.Sets.Settings.GetInstance().BackgroundStartFilename);

            Owner.StartButton.Reload();
            Game.Owner.PreviewMenu.Visibility = Visibility.Collapsed;
        }


        public static void Create(MainWindow WND_Owner, GameType GameType)
        {

            Owner = WND_Owner;
            RandomGenerator = new Random();

            gameType = GameType;

            CreateSettingsWindow();
            mediaContainer = new GameMediaContainer();

            Player Snd = new PlayerWpf("PlayerSound", mediaContainer, Owner.MediaElementSound);
            Player Msc = new PlayerWpf("PlayerMusic", mediaContainer, Owner.MediaElementSound);
            
            Sound = new GameSound(ref Snd);
            Music = new GameMusic(ref Msc);


            Music.MediaGUI.Add(Owner.MusicVolumeSlider);
            Game.Owner.MusicVolumeSlider.DataContext = Music.volume;
            Music.MediaGUI.UIMediaHide();


            if (Settings.GetInstance().VideoPlayerType == VideoPlayerType.wpf)
            {
                Player Vdowpf = new PlayerWpf("PlayerVideoWpf", mediaContainer, Owner.MediaElementVideo);
                VideoWpf = new GameVideo(ref Vdowpf);
                CurVideo = new GameVideo(ref Vdowpf);
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
            im.ImageSource = PictHelper.GetBitmapImage(new Uri(BackgroundFilename));
            Owner.BaseGrid.Background = im;
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
            }
            else
            {
                GameOver();
            }
        }


        public static void GameOver() 
        {
            LoadBackGround(VanyaGame.Sets.Settings.GetInstance().BackgroundGameOverFilename);
        }
        public static void PreviewStart()
        {
           
            LoadBackGround(VanyaGame.Sets.Settings.GetInstance().BackgroundStartFilename);

            Owner.StartButton.Visibility = Visibility.Visible; Owner.StartButton.Opacity = 1;

            /// ??? двойной вызов с ImageBegin_MouseUp   
            Owner.StartButton.MouseUp += StartButton_MouseUp;
        }


         //??? двойной вызов с ImageBegin_MouseUp   
        static void StartButton_MouseUp(object sender, RoutedEventArgs e)
        {

            ToolsTimer.Delay(() =>
            {
                Game.LoadLevels();

                TDrawEffects.BlurHide((FrameworkElement)Game.Owner.StartButton, 0.1, 0, () =>
                {
                    ((FrameworkElement)Game.Owner.StartButton).BeginAnimation(FrameworkElement.OpacityProperty, null);
                    Game.PrevMenuShow();
                });

            }, TimeSpan.FromSeconds(1.5));

        }

        public static void PrevMenuShow()
        {
            Game.LoadBackGround(VanyaGame.Sets.Settings.GetInstance().BackgroundMenuFilename);
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

  


}
