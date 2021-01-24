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

namespace VanyaGame.PrevMenuNS
{
    /// <summary>
    /// Interaction logic for PrevMenu.xaml
    /// </summary>
    public partial class PrevMenu : UserControl
    {
        List<PrevMenuItem> Items;
        public PrevMenu()
        {
            InitializeComponent();
            Items = new List<PrevMenuItem>();
        }
        
        public void AddItem(PrevMenuItem Item, SenderDelegate OnClick)
        {
            Items.Add(Item);
            Item.HorizontalAlignment = HorizontalAlignment.Stretch;
            Item.Margin = new Thickness(20);
            WrapContainer.Children.Add(Item);
            Item.MouseUp += (sender, e) => { OnClick(sender); };
        }




        public void RemoveItem(PrevMenuItem Item)
        {
            Items.Remove(Item);
            WrapContainer.Children.Remove(Item);
        }

        public void RemoveAllItems()
        {
            Items.Clear();
            WrapContainer.Children.Clear();
        }

    }
}
