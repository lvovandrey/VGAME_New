using System;
using System.ComponentModel;
using System.Windows;

namespace ScenesTimeLine.Elements
{
    public class Interval : INotifyPropertyChanged
    {
         TimeScale _Container;
         SceneTimeView _Body;
         TimeSpan _Begin;
         TimeSpan _End;
         int _Zindex;
        bool _LabelVisibility;


        public Interval(TimeScale container, TimeSpan begin, TimeSpan end, int zindex = 1)
        {
            Container = container;
            Begin = begin;
            End = end;
            Zindex = zindex;
            Body = new SceneTimeView();
            Body.HorizontalAlignment = HorizontalAlignment.Left;
            SubscribeDepPropertyEvents();

            Body.Selected = false;
            UpdateView();
        }

        void SubscribeDepPropertyEvents()
        {
            //Подписываемся на изменение DependedcyProperty FullTime у контейнера
            PropertyDescriptor FullTimeDesc = DependencyPropertyDescriptor.FromProperty(TimeScale.FullTimeProperty, typeof(TimeScale));
            FullTimeDesc.AddValueChanged(Container, 
                new EventHandler(
                (sender, e) =>
                {
                    UpdateView();
                }            
            ));

            //Подписываемся на изменение DependedcyProperty ActualWidth у контейнера
            PropertyDescriptor ActualWidthDesc = DependencyPropertyDescriptor.FromProperty(TimeScale.ActualWidthProperty, typeof(TimeScale));
            ActualWidthDesc.AddValueChanged(Container,
                new EventHandler(
                (sender, e) =>
                { UpdateView(); }
            ));

        }

        public TimeScale Container
        {
            get { return _Container; }
            set { _Container = value; OnPropertyChanged("Container"); }
        }

        public SceneTimeView Body
        {
            get { return _Body; }
            set { _Body = value; OnPropertyChanged("Body"); }
        }
        public TimeSpan Begin
        {
            get { return _Begin; }
            set
            {
                if (value == _Begin) return;
                _Begin = value;
                OnPropertyChanged("Begin");
                UpdateView();
            }
        }
        public TimeSpan End
        {
            get { return _End; }
            set
            {
                if (value == _End) return;
                _End = value;
                OnPropertyChanged("End");
                UpdateView();
            }
        }
        public int Zindex
        {
            get { return _Zindex; }
            set { _Zindex = value; OnPropertyChanged("Zindex"); }
        }

        public bool LabelVisibility 
        {
            get { return _LabelVisibility; }
            set
            {
                _LabelVisibility = value;
                if (value) Body.TimeLabel.Visibility = Visibility.Visible;
                else Body.TimeLabel.Visibility = Visibility.Hidden;
                OnPropertyChanged("LabelVisibility");
            }
        }

        public bool Selected
        {
            get { return _LabelVisibility; }
            set
            {
                _LabelVisibility = value;
                if (value) Body.TimeLabel.Visibility = Visibility.Visible;
                else Body.TimeLabel.Visibility = Visibility.Hidden;
                OnPropertyChanged("LabelVisibility");
            }
        }

        public void UpdateView()
        {
            if (Body == null) return;
            double beg = Begin.TotalMilliseconds;
            double end = End.TotalMilliseconds;
            double tfull = Container.FullTime.TotalMilliseconds;
            double conWidth = Container.ActualWidth;

            double NewBodyWidth = conWidth * (end-beg) / tfull;
            if (Body.ActualWidth != NewBodyWidth) Body.Width = NewBodyWidth;

            double NewBodyLeft = conWidth * (beg) / tfull;
            if (Body.Margin.Left != NewBodyLeft) Body.Margin = new Thickness(NewBodyLeft, Body.Margin.Top, Body.Margin.Right, Body.Margin.Bottom);

            if (Body.Width > 90) { LabelVisibility = true; } else { LabelVisibility = false; }
            Body.TimeLabel.End = End;
            Body.TimeLabel.Begin = Begin;


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
}
