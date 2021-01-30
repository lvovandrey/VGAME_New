using MVVMRealization;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardsGameNewDBRepository.Model
{
    public class CardPassing : INPCBase
    {
        [NotMapped]
        private string _DateAndTime { get; set; }
        [NotMapped]
        private int _AttemptsNumber { get; set; }
        [NotMapped]
        private ObservableCollection<Attempt> _Attempts { get; set; } = new ObservableCollection<Attempt>();

        public int Id { get; set; }
        public string DateAndTime { get { return _DateAndTime; } set { _DateAndTime = value; OnPropertyChanged("DateAndTime"); } }
        public int AttemptsNumber { get { return _AttemptsNumber; } set { _AttemptsNumber = value; OnPropertyChanged("AttemptsNumber"); } }
        public ObservableCollection<Attempt> Attempts { get { return _Attempts; } set { _Attempts = value; OnPropertyChanged("Attempts"); } }
    }
}
