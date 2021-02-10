using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace CardsEditor.Tools
{
    public class Miscellanea
    {
        //-------------------------------------------------------------------------------------------------------------------------//
        //Методы PointsToPixels, PixelsToPoints                                                                                    // 
        //позаимстовованы из источника  https://stackoverflow.com/questions/3286175/how-do-i-convert-a-wpf-size-to-physical-pixels //
        // Благодарю пользователя Lu55                                                                                             //

        public static double PointsToPixels(double wpfPoints, LengthDirection direction)
        {
            if (direction == LengthDirection.Horizontal)
            {
                return wpfPoints * Screen.PrimaryScreen.WorkingArea.Width / SystemParameters.WorkArea.Width;
            }
            else
            {
                return wpfPoints * Screen.PrimaryScreen.WorkingArea.Height / SystemParameters.WorkArea.Height;
            }
        }

        public static double PixelsToPoints(int pixels, LengthDirection direction)
        {
            if (direction == LengthDirection.Horizontal)
            {
                return pixels * SystemParameters.WorkArea.Width / Screen.PrimaryScreen.WorkingArea.Width;
            }
            else
            {
                return pixels * SystemParameters.WorkArea.Height / Screen.PrimaryScreen.WorkingArea.Height;
            }
        }

        public enum LengthDirection
        {
            Vertical, // |
            Horizontal // ——
        }
    }
}
