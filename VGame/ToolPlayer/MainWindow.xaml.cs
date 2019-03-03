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
using Microsoft.Win32;
using System.Windows;
using System.Windows.Threading;
using System.IO;
using VanyaGame;


namespace ToolPlayer
{
    public interface IDialogService
    {
        void ShowMessage(string message);   // показ сообщения
        string FilePath { get; set; }   // путь к выбранному файлу
        bool OpenFileDialog();  // открытие файла
       // bool SaveFileDialog();  // сохранение файла
    }

    public class DefaultDialogService : IDialogService
    {
        public string FilePath { get; set; }

        public bool OpenFileDialog()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                FilePath = openFileDialog.FileName;
                return true;
            }
            return false;
        }

        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }
    }
    
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool IsPlaying = false;
        bool IsRewinding = false;
        TimeSpan rewindPosition;
        DispatcherTimer timer;
        DispatcherTimer timerRewind;
        Tlevel lev1 = new Tlevel();
        string filename = "";
        string DirName = "";
        string fullFileName = "";
        string VideoType = "*.avi";

        public MainWindow()
        {
            InitializeComponent();
            player.MediaOpened += player_MediaOpened;
            Tools.CreateAndStart_DispatcherTimer(ref timer, Tick, new TimeSpan(0,0,0,0,100));
            lev1.sublevel = new Tsublevel[10];
            lev1.sublevels_count = 1;
            lev1.number = "1";
            lev1.sublevel[0] = new Tsublevel();
            lev1.sublevel[0].filename = filename;
            lev1.sublevel[0].filenumber = "1";
        }


        
        void player_MediaOpened(object sender, System.Windows.RoutedEventArgs e)
        {
            SliderTime.Minimum = 0;
            SliderTime.Maximum = player.NaturalDuration.TimeSpan.TotalSeconds;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DefaultDialogService DLGsrv = new DefaultDialogService();
            if (DLGsrv.OpenFileDialog())
            {
                IsPlaying = false;
                MyWindow.player.Source = new Uri(@DLGsrv.FilePath);
                fullFileName = @DLGsrv.FilePath;

                filename = System.IO.Path.GetFileName(@fullFileName);

                string DirName_ = System.IO.Path.GetDirectoryName(@fullFileName);

                DirName = DirName_;
                string[] filePaths = Directory.GetFiles(DirName, VideoType, SearchOption.TopDirectoryOnly);
                int fileCount = filePaths.Length;
                for (int i = 0; i < fileCount; i++)
                {
                    if (filePaths[i] == fullFileName)
                    {
                        lev1.sublevel[lev1.sublevels_count - 1].filenumber = (i+1).ToString();
                        break;
                    }
                }

            }
            TxtShow();
        }



        private void Button_Click_1(object sender, System.Windows.RoutedEventArgs e)
        {
            if (IsPlaying)
            {
                try
                {
                    MyWindow.player.Pause();
                    IsPlaying = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка: " + ex.Message);
                }
            }
            else 
            {
                try
                {
                    MyWindow.player.Play();
                    IsPlaying = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка: " + ex.Message);
                }
            }
        }

        private void TxtShow()
        {
            if (MyWindow.TxtBlockVideoType.Text!= "") VideoType = MyWindow.TxtBlockVideoType.Text;
            MyWindow.TxtBlock.Text = "";            
            filename = System.IO.Path.GetFileName(@fullFileName);

            string DirName_ = System.IO.Path.GetDirectoryName(@fullFileName);

            DirName = DirName_;
            string[] filePaths = Directory.GetFiles(DirName, VideoType, SearchOption.TopDirectoryOnly);
            int fileCount = filePaths.Length;
            for (int i = 0; i < fileCount; i++)
            {
                if (filePaths[i] == fullFileName)
                {
                    lev1.sublevel[lev1.sublevels_count - 1].filenumber = (i + 1).ToString();
                    break;
                }
            }

                lev1.sublevel[lev1.sublevels_count-1].filename = filename;
                
                for(int i = 0; i < lev1.sublevels_count; i++)
                {
                    MyWindow.TxtBlock.Text += "\r\n" + lev1.sublevel[i].timeBegin;
                    MyWindow.TxtBlock.Text += "    " + lev1.sublevel[i].timeEnd;
                    MyWindow.TxtBlock.Text += "    " + lev1.sublevel[i].filename;
                    MyWindow.TxtBlock.Text += "    " + lev1.sublevel[i].filenumber;
                    MyWindow.TxtBlock.Text += "    " + lev1.sublevel[i].number;
                }
                

                
        }
        private void Button_Click_2(object sender, System.Windows.RoutedEventArgs e)
        {
            string s = MyWindow.player.Position.ToString(@"hh\:mm\:ss\.ff");
            lev1.sublevel[lev1.sublevels_count - 1].timeBegin = s;
            TxtShow();
        }

        private void Button_Click_4(object sender, System.Windows.RoutedEventArgs e)
        {
            string s = MyWindow.player.Position.ToString(@"hh\:mm\:ss\.ff");
            lev1.sublevel[lev1.sublevels_count - 1].timeEnd = s;

            MyWindow.TxtBlock.Text = "";
            for (int i = 0; i < lev1.sublevels_count; i++)
            {
                MyWindow.TxtBlock.Text += "\r\n" + lev1.sublevel[i].timeBegin;
                MyWindow.TxtBlock.Text += "    " + lev1.sublevel[i].timeEnd;
            }
            TxtShow();
        }


        public void Tick(object sender, EventArgs e)
        {
            try
            {
                if (!IsRewinding)
                {
                    SliderTime.Value = player.Position.TotalSeconds;
                    LabelTime.Content = player.Position.ToString(@"hh\:mm\:ss\.ff");
                }
            }
            catch
            {

            }
        }

        private void SliderTime_ValueChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<double> e)
        {

        }

        public void TickRewind(object sender, EventArgs e)
        {

        
        }



        private void SliderTime_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            
            player.Position = TimeSpan.FromSeconds(SliderTime.Value); 
            //player.Play();
            IsRewinding = false;

        }

        private void SliderTime_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            
            IsRewinding = true;
            player.Pause();
        }

        private void SliderTime_MouseLeave(object sender, MouseEventArgs e)
        {
           IsRewinding = false;
        }

        
        
        
        private void Button_Click_3(object sender, System.Windows.RoutedEventArgs e)
        {
            

        }

        private void Button_Click_5(object sender, System.Windows.RoutedEventArgs e)
        {
            if (lev1.sublevels_count == 1) 
            {
                lev1.sublevel[lev1.sublevels_count - 1].filename = filename;
            }
            
            lev1.sublevels_count++;            
            lev1.sublevel[lev1.sublevels_count - 1] = new Tsublevel();
            lev1.sublevel[lev1.sublevels_count - 1].filename = filename;
            
            filename = System.IO.Path.GetFileName(@fullFileName);

            string DirName_ = System.IO.Path.GetDirectoryName(@fullFileName);

            DirName = DirName_;
            if (MyWindow.TxtBlockVideoType.Text != "") VideoType = MyWindow.TxtBlockVideoType.Text;
            string[] filePaths = Directory.GetFiles(DirName, VideoType, SearchOption.TopDirectoryOnly);
            int fileCount = filePaths.Length;
            for (int i = 0; i < fileCount; i++)
            {
                if (filePaths[i] == fullFileName)
                {
                    lev1.sublevel[lev1.sublevels_count - 1].filenumber = (i + 1).ToString();
                    break;
                }
            }
            TxtShow();
        
        }

        private void Button_Click_6(object sender, System.Windows.RoutedEventArgs e)
        {
            lev1.number = MyWindow.TxtBlock2.Text;

            XMLWriter.WriteTest2(lev1, DirName);
            TxtShow();
        }

        private void Button_Click_7(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            var key = e.Key;
            if (key == Key.Space) { Button_Click_1(null, null); e.Handled = true;}
            
        }







    }
}
