using System;
using System.Drawing;
using System.Net;
using System.IO;
using System.Windows;

namespace VanyaGame.GameNumberDB.Tools
{
    public class PictHelper
    {
        public static System.Drawing.Size GetPictureSize(Uri uri)
        {
            System.Drawing.Size size = new System.Drawing.Size(0, 0);
            string path = @"C:\tmp\GetPictSizeVGameTmpFile.jpg";
            string dir = @"C:\tmp";
            try
            {
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                using (WebClient wClient = new WebClient())
                {
                    wClient.DownloadFile(uri, path);
                }

                if (File.Exists(path))
                {
                    using (Image Image = Image.FromFile(path))
                    {
                        size = Image.Size;
                    }
                }
                File.Delete(path);
            }
            catch (Exception e)
            {
                MessageBox.Show("Ошибка вычисления разрешения сетевого файла превью: " + e.Message);
            }

            return size;
        }
        public static System.Drawing.Size GetLocalPictureSize(string path)
        {
            System.Drawing.Size size = new System.Drawing.Size(0, 0);
            try
            {
                if (File.Exists(path))
                {
                    using (Image Image = Image.FromFile(path))
                    {
                        size = Image.Size;
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Ошибка вычисления разрешения локального файла превью: " + e.Message);
            }

            return size;
        }
    }
}

