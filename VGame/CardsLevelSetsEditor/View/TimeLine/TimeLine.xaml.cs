﻿using LevelSetsEditor.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    /// Логика взаимодействия для TimeLine.xaml
    /// </summary>
    public partial class TimeLine : UserControl, INotifyPropertyChanged
    {
        List<Interval> Intervals;

        public VideoPlayerMVVM.VideoPlayer videoPlayer;
        public TimeLine()
        {
            InitializeComponent();
            DataContext = this;
            Intervals = new List<Interval>();
            FullTime = TimeSpan.FromSeconds(450);

            T1.T_full = FullTime;
            T1.T_el = TimeSpan.FromSeconds(60);
            T1.ChangeDashesHeight(12);
            T1.ChangeDashesWidth(1);

            T2.T_full = FullTime;
            T2.T_el = TimeSpan.FromSeconds(10);
            T2.ChangeDashesHeight(6);

            Cursor1.Container = this;


            Binding binding = new Binding();

            binding.ElementName = "Cursor1"; // элемент-источник
            binding.Path = new PropertyPath("CRPosition"); // свойство элемента-источника
            binding.Mode = BindingMode.TwoWay;
            this.SetBinding(TimeLine.POSProperty, binding); // установка привязки для элемента-приемника

            OnPOSChanged += TimeLine_OnPOSChanged;
            Cursor1.OnCRPChanged += Cursor1_OnCRPChanged;
            Cursor1.OnStartDrag += Cursor1_OnStartDrag;
            Cursor1.OnEndDrag += Cursor1_OnEndDrag;



            OnSelectedItemChanged += TimeLine_OnSelectedItemChanged;
        }

        private void TimeLine_OnSelectedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Interval IntervalForSelection = null;
            if (SelectedItem == null) return;
            {
                IEnumerable<Interval> v = Intervals.Where(n => n.sceneVM.GetHashCode() == SelectedItem.GetHashCode());
                if( v.Count() > 0 )
                    IntervalForSelection = v.First();
                else if (v.Count() != 1)
                    IntervalForSelection = v.Where(n => n.sceneVM.VideoSegment_TimeBegin == SelectedItem.VideoSegment_TimeBegin).FirstOrDefault();
            }
            if (IntervalForSelection!=null)
                SelectInterval(IntervalForSelection, "Событие");

            Console.WriteLine("   SelectedItem.GetHashCode() = " + SelectedItem.GetHashCode());
            foreach (var item in Intervals)
            {
                Console.WriteLine(item.sceneVM.GetHashCode());
            }
        }


        #region Перемещения курсора
        bool wasplayed = false;
        private void Cursor1_OnStartDrag()
        {
            
            if (videoPlayer != null) { wasplayed = videoPlayer.IsPlaying; videoPlayer.pause(); }
        }
        private void Cursor1_OnEndDrag()
        {
            if ((videoPlayer != null) && (wasplayed)) { videoPlayer.play(); }
        }

        bool PosSelf = false;
        private void Cursor1_OnCRPChanged()
        {
            PosSelf = true;
            POS = Cursor1.CRPosition*1000;
        }

        private void TimeLine_OnPOSChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (PosSelf) { PosSelf = false; return; }
            if (POS>-0.1) Cursor1.CRPosition = POS/1000;
        }
        #endregion



        #region DependencyProperty POS - позиция на видео для биндинга к видеоплееру
        public static readonly DependencyProperty POSProperty = DependencyProperty.Register("POS",
         typeof(double), typeof(TimeLine),
         new FrameworkPropertyMetadata(new PropertyChangedCallback(POSPropertyChangedCallback)));

        public double POS
        {
            get { return (double)GetValue(POSProperty); }
            set { SetValue(POSProperty, value); }
        }


        public event PropertyChanged OnPOSChanged;


        static void POSPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (((TimeLine)d).OnPOSChanged != null)
                ((TimeLine)d).OnPOSChanged(d, e);
        }
        #endregion


        #region intervalsOperations
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
            set {
                SetValue(FullTimeProperty, value);
                RefreshDashes();
                OnPropertyChanged("FullTime");
            }
        }

        public static readonly DependencyProperty FullTimeProperty =
            DependencyProperty.Register("FullTime", typeof(TimeSpan), typeof(TimeLine), new PropertyMetadata(TimeSpan.FromSeconds(10)));

        void RefreshDashes()
        {

            T1.T_full = FullTime;
            T2.T_full = FullTime;
            T10.T_full = FullTime;

            T1.T_el = TimeSpan.FromSeconds(60);
            T2.T_el = TimeSpan.FromSeconds(10);
            T10.T_el = TimeSpan.FromSeconds(600);


            int N = (int)Math.Round((FullTime.TotalSeconds / T1.T_el.TotalSeconds))+1;
            T1.ClearDashes();
            T1.FillDashes(N);

            N = (int)Math.Round((FullTime.TotalSeconds / T2.T_el.TotalSeconds)) + 1;
            T2.ClearDashes();
            T2.FillDashes(N);

            N = (int)Math.Round((FullTime.TotalSeconds / T10.T_el.TotalSeconds)) + 1;
            T10.ClearDashes();
            T10.FillDashes(N);


            T1.ChangeDashesHeight(12);
            T1.ChangeDashesWidth(1);

            T2.ChangeDashesHeight(6);

            T10.ChangeDashesHeight(18);
            T10.ChangeDashesWidth(2);

            T1.Visibility = Visibility.Visible;
            T2.Visibility = Visibility.Visible;
            T10.Visibility = Visibility.Visible;

            if (FullTime < TimeSpan.FromMinutes(1))
            {
                T1.TimeLabelVisibility = Visibility.Hidden;
                T2.TimeLabelVisibility = Visibility.Visible;
                T10.TimeLabelVisibility = Visibility.Hidden;
            }
            else if (FullTime<TimeSpan.FromMinutes(20))
            {
                T1.TimeLabelVisibility = Visibility.Visible;
                T2.TimeLabelVisibility = Visibility.Hidden;
                T10.TimeLabelVisibility = Visibility.Hidden;
            }
            else if (FullTime >= TimeSpan.FromMinutes(20))
            {
                T1.TimeLabelVisibility = Visibility.Hidden;
                T2.TimeLabelVisibility = Visibility.Hidden;
                T10.TimeLabelVisibility = Visibility.Visible;
                T2.Visibility = Visibility.Hidden;
            }

        }

        //Добавляем интервал
        public void AddInterval(SceneVM sceneVM)
        {
            Interval interval = new Interval(this, sceneVM);
            this.GridMain.Children.Add(interval.Body);
            interval.Body.Container = this;
            Intervals.Add(interval);
            interval.Body.OnClick += (sender, e) => 
            {
               SelectInterval(interval, "Клик");
            };
            interval.UpdateView();
        }

        public bool SelectionIsAlreadyChange = false;
        //выбрать интервал
        public void SelectInterval(Interval interval, string sender)
        {
            if (SelectionIsAlreadyChange) { return; }
            foreach (Interval i in Intervals)
            {
                i.Body.Selected = false;
                Panel.SetZIndex(i.Body, 0);
            }
            interval.Body.Selected = true;
            Panel.SetZIndex(interval.Body, 1);

            SelectedInterval = interval;
            SelectionIsAlreadyChange = true;
            Tools.ToolsTimer.Delay(() => { SelectionIsAlreadyChange = false; }, TimeSpan.FromMilliseconds(250));
            SelectedItem = interval.sceneVM;
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
        //Удаляем интервалы все
        public void ClearIntervals()
        {
            foreach (Interval I in Intervals)
            {

                this.GridMain.Children.Remove(I.Body);
                I.Body.ClearOnClick();
              //  I.Body = null; // это наверное излишне
            }
            Intervals.Clear();
        }

        #endregion



        #region Свойство зависимости SELECTEDITEM и все что с ним связано
        //создаем свойство инкапсулирующее свойство зависимости
        public SceneVM SelectedItem
        {
            get { return (SceneVM)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        //регистрируем свойство зависимости
        public static readonly DependencyProperty SelectedItemProperty = 
            DependencyProperty.Register("SelectedItem", typeof(SceneVM), typeof(TimeLine), new FrameworkPropertyMetadata(new PropertyChangedCallback(SelectedItemPropertyChangedCallback)));

        //создаем событие уведомляющее об изменении свойства
        public event PropertyChanged OnSelectedItemChanged;

        //создаем метод который вызывает событие изменения свойства
        private static void SelectedItemPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (((TimeLine)d).OnSelectedItemChanged != null)
                ((TimeLine)d).OnSelectedItemChanged(d, e);
        } // Как и многие жизненные проблемы, эту можно решить сгибанием.....

        #endregion


        #region Свойство зависимости SceneVMs и все что с ним связано
        //создаем свойство инкапсулирующее свойство зависимости
        public ObservableCollection<SceneVM> SceneVMs
        {
            get { return (ObservableCollection<SceneVM>)GetValue(SceneVMsProperty); }
            set { SetValue(SceneVMsProperty, value); }
        }

        //регистрируем свойство зависимости
        public static readonly DependencyProperty SceneVMsProperty =
            DependencyProperty.Register("SceneVMs", typeof(ObservableCollection<SceneVM>), typeof(TimeLine), new FrameworkPropertyMetadata(new PropertyChangedCallback(SceneVMsPropertyChangedCallback)));

       
        //создаем метод который вызывает событие изменения свойства
        private static void SceneVMsPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (((TimeLine)d).SceneVMs == null) return;
            ((TimeLine)d).SceneVMs.CollectionChanged += ((TimeLine)d).SceneVMs_CollectionChanged;
            ((TimeLine)d).RefreshAll();

        } // Как и многие жизненные проблемы, эту можно решить сгибанием.....

        private void SceneVMs_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            RefreshAll();
        }

        #endregion
        #region Свойство зависимости CurTime и все что с ним связано
        //создаем свойство инкапсулирующее свойство зависимости
        public TimeSpan CurTime
        {
            get { return (TimeSpan)GetValue(CurTimeProperty); }
            set { SetValue(CurTimeProperty, value); }
        }

        //регистрируем свойство зависимости
        public static readonly DependencyProperty CurTimeProperty =
            DependencyProperty.Register("CurTime", typeof(TimeSpan), typeof(TimeLine));


        #endregion



        /// <summary>
        /// Все удалить нахер и отрисовать заново
        /// </summary>
        public void RefreshAll()
        {
            if (SceneVMs == null) return;

            ClearIntervals();
            foreach (SceneVM sceneVM in SceneVMs)
            {
                AddInterval(sceneVM);
            }
        }

        /// <summary>
        /// Отрисовать все не удаляя.
        /// </summary>
        public void RepaintAll()
        {
            if (SceneVMs == null) return;
            foreach ( Interval I in Intervals)
            {
                I.UpdateView();
            }
        }




        private void Grid_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            //Перемещаем курсор в точку клика на таймлайне
            Cursor1.SetPosition(0, e);

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
