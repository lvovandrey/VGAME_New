using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Xml;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using VanyaGame.Units;
using VanyaGame.Units.Components;
using VanyaGame.Struct;
using VanyaGame.Struct.Components;

namespace VanyaGame
{
    public delegate void DelegateTimerTick(object sender, EventArgs e);

    public static class Imaging
    {
        public static BitmapSource CreateBitmapSourceFromBitmap(System.Drawing.Bitmap bitmap)
        {
            if (bitmap == null)
                throw new ArgumentNullException("bitmap");

            return System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                bitmap.GetHbitmap(),
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());
        }
    }


    public static class ToolsTimer 
    {
        public static void Delay(VoidDelegate Complete, TimeSpan Interval)
        {
            System.Windows.Threading.DispatcherTimer ToolTimer = new System.Windows.Threading.DispatcherTimer();

            if (ToolTimer == null)
            {
                ToolTimer = new System.Windows.Threading.DispatcherTimer();
            }
            ToolTimer.Tick += (s, _) =>
            {
                Complete();
                ToolTimer.Stop();
                ToolTimer = null;
            };
            ToolTimer.Interval = Interval;
            ToolTimer.Start();
        }
    }
    
    public static class Tools
    {

        /// <summary>
        /// Окрашивает непрозрачные области битмапа черным цветом
        /// </summary>
        /// <param name="processedBitmap"></param>
        /// <param name="newBmp"></param>
        private static void DrawPngBlack_UsingLockbits(System.Drawing.Bitmap processedBitmap, System.Drawing.Bitmap newBmp)
        {
            System.Drawing.Rectangle BitmapRect = new System.Drawing.Rectangle(0, 0, processedBitmap.Width, processedBitmap.Height);
            BitmapData bitmapData = processedBitmap.LockBits(BitmapRect, ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format32bppArgb);// processedBitmap.PixelFormat);

            System.Drawing.Rectangle NewBmpRect = new System.Drawing.Rectangle(0, 0, newBmp.Width, newBmp.Height);
            BitmapData NewBmpData = newBmp.LockBits(NewBmpRect, ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format32bppArgb);// processedBitmap.PixelFormat);


            int bytesPerPixel = System.Drawing.Bitmap.GetPixelFormatSize(processedBitmap.PixelFormat) / 8;
            int byteCount = bitmapData.Stride * processedBitmap.Height;
            byte[] pixels = new byte[byteCount];
            byte[] New_pixels = new byte[byteCount];

            IntPtr ptrFirstPixel = bitmapData.Scan0;
            IntPtr ptrNewPixel = NewBmpData.Scan0;

            Marshal.Copy(ptrFirstPixel, pixels, 0, pixels.Length);
            Marshal.Copy(ptrNewPixel, New_pixels, 0, New_pixels.Length);

            int heightInPixels = bitmapData.Height;
            int widthInBytes = bitmapData.Width * bytesPerPixel;

            for (int y = 0; y < heightInPixels; y++)
            {
                int currentLine = y * bitmapData.Stride;
                for (int x = 0; x < widthInBytes; x = x + bytesPerPixel)
                {
                    int oldBlue = pixels[currentLine + x];
                    int oldGreen = pixels[currentLine + x + 1];
                    int oldRed = pixels[currentLine + x + 2];
                    int oldAlpha = pixels[currentLine + x + 3];

                    if (((byte)oldAlpha) > 10)
                    {
                        // calculate new pixel value
                        New_pixels[currentLine + x] = (byte)(0);
                        New_pixels[currentLine + x + 1] = (byte)(0);
                        New_pixels[currentLine + x + 2] = (byte)(0);
                        New_pixels[currentLine + x + 3] = (byte)oldAlpha;
                    }
                }
            }

            // copy modified bytes back
            Marshal.Copy(pixels, 0, ptrFirstPixel, pixels.Length);
            Marshal.Copy(New_pixels, 0, ptrNewPixel, New_pixels.Length);

            processedBitmap.UnlockBits(bitmapData);
            newBmp.UnlockBits(NewBmpData);
        }

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

        /// <summary>
        /// Загружает в Image1 изображение filepath, при этом окрашивая непрозрачные области черным цветом
        /// </summary>
        /// <param name="filepath"></param>
        /// <param name="Image1"></param>
        public static void LoadAndConvertToShadow(string filepath, Image Image1) 
        {

            using (System.Drawing.Bitmap bmp_ = new System.Drawing.Bitmap(@filepath))
            {
                using (System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(@filepath))
                {
                    DrawPngBlack_UsingLockbits(bmp_, bmp); //Этот метод собственно и красит ЧЕРНЫМ ЦВЕТОМ ВСЕ

                    IntPtr hBitmap = bmp.GetHbitmap();
                    IntPtr hBitmap_ = bmp_.GetHbitmap();

                    try
                    {
                        BitmapSource source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, Int32Rect.Empty, System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
                        Image1.Source = source;
                    }
                    finally
                    {
                        DeleteObject(hBitmap);
                        DeleteObject(hBitmap_);
                        GC.Collect(10, System.GCCollectionMode.Forced, true);
                        GC.WaitForPendingFinalizers();
                    }
                }
            }
        }


        public static void CreateAndStart_DispatcherTimer(ref DispatcherTimer timer, DelegateTimerTick TimerTick)
        {
            if (timer == null)
            {
                timer = new System.Windows.Threading.DispatcherTimer();
            }
            timer.Tick += new EventHandler(TimerTick);
            timer.Interval = new TimeSpan(0, 0, 0, 0, 20);
            timer.Start();
        }

        public static void CreateAndStart_DispatcherTimer(ref DispatcherTimer timer, DelegateTimerTick TimerTick, TimeSpan _Interval)
        {
            if (timer == null)
                timer = new System.Windows.Threading.DispatcherTimer();
            else
            {
                StopAndNull_DispatcherTimer(ref timer);
                timer = new System.Windows.Threading.DispatcherTimer();
            }
            timer.Tick += new EventHandler(TimerTick);
            timer.Interval = _Interval;
            timer.Start();
        }
        public static void StopAndNull_DispatcherTimer(ref DispatcherTimer timer)
        {
            if (timer != null)
            {
                timer.Stop();
                timer = null;
            }
        }
        


        /// Return Type: BOOL->int  
        ///X: int  
        ///Y: int  
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll", EntryPoint = "SetCursorPos")]
        [return: System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.Bool)]
        public static extern bool SetCursorPos(int X, int Y); 
        
        /// <summary>
        /// Пересекаестя ли прямоугольник Zone хотя бы с одним из ZonesList
        /// </summary>
        /// <param name="Zone"></param>
        /// <param name="ZonesList"></param>
        /// <returns></returns>
        public static bool IsIntersectionZones(Rect Zone, List<Rect> ZonesList) 
        {
            bool Result = false;
            foreach (Rect Z in ZonesList)
            {
                if (IsIntersect(Zone, Z))
                {
                    Result = true;
                    return Result;
                }
            }
            return Result;
        }
        public static bool IsIntersect(Rect R1, Rect R2) 
        {
            Rect resultRect = Rect.Intersect(R1, R2);
            if (resultRect != Rect.Empty)
            {
                return true;
            }
            else return false;
        }
        
        public static bool IsIntersect2(Rect R1, Rect R2)
        {
            if ((IsIntersectLine(R1.Left, R1.Right, R2.Left, R2.Right)) && (IsIntersectLine(R1.Top, R1.Bottom, R2.Top, R2.Bottom)))
            {
                return true;
            }
            else return false;
        }


        public static Rect ToRect(Unit U)
        {
            FrameworkElement O = U.GetComponent<HaveBody>().Body;
            Rect r = new Rect(O.Margin.Left, O.Margin.Top, O.ActualWidth, O.ActualHeight);
            return r;
        }
        
        /// <summary>
        /// Возвращает расстояние между центрами прямоугольников
        /// </summary>
        /// <param name="R1"></param>
        /// <param name="R2"></param>
        /// <returns></returns>
        public static double RectCenterDistance(Rect R1, Rect R2)
        {
            Point Center1 = new Point();
            Point Center2 = new Point();
            double Distance;
            Center1.X = (R1.Right + R1.Left) / 2;
            Center1.Y = (R1.Top + R1.Bottom) / 2;
            Center2.X = (R2.Right + R2.Left) / 2;
            Center2.Y = (R2.Top + R2.Bottom) / 2;

            Distance = Math.Sqrt(Math.Pow((Center1.X - Center2.X), 2) + Math.Pow((Center1.Y - Center2.Y), 2));
            return Distance;
        }

        /// <summary>
        /// Возвращает true если пересекаются два отрезка
        /// </summary>
        /// <param name="x1">Начало первого отрезка</param>
        /// <param name="x_1">Конец первого отрезка</param>
        /// <param name="x2">Начало второго отрезка</param>
        /// <param name="x_2">Конец второго отрезка</param>
        /// <returns></returns>
        public static bool IsIntersectLine(double x1, double x_1, double x2, double x_2)
        {
            if (((x1 > x2) & (x1 < x_2)) || ((x_1 > x2) & (x_1 < x_2)) || ((x1 < x2) & (x_1 > x_2)))
            {
                return true;
            }
            else return false;
        }
        /// <summary>
        /// Пересекаюстя ли прямоугольники из списка ZonesList
        /// </summary>
        /// <param name="ZonesList"></param>
        /// <returns></returns>
        public static bool IsIntersectionZones(List<Rect> ZonesList)
        {
            bool Result = false;
            int II = 1;
            string tmpmessage="";
            double dist;
            foreach (Rect Z in ZonesList)
            {
                dist = 0;
                for (int i = II; i < ZonesList.Count; i++) 
                {
                    dist = RectCenterDistance(Z, ZonesList[i]);
                    bool IsInters = IsIntersect(Z, ZonesList[i]);
                    bool IsInters2 = IsIntersect2(Z, ZonesList[i]);

                    tmpmessage += "\r\n" + dist.ToString("0") + " " + IsInters.ToString() + "   x1=" + Z.Left +  "   y1=" + Z.Top +  "   x2=" + ZonesList[i].Left +  "   y2=" + ZonesList[i].Top;
                    if (IsInters) 
                    {
                        Result = true;
                        break; 
                    }
                }
                if (Result) break;
                II++;
            }
            
            return Result;
        }
    
    
    }



    public static class XMLTools 
    {


        //МЕТОД НУЖДАЕТСЯ В ДОРАБОТКЕ!!!!!!!!!!!!!!!!
        public static void LoadSetsFromXML(Scene Scene, string filename)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(filename);
            // получим корневой элемент
            XmlElement xRoot = xDoc.DocumentElement;
        }


        public static void LoadSetsFromXML(Level Level, string filename)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(filename);
            // получим корневой элемент
            XmlElement xRoot = xDoc.DocumentElement;

            List<Scene> Scenes = new List<Scene>();
            //надо превратить нашу очередь в List
            foreach (KeyValuePair<string, Scene> SL in Level.Scenes)
            {
                Scenes.Add(SL.Value);
            }
            int i = -1;
            // обход всех узлов в корневом элементе
            foreach (XmlNode xnode in xRoot)
            {

                
                // получаем атрибут name
                if (xnode.Attributes.Count > 0)
                {
                    XmlNode attr = xnode.Attributes.GetNamedItem("name");
                    if (attr != null)
                        Game.Msg(attr.Value);
                }

                if (xnode.Name == "sets")
                    foreach (XmlNode childnode in xnode.ChildNodes)
                        switch (childnode.Name)
                        {
                            case "description"      : Level.Sets.Description = childnode.InnerText; break;
                            case "name"             : Level.Sets.Name = childnode.InnerText; break;
                            case "preview"          : Level.Sets.PreviewType = childnode.InnerText; break;
                            case "background"       : Level.Sets.BackgroundType = childnode.InnerText; break;
                            case "basevideofilename": Level.Sets.BaseVideoFilename = childnode.InnerText; break;
                        }




                if (xnode.Name == "scene")
                {
                    i++;
                    foreach (XmlNode childnode in xnode.ChildNodes)
                    {
                        if (Scenes.Count >= (i + 1))
                        {
                            if (Scenes[i].Sets.GetComponent<InnerVideoSets>() == null) continue;

                            //Scenes[i].Sets.GetComponent<InnerVideoSets>().VideoFileType = VideoType.unknown;
                            //Scenes[i].Sets.GetComponent<InnerVideoSets>().VideoFileName = "";
                            //Scenes[i].Sets.GetComponent<InnerVideoSets>().VideoTimeBegin = TimeSpan.FromMilliseconds(0);
                            //Scenes[i].Sets.GetComponent<InnerVideoSets>().VideoTimeEnd = TimeSpan.FromMilliseconds(0);
                            // если узел - company
                            if (childnode.Name == "filename")
                            {
                                Game.Msg("filename: " + childnode.InnerText);
                                Scenes[i].Sets.GetComponent<InnerVideoSets>().VideoFileName = childnode.InnerText;
                            }
                            // если узел - filetype
                            if (childnode.Name == "filetype")
                            {
                                VideoType type = VideoType.unknown;
                                Game.Msg("filetype: " + childnode.InnerText);
                                switch (childnode.InnerText)
                                {
                                    case "local": type = VideoType.local; break;
                                    case "net": type = VideoType.net; break;
                                    case "youtube": type = VideoType.youtube; break;
                                    case "ipcam": type = VideoType.ipcam; break;
                                }
                                Scenes[i].Sets.GetComponent<InnerVideoSets>().VideoFileType = type;
                            }
                            // если узел - company
                            if (childnode.Name == "filenumber")
                            {
                                Game.Msg("filenumber: " + childnode.InnerText);
                                Scenes[i].Sets.GetComponent<InnerVideoSets>().VideoFileNumber = childnode.InnerText;
                            }
                            // если узел - company
                            if (childnode.Name == "timeBegin")
                            {
                                Game.Msg("Beg: " + childnode.InnerText);
                                Scenes[i].Sets.GetComponent<InnerVideoSets>().VideoTimeBegin = TimeSpan.Parse(childnode.InnerText);
                            }
                            // если узел - company
                            if (childnode.Name == "timeEnd")
                            {
                                Game.Msg("end: " + childnode.InnerText);
                                Scenes[i].Sets.GetComponent<InnerVideoSets>().VideoTimeEnd = TimeSpan.Parse(childnode.InnerText);
                            }
                        }

                    }
                }
            }
        }

        public static string LoadFromXML(string filename, string nodeName)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(filename);
            // получим корневой элемент
            XmlElement xRoot = xDoc.DocumentElement;

            // обход всех узлов в корневом элементе
            foreach (XmlNode xnode in xRoot)
            {

                if (xnode.Name == nodeName)
                {
                    return @xnode.InnerText.ToString();
                }

            }
            return "";
        }
    }



}
