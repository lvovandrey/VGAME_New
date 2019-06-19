using LevelSetsEditor.ViewModel;
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

namespace LevelSetsEditor.View.TimeLine
{
    /// <summary>
    /// Логика взаимодействия для TimeLine.xaml
    /// </summary>
    public partial class TimeLine : UserControl
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
            if (SelectedItem == null) return;
            Interval IntervalForSelection=Intervals.Where(n => n.sceneVM.SceneId == SelectedItem.SceneId).FirstOrDefault();
            if (IntervalForSelection!=null)
            SelectInterval(IntervalForSelection, "Событие");
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
            set { SetValue(FullTimeProperty, value);
                RefreshDashes();
            }
        }

        public static readonly DependencyProperty FullTimeProperty =
            DependencyProperty.Register("FullTime", typeof(TimeSpan), typeof(TimeLine), new PropertyMetadata(TimeSpan.FromSeconds(10)));

        void RefreshDashes()
        {

            T1.T_full = FullTime;
            T2.T_full = FullTime;
            int N = (int)Math.Round((FullTime.TotalSeconds / T1.T_el.TotalSeconds))+1;
            T1.FillDashes(N);
            N = (int)Math.Round((FullTime.TotalSeconds / T2.T_el.TotalSeconds)) + 1;
            T2.FillDashes(N);
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

            //if (sender == "Клик")
            //{
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
            Tools.ToolsTimer.Delay(() => { SelectionIsAlreadyChange = false; }, TimeSpan.FromMilliseconds(5));
            SelectedItem = interval.sceneVM;
            //}
            //if (sender == "Событие")
            //{
            //    if (SelectionIsAlreadyChange) { SelectionIsAlreadyChange = false; return; }
            //    foreach (Interval i in Intervals)
            //    {
            //        i.Body.Selected = false;
            //    }
            //    interval.Body.Selected = true;
            //    SelectedInterval = interval;
            //    SelectionIsAlreadyChange = true;
            //    SelectedItem = interval.sceneVM;
            //}
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
                I.Body = null; // это наверное излишне
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



    }



}
