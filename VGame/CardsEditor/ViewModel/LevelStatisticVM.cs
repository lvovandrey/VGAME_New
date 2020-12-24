using CardsEditor.Abstract;
using CardsEditor.Model;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardsEditor.ViewModel
{
    public class LevelStatisticVM : INPCBase
    {
        #region Constructors
        public LevelStatisticVM(Level level, LevelVM levelVM, VM vm)
        {
            _level = level;
            _vm = vm;
            _levelVM = levelVM;
        }


        #endregion

        #region Fields
        Level _level;
        VM _vm;
        LevelVM _levelVM;

        #endregion

        #region Properties
        public int? CardsCount { get { return _level?.Cards?.Count(); } }
        public int? LevelPassingsCount { get { return _level?.LevelPassings?.Count(); } }

        private SeriesCollection fallsCountSeriesCollection;
        public SeriesCollection FallsCountSeriesCollection
        {
            get
            {
                var FallsCounts = levelPassingFalls.Select(f => (double)f.FallsCount);

                fallsCountSeriesCollection = new SeriesCollection
                {
                    new ColumnSeries
                    {
                        Title = "Кол-во ошибок",
                        Values = new ChartValues<double> (FallsCounts)
                    }
                };
                return fallsCountSeriesCollection;
            }
            set { fallsCountSeriesCollection = value; OnPropertyChanged("FallsCountSeriesCollection"); }
        }

        class LevelPassingFalls
        {
            public LevelPassingFalls(string dateTime, int fallsCount, int cardsCount)
            {
                DateTime = dateTime;
                FallsCount = fallsCount;
                CardsCount = cardsCount;
            }
            public string DateTime;
            public int FallsCount;
            public int CardsCount;
            public double FallsPercent { get { if (CardsCount > 0) return (double)FallsCount / CardsCount; else return 0; } }

        }

        private List<LevelPassingFalls> levelPassingFalls
        {
            get
            {

                var levelPassingFalls = new List<LevelPassingFalls>();
                if (_level != null && _level.LevelPassings != null)
                    foreach (var lvlPassing in _level.LevelPassings)
                    {
                        LevelPassingFalls levelPassingFall = new LevelPassingFalls(lvlPassing.DateAndTime, 0, lvlPassing.CardPassings.Count);
                        if (lvlPassing.CardPassings != null)
                            foreach (var cardPassing in lvlPassing.CardPassings)
                            {
                                if (cardPassing.AttemptsNumber > 1) levelPassingFall.FallsCount++;
                            }
                        levelPassingFalls.Add(levelPassingFall);
                    }
                return levelPassingFalls;
            }
        }



        public string[] FallsCountChartLabels { get { return new[] { "Maria", "Susan", "Charles", "Frida" }; } }
        public Func<double, string> FallsCountChartYFormatter { get { return value => value.ToString("N"); } }


        #endregion

    }
}
