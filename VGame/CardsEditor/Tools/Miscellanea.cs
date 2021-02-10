using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using DirectShowLib;
using DirectShowLib.DES;
using System.Runtime.InteropServices;
using System.IO;

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


        public static int GetVideoBitRate(string FileName)
        {
            int bitrate = 400_000;
            try
            {
                var mediaDet = (IMediaDet)new MediaDet();
                DsError.ThrowExceptionForHR(mediaDet.put_Filename(FileName));

                // retrieve some measurements from the video
                double frameRate;
                mediaDet.get_FrameRate(out frameRate);

                var mediaType = new AMMediaType();
                mediaDet.get_StreamMediaType(mediaType);
                var videoInfo = (VideoInfoHeader)Marshal.PtrToStructure(mediaType.formatPtr, typeof(VideoInfoHeader));
                DsUtils.FreeAMMediaType(mediaType);
                var width = videoInfo.BmiHeader.Width;
                var height = videoInfo.BmiHeader.Height;
                bitrate = videoInfo.BitRate;
            }
            catch 
            {
                Console.WriteLine("Ошибка определения битрейта");
                return bitrate;
            }
            return bitrate;
        }



    }
}
