using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
using VGameCore.Abstract;

namespace VanyaGame.Interface
{
    /// <summary>
    /// Interaction logic for RoundProgressBar.xaml
    /// </summary>
    public partial class RoundProgressBarSimple : UserControl, INotifyPropertyChanged
    {
        private double radius = 100;

    private double percentage = 13;

    public RoundProgressBarSimple()
    {
        this.InitializeComponent();
        this.DataContext = this;
    }

    /// <summary>
    /// Задает процент
    /// </summary>
    public double Percentage
    {
        get
        {
            return this.percentage;
        }

        set
        {
            this.percentage = value;
            this.OnPropertyChanged();

            this.OnPropertyChanged("Angle");
            this.OnPropertyChanged("IsLarge");
            this.OnPropertyChanged("EndPoint");
        }
    }

    /// <summary>
    /// Радиус закругления
    /// </summary>
    public double Radius
    {
        get
        {
            return this.radius;
        }

        set
        {
            this.radius = value;
            this.OnPropertyChanged();

            this.OnPropertyChanged("StartPoint");
            this.OnPropertyChanged("Center");
            this.OnPropertyChanged("Size");
            this.OnPropertyChanged("EndPoint");
        }
    }

    /// <summary>
    /// Координаты центра окружности
    /// </summary>
    public Point Center
    {
        get
        {
            return new Point(this.Width / 2, this.Height / 2);
        }
    }

    /// <summary>
    /// Gets the starting point of the path
    /// </summary>
    public Point StartPoint
    {
        get
        {
            return new Point(this.Center.X, this.Center.Y - this.radius);
        }
    }

    /// <summary>
    /// Размеры сегмента
    /// </summary>
    public Size Size
    {
        get
        {
            return new Size(this.radius, this.radius);
        }
    }

    /// <summary>
    /// Угол, на котором необходимо остановиться
    /// </summary>
    public double Angle
    {
        get
        {
            return 359.99 * this.Percentage / 100;
        }
    }

    /// <summary>
    /// Корректировка при переходе через 180 градусов
    /// </summary>
    public bool IsLarge
    {
        get
        {
            return (this.Percentage > 50);
        }
    }

    /// <summary>
    /// Вычисление конечной точки
    /// </summary>
    public Point EndPoint
    {
        get
        {
            // Pi correction
            var angle = this.Angle - 90;
            return this.CalculateCircleCoordinates(this.Center, this.radius, angle);
        }
    }

    private Point CalculateCircleCoordinates(Point center, double radius, double angle)
    {
        var radians = (angle * Math.PI) / 180;
        return new Point(center.X + radius * Math.Cos(radians), center.Y + radius * Math.Sin(radians));
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChangedEventHandler handler = this.PropertyChanged;
        if (handler != null)
        {
            handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
}
