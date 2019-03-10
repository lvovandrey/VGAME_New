using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

namespace XmlYoutubeImport
{

    public class SceneVideo
        {
            public int Id { get; set; }
            public TimeSpan TimeBegin { get; set; } // производитель
            public TimeSpan TimeEnd { get; set; }
            public string ImagePath { get; set; }// путь к изображению скриншота

    }
    /// <summary>
    /// Логика взаимодействия для VideoPartsControl.xaml
    /// </summary>
    public partial class VideoPartsControl : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        //private SceneData test;
        //public SceneData TEST
        //{
        //    get { return test; }
        //    set
        //    {
        //        test = value;
        //        OnPropertyChanged("test");
        //    }
        //}


        public VideoPartsControl()
        {
            InitializeComponent();
        //    TEST = new SceneData { Begin = TimeSpan.FromSeconds(10) };

        //    DataContext = Global.VideoData;

        //    SceneVideos = new ObservableCollection<SceneVideo>
        //{
        //    new SceneVideo() {ImagePath= @"C:\vlctmp.png", Id = 1, TimeEnd = TimeSpan.FromSeconds(123) , TimeBegin = TimeSpan.FromSeconds(1443)},
        //    new SceneVideo(),
        //    new SceneVideo(),
        //    new SceneVideo()
        //};
        //    phonesList.ItemsSource = SceneVideos;
        //}

        //public ObservableCollection<SceneVideo> SceneVideos { get; set; }

         void phonesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
            { }
        //    SceneVideo p = (SceneVideo)phonesList.SelectedItem;

        //    // Global.VideoData.selectedSceneData.Begin = p.TimeEnd;
        //    Global.VideoData.SelectedSceneData.End = p.TimeEnd;
        //    Global.VideoData.SelectedSceneData.Begin = p.TimeBegin;
        //    Global.VideoData.SelectedSceneData.Num = p.Id;
        }
    }
}
