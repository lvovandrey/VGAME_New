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
            ((SettingsWindowVM)DataContext).RefreshAllDependencyProperties();
            string ElementUsefulName = ((ListBoxItem)e.AddedItems[0]).Content.ToString();
            SettingsElementShow(ElementUsefulName);
        }


        public void SettingsElementShow(string ElementUsefulName)
        {
            foreach (var element in SettingsElements)
                element.Value.Visibility = Visibility.Hidden;
            SettingsElements[ElementUsefulName].Visibility = Visibility.Visible;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SettingsElements.Add("Подсказки", HintSettins);
            SettingsElements.Add("Текст озвучки", SpeakSettings);
            SettingsElements.Add("Голос", VoiceSettings);
            SettingsElements.Add("Вид", ViewSettings);
            SettingsElements.Add("База данных", DBSettings);
            SettingsElements.Add("Музыка", MusicSettings);
            SettingsElements.Add("Управление настройками", ImportExportSettings);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
            Game.Owner.Activate();
        }
    }
}
