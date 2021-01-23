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
using System.Windows.Threading;
using System.Threading;

namespace VanyaGame.GameCardsNewDB.Units
{
    /// <summary>
    /// Логика взаимодействия для NumberElement.xaml
    /// </summary>
    public partial class CardUnitElement : UserControl
    {
        public CardUnitElement()
        {
            InitializeComponent();
        }

        private void UserControl_MouseLeave(object sender, MouseEventArgs e)
        {
            Storyboard s = (Storyboard)TryFindResource("ShowSB");
            s.Begin();
        }

        private void UserControl_MouseEnter(object sender, MouseEventArgs e)
        {
            Storyboard s = (Storyboard)TryFindResource("Hide6SB");
            s.Begin();
        }

        private void MouseDown(object sender, MouseButtonEventArgs e)
        {
            Console.WriteLine("!!!!!!!!!!!!!!!!Down");
            var uri = new Uri("pack://application:,,,/Images/HandPushDown.cur");
            var stream = Application.GetResourceStream(uri).Stream;
            var cursor = new Cursor(stream);
            Cursor = cursor;

            Storyboard s = (Storyboard)TryFindResource("HideSB");
            s.Begin();
        }


        private void MouseUp(object sender, MouseButtonEventArgs e)
        {
            Console.WriteLine("!!!!!!!!!!!!!!!! UP");
            var uri = new Uri("pack://application:,,,/Images/HandPush.cur");
            var stream = Application.GetResourceStream(uri).Stream;
            var cursor = new Cursor(stream);
            Cursor = cursor;

            Storyboard s = (Storyboard)TryFindResource("Hide6SB");
            s.Begin();
        }


        public void Flash(TimeSpan Period, double Size)
        {
            DoubleAnimation AW = new DoubleAnimation();
            AW.From = 0;
            AW.To = Size;
            AW.AutoReverse = true;
            AW.Duration = Period;

            DoubleAnimation AH = new DoubleAnimation();
            AH.From = 0;
            AH.To = Size;
            AH.AutoReverse = true;
            AH.Duration = Period;

            FlashedRect.BeginAnimation(Button.WidthProperty, AW);
            FlashedRect.BeginAnimation(Button.HeightProperty, AH);
        }


    }
}
