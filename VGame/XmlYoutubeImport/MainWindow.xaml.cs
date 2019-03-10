using System;
using System.Collections.Generic;
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
    public class SceneData : INotifyPropertyChanged
    {   public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
        private TimeSpan begin;
        public TimeSpan Begin
        {
            get { return begin; }
            set
            {
                begin = value;
                OnPropertyChanged("Begin");
            }
        }

        private TimeSpan end;
        public TimeSpan End
        {
            get { return end; }
            set
            {
                end = value;
                OnPropertyChanged("End");
            }
        }

        int num;
        public int Num
        {
            get { return num; }
            set
            {
                num = value;
                OnPropertyChanged("Num");
            }
        }

    }


    public  class VideoDataContext: INotifyPropertyChanged
    {
        public List<SceneData> sceneDatas;

        public SceneData selectedSceneData;
        public SceneData SelectedSceneData
        {
            get { return selectedSceneData; }
            set
            {
                selectedSceneData = value;
                OnPropertyChanged("SelectedSceneData");
            }
        }
        private string title;
        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                OnPropertyChanged("Title");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public static class Global
    {
        public static VideoDataContext VideoData;
    }

    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
    
        public MainWindow()
        {

            InitializeComponent();

            Global.VideoData = new VideoDataContext();
            DataContext = Global.VideoData;

            Global.VideoData.Title = "Пустой заголовок";
            Global.VideoData.SelectedSceneData = new SceneData();


        }
    }
}
