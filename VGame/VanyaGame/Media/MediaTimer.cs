using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace VanyaGame.Media
{
    /// <summary>
    /// Таймер используемый для работы с медиа
    /// </summary>
    public class MediaTimer
    {
        public TimeSpan TimeBegin;
        public TimeSpan TimeEnd;
        private DispatcherTimer timer;
        public MediaTimer(TimeSpan _TimeBegin, TimeSpan _TimeEnd)
        {
            TimeBegin = _TimeBegin;
            TimeEnd = _TimeEnd;
        }
        public void Start(DelegateTimerTick TimerTick)
        {
            TimeSpan t = new TimeSpan(0, 0, 0, 0, 100);

            ToolsClass.CreateAndStart_DispatcherTimer(ref timer, TimerTick, t);
        }
        public void Stop()
        {
            ToolsClass.StopAndNull_DispatcherTimer(ref timer);
        }
        public void Pause()
        {
            ToolsClass.StopAndNull_DispatcherTimer(ref timer);
        }

    }
}
