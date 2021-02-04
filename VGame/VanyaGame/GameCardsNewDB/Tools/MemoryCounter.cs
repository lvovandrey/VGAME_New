using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VanyaGame.GameCardsNewDB.Tools
{
    public class MemoryCounter
    {
        private PerformanceCounter cpuCounter;
        private PerformanceCounter ramCounter;
        public MemoryCounter()
        {
            InitialiseCPUCounter();
            InitializeRAMCounter();
            // устанавливаем метод обратного вызова
            TimerCallback tm = new TimerCallback(updateTimer_Tick);
            // создаем таймер
            Timer timer = new Timer(tm, this, 0, 1000);
        }

        Timer timer;

        private void updateTimer_Tick(object sender)
        {
            Console.WriteLine( "CPU Usage: " +
            Convert.ToInt32(cpuCounter.NextValue()).ToString() +
            "%");

            Console.WriteLine(Convert.ToInt32(ramCounter.NextValue()).ToString() + "Mb");
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
