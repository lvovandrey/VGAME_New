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
    public class Level : INPCBase
    {
        [NotMapped]
        private string _Name { get; set; }
        [NotMapped]
        private string _ImageAddress { get; set; }
        [NotMapped]
        private ObservableCollection<Card> _Cards { get; set; }
        [NotMapped]
        private ObservableCollection<LevelPassing> _LevelPassings { get; set; }



        public int Id { get; set; }
        public ObservableCollection<Card> Cards { get { return _Cards; } set { _Cards = value; OnPropertyChanged("Cards"); } }
        public string Name { get { return _Name; } set { _Name = value; OnPropertyChanged("Name"); } }
        public string ImageAddress { get { return _ImageAddress; } set { _ImageAddress = value; OnPropertyChanged("ImageAddress"); } }
        public ObservableCollection<LevelPassing> LevelPassings { get { return _LevelPassings; } set { _LevelPassings = value; OnPropertyChanged("LevelPassings"); } }


        [NotMapped]
        public Uri Source
        {
            get
            {
                if (ImageAddress == null) ImageAddress = "http://localhost/";
                return new Uri(ImageAddress);
            }
            set
            {
                ImageAddress = value.ToString();
                OnPropertyChanged("ImageAddress");
            }
        }
    }
}
