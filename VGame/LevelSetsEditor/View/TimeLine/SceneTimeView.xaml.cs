using System;
using System.Collections.Generic;
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
using System.Drawing;

namespace LevelSetsEditor.View.TimeLine
{
    /// <summary>
    /// Логика взаимодействия для SceneTimeView.xaml
    /// </summary>
    public partial class SceneTimeView : UserControl, INotifyPropertyChanged
    {
        Interval interval;
        public SceneTimeView(Interval _interval)
        {
            InitializeComponent();
            DataContext = this;
            interval = _interval;

            LeftLimit.MouseDown += StartDrag;
            RightLimit.MouseDown += StartDrag;
            CentralLimit.MouseDown += StartDrag;

        }

        public event EventHandler OnClick;
        public void ClearOnClick()
        {
            OnClick = null;
        }

        bool _Selected { get; set; }
       
        public bool Selected
        {
            get { return _Selected; }
            set
            {
                _Selected = value;
                OnPropertyChanged("Selected");
                if (_Selected)
                    RectColor = new SolidColorBrush(System.Windows.Media.Color.FromArgb(128,139, 255, 0));
                else RectColor = new SolidColorBrush(System.Windows.Media.Color.FromArgb(128, 34, 139, 34));
            }
        }


        SolidColorBrush _RectColor { get; set; }

        public  SolidColorBrush RectColor
        {
            get { return _RectColor; }
            set
            {
                _RectColor = value;
                OnPropertyChanged("RectColor");
            }
        }



        #region Реализация Drag'n'Drop  Левого элемента
        public FrameworkElement Container;
        FrameworkElement draggedObject;
        Vector relativeMousePos;
        double WidthFirst;

        double oldPos;
        public event Action OnStartDragLeft;
        public event Action OnEndDragLeft;
        public event Action OnStartDragRight;
        public event Action OnEndDragRight;
        public event Action OnStartDragCentral;
        public event Action OnEndDragCentral;


        void StartDrag(object sender, MouseButtonEventArgs e)
        {
            if (((FrameworkElement)sender).Name == "LeftLimit") OnStartDragLeft?.Invoke();
            if (((FrameworkElement)sender).Name == "RightLimit") OnStartDragRight?.Invoke();
            if (((FrameworkElement)sender).Name == "CentralLimit") OnStartDragCentral?.Invoke();

            oldPos = Margin.Left;
            draggedObject = (FrameworkElement)sender;
            WidthFirst = ActualWidth;
            relativeMousePos = e.GetPosition(draggedObject) - new System.Windows.Point();
            draggedObject.MouseMove += OnDragMove;
            draggedObject.LostMouseCapture += OnLostCapture;
            draggedObject.MouseUp += OnMouseUp;
            Mouse.Capture(draggedObject);
        }

        void OnDragMove(object sender, MouseEventArgs e)
        {
            UpdatePosition(e);
        }

        void UpdatePosition(MouseEventArgs e)
        {
            var point = e.GetPosition(Container);
            var newPos = point - relativeMousePos;
            double Top = Margin.Top;
            double Bottom = Margin.Bottom;

            if (draggedObject.Name == "LeftLimit")
            {
                if (newPos.X < 0) Margin = new Thickness(0, Top, 0, Bottom);
                else if (newPos.X > Container.ActualWidth) Margin = new Thickness(Container.ActualWidth, Top, 0, Bottom);
                else
                {
                    double curWidth = WidthFirst - newPos.X + oldPos;
                    if (curWidth < 5) return;
                    Margin = new Thickness(newPos.X, Top, 0, Bottom);
                    Width = curWidth;
                }
            }
            if (draggedObject.Name == "RightLimit")
            {
                if (newPos.X < 0) return; //Margin = new Thickness(0, Top, 0, Bottom);
                else if (newPos.X > Container.ActualWidth) return;//Margin = new Thickness(Container.ActualWidth, Top, 0, Bottom);
                else
                {
                    double curWidth = newPos.X - oldPos;
                    if (curWidth < 5) return;
                    //                    Margin = new Thickness(newPos.X, Top, 0, Bottom);
                    Width = curWidth;
                }
            }
            if (draggedObject.Name == "CentralLimit")
            {
                if (newPos.X < 0) Margin = new Thickness(0, Top, 0, Bottom);
                else if (newPos.X > Container.ActualWidth) Margin = new Thickness(Container.ActualWidth, Top, 0, Bottom);
                else
                {
                    //double curWidth = newPos.X - oldPos;
                    //if (curWidth < 5) return;
                    Margin = new Thickness(newPos.X, Top, 0, Bottom);
                    //Width = curWidth;
                }
            }
            interval.UpdateFromUI();
        }

        void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            FinishDrag(sender, e);
            Mouse.Capture(null);
        }

        void OnLostCapture(object sender, MouseEventArgs e)
        {
            FinishDrag(sender, e);
        }

        void FinishDrag(object sender, MouseEventArgs e)
        {
            draggedObject.MouseMove -= OnDragMove;
            draggedObject.LostMouseCapture -= OnLostCapture;
            draggedObject.MouseUp -= OnMouseUp;
            UpdatePosition(e);
            if (draggedObject.Name == "LeftLimit")  OnEndDragLeft?.Invoke();
            if (draggedObject.Name == "RightLimit") OnEndDragRight?.Invoke();
            if (draggedObject.Name == "CentralLimit") OnEndDragCentral?.Invoke();

        }
        #endregion


        #region mvvm
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        private void Grid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (OnClick!= null)
            OnClick(this, e);
        }
    }
}
