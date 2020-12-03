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
using System.Windows.Shapes;

namespace VanyaGame.GameCardsNewDB.Interface
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        Dictionary<string, UIElement> SettingsElements;
        public SettingsWindow()
        {
            InitializeComponent();
            SettingsElements = new Dictionary<string, UIElement>();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string ElementUsefulName = ((ListBoxItem)e.AddedItems[0]).Content.ToString();
            SettingsElementShow(ElementUsefulName);
        }

        private void SettingsElementShow(string ElementUsefulName)
        {
            foreach (var element in SettingsElements)
                element.Value.Visibility = Visibility.Hidden;
            SettingsElements[ElementUsefulName].Visibility = Visibility.Visible;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SettingsElements.Add("Подсказки", HintSettins);
            SettingsElements.Add("Озвучка", SpeakSettings);
            SettingsElements.Add("Вид", ViewSettings);
            SettingsElements.Add("База данных", DBSettings);

        }
    }
}
