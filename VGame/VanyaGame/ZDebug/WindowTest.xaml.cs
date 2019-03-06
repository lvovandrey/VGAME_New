using System;
using System.Windows;
using System.Windows.Controls;
using VanyaGame.GameNumber.Interface;
using VanyaGame.GameNumber.Units;
using VanyaGame.Units;
using VanyaGame.Units.Components;
using YouTubeUrlSupplier;

namespace VanyaGame.ZDebug
{
    /// <summary>
    /// Логика взаимодействия для WindowTest.xaml
    /// </summary>
    public partial class WindowTest : Window
    {
        public WindowTest()
        {
            InitializeComponent();
        }
        //VanyaGame.Units.Unit U;
        //HaveBody B;
        //HaveBox HB;
        //CheckedSymbol ChS;
        //DragAndDrop DaD;
        //private Moveable M;
        //PaperGrid paperGrid;

        //private void Button_Click(object sender, RoutedEventArgs e)
        //{
        //    U = new VanyaGame.Units.Unit();
        //    B = new HaveBody("HaveBody", U, new NumberElement());
           
        //    HB = new HaveBox("HaveBox",this, GridMain, U);
        //    M = new Moveable("Moveable", U);
        //    ChS = new CheckedSymbol("CheckedSymbol", U, "4");
        //    DaD = new DragAndDrop("DragAndDrop", U);
        //    HiderShower ShowComp = new HiderShower("HiderShower", U);


        //    paperGrid = new PaperGrid();
        //    Panel.SetZIndex(paperGrid, -100);
        //    paperGrid.Width = 100;
        //    paperGrid.Height = 100;
        //    this.wrap.Children.Add(paperGrid);

        //    U.GetComponent<HaveBody>().Body.HorizontalAlignment = 0;
        //}

        //private void Button_Click_1(object sender, RoutedEventArgs e)
        //{
        //    U.GetComponent<HaveBox>().AddInBox(GridMain, GrdPar);
        //  //  U.GetComponent<HaveBody>().Body.HorizontalAlignment = HorizontalAlignment.Center;
        //    //U.GetComponent<Moveable>().MoveRight();
        //}

        //private void Button_Click_2(object sender, RoutedEventArgs e)
        //{
        //    int i = (int)U.GetComponent<HaveBody>().Body.HorizontalAlignment;
        //    if (i < 3)
        //        i++;
        //    U.GetComponent<HaveBody>().Body.HorizontalAlignment = (HorizontalAlignment)i;
        //    //U.GetComponent<HaveBody>().Body.HorizontalAlignment = ;
        //    // 

        //}

        //private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        //{
        //    string s;
        //    s = e.Key.ToString();
        //    s = e.Key.ToString();
        //    this.Title = s;
        //    s = s.Replace("D", "");
        //    this.Title += "  " + s;
        //    Panel panel = paperGrid.PaperGird;
        //    if (U.GetComponent<CheckedSymbol>().IsPrintedSimbolMatch(s))
        //    {
        //        HiderShower HS = U.GetComponent<HiderShower>();
        //        HS.Complete += () =>
        //         {
        //             HS.ResetCompleteSubscriptions();
        //             U.GetComponent<HaveBox>().AddInBox(GridMain, panel);
        //             Panel.SetZIndex(U.GetComponent<HaveBody>().Body, 100);
        //             HS.Show(1, TimeSpan.FromSeconds(0.3), new Thickness(20), TimeSpan.FromSeconds(0.3));
        //         };
        //        HS.Show(0.5, TimeSpan.FromSeconds(0.3), new Thickness(50), TimeSpan.FromSeconds(0.3));
        //        Panel.SetZIndex(U.GetComponent<HaveBody>().Body, 100);
        //    }
        //    Title = U.GetComponent<HaveBody>().Body.Margin.ToString() + U.GetComponent<HaveBox>().InnerBox.Name.ToString();
        //}

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            TxtLink.Text = YouTubeUrlSupplier.YoutubeGet.GetVideoDirectURL(TxtUri.Text);
            vlc1.vlcPlayer.MediaPlayer.VlcLibDirectory  =new System.IO.DirectoryInfo(@Environment.CurrentDirectory + @"\Tools\vlcLib\");
            // new System.IO.DirectoryInfo(@"c:\Program Files\VideoLAN\VLC\");
            //new System.IO.DirectoryInfo(@"c:\vlcLib\");
            vlc1.vlcPlayer.MediaPlayer.EndInit();

            vlc1.vlcPlayer.MediaPlayer.Play(@TxtLink.Text);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Uri U = new Uri(@YouTubeUrlSupplier.YoutubeGet.GetVideoDirectURL(TxtUri.Text), UriKind.Absolute);//@TxtLink.Text);   (@"c:\1.VANYA GAME\LEVELS\VanjaGame\Level_001\video\1.wmv");
        //    MediaElement1.Source = U;

           // MediaElement1.Source = new Uri(@"http://clips.vorwaerts-gmbh.de/big_buck_bunny.mp4");

        }

        private void Button_Click2(object sender, RoutedEventArgs e)
        {
            vlc1.vlcPlayer.MediaPlayer.Play(new Uri(@"c:\test.mp4"));

          //  MediaElement1.Play();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            vlc1.Visibility = System.Windows.Visibility.Hidden;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            vlc1.Visibility = System.Windows.Visibility.Visible;
        }
    }
}
