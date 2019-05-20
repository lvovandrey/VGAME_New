using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;


namespace ScenesTimeLine.Elements
{
    public delegate void PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e);


    /// <summary>
    /// Логика взаимодействия для ScenesTimeLine.xaml
    /// </summary>
    public partial class ScenesTimeLine : UserControl
    {
        public ScenesTimeLine()
        {
            InitializeComponent();
            Dashes = new ObservableCollection<Dash>();
        }


        public TimeSpan Interval { get; set; }

    

        public static readonly DependencyProperty PositionProperty = DependencyProperty.Register("Position",
          typeof(double), typeof(ScenesTimeLine),
          new FrameworkPropertyMetadata(new PropertyChangedCallback(PositionPropertyChangedCallback)));

        public static readonly DependencyProperty DurationProperty = DependencyProperty.Register("Duration",
          typeof(TimeSpan), typeof(ScenesTimeLine),
          new FrameworkPropertyMetadata(new PropertyChangedCallback(DurationPropertyChangedCallback)));

        public static readonly DependencyProperty CurTimeProperty = DependencyProperty.Register("CurTime",
            typeof(TimeSpan), typeof(ScenesTimeLine),
            new FrameworkPropertyMetadata(new PropertyChangedCallback(CurTimePropertyChangedCallback)));

        public double Position { get { return (double)GetValue(PositionProperty); } set { SetValue(PositionProperty, value); } }
        public TimeSpan Duration { get { return (TimeSpan)GetValue(DurationProperty); } set { SetValue(DurationProperty, value); } }
        public TimeSpan CurTime { get { return (TimeSpan)GetValue(CurTimeProperty); } set { SetValue(CurTimeProperty, value); } }

        public event PropertyChanged OnPositionChanged;
        public event PropertyChanged OnDurationChanged;
        public event PropertyChanged OnCurTimeChanged;

        public ObservableCollection<Dash> Dashes;


        static void PositionPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (((ScenesTimeLine)d).OnPositionChanged != null)
                ((ScenesTimeLine)d).OnPositionChanged(d, e);
        }

        static void DurationPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (((ScenesTimeLine)d).OnDurationChanged != null)
                ((ScenesTimeLine)d).OnDurationChanged(d, e);
        }

        static void CurTimePropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (((ScenesTimeLine)d).OnCurTimeChanged != null)
                ((ScenesTimeLine)d).OnCurTimeChanged(d, e);
        }

    

        public void SetCurTime(TimeSpan time)
        {
        }

        private void CalcDashesPosition()
        {
            int NDashes;
            NDashes = (int)Duration.TotalMinutes;
            Dashes = new ObservableCollection<Dash>();
            double width = this.ActualWidth;
            double dashIntervals = width / Duration.TotalMinutes;
            double curDashLeftCoord = 0;
            for (int i=1; i <= NDashes; i++)
            {
                Dash dash = new Dash();
                dash.Margin = new Thickness(curDashLeftCoord, 0, 0, 0);
                Dashes.Add(dash);
                curDashLeftCoord += dashIntervals;
            }
        }
        public void UpdateDashes()
        {
            if (Dashes.Count > 0)
                for (int i = 0; i < Dashes.Count; i++)
                {
                    MainStack.Children.Add(Dashes[i]);
                }
        }

    }
}


