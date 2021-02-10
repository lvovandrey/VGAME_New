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

namespace CardsEditor.View
{
    /// <summary>
    /// Логика взаимодействия для CardView.xaml
    /// </summary>
    public partial class CardView : UserControl
    {
        public CardView()
        {
            InitializeComponent();
        }

        private void MediaElement_Loaded(object sender, RoutedEventArgs e)
        {
            MediaElement.Position = TimeSpan.Zero;
            MediaElement.Play();
            isPlaying = true;
        }

        private bool isPlaying;
        private void MediaElement_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (isPlaying) { MediaElement.Pause(); isPlaying = false; }
            else { MediaElement.Play(); isPlaying = true; }
        }

        private void MediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            MediaElement.Position = TimeSpan.Zero;
            isPlaying = false;
        }
    }
}
