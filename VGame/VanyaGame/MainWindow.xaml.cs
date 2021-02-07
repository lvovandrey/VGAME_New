using System;
using System.Windows;
using System.Windows.Input;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Data;
using System.Globalization;
using VanyaGame.GameCardsNewDB.Tools;

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
            Game.Create(this, GameType.CardsNewDB);
            MyWindow.MouseMove += Game.UserActivity.MyWindow_MouseMove;
            MyWindow.MediaElementMusic.MediaEnded += Game.UserActivity.MediaElementMusic_MediaEnded;
            MyWindow.SizeChanged += Game.UserActivity.MyWindow_SizeChanged;
            MyWindow.PreviewKeyDown += Game.UserActivity.MyWindow_PreviewKeyDown;
            MyWindow.PreviewMouseDown += Game.UserActivity.PreviewMouseDown;
            MyWindow.PreviewMouseUp += Game.UserActivity.PreviewMouseUp;

            
            Game.PreviewStart();
            this.Show();
            mc = new MemoryCounter();
        }
        MemoryCounter mc;



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



        private void Close_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            Environment.Exit(0);
        }

        private void StopLevel_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            Game.Level.Abort();
        }
    }
}
