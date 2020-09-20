using Prism.Commands;
using System;
using System.Collections.Generic;
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

namespace LevelSetsEditor.View
{
    /// <summary>
    /// Логика взаимодействия для TimeWatcherView.xaml
    /// </summary>
    public partial class TimeWatcherView : UserControl
    {

        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("Command",
           typeof(DelegateCommand<MouseWheelEventArgs>), typeof(TimeWatcherView),
           new FrameworkPropertyMetadata());

        public DelegateCommand<MouseWheelEventArgs> Command {
            get { return (DelegateCommand<MouseWheelEventArgs>)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); } }

        public TimeWatcherView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
