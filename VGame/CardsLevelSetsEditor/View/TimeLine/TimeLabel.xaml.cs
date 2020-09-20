using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
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
    /// Логика взаимодействия для TimeLabel.xaml
    /// </summary>
    public partial class TimeLabel : UserControl, INotifyPropertyChanged
    {
        public TimeLabel()
        {
            InitializeComponent();
            DataContext = this;
        }

        TimeSpan _Begin { get; set; }
        TimeSpan _End { get; set; }

        public TimeSpan Begin
        {
            get { return _Begin; }
            set { _Begin = value; OnPropertyChanged("Begin"); OnPropertyChanged("TimeInterval"); }
        }
        public TimeSpan End
        {
            get { return _End; }
            set { _End = value; OnPropertyChanged("End"); OnPropertyChanged("TimeInterval"); }
        }


        public string TimeInterval
        {
            get
            {
                string t1 = Begin.ToString(@"hh\:mm\:ss");
                string t2 = End.ToString(@"hh\:mm\:ss");
                return t1 + " - " + t2; 
            }
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
