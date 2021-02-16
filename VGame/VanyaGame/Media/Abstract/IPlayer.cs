using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;
using VGameCore.Abstract;

namespace VanyaGame.Media.Abstract
{
    public interface IPlayer: IComponent
    {
        double Volume { get; set; }
        TimeSpan Position { get; set; }
        void BeginAnimation(DependencyProperty property, AnimationTimeline animation);
        string Source { get; set; }
        void Play();
        void Pause();
        void Stop();
        FrameworkElement Body  { get; set; }
    }
}
