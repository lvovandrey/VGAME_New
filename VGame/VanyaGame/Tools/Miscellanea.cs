using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Net;
using DirectShowLib;
using DirectShowLib.DES;
using System.Runtime.InteropServices;

namespace VanyaGame
{
    public class Miscellanea
    {
        static public string GetExeDirectory()
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            path = System.IO.Path.GetDirectoryName(path);
            return path;
        }
        static public void OpenFileInExplorer(string file)
        {
            if (!File.Exists(file))
            {
                MessageBox.Show("Файл " + file + " не найден", "Файл не найден");
                return;
            }
            Process PrFolder = new Process();
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.CreateNoWindow = true;
            psi.WindowStyle = ProcessWindowStyle.Normal;
            psi.FileName = "explorer";
            psi.Arguments = @"/n, /select, " + file;
            PrFolder.StartInfo = psi;
            PrFolder.Start();
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
            int bitrate = 900_000;
            try
            {
                var mediaDet = (IMediaDet)new MediaDet();
                DsError.ThrowExceptionForHR(mediaDet.put_Filename(FileName));

                double frameRate;
                mediaDet.get_FrameRate(out frameRate);

                double mediaLength;
                mediaDet.get_StreamLength(out mediaLength);
                var frameCount = (int)(frameRate * mediaLength);
                var duration = frameCount / frameRate;

                bitrate = (int)((new FileInfo(FileName)).Length / duration);
                if (bitrate <= 0 || bitrate>100_000_000) return 400_000;
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
