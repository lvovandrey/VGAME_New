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

        public override double Volume { get; set; }


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
