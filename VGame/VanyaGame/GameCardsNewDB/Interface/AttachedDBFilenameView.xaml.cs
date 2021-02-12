using System;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace VanyaGame.GameCardsNewDB.Interface
{
    /// <summary>
    /// Interaction logic for AttachedDBFilenameView.xaml
    /// </summary>
    public partial class AttachedDBFilenameView : UserControl
    {
        public AttachedDBFilenameView()
        {
            InitializeComponent();
        }

        #region Mouse
        bool isFlieing = false;

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            var uri = new Uri("pack://application:,,,/Images/HandPushDown.cur");
            var stream = Application.GetResourceStream(uri).Stream;
            var cursor = new Cursor(stream);
            Cursor = cursor;
        }

        private void OnMouseLeave(object sender, MouseEventArgs e)
        {
            var uri = new Uri("pack://application:,,,/Images/HandPush.cur");
            var stream = Application.GetResourceStream(uri).Stream;
            var cursor = new Cursor(stream);
            Cursor = cursor;

        }

        private void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            isFlieing = true;

            var uri = new Uri("pack://application:,,,/Images/HandPush.cur");
            var stream = Application.GetResourceStream(uri).Stream;
            var cursor = new Cursor(stream);
            Cursor = cursor;

        }


        private void OnMouseEnter(object sender, MouseEventArgs e)
        {
            if (!isFlieing)
            {
                var uri = new Uri("pack://application:,,,/Images/HandPush.cur");
                var stream = Application.GetResourceStream(uri).Stream;
                var cursor = new Cursor(stream);
                Cursor = cursor;
            }
        }
        #endregion
    }

    public class AttachedDBCardsFilenameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is string) || !File.Exists((string)value)) return "БД не выбрана или не существует";

            return "Загружаемая БД: " + Path.GetFileName((string)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
