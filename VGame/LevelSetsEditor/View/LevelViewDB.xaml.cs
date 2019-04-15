using LevelSetsEditor.DB;
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
    /// Логика взаимодействия для LevelViewDB.xaml
    /// </summary>
    public partial class LevelViewDB : UserControl
    {
        public LevelViewDB()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            using (LevelSetContext context = new LevelSetContext())
            {

                //MainWindow.mainWindow.ViewModel.se
                //    context.LevelSets = MainWindow.mainWindow.ViewModel.LevelSets;
                int X = int.Parse((MainWindow.mainWindow.ViewModel.LevelSetVMs.First().Name).ToString());
                MainWindow.mainWindow.ViewModel.LevelSetVMs.First().Name = (X+1).ToString();
                context.LevelSets.First().Name = (X+1).ToString();
                context.SaveChanges();
                foreach(Model.LevelSet L in context.LevelSets)
                {
               //     MessageBox.Show(L.Name);
                }
            }
        }
    }
}
