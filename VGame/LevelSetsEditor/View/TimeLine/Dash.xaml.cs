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

namespace LevelSetsEditor.View.TimeLine
{
    /// <summary>
    /// Логика взаимодействия для Dash.xaml
    /// </summary>
    public partial class Dash : UserControl
    {
        public DashSets sets;
        public Dash()
        {
            InitializeComponent();
            sets = new DashSets();
            DataContext = sets;
        }
    }

    public class DashSets:INotifyPropertyChanged
    {
        double lineWidth = 0.5;
        public double LineWidth
        {
            get
            {
                return lineWidth;
            }
            set
            {
                if (value <= 0.001) return;
                lineWidth = value;
                OnPropertyChanged("LineWidth");
            }
        }

        double lineHeight = 5;
        public double LineHeight
        {
            get
            {
                return lineHeight;
            }
            set
            {
                if (value <= 0.001) return;
                lineHeight = value;
                OnPropertyChanged("LineHeight");
            }
        }

        #region INPC
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

    }
}
