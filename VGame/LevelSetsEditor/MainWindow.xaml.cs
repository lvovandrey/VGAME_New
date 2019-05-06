using LevelSetsEditor.ViewModel;
using System;
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
using System.Drawing;
using CefSharp;
using CefSharp.Wpf;
using YouTubeUrlSupplier;
using PlayerVlcControl;
using System.IO;
using LevelSetsEditor.DB;
using LevelSetsEditor.Model;
using System.Collections.ObjectModel;

namespace LevelSetsEditor
{
    
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        WebBrowserVM WebBrowserVM;
        public VM ViewModel;
        public static MainWindow mainWindow;
        public MainWindow()
        {
            CefSettings settings = new CefSettings();
            settings.CachePath = @"C:\CEFcookies"; // Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\CEF";
            CefSharp.Cef.Initialize(settings);

            
            //ViewModel.SelectedLevelSet.LevelSet = new Model.LevelSet();

            InitializeComponent();
            
            WebBrowserVM = new WebBrowserVM(Browser);
            GridBrowser.DataContext = WebBrowserVM;
            mainWindow = this;


           


            //ViewModel.SelectedLevelSet.LevelSet.Scenes = new List<Model.Scene>();
            //ViewModel.SelectedLevelSet.LevelSet.VideoInfo = new Model.VideoInfo();
            //ViewModel.SelectedLevelSet.LevelSet.VideoInfo.Preview = new Model.Preview();


            //   Button_Click_2(null, null);


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            WebBrowserVM.CurURL = TextURL.Text;
        }

        private void TextURL_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) WebBrowserVM.CurURL = TextURL.Text;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            WebBrowserVM.CurURL = "Youtube.com";
        }


        private void RefreshYoutubeVideoInfo(VideoInfoVM videoInfoVM)
        {

            if (videoInfoVM == null) return;
            if (videoInfoVM.Title == null) return;
            if (videoInfoVM.Type != Model.VideoType.youtube) return;
            
            YoutubeVidInfo vidInfo = new YoutubeVidInfo(videoInfoVM.Title);
            if (vidInfo.DirectURL == "") return;

            videoInfoVM.Source = new Uri(vidInfo.DirectURL);
            videoInfoVM.Duration = vidInfo.Duration;
            videoInfoVM.Resolution = vidInfo.Resolution;
          
            videoInfoVM.PreviewVM.Source = new Uri(vidInfo.ImageUrl);
            videoInfoVM.PreviewVM.Size = new System.Drawing.Size(480, 360);

            ViewModel.SelectedLevelVM.VideoInfoVM.PreviewVM.Type = Model.PreviewType.youtube;
            ObservableCollection<Uri> uris = new ObservableCollection<Uri>();
            for (int i = 0; i < 3; i++)
                uris.Add(new Uri(vidInfo.PrevImagesUrl[i]));

            ViewModel.SelectedLevelVM.VideoInfoVM.PreviewVM.MultiplePrevSources = uris;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            TabItemEditor.Focus();
            if (ViewModel.SelectedLevelVM == null) return;

            YoutubeVidInfo vidInfo = new YoutubeVidInfo(TextURL.Text);
            if (vidInfo.DirectURL == "") return;

            ViewModel.SelectedLevelVM.VideoInfoVM.Source = new Uri(vidInfo.DirectURL);
            ViewModel.SelectedLevelVM.VideoInfoVM.Address = TextURL.Text;
            ViewModel.SelectedLevelVM.VideoInfoVM.Description = vidInfo.Title;
            ViewModel.SelectedLevelVM.VideoInfoVM.Duration = vidInfo.Duration;
            ViewModel.SelectedLevelVM.VideoInfoVM.Resolution = vidInfo.Resolution;
            ViewModel.SelectedLevelVM.VideoInfoVM.Title = vidInfo.Title;
            ViewModel.SelectedLevelVM.VideoInfoVM.Type = Model.VideoType.youtube;
            //    ViewModel.SelectedLevelVM.VideoInfoVM.Preview.
            //  ViewModel.SelectedLevelVM.Scenes.

            ViewModel.SelectedLevelVM.VideoInfoVM.PreviewVM.Source = new Uri(vidInfo.ImageUrl);
            ViewModel.SelectedLevelVM.VideoInfoVM.PreviewVM.Size = new System.Drawing.Size(480, 360);

            ViewModel.SelectedLevelVM.VideoInfoVM.PreviewVM.Type = Model.PreviewType.youtube;
            ObservableCollection<Uri> uris = new ObservableCollection<Uri>();
            for (int i = 0; i < 3; i++)
                uris.Add(new Uri(vidInfo.PrevImagesUrl[i]));

            ViewModel.SelectedLevelVM.VideoInfoVM.PreviewVM.MultiplePrevSources = uris;
            ViewModel.SelectedLevelVM.SegregateScenes();
        }

        private void AutoSegregateVideoToScenes(LevelVM LSet)
        {
        }

        private void TextBox_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            //TimeSpan dtime;
            //if (e.Delta > 0) dtime = TimeSpan.FromSeconds(1);
            //else dtime = TimeSpan.FromSeconds(-1);

            //object o = ((TextBox)sender).DataContext;

            //string text = ((TextBox)sender).Text;
            //TimeSpan time;
            //if (TimeSpan.TryParse(text, out time))
            //{
            //    time += dtime;
            //    ((TextBox)sender).Text = time.ToString();
            //}


        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            
            ////////this.VideoPlayer.SetPosition( 1000*ViewModel.SelectedLevelSet.SelectedSceneVM.VideoSegment_TimeBegin.TotalSeconds/ ViewModel.SelectedLevelSet.VideoInfoVM.Duration.TotalSeconds);
        }

        private void CreateBD()
        {

            ViewModel = new VM();
            // ViewModel.SelectedLevelVM = ViewModel.LevelVMs.First(); 
            DataContext = ViewModel;
            //   TabItemEditor.DataContext = ViewModel.SelectedLevelSet;

            //     LevelsFromDB l = new LevelsFromDB();

            //using (LevelSetContext db = new LevelSetContext())
            //{
            //    // создаем два объекта User
            //    LevelSet level = new LevelSet();
            //    level.Name = "Some Level name";
            //    db.LevelSets.Add(level);
            //    db.SaveChanges();
            //}
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
          //  CreateBD(); LBL.DataContext = mainWindow.ViewModel.LevelSetVMs.First();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //ViewModel.CloseDb();
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
          
            //CreateBD();
            //LBL.DataContext = mainWindow.ViewModel.LevelSetVMs.First();
            ////            this.DataContext = ViewModel;
            //           LBL.Content = mainWindow.ViewModel.SelectedLevelSet.Name;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CreateBD();
            ////////LBL.DataContext = mainWindow.ViewModel.LevelSetVMs.First();
        }
    }
}
