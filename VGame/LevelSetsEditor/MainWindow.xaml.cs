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
using System.Threading;
using LevelSetsEditor.View;

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


            InitializeComponent();
            
            WebBrowserVM = new WebBrowserVM(Browser);
            GridBrowser.DataContext = WebBrowserVM;
            mainWindow = this;

            StartWindowInfo();

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
            ViewModel.SelectedLevelVM.VideoInfoVM.PreviewVM.Source = new Uri(vidInfo.ImageUrl);
            ViewModel.SelectedLevelVM.VideoInfoVM.PreviewVM.Size = new System.Drawing.Size(480, 360);

            ViewModel.SelectedLevelVM.VideoInfoVM.PreviewVM.Type = Model.PreviewType.youtube;
            ObservableCollection<Uri> uris = new ObservableCollection<Uri>();
            for (int i = 0; i < 3; i++)
                uris.Add(new Uri(vidInfo.PrevImagesUrl[i]));

            ViewModel.SelectedLevelVM.VideoInfoVM.PreviewVM.MultiplePrevSources = uris;
       //     ViewModel.SelectedLevelVM.SegregateScenes();
        }



      

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel = new VM(new VideoPlayerVM(VideoPlayer), this);
        }

        private void Button_Click_JOIN(object sender, RoutedEventArgs e)
        {
            Button_Click_2(sender, e);
        }

        private void Button_Click_NEW_LVL(object sender, RoutedEventArgs e)
        {

        }

        private void LevelViewDB_Loaded(object sender, RoutedEventArgs e)
        {

        }



        private void StartWindowInfo()
        {
            Thread newWindowThread = new Thread(new ThreadStart(ThreadStartingPoint));
            newWindowThread.SetApartmentState(ApartmentState.STA);
            newWindowThread.IsBackground = true;
            newWindowThread.Start();
        }
        public WindowProgress windowProgress;
        private void ThreadStartingPoint()
        {
            windowProgress = new WindowProgress();
            windowProgress.Hide();
            System.Windows.Threading.Dispatcher.Run();
        }



        private void Button_Click_3(object sender, RoutedEventArgs e)
        {

        }

        private void SplitScene_Click(object sender, RoutedEventArgs e)
        {
            VideoPlayer.pause();
        }


    }
}
