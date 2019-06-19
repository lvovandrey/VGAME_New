using LevelSetsEditor.ViewModel;
using System;
using System.ComponentModel;
using System.Windows;

namespace LevelSetsEditor.View.TimeLine
{
    public class Interval : INotifyPropertyChanged
    {
         TimeLine _Container;
         SceneTimeView _Body;
        public SceneVM sceneVM; 
         int _Zindex;
        bool _LabelVisibility;



        public Interval(TimeLine container, SceneVM _sceneVM, int zindex = 1)
        {
            Container = container;
            Zindex = zindex;
            Body = new SceneTimeView(this);
            Body.HorizontalAlignment = HorizontalAlignment.Left;
            sceneVM = _sceneVM;
            SubscribeDepPropertyEvents();

            Body.Selected = false;
            UpdateView();
        }

        void SubscribeDepPropertyEvents()
        {
            //Подписываемся на изменение DependedcyProperty FullTime у контейнера
            PropertyDescriptor FullTimeDesc = DependencyPropertyDescriptor.FromProperty(TimeLine.FullTimeProperty, typeof(TimeLine));
            FullTimeDesc.AddValueChanged(Container, 
                new EventHandler(
                (sender, e) =>
                {
                    UpdateView();
                }            
            ));

            //Подписываемся на изменение DependedcyProperty ActualWidth у контейнера
            PropertyDescriptor ActualWidthDesc = DependencyPropertyDescriptor.FromProperty(TimeLine.ActualWidthProperty, typeof(TimeLine));
            ActualWidthDesc.AddValueChanged(Container,
                new EventHandler(
                (sender, e) =>
                { UpdateView(); }
            ));

        }

        public TimeLine Container
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
            get { return sceneVM.VideoSegment_TimeBegin; }
            set
            {
                if (value == sceneVM.VideoSegment_TimeBegin) return;
                sceneVM.VideoSegment_TimeBegin = value;
                OnPropertyChanged("Begin");
                UpdateView();
            }
        }
        public TimeSpan End
        {
            get { return sceneVM.VideoSegment_TimeEnd; }
            set
            {
                if (value == sceneVM.VideoSegment_TimeEnd) return;
                sceneVM.VideoSegment_TimeEnd = value;
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
            //if (Body.ActualWidth != NewBodyWidth)
                Body.Width = NewBodyWidth;

            double NewBodyLeft = conWidth * (beg) / tfull;
            //if (Body.Margin.Left != NewBodyLeft)
                Body.Margin = new Thickness(NewBodyLeft, Body.Margin.Top, Body.Margin.Right, Body.Margin.Bottom);

            if (Body.Width > 90) { LabelVisibility = true; } else { LabelVisibility = false; }
            Body.TimeLabel.End = End;
            Body.TimeLabel.Begin = Begin;
        }

        public void UpdateFromUI()
        {
            if (Body.ActualWidth > 90) { LabelVisibility = true; } else { LabelVisibility = false; }

            double tfull = Container.FullTime.TotalMilliseconds;
            double conWidth = Container.ActualWidth;
            double NewBegin = Body.Margin.Left * tfull / conWidth;
            double NewEnd = Body.ActualWidth * tfull / conWidth + NewBegin;

            sceneVM.VideoSegment_TimeBegin = TimeSpan.FromMilliseconds(NewBegin);
            sceneVM.VideoSegment_TimeEnd = TimeSpan.FromMilliseconds(NewEnd);

            Body.TimeLabel.End = End;
            Body.TimeLabel.Begin = Begin;

            OnPropertyChanged("Begin");
            OnPropertyChanged("End");
          //  UpdateView();
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
