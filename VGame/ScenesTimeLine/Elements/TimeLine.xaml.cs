using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace ScenesTimeLine.Elements
{
    /// <summary>
    /// Логика взаимодействия для TimeLine.xaml
    /// </summary>
    public partial class TimeLine : UserControl
    {
        ObservableCollection<Interval> Intervals;
      

        public TimeLine()
        {
            InitializeComponent();

            Intervals = new ObservableCollection<Interval>();
            FullTime = TimeSpan.FromSeconds(450);

            T1.T_full = FullTime;
            T1.T_el = TimeSpan.FromSeconds(60);
            T1.ChangeDashesHeight(12);
            T1.ChangeDashesWidth(1);

            T2.T_full = FullTime;
            T2.T_el = TimeSpan.FromSeconds(10);
            T2.ChangeDashesHeight(6);

            Cursor1.Container = this;

        }

        //DependencyProperty SelectedInterval  - Попробовать с ним поработать снаружи
        public Interval SelectedInterval
        {
            get { return (Interval)GetValue(SelectedIntervalProperty); }
            set { SetValue(SelectedIntervalProperty, value); }
        }
        public static readonly DependencyProperty SelectedIntervalProperty =
            DependencyProperty.Register("SelectedInterval", typeof(Interval), typeof(TimeLine), new PropertyMetadata(null));
    

        //DependencyProperty FullTime  - чтобы можно было подписаться на него
        public TimeSpan FullTime
        {
            get { return (TimeSpan)GetValue(FullTimeProperty); }
            set { SetValue(FullTimeProperty, value); }
        }
        public static readonly DependencyProperty FullTimeProperty =
            DependencyProperty.Register("FullTime", typeof(TimeSpan), typeof(TimeLine), new PropertyMetadata(TimeSpan.FromSeconds(10)));

        //Добавляем интервал
        public void AddInterval(TimeSpan begin, TimeSpan end)
        {
            Interval interval = new Interval(this, begin, end);
            this.GridMain.Children.Add(interval.Body);
            Intervals.Add(interval);
            interval.Body.OnClick += (sender, e) => 
            {
                foreach (Interval i in Intervals)
                {
                    i.Body.Selected = false;
                }
                ((SceneTimeView)sender).Selected = true;
                SelectedInterval = interval;
            };
            interval.UpdateView();
        }
        
        //Удаляем интервал
        public void RemoveInterval(Interval interval)
        {
            this.GridMain.Children.Remove(interval.Body);
            Intervals.Remove(interval);
            interval.Body.ClearOnClick();
            interval.Body = null; // это наверное излишне
            interval = null;
        }

    }



}
