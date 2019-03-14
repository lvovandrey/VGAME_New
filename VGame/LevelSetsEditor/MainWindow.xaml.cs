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
using LevelSetsEditor.Model;

namespace LevelSetsEditor
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        WebBrowserVM WebBrowserVM;
     //   LevelSetVM LevelSetVMDataContext;
        public MainWindow()
        {
            CefSettings settings = new CefSettings();
            settings.CachePath = @"C:\CEFcookies"; // Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\CEF";
            CefSharp.Cef.Initialize(settings);

            InitializeComponent();
            //WebBrowserVM Browser = new WebBrowserVM(new Model.WebBrowserModel());
            WebBrowserVM = new WebBrowserVM(Browser);
            GridBrowser.DataContext = WebBrowserVM;

         //   LevelSetVMDataContext = new LevelSetVM();

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

            VideoInfo VI = new VideoInfo();
            VideoInfoVM VIVM = new VideoInfoVM(VI);
            string adress = TextURL.Text;

            VIVM.Source = new Uri(vidInfo.DirectURL);
            VIVM.Address = adress;
            VIVM.Description = vidInfo.Title;
            VIVM.Duration = vidInfo.Duration;
            VIVM.Resolution = vidInfo.Resolution;
            VIVM.Title = vidInfo.Title;
            VIVM.Type = Model.VideoType.youtube;
          //  LevelSetVMDataContext.SceneSets.

            //VIVM.Preview = new Preview()
            //VIVM.Preview.Source = new Uri(vidInfo.ImageUrl);
            //VIVM.Preview.Size = new System.Drawing.Size(320, 180);
            //VIVM.Preview.Type = Model.PreviewType.youtube;
            //for (int i = 0; i < 3; i++)
            //   VIVM.Preview.MultiplePrevSources[i] = new Uri(vidInfo.PrevImagesUrl[i]);

            LevelSetVMDataContext.VideoInfoVM = VIVM;
            //  YouTubeUrlSupplier.YoutubeGet.
            //  YoutubeVidInfo VidInfo = new YoutubeVidInfo




        }

        private void AutoSegregateVideoToScenes(LevelSetVM LSet)
        {
        }


    }
}
