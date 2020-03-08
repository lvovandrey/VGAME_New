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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace VanyaGame.GameNumberDB.Interface
{
    /// <summary>
    /// Логика взаимодействия для KeyboardClickAnimation.xaml
    /// </summary>
    public partial class KeyboardClickAnimation : UserControl
    {
        public KeyboardClickAnimation()
        {
            InitializeComponent();
        }
        public void StartAnimation()
        {
            // Locate Storyboard resource
            Storyboard s = (Storyboard)TryFindResource("AnimationStoryboard");
            s.RepeatBehavior = new RepeatBehavior(7);
            s.Begin();  // Start animation}
        }


    }
}
