using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;
using VanyaGame.Abstract;
using VanyaGame.Interface;
using VanyaGame.Media.Abstract;

namespace VanyaGame.Media
{
    public class PlayerVlc : Player
    {
        public PlayerVlc(string name, IComponentContainer container, VlcWrap body) : base(name, container)
        {
            if (body == null)
            {
                MessageBox.Show("Отсутствует компонент VLCPlayer (видеопроигрвыатель). ", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Body = body;
            if (!((VlcWrap)Body).InitializePlayer())
                MessageBox.Show("Ошибка инициализации компонента VLCPlayer (видеопроигрвыателя)", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public override double Volume
        {
            get
            {
                double v1 = ((double)((VlcWrap)Body).vlcPlayer.MediaPlayer.Audio.Volume) / 100;
                double v = Math.Pow(v1, 3.33333333333);
                return v;
            }
            set
            {
                double v = Math.Pow(value, 0.3);
                ((VlcWrap)Body).vlcPlayer.MediaPlayer.Audio.Volume = (int)(v*100);
            }
        }


        public override TimeSpan Position
        { //get; set;
            get
            {
                if (Source == "") return TimeSpan.FromSeconds(0);

                return TimeSpan.FromMilliseconds(((VlcWrap)Body).vlcPlayer.MediaPlayer.Time);
            }

            set
            {
                if (Source == "") return;
                ((VlcWrap)Body).vlcPlayer.MediaPlayer.Time = (long)(Math.Round(value.TotalMilliseconds));
            }
        }

        public override TimeSpan Duration { get { return TimeSpan.FromMilliseconds(((VlcWrap)Body).vlcPlayer.MediaPlayer.Length); } }

        public override string Source { get; set; }
        public override FrameworkElement Body { get; set; }

        public override event MediaEvent Ended;

        public override void BeginAnimation(DependencyProperty property, AnimationTimeline animation)
        {

        }

        public override void Pause()
        {
            if (Source == "") return;
            ((VlcWrap)Body).vlcPlayer.MediaPlayer.Pause();
        }

        public override void UnPaused()
        {
            if (Source == "") return;
            ((VlcWrap)Body).vlcPlayer.MediaPlayer.Play();
        }

        public override void Play()
        {
            if (Source == "") return;            
            ((VlcWrap)Body).vlcPlayer.MediaPlayer.Play(new Uri(Source));
            ((VlcWrap)Body).vlcPlayer.MediaPlayer.Stopped += MediaPlayer_Stopped;
        }


        ///ПЛОХАЯ РЕАЛИЗАЦИЯ!!!!!
        private void MediaPlayer_Stopped(object sender, Vlc.DotNet.Core.VlcMediaPlayerStoppedEventArgs e)
        {
          //  Ended();
        }

        public override void Stop()
        {
            if (Source == "") return;
            ((VlcWrap)Body).vlcPlayer.MediaPlayer.Stop();
            ((VlcWrap)Body).vlcPlayer.MediaPlayer.Stopped -= MediaPlayer_Stopped;
            Source = "";
        }


        public override void Show()
        {
            Body.Visibility = Visibility.Visible;
        }
        public override void Hide()
        {
            Body.Visibility = Visibility.Hidden;
        }

    }
}
