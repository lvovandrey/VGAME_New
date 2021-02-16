using GameCore;
using System;
using System.Windows;
using System.Windows.Input;
using VGameCore;

namespace VanyaGame
{


    ///ЧТО ЭТО ЗА МУСОРКА!!!!!!!!!!!!!
    ///
    /// 
    /// 
    /// 
    /// <summary>
    /// В данном случае описываются все реакции 
    /// </summary>
    public class TUserActivity

    {

        public event TUserDoSomething UserDoSomethingEvent;
        public void Clear_UserDoSomethingEvent()
        {
            UserDoSomethingEvent = null;
        }
        public void MyWindow_MouseMove(object sender, MouseEventArgs e)
        {
            Point Pos = e.MouseDevice.GetPosition(Game.Owner);
            UserDoSomething(e, null, null);
        }

        public void PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            MouseEventArgs mouse = (MouseEventArgs)e;
            UserDoSomething(mouse, e, null);
        }
        public void PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            MouseEventArgs mouse = (MouseEventArgs)e;
            UserDoSomething(mouse, e, null);
        }


        public void MediaElementMusic_MediaEnded(object sender, RoutedEventArgs e)
        {
            Game.Music.End();
        }
        public void MyWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (Game.CurVideo.IsPlaying)
            {
                Game.CurVideo.player.Body.Width = Game.Owner.ActualWidth;
            }
        }
        //public void ImageBegin_MouseUp(object sender, MouseButtonEventArgs e)
        //{
        //    Game.Start();   
        //}

        public void ImageBegin_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        public void ImageBegin_MouseLeave(object sender, MouseEventArgs e)
        {

        }
        public void basketGrid_PreviewMouseMove(object sender, MouseEventArgs e)
        {

        }
        public void MyWindow_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            
            if (e.Key == Key.Escape) { Environment.Exit(0); }
            if (e.Key == Key.Space)
            {
                if(Game.CurVideo.IsPaused) Game.CurVideo.Play();
                else Game.CurVideo.Pause();
                Game.CurVideo.MediaGUI.UIMediaShowAndHideFull();
                e.Handled = true;
            }
            if (e.Key == Key.Enter)
            {
                Game.CurVideo.Play();
                Game.CurVideo.MediaGUI.UIMediaShowAndHideFull();
                e.Handled = true;
            }
            if (e.Key == Key.F)
            {
                if (Game.CurVideo.IsPlaying)
                    Game.CurVideo.player.Position = TimeSpan.FromMilliseconds(Game.CurVideo.player.Duration.TotalMilliseconds - 500);
                e.Handled = true;
            }


            if (e.Key == Key.Left)
            {
                Game.CurVideo.Rewind(new TimeSpan(0, 0, 5), true);
                Game.CurVideo.MediaGUI.UIMediaShowAndHideFull();
                e.Handled = true;
            }
            if (e.Key == Key.Right)
            {
                Game.CurVideo.Rewind(new TimeSpan(0, 0, 5), false);
                Game.CurVideo.MediaGUI.UIMediaShowAndHideFull();
                e.Handled = true;
            }

            if (e.Key == Key.Up)
            {
                RaiseVolume(10);
                e.Handled = true;
            }
            if (e.Key == Key.Down)
            {
                RaiseVolume(-10);
                e.Handled = true;
            }

            UserDoSomething(null, null, e);
        }


        private void RaiseVolume(double VolumeLevelAddition)
        {
            if (Game.CurVideo.IsPlaying || Game.CurVideo.IsPaused)
            {
                Game.CurVideo.MediaGUI.UIMediaShowAndHideFull();
            }
            else
            {
                Game.Owner.MusicVolumeSlider.Value += VolumeLevelAddition;
                Game.Music.MediaGUI.UIMediaShowAndHideFull();            
            }
        }

        public void BtnNextLevel_Click(object sender, RoutedEventArgs e)
        {
            TDrawEffects.BlurHide((FrameworkElement)e.Source, 1);
            Game.NextLevel();
        }

        private void UserDoSomething(MouseEventArgs mouse, MouseButtonEventArgs mousebutton, KeyEventArgs key)
        {
            if (Game.Level != null)
            {
                UserDoSomethingEvent?.Invoke(mouse, mousebutton, key);
            }
        }


    }

}