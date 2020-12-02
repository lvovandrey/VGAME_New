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

namespace VanyaGame.GameCardsNewDB.Interface.Elements
{
    /// <summary>
    /// Interaction logic for PropertyViewer_DoubleNums_UpDown.xaml
    /// </summary>
    public partial class PropertyViewer_DoubleNums_UpDown : UserControl
    {
        public PropertyViewer_DoubleNums_UpDown()
        {
            InitializeComponent();
        }
        private void ButtonUp_Click(object sender, RoutedEventArgs e)
        {
            double d = 0;

            if (double.TryParse(TextBoxValue.Text, out d))
            {
                if (d > -0.2 && d < 0.2) d += 0.01;
                else if (d > -2 && d < 2) d += 0.1;
                else if (d > -20 && d < 20) d += 1;
                else if (d > -200 && d < 200) d += 10;
                else if (d > -2000 && d < 2000) d += 100;
                else if (d > -20000 && d < 20000) d += 1000;
                else if (d > -200000 && d < 200000) d += 10000;
                else if (d > -2000000 && d < 2000000) d += 100000;
                else d += 100000;
            }
            if (double.TryParse(TextBoxValue.Text.Replace(".", ","), out d))
            {
                if (d > -0.2 && d < 0.2) d += 0.01;
                else if (d > -2 && d < 2) d += 0.1;
                else if (d > -20 && d < 20) d += 1;
                else if (d > -200 && d < 200) d += 10;
                else if (d > -2000 && d < 2000) d += 100;
                else if (d > -20000 && d < 20000) d += 1000;
                else if (d > -200000 && d < 200000) d += 10000;
                else if (d > -2000000 && d < 2000000) d += 100000;
                else d += 100000;
            }

            string s = d.ToString(System.Globalization.CultureInfo.CurrentCulture);
            TextBoxValue.Text = s.Replace(",", ".");
        }

        private void ButtonDown_Click(object sender, RoutedEventArgs e)
        {
            double d = 0;

            if (double.TryParse(TextBoxValue.Text, out d))
            {
                if (d > -0.2 && d < 0.2) d -= 0.01;
                else if (d > -2 && d < 2) d -= 0.1;
                else if (d > -20 && d < 20) d -= 1;
                else if (d > -200 && d < 200) d -= 10;
                else if (d > -2000 && d < 2000) d -= 100;
                else if (d > -20000 && d < 20000) d -= 1000;
                else if (d > -200000 && d < 200000) d -= 10000;
                else if (d > -2000000 && d < 2000000) d -= 100000;
                else d -= 100000;
            }
            else
            if (double.TryParse(TextBoxValue.Text.Replace(".", ","), out d))
            {
                if (d > -0.2 && d < 0.2) d -= 0.01;
                else if (d > -2 && d < 2) d -= 0.1;
                else if (d > -20 && d < 20) d -= 1;
                else if (d > -200 && d < 200) d -= 10;
                else if (d > -2000 && d < 2000) d -= 100;
                else if (d > -20000 && d < 20000) d -= 1000;
                else if (d > -200000 && d < 200000) d -= 10000;
                else if (d > -2000000 && d < 2000000) d -= 100000;
                else d -= 100000;
            }

            string s = d.ToString(System.Globalization.CultureInfo.CurrentCulture);
            TextBoxValue.Text = s.Replace(",", ".");
        }
    }
}
