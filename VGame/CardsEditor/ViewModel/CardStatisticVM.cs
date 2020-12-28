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
            InterestsPassingsCount = 3;
        }


        #endregion

        #region Fields
        Card _card;
        VM _vm;
        CardVM _cardVM;

        #endregion

        #region Properties

        public int? CardPassingsCount { get { return _card?.CardPassings?.Count(); } }
        public int TotalFallsCount { get { return CalcTotalFallsCount(); } }

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

        public int? FallsOnLastNPassingsCount { get { return GetFallsOnLastNPassingsCount(InterestsPassingsCount); } }

        private int GetFallsOnLastNPassingsCount(int N)
        {
            if (N < 1) return 0;
            var AllAttemptsNumbers = _card.CardPassings.Select(cp => new
            {
                AttemptsNumber = cp.AttemptsNumber,
                DateAndTime = GetDateTime(cp.DateAndTime)
            });

            return AllAttemptsNumbers.OrderByDescending(lp => lp.DateAndTime).Take(N).Where(n => n.AttemptsNumber > 1).Count();
        }

        private DateTime GetDateTime(string dateAndTime)
        {
            DateTime dt;
            if (DateTime.TryParse(dateAndTime, out dt)) return dt;
            else return new DateTime(2000, 1, 1);
        }

        private int interestsPassingsCount;
        public int InterestsPassingsCount
        {
            get { return interestsPassingsCount; }
            set
            {
                if (CardPassingsCount == null) interestsPassingsCount = 0;
                if (value > CardPassingsCount)
                    interestsPassingsCount = (int)CardPassingsCount;
                else if (value < 1) interestsPassingsCount = 1;
                else interestsPassingsCount = value;
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

                    new ColumnSeries
                    {
                    Values = new ChartValues<double> (AttemptsNumbers),
                    Foreground = Brushes.Black,
                    Title="Кол-во попыток" }
                };
                return attemptsNumberSeriesCollection;
            }
            set { attemptsNumberSeriesCollection = value; OnPropertyChanged("AttemptsNumberSeriesCollection"); }
        }



        public int MaxAttempts
        {
            get
            {
                if (_card == null || _card.CardPassings == null || _card.CardPassings.Count == 0) return 0;
                var AttemptsNumbers = _card.CardPassings.Select(a => (double)a.AttemptsNumber);
                return (int)AttemptsNumbers.Max();
            }
        }

        public string[] AttemptsChartLabels => Enumerable.Range(1, (int)CardPassingsCount).Select(v => v.ToString()).ToArray();
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
            if (_card == null || _card.CardPassings == null) return;
            foreach (var cp in _card.CardPassings.ToArray())
            {
                DBTools.Context.Entry(cp).State = System.Data.Entity.EntityState.Deleted;
            }
            DBTools.Context.SaveChanges();
            _cardVM.OnClearCardStatisticsVM();
            _vm.OnPropertyChangedCardVMs();
        }
        #endregion
    }
}