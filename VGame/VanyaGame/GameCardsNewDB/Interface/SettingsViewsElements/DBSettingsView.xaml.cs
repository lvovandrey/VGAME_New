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

namespace VanyaGame.GameCardsNewDB.Interface.SettingsViewsElements
{
    /// <summary>
    /// Interaction logic for DBSettingsView.xaml
    /// </summary>
    public partial class DBSettingsView : UserControl
    {
        public DBSettingsView()
        {
            InitializeComponent();
        }

        private void Label_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (((Label)sender).DataContext == null) return;
        }
    }

    public class ListItemToPositionConverter : IValueConverter
    {
        #region Implementation of IValueConverter

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var item = value as FrameworkElement;
            if (item != null)
            {
                var lb = FindAncestor<ItemsControl>(item);
                if (lb != null)
                {
                    var index = lb.Items.IndexOf(item.DataContext);
                    return index+1;
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
           return DependencyProperty.UnsetValue;
        }

        public static T FindAncestor<T>(DependencyObject from) where T : class
        {
            if (from == null)
                return null;

            var candidate = from as T;
            return candidate ?? FindAncestor<T>(VisualTreeHelper.GetParent(from));
        }
        #endregion
    }
}
