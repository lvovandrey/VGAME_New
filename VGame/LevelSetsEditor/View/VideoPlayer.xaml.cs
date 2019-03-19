﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LevelSetsEditor.View
{


    public delegate void PlayerMySourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e);


    /// <summary>
    /// Логика взаимодействия для VideoPlayer.xaml
    /// </summary>
    public partial class VideoPlayer : UserControl
    {

        public event PlayerMySourceChanged OnPlayerMySourceChanged;

        public static readonly DependencyProperty MySourceProperty = DependencyProperty.Register("MySource",
            typeof(Uri), typeof(VideoPlayer),
            new FrameworkPropertyMetadata(new PropertyChangedCallback(MySourcePropertyChangedCallback)));

        public Uri MySource { get { return (Uri)GetValue(MySourceProperty); } set { SetValue(MySourceProperty, value); } }


        static void MySourcePropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (((VideoPlayer)d).OnPlayerMySourceChanged != null)
                ((VideoPlayer)d).OnPlayerMySourceChanged(d, e);
        }




        public string MyString
        {
            get { return (string)GetValue(MyStringProperty); }
            set { SetValue(MyStringProperty, value); }
        }
        // Using a DependencyProperty as the backing store for MyString.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MyStringProperty =
            DependencyProperty.Register("MyString", typeof(string), typeof(VideoPlayer), new UIPropertyMetadata("ЫВААВЫАВЫА"));

        //public Uri MySource
        //{
        //    get { return (Uri)GetValue(MySourceProperty); }
        //    set {
        //        SetValue(MySourceProperty, value);
        //        vlc.MediaPlayer.Play(value);
        //    }
        //}
        //// Using a DependencyProperty as the backing store for MyString.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty MySourceProperty =
        //    DependencyProperty.Register("MySource", typeof(Uri), typeof(VideoPlayer), 
        //        new UIPropertyMetadata(new Uri(@"C:\1.png")));




        public VideoPlayer() : base()
        {
            InitializeComponent();
            MyString = "some Text";
            vlc.MediaPlayer.VlcLibDirectory = new System.IO.DirectoryInfo(@"c:\Program Files\VideoLAN\VLC\");
            vlc.MediaPlayer.EndInit();
            vlc.MediaPlayer.Play(new Uri(@"C:\1.wmv"));

            this.OnPlayerMySourceChanged += VideoPlayer_OnPlayerMySourceChanged; 
        }

        private void VideoPlayer_OnPlayerMySourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            this.TimeTextBox.Text = "СОБЫТИЕ ПРОШЛО ";
            MessageBox.Show(MySource.ToString());

            vlc.MediaPlayer.Play(MySource);
        }

       
    }





}
