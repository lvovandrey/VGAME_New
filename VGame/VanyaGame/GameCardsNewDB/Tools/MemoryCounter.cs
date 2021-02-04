using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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

        

        private void updateTimer_Tick(object obj)
        {
            Console.Write( "CPU Usage: " +
            Convert.ToInt32(cpuCounter.NextValue()).ToString() +
            "%          ");

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
