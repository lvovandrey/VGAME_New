using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

using System.ComponentModel;
using System.Globalization;


namespace ScenesTimeLine.Elements
{
    /// <summary>
    /// Логика взаимодействия для Cursor.xaml
    /// </summary>
    public partial class Cursor : UserControl
    {
        public Cursor()
        {
            InitializeComponent();
        }
    }

    public class HeightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((double)value + 12);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }

}
