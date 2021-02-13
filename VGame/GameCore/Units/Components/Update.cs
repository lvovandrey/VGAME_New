using System;
using System.Threading;
using System.Windows.Threading;
using VGameCore.Abstract;

namespace VGameCore.Units.Components
{
    public class Update : Component
    {
        #region constructors
        /// <summary>
        /// Создаем бесконечный цикл обновления с заданной частототой
        /// </summary>
        /// <param name="name"></param>
        /// <param name="container"></param>
        /// <param name="freq">Частота обновления</param>
        public Update(string name, IComponentContainer container, double freq=60) : base(name, container)
        {
            if (freq <= 0)
            {
                throw new Exception("Frequency in Update component must be grater than zero");
            }
            Frequency = freq;
            thread = new Thread( new ThreadStart(StartCicle));
        }
        #endregion

        #region variables
        private Thread thread;
        #endregion

        #region properties
        private double Frequency { get; set; }
        #endregion

        #region methods
        public event VoidDelegate Up;

        private void StartCicle()
        {
            DispatcherTimer ToolTimer = new DispatcherTimer();
            ToolTimer.Tick += (s, _) =>
            {
                Up();
            };
            ToolTimer.Interval = TimeSpan.FromSeconds(1/Frequency);
            ToolTimer.Start();
        }
        #endregion

    }
}
