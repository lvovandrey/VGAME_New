using MVVMRealization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardsGameNewDBRepository.Model
{
    public class Attempt : INPCBase
    {
        [NotMapped]
        private string _DateAndTimeBegin { get; set; }
        [NotMapped]
        private string _DateAndTimeEnd { get; set; }
        [NotMapped]
        private Card _AskedCard { get; set; }

        public int Id { get; set; }
        public string DateAndTimeBegin { get { return _DateAndTimeBegin; } set { _DateAndTimeBegin = value; OnPropertyChanged("DateAndTimeBegin"); } }
        public string DateAndTimeEnd { get { return _DateAndTimeEnd; } set { _DateAndTimeEnd = value; OnPropertyChanged("DateAndTimeEnd"); } }

        public Card AskedCard { get { return _AskedCard; } set { _AskedCard = value; OnPropertyChanged("SaidCard"); } }
        public Card AnswerCard { get { return _AskedCard; } set { _AskedCard = value; OnPropertyChanged("SaidCard"); } }
    }
}
