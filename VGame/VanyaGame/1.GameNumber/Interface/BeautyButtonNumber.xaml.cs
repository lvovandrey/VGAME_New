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
using System.Windows.Media.Animation;

namespace VanyaGame.GameNumber.Interface
{
    /// <summary>
    /// Логика взаимодействия для BeautyButton.xaml
    /// </summary>
    public partial class BeautyButtonNumber : UserControl
    {
        bool isFlieing = false;
        public BeautyButtonNumber()
        {
            InitializeComponent();
        }

        private void Img_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Img.Source = new BitmapImage(new Uri("pack://application:,,,/1.GameNumber/Images/btn_start2.png"));
            var uri = new Uri("pack://application:,,,/1.GameNumber/Images/HandPushDown.cur");
            var stream = Application.GetResourceStream(uri).Stream;
            var cursor = new Cursor(stream);
            Img.Cursor = cursor;
            //    AddDropShadowEffect(Img, 10, 20, 80);
        }

        private void Img_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!isFlieing)
            this.Img.Source = new BitmapImage(new Uri("pack://application:,,,/1.GameNumber/Images/btn_start.png"));
            

        }

        private void Img_MouseUp(object sender, MouseButtonEventArgs e)
        {
            isFlieing = true;

            this.Img.Source = new BitmapImage(new Uri("pack://application:,,,/1.GameNumber/Images/btn_start3.png"));
            var uri = new Uri("pack://application:,,,/1.GameNumber/Images/HandPush.cur");
            var stream = Application.GetResourceStream(uri).Stream;
            var cursor = new Cursor(stream);
            Img.Cursor = cursor;


            Storyboard s = (Storyboard)TryFindResource("AnimationHide");
            s.Begin();  // Start animation}
        }


        private void Img_MouseEnter(object sender, MouseEventArgs e)
        {
            if (!isFlieing)
            {
                this.Img.Source = new BitmapImage(new Uri("pack://application:,,,/1.GameNumber/Images/btn_start1.png"));
                var uri = new Uri("pack://application:,,,/1.GameNumber/Images/HandPush.cur");
                var stream = Application.GetResourceStream(uri).Stream;
                var cursor = new Cursor(stream);
                Img.Cursor = cursor;
            }
        }


    }
}
