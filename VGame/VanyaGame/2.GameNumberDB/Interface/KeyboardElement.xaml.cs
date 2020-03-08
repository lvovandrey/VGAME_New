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
using System.Windows.Media.Animation;
using System.Windows.Threading;
using VanyaGame.GameNumber.Data;

namespace VanyaGame.GameNumber.Interface
{
    /// <summary>
    /// Логика взаимодействия для KeyboardElement.xaml
    /// </summary>
    public partial class KeyboardElement : UserControl
    {

        public bool Hided = true;
        public Dictionary<string, Point> KeysLocations;


        public KeyboardElement()
        {
            InitializeComponent();
            Load();
            
        }

        public void Show(string sss)
        {
            if (!KeysLocations.ContainsKey(sss)) { Game.Msg("Unknown key. Have no in BD"); return; }

            Storyboard s = (Storyboard)TryFindResource("GrdBaseStoryboardUp");
            s.Begin();  // Start animation}
            Hided = false;
            ToolsTimer.Delay(() => 
            {
                double x = KeysLocations[sss].X;
                double y = KeysLocations[sss].Y;
                TDrawEffects.MoveTo(click_animation, x-50, y-50);

                if (!Hided)
                    ToolsTimer.Delay(() => { click_animation.StartAnimation(); }, TimeSpan.FromSeconds(0.1));
                Lbl.Content += sss;

            }, TimeSpan.FromSeconds(1.3));
            ToolsTimer.Delay(() =>
            {
                if(!Hided)
                    Hide();
            }, TimeSpan.FromSeconds(7));

            }




        public void Hide()
        {
            if (!Hided)
            {
                Storyboard s = (Storyboard)TryFindResource("GrdBaseStoryboardDown");
                s.Begin();  // Start animation}
                Hided = true;
            }
        }


        private void Load()
        {
            string path = System.IO.Path.Combine(Game.Sets.InterfaceDir, @"Numbers/controls/keyboard.png");
            if (System.IO.File.Exists(path))
            {
                Keyboard.Source = new BitmapImage(new Uri(@path));
                Opacity = 1;
                double x = (Game.Owner.Width / 2) - (590);
                Margin = new Thickness(x, Game.Owner.Height - 260, 0, 0);

                Panel.SetZIndex(this, 30);
            }
            else
            {
                MessageBox.Show("Файл изображения клавиатуры не найден:  " + path);
            }

            KeysLocations = new Dictionary<string, Point>();        
            DBKeyboardTools dBTools = new DBKeyboardTools();
            dBTools.LoadKeyLocationsFromDB(KeysLocations);
        }
    }
}
