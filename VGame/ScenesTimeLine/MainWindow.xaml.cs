using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace ScenesTimeLine
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            S = TimeSpan.FromSeconds(0);
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            //for (int i = 0; i < 1000; i++)
            //{
            //    T3.addDash();
            //}

            //T1.T_el = TimeSpan.FromSeconds(60); T1.ChangeDashesHeight(12);T1.ChangeDashesWidth(1);
            //T2.T_el = TimeSpan.FromSeconds(10); T2.ChangeDashesHeight(6);
            //T3.T_el = TimeSpan.FromSeconds(5); T3.ChangeDashesHeight(3);
            //T3.Opacity = 0;
        }
        TimeSpan S;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
           
            TS.AddInterval(S, S+ TimeSpan.FromSeconds(60));
            S += TimeSpan.FromSeconds(40);
        }
    }
    public class TimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (((TimeSpan)value).ToString(@"hh\:mm\:ss"));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
