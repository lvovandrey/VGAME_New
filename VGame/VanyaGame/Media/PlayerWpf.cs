using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using VanyaGame.Abstract;
using VanyaGame.Media.Abstract;

namespace VanyaGame.Media
{
    /// <summary>
    /// 
    /// </summary>
    class PlayerWpf : Player
    {
        public PlayerWpf(string name, IComponentContainer container, MediaElement body) : base(name, container)
        {
            if (body == null)
            {
                MessageBox.Show("Отсутствует компонент VLCPlayer (видеопроигрвыатель). ", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Body = body;
        }

        public override double Volume
        {
            get { return ((MediaElement)Body).Volume; }
            set {
                ((MediaElement)Body).BeginAnimation(MediaElement.VolumeProperty, null);
                ((MediaElement)Body).Volume = value;
            }
        }
        public override TimeSpan Position {
            get
            {
                
                if (Source == "") return TimeSpan.FromSeconds(0);
                return ((MediaElement)Body).Position;
            }

            set
            {
                if (Source == "") return;
                ((MediaElement)Body).Position = value;
            }
        }

        public override TimeSpan Duration { get { return ((MediaElement)Body).NaturalDuration.TimeSpan; } }

        public override string Source { get ; set ; }
        public override FrameworkElement Body { get ; set ; }

        public override event MediaEvent Ended;

        public override void BeginAnimation(DependencyProperty property, AnimationTimeline animation)
        {
            
        }

        public override void Pause()
        {
            if (Source == "") return;
            ((MediaElement)Body).Pause();
        }

        public override void UnPaused()
        {
            if (Source == "") return;
            ((MediaElement)Body).Play();
        }

        public override void Play()
        {
            if (Source == "") return;
            ((MediaElement)Body).MediaEnded += PlayerWpf_MediaEnded;
            ((MediaElement)Body).Source = new Uri(@Source);
            ((MediaElement)Body).Play();
           
        }

        private void PlayerWpf_MediaEnded(object sender, RoutedEventArgs e)
        {
            Ended();
        }

        public override void Stop()
        {
            if (Source == "") return;
            ((MediaElement)Body).Stop();
            ((MediaElement)Body).MediaEnded -= PlayerWpf_MediaEnded;
            Source = "";
        }

        public override void Show()
        {
            TDrawEffects.BlurShow(Body, 0.5);
        }
        public override void Hide()
        {
            TDrawEffects.BlurHide(Body, 0.5);
        }
    }
}
