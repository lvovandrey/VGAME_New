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


        //private SeriesCollection fallsCountSeriesCollection;
        //public SeriesCollection FallsCountSeriesCollection
        //{
        //    get
        //    {
        //        var FallsCounts = levelPassingFalls.Select(f => (double)f.FallsCount);
        //        Brush brush = new SolidColorBrush(Colors.DarkOrange);
        //        brush.Opacity = 0.5;
        //        fallsCountSeriesCollection = new SeriesCollection
        //        {

        //            new StackedAreaSeries
        //        {
        //            Values = new ChartValues<double> (FallsCounts),
        //            LineSmoothness = 0 , Foreground = Brushes.Black, Fill = brush, PointForeground=Brushes.Black,
        //            Title="Ошибки",
        //            LabelPoint = point => levelPassingFalls[(int)point.X].DateTime
        //    }
        //};
        //        return fallsCountSeriesCollection;
        //    }
        //    set { fallsCountSeriesCollection = value; OnPropertyChanged("FallsCountSeriesCollection"); }
        //}



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

        //public int MaxFallsCount
        //{
        //    get
        //    {
        //        if (levelPassingFalls == null || levelPassingFalls.Count == 0) return 0;
        //        return (int)levelPassingFalls.Select(f => (double)f.FallsCount).Max();
        //    }
        //}

        //public string[] FallsCountChartLabels => Enumerable.Range(1, levelPassingFalls.Count).Select(v => v.ToString()).ToArray();
        //public string[] FallsCountChartYLabels => Enumerable.Range(0, MaxFallsCount + 1).Select(v => v.ToString()).ToArray();


        //public Func<double, string> FallsCountChartYFormatter { get { return value => value.ToString(); } }


        #endregion


        #region Commands

        private RelayCommand clearLevelPassingsStatisticCommand;
        public RelayCommand ClearLevelPassingsStatisticCommand
        {
            get
            {
                return clearLevelPassingsStatisticCommand ?? (clearLevelPassingsStatisticCommand = new RelayCommand(obj =>
                {
                    if (MessageBox.Show("Вы уверены что хотите полностью сбросить статистику для этого уровня?", "Сброс статистики", MessageBoxButton.YesNo) == MessageBoxResult.No) return;
                    ClearLevelPassingsStatistic();
                }));
            }
        }

        internal void ClearLevelPassingsStatistic()
        {
            if (_level == null || _level.LevelPassings == null) return;
            foreach (var lp in _level.LevelPassings.ToArray())
            {
                if (lp.CardPassings != null)
                    foreach (var cp in lp.CardPassings.ToArray())
                    {
                        DBTools.Context.Entry(cp).State = System.Data.Entity.EntityState.Deleted;
                    }
                DBTools.Context.Entry(lp).State = System.Data.Entity.EntityState.Deleted;
            }
            DBTools.Context.SaveChanges();
            _levelVM.OnClearLevelStatisticsVM();
            _vm.OnPropertyChangedCardVMs();
        }
        #endregion
    }
}