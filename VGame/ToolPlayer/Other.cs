using System;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;


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
        public static void Delay(Action Complete, TimeSpan Interval)
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

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);



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





    }
}

