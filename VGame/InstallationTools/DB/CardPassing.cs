using CardsEditor.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardsEditor.Model
{
    public class CardPassing : INPCBase
    {
        [NotMapped]
        private string _DateAndTime { get; set; }
        [NotMapped]
        private int _AttemptsNumber { get; set; }

        public int Id { get; set; }
        public string DateAndTime { get { return _DateAndTime; } set { _DateAndTime = value; OnPropertyChanged("DateAndTime"); } }
        public int AttemptsNumber { get { return _AttemptsNumber; } set { _AttemptsNumber = value; OnPropertyChanged("AttemptsNumber"); } }
    }
}
