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

namespace CardsEditor.View.Elements
{
    /// <summary>
    /// Interaction logic for PropertyViewer_DoubleNums_UpDown.xaml
    /// </summary>
    public partial class PropertyViewer_IntNums_UpDown : UserControl
    {
        public PropertyViewer_IntNums_UpDown()
        {
            InitializeComponent();
        }
        private void ButtonUp_Click(object sender, RoutedEventArgs e)
        {
            int d = 0;

            if (int.TryParse(TextBoxValue.Text, out d))
            {
                d++;
                string s = d.ToString();
                TextBoxValue.Text = s;
            }
        }

        private void ButtonDown_Click(object sender, RoutedEventArgs e)
        {
            int d = 0;

            if (int.TryParse(TextBoxValue.Text, out d))
            {
                d--;
                string s = d.ToString();
                TextBoxValue.Text = s;
            }
        }
    }
}
