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

namespace LevelSetsEditor.View.LevelsDBViews
{
    /// <summary>
    /// Логика взаимодействия для ListLevelsViewDB.xaml
    /// </summary>
    public partial class ListLevelsViewDB : UserControl
    {
        public ListLevelsViewDB()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Model.LevelSet levelSet = new Model.LevelSet();
            levelSet.Name = "Самая зайка любимочка!";
            MainWindow.mainWindow.ViewModel.db.LevelSets.Add(levelSet);
            MainWindow.mainWindow.ViewModel.db.SaveChanges();
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
         //   MessageBox.Show(MainWindow.mainWindow.ViewModel.SelectedLevelSet.Name);
        }
    }
}
