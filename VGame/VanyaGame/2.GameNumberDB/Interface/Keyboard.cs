using System;
using System.Collections.Generic;
using System.Windows;
using VanyaGame.GameNumber.Units;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using VanyaGame.GameNumber.Data;

namespace VanyaGame.GameNumberDB.Interface
{
    public class Keyboard 
    {
        public string Symbol = "";
        public KeyboardElement Body;
        public double shiftX = 0;
        public double curY = 300;
        public Dictionary<string, Point> KeysLocations;

        public Number Number;

        //public override void Create(string s)
        //{
        //    base.Create(s);
        //    type = "KeyBoard";

        //    DragXShift = 0;
        //    DragYShift = -260;
        //    Body = new KeyboardElement();

        //    string path = System.IO.Path.Combine(Game.Sets.InterfaceDir, @"Numbers/controls/keyboard.png");
        //    if (System.IO.File.Exists(path))
        //    {
        //        Body.Keyboard.Source = new BitmapImage(new Uri(@path));
        //        Body.Opacity = 1;
        //        Game.Owner.GridMain.Children.Add(Body);
        //        Body.Visibility = System.Windows.Visibility.Visible;
        //        double x = (Game.Owner.Width / 2) - (590);
        //        Body.Margin = new Thickness(x, Game.Owner.Height - 260, 0, 0);

        //        Panel.SetZIndex(Body, 30);
        //    }
        //    else
        //    {
        //        MessageBox.Show("Файл изображения клавиатуры не найден:  " + path);
        //    }
        //    Filename = s;

        //    KeysLocations = new Dictionary<string, Point>();
        //    DBTools dBTools = new DBTools();
        //    dBTools.LoadKeyLocationsFromDB(KeysLocations);

        //}

        //public void NumberHook_PreviewMouseMove(object sender, MouseEventArgs e)
        //{
        //    UpdatePosition(e);
        //}




        //public override void ShowMethod()
        //{

        //    Point point = KeysLocations[Number.Symbol];

        //    Body.Show(Number.Symbol, point, this);

        //    //if (Game.Owner.GridMain.Children.Contains(Body))
        //    //{ }
        //    //else
        //    //{



        //    //    //Body.Opacity = 0;
        //    //    //
        //    //    //Body.BeginAnimation(OpacityProperty, null);
        //    //    //Body.BeginAnimation(OpacityProperty, null);
        //    //    //Body.Visibility = System.Windows.Visibility.Visible;
        //    //    //Panel.SetZIndex(Body, 3);
        //    //    //var a = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(1));
        //    //    //Body.Margin = new Thickness(300, 1080, 0, 0);
        //    //    //a.Completed += (sender, e) =>
        //    //    //{
        //    //    //    Body.BeginAnimation(OpacityProperty, null);
        //    //    //    Body.Opacity = 1;
        //    //    //    Body.Show();
        //    //    //};
        //    //    //ToolsTimer.Delay(() => { Body.BeginAnimation(OpacityProperty, a); }, TimeSpan.FromSeconds(0.5));

        //    //}
        //    isHided = false;
        //    IsShowed = true;
        //}
        //public override void HideMethod()
        //{

        //    Body.Hide();

        //    isHided = true;
        //    IsShowed = false;
        //}
    }



}
