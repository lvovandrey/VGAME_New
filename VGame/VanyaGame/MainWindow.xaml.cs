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
            Game.Create(this);
            MyWindow.MouseMove += Game.UserActivity.MyWindow_MouseMove;
            MyWindow.MediaElementMusic.MediaEnded += Game.UserActivity.MediaElementMusic_MediaEnded;
            MyWindow.SizeChanged += Game.UserActivity.MyWindow_SizeChanged;
            MyWindow.PreviewKeyDown += Game.UserActivity.MyWindow_PreviewKeyDown;
            MyWindow.BtnNextLevel.Click += Game.UserActivity.BtnNextLevel_Click;
            MyWindow.PreviewMouseDown += Game.UserActivity.PreviewMouseDown;
            MyWindow.PreviewMouseUp += Game.UserActivity.PreviewMouseUp;


            Game.PreviewStart();
        }

        #region События в окне
        private void MyWindow_MouseMove(object sender, MouseEventArgs e)
        {
            
        }
        
        private void MediaElementMusic_MediaEnded(object sender, RoutedEventArgs e)
        {

        }

        private void MyWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {

        }

        private void MyWindow_KeyDown(object sender, KeyEventArgs e)
        {

        }
        #endregion

        private void BtnNextLevel_Click(object sender, RoutedEventArgs e)
        {


        }

        private void MediaElementVideo_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void MyWindow_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Game.HasCursor)
            {
                if (Game.Level != null)
                    if (Game.Level.CurScene != null)
                        if (Game.Level.CurScene.Sets != null) { }
                           // ВКЛЮЧИТЬ КУРСОР 2
                                // Game.Owner.Cursor = Game.Level.CurScene.Sets.Cursor2;
            }
        }

        private void MyWindow_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (Game.HasCursor)
            {
                if (Game.Level != null)
                    if (Game.Level.CurScene != null)
                        if (Game.Level.CurScene.Sets != null) { }
                            // ВКЛЮЧИТЬ КУРСОР 1
                            //Game.Owner.Cursor = Game.Level.CurScene.Sets.Cursor;
            }
        }

        private void VolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
           // Label1.Content = Game.Music.player.Volume + " " + Game.Sound.player.Volume + " " + Game.Video.player.Volume;
        }

        private void Image_MouseEnter(object sender, MouseEventArgs e)
        {
            Game.Video.MediaGUI.UIMediaShowFull();
//            TDrawEffects.ShowUI_MouseEnter((FrameworkElement)sender);
        }


        private void Image_MouseLeave(object sender, MouseEventArgs e)
        {
            Game.Video.MediaGUI.UIMediaHideNotFull();
            ///TDrawEffects.HideUI_MouseDown((FrameworkElement)sender);
        }

        
        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TDrawEffects.PushUI_MouseDown((FrameworkElement)sender);
        }

        private void NextImgButton_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Game.Video.Rewind(new TimeSpan(0, 0, 5), false);
        }

        private void BackImgButton_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Game.Video.Rewind(new TimeSpan(0, 0, 5), true);
        }



        private void VideoTimeSlider_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            Game.Video.player.Position = new TimeSpan(0, 0, (int)VideoTimeSlider.Value);
            Game.Video.Play();
        }

        private void VideoTimeSlider_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Game.Video.Pause();
        }

        private void PlayImgButton_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Game.Video.PlayBtnClick();
        }

        private void MyWindow_PreviewMouseMove(object sender, MouseEventArgs e)
        {

        }

        private void MusicVolumeSlider_MouseEnter(object sender, MouseEventArgs e)
        {
            TDrawEffects.ShowUI_MouseEnter((FrameworkElement)sender);
        }

        private void MusicVolumeSlider_MouseLeave(object sender, MouseEventArgs e)
        {
            TDrawEffects.HideUI_MouseDown((FrameworkElement)sender);
        }

        private void Grid_PreviewMouseMove(object sender, MouseEventArgs e)
        {

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

        SqlConnection sqlConnection;
        private async void DB_button_Click(object sender, RoutedEventArgs e)
        {
            string DBpath = Game.Sets.MainDir + @"\data\DatabaseKeyboard.mdf";
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + DBpath + @";Integrated Security=True";
            sqlConnection = new SqlConnection(connectionString);
            await sqlConnection.OpenAsync();

            SqlDataReader sqlReader = null;
            SqlCommand command = new SqlCommand("SELECT * FROM [KeyCoordinates]", sqlConnection);
            try
            {
                 sqlReader = await command.ExecuteReaderAsync();
                while (await sqlReader.ReadAsync())
                {
                    Game.Msg(Convert.ToString(sqlReader["Id"] + " " + sqlReader["Key"] + " " + sqlReader["x"] + " " + sqlReader["y"]));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally {
                if (sqlReader != null) { sqlReader.Close(); }
            }
        }

        private void MyWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (sqlConnection != null && sqlConnection.State!= ConnectionState.Closed)
            {
                sqlConnection.Close();
            }
        }

        private void VIDEOTEST_Click(object sender, RoutedEventArgs e)
        {
            Vlc.vlcPlayer.MediaPlayer.VlcLibDirectory = new System.IO.DirectoryInfo(@"c:\Program Files\VideoLAN\VLC\"); //@Environment.CurrentDirectory + @"\Tools\vlcLib\");
            Vlc.vlcPlayer.MediaPlayer.EndInit();
            Vlc.vlcPlayer.MediaPlayer.Play(new Uri(@"http://clips.vorwaerts-gmbh.de/big_buck_bunny.mp4"));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //            Vlc.vlcPlayer.MediaPlayer.Position += 0.1f;
            L1.Content = Vlc.vlcPlayer.MediaPlayer.Time.ToString(); //Vlc.vlcPlayer.MediaPlayer.Position.ToString();
        }
    }


}
