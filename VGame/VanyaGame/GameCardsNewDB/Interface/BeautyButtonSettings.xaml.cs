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
    public partial class BeautyButtonSettings : UserControl
    {
        public BeautyButtonSettings()
        {
            InitializeComponent();
        }

        bool isFlieing = false;

        private void Img_MouseDown(object sender, MouseButtonEventArgs e)
        {
           // this.Img.Source = new BitmapImage(new Uri("pack://application:,,,/1.GameNumber/Images/btn_start2.png"));
            var uri = new Uri("pack://application:,,,/Images/HandPushDown.cur");
            var stream = Application.GetResourceStream(uri).Stream;
            var cursor = new Cursor(stream);
            Img.Cursor = cursor;
            //    AddDropShadowEffect(Img, 10, 20, 80);
        }

        private void Img_MouseLeave(object sender, MouseEventArgs e)
        {
            //if (!isFlieing)
            //    this.Img.Source = new BitmapImage(new Uri("pack://application:,,,/1.GameNumber/Images/btn_start.png"));


        }

        private void Img_MouseUp(object sender, MouseButtonEventArgs e)
        {
            isFlieing = true;

            var uri = new Uri("pack://application:,,,/Images/HandPush.cur");
            var stream = Application.GetResourceStream(uri).Stream;
            var cursor = new Cursor(stream);
            Img.Cursor = cursor;

        }


        private void Img_MouseEnter(object sender, MouseEventArgs e)
        {
            if (!isFlieing)
            {
                var uri = new Uri("pack://application:,,,/Images/HandPush.cur");
                var stream = Application.GetResourceStream(uri).Stream;
                var cursor = new Cursor(stream);
                Img.Cursor = cursor;
            }
        }


    }
}

