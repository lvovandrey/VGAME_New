using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using VanyaGame.GameNumber.Units;
using VanyaGame.GameNumber.Interface;



namespace VanyaGame
{
    public partial class TDrawEffects
    {
        public static void ChildrenRemoveAndAdd(Number F, Panel Container1, Panel Container2)
        {
            //F.Body.Opacity = 0;


            //ToolsTimer.Delay(() =>
            //{
            //    F.Body.Opacity = 0;
            //    F.Body.ToStackPanel();

            //    Container1.Children.Remove(F.Body);

            //    if (F.MyPaperBox != null)
            //    {
            //        F.MyPaperBox.PaperGird.Children.Add(F.Body);
            //    }
            //    else
            //    {
            //        PaperGrid PaperGrd = new PaperGrid
            //        {
            //            Width = 120,
            //            Height = 120
            //        };
            //        PaperGrd.PaperGird.Children.Add(F.Body);
            //        Container2.Children.Add(PaperGrd);
            //    }
            //    Panel.SetZIndex(F.Body, 1000);
            //    F.Body.VerticalAlignment = VerticalAlignment.Center;
            //    F.Body.HorizontalAlignment = HorizontalAlignment.Center;

            //    F.Body.Grd.VerticalAlignment = VerticalAlignment.Center;
            //    F.Body.Grd.HorizontalAlignment = HorizontalAlignment.Center;

            //    F.Body.Img.VerticalAlignment = VerticalAlignment.Center;
            //    F.Body.Img.HorizontalAlignment = HorizontalAlignment.Center;
            //    F.Body.Img.Height = 120;

            //    if ((F is Unit) && (((Unit)F).type == "Fish"))
            //    {
            //        ((Unit)F).IsOnStackPanel = true;
            //    }
            //    // F.Body.BeginAnimation(FrameworkElement.OpacityProperty, a);

            //}, TimeSpan.FromSeconds(0.3));
            //ToolsTimer.Delay(() =>
            //{

            //    F.Body.Opacity = 0;
            //    F.Body.BeginAnimation(FrameworkElement.OpacityProperty, new DoubleAnimation(1, TimeSpan.FromSeconds(0.34)));
            //    F.State = NumberState.InStack;


            //}, TimeSpan.FromSeconds(0.33));

        }
    }
}
