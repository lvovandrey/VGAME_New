using System;
using System.Windows;
using System.Windows.Input;
using System.Data;
using System.Data.SqlClient;

namespace VanyaGame
{

    public partial class MainWindow : Window
    {


        public MainWindow()
        {
            InitializeComponent();
        }

        private void MyWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.Hide();
            Game.Create(this);
            MyWindow.MouseMove += Game.UserActivity.MyWindow_MouseMove;
            MyWindow.MediaElementMusic.MediaEnded += Game.UserActivity.MediaElementMusic_MediaEnded;
            MyWindow.SizeChanged += Game.UserActivity.MyWindow_SizeChanged;
            MyWindow.PreviewKeyDown += Game.UserActivity.MyWindow_PreviewKeyDown;
            MyWindow.PreviewMouseDown += Game.UserActivity.PreviewMouseDown;
            MyWindow.PreviewMouseUp += Game.UserActivity.PreviewMouseUp;

            this.Show();
            Game.PreviewStart();
        }


        private void Image_MouseEnter(object sender, MouseEventArgs e)
        {
            Game.CurVideo.MediaGUI.UIMediaShowFull();
        }


        private void Image_MouseLeave(object sender, MouseEventArgs e)
        {
            Game.CurVideo.MediaGUI.UIMediaHideNotFull();
        }

        
        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TDrawEffects.PushUI_MouseDown((FrameworkElement)sender);
        }

        private void NextImgButton_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Game.CurVideo.Rewind(new TimeSpan(0, 0, 5), false);
        }

        private void BackImgButton_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Game.CurVideo.Rewind(new TimeSpan(0, 0, 5), true);
        }



        private void VideoTimeSlider_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            Game.CurVideo.player.Position = new TimeSpan(0, 0, (int)VideoTimeSlider.Value);
            Game.CurVideo.Play();
        }

        private void VideoTimeSlider_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Game.CurVideo.Pause();
        }

        private void PlayImgButton_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Game.CurVideo.PlayBtnClick();
        }

        private void MusicVolumeSlider_MouseEnter(object sender, MouseEventArgs e)
        {
            TDrawEffects.ShowUI_MouseEnter((FrameworkElement)sender);
        }

        private void MusicVolumeSlider_MouseLeave(object sender, MouseEventArgs e)
        {
            TDrawEffects.HideUI_MouseDown((FrameworkElement)sender);
        }


        private void MyWindow_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
          if(PreviewMenu.Visibility == Visibility.Visible)  
            if (e.Delta > 0)
                for(int i = 1;i<5;i++)
                    PreviewMenu.Scroll.LineUp();
            if (e.Delta < 0)
                for (int i = 1; i < 5; i++)
                    PreviewMenu.Scroll.LineDown();
        }

        private void SettingsWindowShowButtonClick(object sender, MouseButtonEventArgs e)
        {
            Game.ShowSettingsWindow();
        }
    }


}
