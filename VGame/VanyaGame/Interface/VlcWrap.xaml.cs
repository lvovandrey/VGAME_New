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

namespace VanyaGame.Interface
{
    /// <summary>
    /// Логика взаимодействия для VlcWrap.xaml
    /// </summary>
    public partial class VlcWrap : UserControl
    {
        public VlcWrap()
        {
            InitializeComponent();
        }
        public bool InitializePlayer()
        {
            try
            {
                vlcPlayer.MediaPlayer.VlcLibDirectory = new System.IO.DirectoryInfo(@Environment.CurrentDirectory + @"\Tools\vlcLib\");
                vlcPlayer.MediaPlayer.EndInit();
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
            }
        }
    }
}
