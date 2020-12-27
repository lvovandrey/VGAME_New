using CardsEditor.Abstract;
using CardsEditor.DB;
using CardsEditor.Model;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;
using System.Xml;

namespace CardsEditor.ViewModel
{
    public class CardStatisticVM : INPCBase
    {
        #region Constructors
        public CardStatisticVM(Card card, CardVM cardVM, VM vm)
        {
            _card = card;
            _vm = vm;
            _cardVM = cardVM;
        }


        #endregion

        #region Fields
        Card _card;
        VM _vm;
        CardVM _cardVM;

        #endregion

        #region Properties

        public int? CardPassingsCount { get { return _card?.CardPassings?.Count(); } }
        public int? TotalFallsCount { get { return CalcTotalFallsCount(); } }

        private int CalcTotalFallsCount()
        {
            if (_card == null || _card.CardPassings == null) return 0;
            int count = 0;
            foreach (var cp in _card.CardPassings)
            {
                if (cp.AttemptsNumber > 1) count++;
            }
            return count;
        }

        public int? FallsOnLastNPassingsCount { get { return 3; } }

        private int? interestsPassingsCount;
        public int? InterestsPassingsCount
        {
            get { return interestsPassingsCount; }
            set
            {
                if (value > CardPassingsCount)
                    interestsPassingsCount = CardPassingsCount;
                if (value < 1) interestsPassingsCount = 1;
                OnPropertyChanged("FallsOnLastNPassingsCount");
                OnPropertyChanged("InterestsPassingsCount");
            }
        }


        private SeriesCollection attemptsNumberSeriesCollection;
        public SeriesCollection AttemptsNumberSeriesCollection
        {
            get
            {
                var AttemptsNumbers = _card.CardPassings.Select(a => (double)a.AttemptsNumber);

                attemptsNumberSeriesCollection = new SeriesCollection
                {

                    new StackedAreaSeries
                    {   
                    Values = new ChartValues<double> (AttemptsNumbers),
                    LineSmoothness = 0 , Foreground = Brushes.Black, PointForeground=Brushes.Black,
                    Title="Кол-во попыток" }
                };
                return attemptsNumberSeriesCollection;
            }
            set { attemptsNumberSeriesCollection = value; OnPropertyChanged("AttemptsNumberSeriesCollection"); }
        }



        //class LevelPassingFalls
        //{
        //    public LevelPassingFalls(string dateTime, int fallsCount, int cardsCount)
        //    {
        //        DateTime = dateTime;
        //        FallsCount = fallsCount;
        //        CardsCount = cardsCount;
        //    }
        //    public string DateTime;
        //    public int FallsCount;
        //    public int CardsCount;
        //    public double FallsPercent { get { if (CardsCount > 0) return (double)FallsCount / CardsCount; else return 0; } }

        //}

        //private List<LevelPassingFalls> levelPassingFalls
        //{
        //    get
        //    {

        //        var levelPassingFalls = new List<LevelPassingFalls>();
        //        if (_level != null && _level.LevelPassings != null)
        //            foreach (var lvlPassing in _level.LevelPassings)
        //            {
        //                LevelPassingFalls levelPassingFall = new LevelPassingFalls(lvlPassing.DateAndTime, 0, lvlPassing.CardPassings.Count);
        //                if (lvlPassing.CardPassings != null)
        //                    foreach (var cardPassing in lvlPassing.CardPassings)
        //                    {
        //                        if (cardPassing.AttemptsNumber > 1) levelPassingFall.FallsCount++;
        //                    }
        //                levelPassingFalls.Add(levelPassingFall);
        //            }
        //        return levelPassingFalls;
        //    }
        //}

        public int MaxAttempts
        {
            get
            {
                if (_card == null || _card.CardPassings==null || _card.CardPassings.Count == 0) return 0;
                var AttemptsNumbers = _card.CardPassings.Select(a => (double)a.AttemptsNumber);
                return (int)AttemptsNumbers.Max();
            }
        }

        public string[] AttemptsChartLabels => Enumerable.Range(1, (int)TotalFallsCount).Select(v => v.ToString()).ToArray();
        public string[] AttemptsChartYLabels => Enumerable.Range(0, MaxAttempts + 1).Select(v => v.ToString()).ToArray();


        public Func<double, string> AttemptsChartYFormatter { get { return value => value.ToString(); } }


        #endregion


        #region Commands

        private RelayCommand clearCardPassingsStatisticCommand;
        public RelayCommand ClearCardPassingsStatisticCommand
        {
            get
            {
                return clearCardPassingsStatisticCommand ?? (clearCardPassingsStatisticCommand = new RelayCommand(obj =>
                {
                    if (MessageBox.Show("Вы уверены что хотите полностью сбросить статистику для этой карточки?", "Сброс статистики", MessageBoxButton.YesNo) == MessageBoxResult.No) return;
                    ClearCardPassingsStatistic();
                }));
            }
        }

        internal void ClearCardPassingsStatistic()
        {
            //if (_level == null || _level.LevelPassings == null) return;
            //foreach (var lp in _level.LevelPassings.ToArray())
            //{
            //    if (lp.CardPassings != null)
            //        foreach (var cp in lp.CardPassings.ToArray())
            //        {
            //            DBTools.Context.Entry(cp).State = System.Data.Entity.EntityState.Deleted;
            //        }
            //    DBTools.Context.Entry(lp).State = System.Data.Entity.EntityState.Deleted;
            //}
            //DBTools.Context.SaveChanges();
            //_levelVM.OnClearLevelStatisticsVM();
            //_vm.OnPropertyChangedCardVMs();
        }
        #endregion
    }
}