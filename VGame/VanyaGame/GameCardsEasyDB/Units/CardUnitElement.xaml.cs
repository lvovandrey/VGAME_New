﻿using System;
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

namespace VanyaGame.GameCardsEasyDB.Units
{
    /// <summary>
    /// Логика взаимодействия для NumberElement.xaml
    /// </summary>
    public partial class CardUnitElement : UserControl
    {
        public CardUnitElement()
        {
            InitializeComponent();
        }

        private void Img_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var uri = new Uri("Images/HandPushDown.cur", UriKind.Relative);
            var stream = Application.GetResourceStream(uri).Stream;
            var cursor = new Cursor(stream);
            Img.Cursor = cursor;
            //    AddDropShadowEffect(Img, 10, 20, 80);

        }

        private void Img_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var uri = new Uri("Images/HandPush.cur", UriKind.Relative);
            var stream = Application.GetResourceStream(uri).Stream;
            var cursor = new Cursor(stream);
            Img.Cursor = cursor;

        }

        private void Img_MouseLeave(object sender, MouseEventArgs e)
        {
            var uri = new Uri("Images/HandPush.cur", UriKind.Relative);
            var stream = Application.GetResourceStream(uri).Stream;
            var cursor = new Cursor(stream);
            Img.Cursor = cursor;
        }
    }
}