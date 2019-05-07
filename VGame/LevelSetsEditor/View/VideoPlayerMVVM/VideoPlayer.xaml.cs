using LevelSetsEditor.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LevelSetsEditor.View.VideoPlayerMVVM
{


    public delegate void PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e);


    /// <summary>
    /// Логика взаимодействия для VideoPlayer.xaml
    /// </summary>
    public partial class VideoPlayer : UserControl
    {

        public static readonly DependencyProperty PositionProperty = DependencyProperty.Register("Position",
          typeof(double), typeof(VideoPlayer),
          new FrameworkPropertyMetadata(new PropertyChangedCallback(PositionPropertyChangedCallback)));

        public static readonly DependencyProperty DurationProperty = DependencyProperty.Register("Duration",
          typeof(TimeSpan), typeof(VideoPlayer),
          new FrameworkPropertyMetadata(new PropertyChangedCallback(DurationPropertyChangedCallback)));

        public static readonly DependencyProperty CurTimeProperty = DependencyProperty.Register("CurTime",
            typeof(TimeSpan), typeof(VideoPlayer),
            new FrameworkPropertyMetadata(new PropertyChangedCallback(CurTimePropertyChangedCallback)));

        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register("Source",
            typeof(Uri), typeof(VideoPlayer),
            new FrameworkPropertyMetadata(new PropertyChangedCallback(SourcePropertyChangedCallback)));

        public double Position { get { return (double)GetValue(PositionProperty); } set { SetValue(PositionProperty, value); } }
        public TimeSpan Duration { get { return (TimeSpan)GetValue(DurationProperty); }  set {  SetValue(DurationProperty, value); } }
        public TimeSpan CurTime { get { return (TimeSpan)GetValue(CurTimeProperty); } set {  SetValue(CurTimeProperty, value); } }
        public Uri Source { get { return (Uri)GetValue(SourceProperty); } set { SetValue(SourceProperty, value); } }


        public event PropertyChanged OnPositionChanged;
        public event PropertyChanged OnDurationChanged;
        public event PropertyChanged OnCurTimeChanged;
        public event PropertyChanged OnSourceChanged;


        static void PositionPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (((VideoPlayer)d).OnPositionChanged != null)
                ((VideoPlayer)d).OnPositionChanged(d, e);
        }

        static void DurationPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (((VideoPlayer)d).OnDurationChanged != null)
                ((VideoPlayer)d).OnDurationChanged(d, e);
        }

        static void CurTimePropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (((VideoPlayer)d).OnCurTimeChanged != null)
                ((VideoPlayer)d).OnCurTimeChanged(d, e);
        }

        static void SourcePropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (((VideoPlayer)d).OnSourceChanged != null)
                ((VideoPlayer)d).OnSourceChanged(d, e);
        }




        System.Windows.Threading.DispatcherTimer timer;

        public VideoPlayer() : base()
        {
            InitializeComponent();



            vlc.MediaPlayer.VlcLibDirectory = new System.IO.DirectoryInfo(@"c:\Program Files\VideoLAN\VLC\");
            vlc.MediaPlayer.EndInit();
            vlc.MediaPlayer.Play(new Uri(@"C:\1.wmv"));

            this.OnSourceChanged += VideoPlayer_OnPlayerSourceChanged;
            this.OnPositionChanged += Player_OnPositionChanged;
            this.OnCurTimeChanged += Player_OnCurTimeChanged;



            timer = new System.Windows.Threading.DispatcherTimer();

            timer.Tick += new EventHandler(timerTick);
            timer.Interval = TimeSpan.FromSeconds(0.01);
            timer.Start();
        }

        private void timerTick(object sender, EventArgs e)
        {
            if (vlc.MediaPlayer.Video!= null)
            {
                Duration = TimeSpan.FromMilliseconds(vlc.MediaPlayer.Length);
                CurTime = TimeSpan.FromMilliseconds(vlc.MediaPlayer.Time);
                Position = 1000 * CurTime.TotalMilliseconds / Duration.TotalMilliseconds;
            }
        }

        public void SetCurTime(TimeSpan time)
        {
            vlc.MediaPlayer.Time = (long)time.TotalMilliseconds;
        }

        private void VideoPlayer_OnPlayerSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (Source == null) return;
            vlc.MediaPlayer.Play(Source);
        }

        private void Player_OnPositionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((vlc.MediaPlayer.Video != null) && (!vlc.MediaPlayer.IsPlaying))
            {
                vlc.MediaPlayer.Time = (long)Math.Round(Position * Duration.TotalMilliseconds / 1000);
            }
        }

        private void Player_OnCurTimeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }
        private void PlayBtn_Click_1(object sender, RoutedEventArgs e)
        {
            if (vlc.MediaPlayer.IsPlaying) vlc.MediaPlayer.Pause();
            else vlc.MediaPlayer.Play();
        }

        private void SplitBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MuteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (vlc.MediaPlayer.Audio.Volume != 0) vlc.MediaPlayer.Audio.Volume = 0;
            else vlc.MediaPlayer.Audio.Volume = 50;
        }


        bool WasPlaing;

        private void TimeSlider_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            WasPlaing = vlc.MediaPlayer.IsPlaying;
            if (vlc.MediaPlayer.IsPlaying) { vlc.MediaPlayer.Pause(); timer.Stop(); }
        }

        private void TimeSlider_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (!vlc.MediaPlayer.IsPlaying)
            {
                if (WasPlaing)
                {
                    vlc.MediaPlayer.Play(); timer.Start();
                }
                else
                {

                }
            }
        }

        public void SetPosition(double position)// ЧТО ЭТО ЗА ЖЕСТЬ?!!!!!!
        {
            timer.Stop();
            if (vlc.MediaPlayer.IsPlaying)
            {
                vlc.MediaPlayer.Pause();
                timer.Stop();

                Position = position;
                CurTime = TimeSpan.FromSeconds(Position / 1000 * Duration.TotalSeconds);
                vlc.MediaPlayer.Time = (long)Math.Round(Position * Duration.TotalMilliseconds / 1000);

                ToolsTimer.Delay(() => { vlc.MediaPlayer.Play(); }, TimeSpan.FromMilliseconds(1000));
            }
            else
            {
                timer.Stop();

                Position = position;
                CurTime = TimeSpan.FromSeconds(Position / 1000 * Duration.TotalSeconds);
                vlc.MediaPlayer.Time = (long)Math.Round(Position * Duration.TotalMilliseconds / 1000);

            }

            ToolsTimer.Delay(() => { timer.Start(); }, TimeSpan.FromMilliseconds(1000));

        }
    }





}

