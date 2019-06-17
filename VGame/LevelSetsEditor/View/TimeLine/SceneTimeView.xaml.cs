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
        public SceneTimeView()
        {
            InitializeComponent();
            DataContext = this;
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
