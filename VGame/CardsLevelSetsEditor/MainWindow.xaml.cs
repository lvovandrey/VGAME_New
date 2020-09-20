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
using LevelSetsEditor.Tools;

namespace LevelSetsEditor
{
    
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public VM ViewModel;
        public static MainWindow mainWindow;
        public MainWindow()
        {
            CefSettings settings = new CefSettings();
            settings.CachePath = @"C:\CEFcookies"; // Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\CEF";
            CefSharp.Cef.Initialize(settings);


            InitializeComponent();

          
            mainWindow = this;
            startNewWindowProgress();
        }
            


        private async void RefreshYoutubeVideoInfo(VideoInfoVM videoInfoVM)
        {

            if (videoInfoVM == null) return;
            if (videoInfoVM.Title == null) return;
            if (videoInfoVM.Type != Model.VideoType.youtube) return;
            
            YoutubeVidInfo vidInfo = new YoutubeVidInfo(videoInfoVM.Title);
            await vidInfo.NEWLIBRARY_GetVideoAsync();
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



      

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel = new VM( this);
            //подписываемся на событие выбора нового Level -поверь пригодится...
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;

        }

        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedLevelVM" && ViewModel.SelectedLevelVM!=null)
            {
                //для начала отвязать попробуем
                try { ViewModel.SelectedLevelVM.PropertyChanged -= SelectedLevelVM_PropertyChanged; }
                finally
                {
                    //подписываемся на событие выбора новой сцены (т.к. ListView надо бы обновлять)
                    ViewModel.SelectedLevelVM.PropertyChanged += SelectedLevelVM_PropertyChanged;
                }
            }
        }

        bool Hadndled1 = false;//флаг для предотвращения бесконечного цикла  выбора сцены
        private void SelectedLevelVM_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (Hadndled1) { Hadndled1 = false; return; }

            //Обработка события выбора новой сцены (т.к. ListView надо бы обновлять)
            if (e.PropertyName == "SelectedSceneVM")
            {
                
               // index = 4;
                Tools.ToolsTimer.Delay(() =>   //Die, die die my darling I'll be seeing you in Hell
                {
                    Hadndled1 = true;
                    int i = 0;
                    SceneVM sceneVM = ViewModel.SelectedLevelVM.SelectedSceneVM;
                    foreach(SceneVM s in ViewModel.SelectedLevelVM.SceneVMs)
                    {
                        i++;
                        if (s.SceneId == sceneVM.SceneId) {  break; }
                    }

                   
                //    SceneListBox.SelectedIndex = i-1;
                }, TimeSpan.FromMilliseconds(20));
            }
        }




        private void Button_Click_NEW_LVL(object sender, RoutedEventArgs e)
        {

        }

        private void LevelViewDB_Loaded(object sender, RoutedEventArgs e)
        {

        }




        public WindowProgress windowProgress;
        public void startNewWindowProgress()
        {
            if (WindowProgress.count > 0) { MessageBox.Show("Можно создать только одно окно infoUI."); }
            //сперва хотел синглтон, потом сделал просто счетчик. 
            //Потому что синглтон в своем потоке - это как-то сложно пока для меня
            else
            {
                WindowProgressThread = new Thread(new ThreadStart(ThreadStartingPoint));
                WindowProgressThread.SetApartmentState(ApartmentState.STA);
                WindowProgressThread.IsBackground = true;        
                WindowProgressThread.Start();
            }

        }
        Thread WindowProgressThread;
        private void ThreadStartingPoint()
        {
                windowProgress = new WindowProgress();
                windowProgress.Hide();
                System.Windows.Threading.Dispatcher.Run();
       }
       



        private void Button_Click_3(object sender, RoutedEventArgs e)
        {

        }


        private void MAINWINDOW_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (WindowProgress.count > 0)
                try
                {
                    WindowProgressThread.Abort();
                }
                catch
                {
                }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            Speaker.Speak(txtSpeak.Text);         
        }



        private void VideoPlayer_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
