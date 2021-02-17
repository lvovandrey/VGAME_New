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
using System.Net;

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

        //------------------------------------------------------------------------------------------//
        //Методы UrlExists, ExtractCharsetAndMimeType и PrepareRequest                              // 
        //позаимстовованы из источника https://www.sql.ru/forum/656765/sushhestvovanie-fayla-po-url //
        // Благодарю пользователя Nisus                                                             //

        public static Boolean UrlExists(string url)
        {
            string mimeType = null;
            string charset = null;
            try
            {
                HttpWebRequest req = PrepareRequest(url);
                req.Method = "HEAD";
                using (HttpWebResponse resp = (HttpWebResponse)req.GetResponse())
                using (Stream resst = resp.GetResponseStream())
                    if (resp.StatusCode == HttpStatusCode.OK)
                    {
                        ExtractCharsetAndMimeType(resp, out mimeType, out charset);
                        if (mimeType.Length > 0)
                            return true;
                    }
                req = PrepareRequest(url);
                req.Method = "GET";
                using (HttpWebResponse resp = (HttpWebResponse)req.GetResponse())
                using (Stream resst = resp.GetResponseStream())
                    if (resp.StatusCode == HttpStatusCode.OK)
                    {
                        ExtractCharsetAndMimeType(resp, out mimeType, out charset);
                        if (mimeType.Length > 0)
                            return true;
                    }
            }
            catch { }
            return false;
        }

        private static void ExtractCharsetAndMimeType(HttpWebResponse resp
            , out string mimeType
            , out string charset)
        {
            mimeType = string.Empty;
            charset = string.Empty;
            if (!String.IsNullOrEmpty(resp.CharacterSet))
                charset = resp.CharacterSet;
            if (!String.IsNullOrEmpty(resp.ContentType)
                && resp.ContentType.Trim().Length > 0)
            {
                int index = resp.ContentType.IndexOf(';');
                if (index > -1)
                    mimeType = resp.ContentType.Substring(0, index);
                else
                    mimeType = resp.ContentType;
                mimeType = mimeType.Trim();
            }
        }

        private static HttpWebRequest PrepareRequest(string url)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.0; .NET CLR 1.1.4322; .NET CLR 2.0.40607)";
            req.AllowAutoRedirect = true;
            req.AutomaticDecompression = DecompressionMethods.Deflate
                | DecompressionMethods.GZip;
            req.MaximumAutomaticRedirections = 5;
            return req;
        }

        //-------------------------------------------------------------------------------------------------//
        //Код метода GetVideoBitRate частично                                                              // 
        //позаимстовован из источника  https://stackoverflow.com/questions/6215185/getting-length-of-video //
        // Благодарю пользователя nZeus                                                                    //
        public static int GetVideoBitRate(string FileName)
        {
            int bitrate = 400_000;
            try
            {
                var mediaDet = (IMediaDet)new MediaDet();
                DsError.ThrowExceptionForHR(mediaDet.put_Filename(FileName));

                double frameRate;
                mediaDet.get_FrameRate(out frameRate);

                var mediaType = new AMMediaType();
                mediaDet.get_StreamMediaType(mediaType);
                var videoInfo = (VideoInfoHeader)Marshal.PtrToStructure(mediaType.formatPtr, typeof(VideoInfoHeader));
                DsUtils.FreeAMMediaType(mediaType);
                var width = videoInfo.BmiHeader.Width;
                var height = videoInfo.BmiHeader.Height;
                bitrate = videoInfo.BitRate;

                if (bitrate != 0) return bitrate;

                double mediaLength;
                mediaDet.get_StreamLength(out mediaLength);
                var frameCount = (int)(frameRate * mediaLength);
                var duration = frameCount / frameRate;

                bitrate = (int)((new FileInfo(FileName)).Length / duration);
                if (bitrate == 0) return 400_000;
            }
            catch 
            {
                Console.WriteLine("Ошибка определения битрейта");
                return bitrate;
            }
            return bitrate;
        }

        public static bool ExstentionCheck(string filename, string[] extentions)
        {
            bool result = false;
            string fileExtention = Path.GetExtension(filename);
            foreach (var extention in extentions)
            {
                if (fileExtention == extention)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

    }
}
