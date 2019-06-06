﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

using System.ComponentModel;
using System.Globalization;
using System.Windows.Input;
using System.Windows.Media;

namespace ScenesTimeLine.Elements
{
    /// <summary>
    /// Логика взаимодействия для Cursor.xaml
    /// </summary>
    public partial class Cursor : UserControl, INotifyPropertyChanged
    {
        public Cursor()
        {
            InitializeComponent();
            DataContext = this;
            MouseLeftButtonDown += StartDrag;
            
            CursorColor = new SolidColorBrush(Color.FromArgb(255, 7, 0, 71));
        }



        SolidColorBrush _CursorColor { get; set; }
        public SolidColorBrush CursorColor
        {
            get { return _CursorColor; }
            set
            {
                _CursorColor = value;
                OnPropertyChanged("CursorColor");
            }
        }


        #region Реализация Drag'n'Drop

        public UIElement Container;
        Vector relativeMousePos;
        FrameworkElement draggedObject;

        void StartDrag(object sender, MouseButtonEventArgs e)
        {
            draggedObject = (FrameworkElement)sender;
            relativeMousePos = e.GetPosition(draggedObject) - new Point();
            draggedObject.MouseMove += OnDragMove;
            draggedObject.LostMouseCapture += OnLostCapture;
            draggedObject.MouseUp += OnMouseUp;
            Mouse.Capture(draggedObject);
            CursorColor = new SolidColorBrush(Color.FromArgb(255, 83, 100, 255));
        }

        void OnDragMove(object sender, MouseEventArgs e)
        {
            UpdatePosition(e);
        }

        void UpdatePosition(MouseEventArgs e)
        {
            var point = e.GetPosition(Container);
            var newPos = point - relativeMousePos;
            draggedObject.Margin = new Thickness(newPos.X, -5, 0, -5);
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
            if(IsMouseOver) CursorColor = new SolidColorBrush(Color.FromArgb(255, 0, 50, 255));
            else CursorColor = new SolidColorBrush(Color.FromArgb(255, 7, 0, 71));
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

        private void Cursor_MouseEnter(object sender, MouseEventArgs e)
        {
            CursorColor = new SolidColorBrush(Color.FromArgb(255, 0 , 50, 255));
        }

        private void Cursor_MouseLeave(object sender, MouseEventArgs e)
        {
            CursorColor = new SolidColorBrush(Color.FromArgb(255, 7, 0, 71));
        }


    }

    public class HeightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((double)value + 12);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }

}