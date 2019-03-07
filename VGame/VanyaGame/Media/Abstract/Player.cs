using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;
using VanyaGame.Abstract;

namespace VanyaGame.Media.Abstract
{
    public abstract class Player:Component, IPlayer
    {
        #region constructors
        public Player(string name, IComponentContainer container):base(name,container)
        {
        }
        #endregion

        #region variables

        #endregion

        #region properties

        //IPlayer impl
        public abstract double Volume { get; set; }
        public abstract TimeSpan Position { get; set; }
        public abstract string Source { get; set; }
        public abstract FrameworkElement Body { get; set; }
        public abstract TimeSpan Duration { get; }

        #endregion

        #region methods
        //IPlayer impl
        public abstract void BeginAnimation(DependencyProperty property, AnimationTimeline animation);
        public abstract void Pause();
        public abstract void UnPaused();
        public abstract void Play();
        public abstract void Stop();

        public abstract void Show();
        public abstract void Hide();
        #endregion

        #region events
        public abstract event MediaEvent Ended;
        #endregion

        }
}
