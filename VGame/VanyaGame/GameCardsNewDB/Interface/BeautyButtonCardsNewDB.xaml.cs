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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace VanyaGame.GameCardsNewDB.Interface
{
    /// <summary>
    /// Логика взаимодействия для BeautyButtonCardsNewDB.xaml
    /// </summary>
    public partial class BeautyButtonCardsNewDB : UserControl
    {
        public BeautyButtonCardsNewDB()
        {
            InitializeComponent();
        }

        bool isFlieing = false;

        private void MouseDown(object sender, MouseButtonEventArgs e)
        {
            var uri = new Uri("pack://application:,,,/Images/HandPushDown.cur");
            var stream = Application.GetResourceStream(uri).Stream;
            var cursor = new Cursor(stream);
            Img.Cursor = cursor;
        }


        private void MouseUp(object sender, MouseButtonEventArgs e)
        {
            isFlieing = true;

            var uri = new Uri("pack://application:,,,/Images/HandPush.cur");
            var stream = Application.GetResourceStream(uri).Stream;
            var cursor = new Cursor(stream);
            Img.Cursor = cursor;

            Storyboard s = (Storyboard)TryFindResource("AnimationHide");
            s.Begin();  // Start animation
        }

        public void Reload()
        {
            this.BeginAnimation(FrameworkElement.OpacityProperty, null);
            Img.BeginAnimation(FrameworkElement.OpacityProperty, null);
            Visibility = Visibility.Visible;
            Opacity = 1;
            Storyboard s = (Storyboard)TryFindResource("AnimationShow");
            s.Begin();  // Start animation
            this.Opacity = 1;
        }




    }
}

