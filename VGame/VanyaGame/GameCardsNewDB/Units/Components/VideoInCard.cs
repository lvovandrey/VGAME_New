using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using VanyaGame.Abstract;
using VanyaGame.Units.Components;

namespace VanyaGame.GameCardsNewDB.Units.Components
{
    /// <summary>
    /// Позволяет добавить видео в карточки
    /// </summary>
    class VideoInCard: Component
    {
        #region constructors
        public VideoInCard(string name, IComponentContainer container, Panel panel) : base(name, container)
        {
            Panel = panel;
        }
        #endregion

        #region variables
        MediaElement ME;
        #endregion

        #region properties
        public string SourceFilename
        { get; set; }
        public Panel Panel { get; private set; }

        #endregion

        #region methods
        public void Run(string sourceFilename)
        {
            SourceFilename = sourceFilename;
            ME = new MediaElement()
            {
                Source = new Uri(SourceFilename),
                LoadedBehavior = MediaState.Manual,
                Margin = new Thickness(10),
                Volume = 0
            };
            ME.Loaded += (s, e) =>
            {
                ME.Position = TimeSpan.Zero;
                ME.Play();
            };
            ME.MouseEnter += ME_MouseEnter;
            ME.MouseLeave += ME_MouseLeave;
            ME.MediaEnded += (s, e) =>
            {
                ME.Position = TimeSpan.Zero;
                ME.Play();
            };
            foreach (var el in Panel.Children)
            {
                if(el is FrameworkElement)
                   ((FrameworkElement)el).Visibility = System.Windows.Visibility.Hidden;
            }
            
            Panel.Children.Add(ME);
        }

        private void ME_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            ME.BeginAnimation(FrameworkElement.OpacityProperty, null);
            DoubleAnimation animation = new DoubleAnimation();
            animation.From = ME.Opacity;
            animation.To = 1;
            animation.Duration = TimeSpan.FromSeconds(0.3);
            ME.BeginAnimation(FrameworkElement.OpacityProperty, animation);
        }

        private void ME_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            ME.BeginAnimation(FrameworkElement.OpacityProperty, null);
            DoubleAnimation animation = new DoubleAnimation();
            animation.From = ME.Opacity;
            animation.To = 0.6;
            animation.Duration = TimeSpan.FromSeconds(0.3);
            ME.BeginAnimation(FrameworkElement.OpacityProperty, animation);
        }

        public void Delete()
        {
            ME.Stop();
            Panel.Children.Remove(ME);
            ME = null;

            foreach (var el in Panel.Children)
            {
                if (el is FrameworkElement)
                    ((FrameworkElement)el).Visibility = System.Windows.Visibility.Visible;
            }
        }
        #endregion
    }
}
