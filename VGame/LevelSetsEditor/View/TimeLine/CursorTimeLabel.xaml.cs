using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace LevelSetsEditor.View.TimeLine
{
    /// <summary>
    /// Логика взаимодействия для CursorTimeLabel.xaml
    /// </summary>
    public partial class CursorTimeLabel : UserControl, INotifyPropertyChanged
    {
        public CursorTimeLabel()
        {
            InitializeComponent();

            Binding bindingTimeLabelVis = new Binding();
            bindingTimeLabelVis.Source = this;  // элемент-источник
            bindingTimeLabelVis.Path = new PropertyPath("Position"); // свойство элемента-источника
            PosLabel.SetBinding(PosLabel.TimeLabelVisibilityProperty, bindingTimeLabelVis); // установка привязки для элемента-приемника
        }

        public static readonly DependencyProperty TTTProperty = DependencyProperty.Register("TTT",
        typeof(double), typeof(CursorTimeLabel));
        public double TTT
        {
            get { return Position; }
        }

        public static readonly DependencyProperty DurationProperty = DependencyProperty.Register("Duration",
        typeof(TimeSpan), typeof(CursorTimeLabel));

        public TimeSpan Duration {
            get { return (TimeSpan)GetValue(DurationProperty); }
            set { SetValue(DurationProperty, value); OnPropertyChanged("TTT");
            } }

        public static readonly DependencyProperty TimeProperty = DependencyProperty.Register("Time",
        typeof(TimeSpan), typeof(CursorTimeLabel));

        public TimeSpan Time
        {
            get { return (TimeSpan)GetValue(TimeProperty); }
            set
            {
                SetValue(TimeProperty, value);
            }
        }

        public static readonly DependencyProperty PositionProperty = DependencyProperty.Register("Position",
        typeof(double), typeof(CursorTimeLabel));

        public double Position
        {
            get { return (double)GetValue(PositionProperty); }
            set
            {
                SetValue(PositionProperty, value); OnPropertyChanged("TTT");
            }
        }


        #region mvvm
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }



    /// <summary>
    /// Конвертер преобразовывает Margin - прибавляет к нему знаения указанные в ConverterParameter. При этом 
    /// параметр на вход требует строку типа  10#23#0#-55 с разделителями в виде шарпов - она соответствует записи Thickness
    ///  т.е. Left#Top#Right#Bottom
    /// </summary>
    public class CursorTimeLabelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Thickness t = (Thickness)value;

            string[] p = ((string)parameter).Split(new char[] { '#' });
            if (p.Length < 4) return new Thickness();
            List<double> pd = new List<double>();
            foreach (string s in p)
            {
                double dd = 0;
                double.TryParse(s, out dd);
                pd.Add(dd);
            } 

            return (new Thickness(t.Left+pd[0],t.Top + pd[1], t.Right + pd[2], t.Bottom + pd[3]));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue; //-fuck u.   -what?    - I! SAY! FUCK!U!
        }
    }
}
