using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace VanyaGame.GameCardsNewDB.Tools
{
    public class MemoryCounter
    {
        private PerformanceCounter cpuCounter;
        private PerformanceCounter ramCounter;
        Timer timer;
        public MemoryCounter()
        {
            int num = 0;
            InitialiseCPUCounter();
            InitializeRAMCounter();
            // устанавливаем метод обратного вызова
            TimerCallback tm = new TimerCallback(updateTimer_Tick);
            // создаем таймер
            timer = new Timer(tm, num, 0, 600);

        }

        private static bool IsEnoughtMemoryForLevelLoad(GameCardsNewDB.Struct.CardsNewDBLevel level) 
        {
           // double ImgMemoryKoef = 

            return true;
        }

        public static bool Is64Bit
        {

            get { return Marshal.SizeOf(typeof(IntPtr)) == 8; }
        }

        private void updateTimer_Tick(object obj)
        {
            string AppBitness = Is64Bit ? "x64" : "x86";
            Console.Write( "CPU Usage: " +
            Convert.ToInt32(cpuCounter.NextValue()).ToString() +
            "%          "+ AppBitness +"        ");

            Console.WriteLine(Convert.ToInt32(ramCounter.NextValue()).ToString() + "Mb");
            var memoryUsed = GC.GetTotalMemory(true);
            Console.WriteLine("Memory Used: {0} megabytes", (memoryUsed / 1024f) / 1024f);
        }


        private void InitialiseCPUCounter()
        {
            cpuCounter = new PerformanceCounter(
            "Processor",
            "% Processor Time",
            "_Total",
            true
            );
        }

        private void InitializeRAMCounter()
        {
            ramCounter = new PerformanceCounter("Memory", "Available MBytes", true);
        }
    }
}
