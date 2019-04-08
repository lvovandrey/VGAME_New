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

namespace LevelSetsEditor
{
    
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        WebBrowserVM WebBrowserVM;
        public MyViewModel ViewModel;
        public static MainWindow mainWindow;
        public MainWindow()
        {
            CefSettings settings = new CefSettings();
            settings.CachePath = @"C:\CEFcookies"; // Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\CEF";
            CefSharp.Cef.Initialize(settings);

            
            //LevelSetVMDataContext.LevelSet = new Model.LevelSet();

            InitializeComponent();
            //WebBrowserVM Browser = new WebBrowserVM(new Model.WebBrowserModel());
            WebBrowserVM = new WebBrowserVM(Browser);
            GridBrowser.DataContext = WebBrowserVM;
            mainWindow = this;

            ViewModel = new MyViewModel();
            ViewModel.OpenDb();
            MyViewModel.LevelsFromDB = new LevelsFromDB(ViewModel.db);
            TabItemLevels.DataContext = MyViewModel.LevelsFromDB;

            //LevelSetVMDataContext.LevelSet.SceneSets = new List<Model.SceneSet>();
            //LevelSetVMDataContext.LevelSet.VideoInfo = new Model.VideoInfo();
            //LevelSetVMDataContext.LevelSet.VideoInfo.Preview = new Model.Preview();


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

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            TabItemEditor.Focus();
            
            YoutubeVidInfo vidInfo = new YoutubeVidInfo(TextURL.Text);
            if (vidInfo.DirectURL == "") return;

            LevelSetVMDataContext.VideoInfoVM.Source = new Uri(vidInfo.DirectURL);
            LevelSetVMDataContext.VideoInfoVM.Address = TextURL.Text;
            LevelSetVMDataContext.VideoInfoVM.Description = vidInfo.Title;
            LevelSetVMDataContext.VideoInfoVM.Duration = vidInfo.Duration;
            LevelSetVMDataContext.VideoInfoVM.Resolution = vidInfo.Resolution;
            LevelSetVMDataContext.VideoInfoVM.Title = vidInfo.Title;
            LevelSetVMDataContext.VideoInfoVM.Type = Model.VideoType.youtube;
        //    LevelSetVMDataContext.VideoInfoVM.Preview.
            //  LevelSetVMDataContext.SceneSets.

            LevelSetVMDataContext.VideoInfoVM.PreviewVM.Source = new Uri(vidInfo.ImageUrl);
            LevelSetVMDataContext.VideoInfoVM.PreviewVM.Size = new System.Drawing.Size(480, 360);

            LevelSetVMDataContext.VideoInfoVM.PreviewVM.Type = Model.PreviewType.youtube;
            List<Uri> uris = new List<Uri>();
            for (int i = 0; i < 3; i++)
               uris.Add(new Uri(vidInfo.PrevImagesUrl[i]));

            LevelSetVMDataContext.VideoInfo.Preview.MultiplePrevSources = uris;
            LevelSetVMDataContext.SegregateScenes();




            //Model.SceneSet ss = new Model.SceneSet();
            //ss.VideoSegment = new Model.VideoSegment();
            //ss.VideoSegment.TimeBegin = TimeSpan.FromSeconds(1111);
            //LevelSetVMDataContext.SceneSetVMs.Add(new SceneSetVM(ss));

            //Model.SceneSet ss2 = new Model.SceneSet();
            //ss2.VideoSegment = new Model.VideoSegment();
            //ss2.VideoSegment.TimeBegin = TimeSpan.FromSeconds(2222);
            //LevelSetVMDataContext.SceneSetVMs.Add(new SceneSetVM(ss2));

            //Model.SceneSet ss3 = new Model.SceneSet();
            //ss3.VideoSegment = new Model.VideoSegment();
            //ss3.VideoSegment.TimeBegin = TimeSpan.FromSeconds(3333);
            //LevelSetVMDataContext.SceneSetVMs.Add(new SceneSetVM(ss3));


            ////  YouTubeUrlSupplier.YoutubeGet.
            //  YoutubeVidInfo VidInfo = new YoutubeVidInfo
        }

        private void AutoSegregateVideoToScenes(LevelSetVM LSet)
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
            
            this.VideoPlayer.SetPosition( 1000*LevelSetVMDataContext.SelectedSceneSetVM.VideoSegment_TimeBegin.TotalSeconds/LevelSetVMDataContext.VideoInfoVM.Duration.TotalSeconds);
        }

        private void CreateBD()
        {

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
            CreateBD();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ViewModel.CloseDb();
        }
    }
}
