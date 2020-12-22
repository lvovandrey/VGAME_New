using CardsEditor.Abstract;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardsEditor.Model
{
    public class LevelPassing : INPCBase
    {
        [NotMapped]
        private string _DateAndTime { get; set; }
        [NotMapped]
        private bool _IsComplete { get; set; }
        [NotMapped]
        private ObservableCollection<CardPassing> _CardPassings { get; set; }


        public ObservableCollection<CardPassing> CardPassings { get { return _CardPassings; } set { _CardPassings = value; OnPropertyChanged("CardPassings"); } }
        public int Id { get; set; }
        public string DateAndTime { get { return _DateAndTime; } set { _DateAndTime = value; OnPropertyChanged("DateAndTime"); } }
        public bool IsComplete { get { return _IsComplete; } set { _IsComplete = value; OnPropertyChanged("IsComplete"); } }
    }
}
