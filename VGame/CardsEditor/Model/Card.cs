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
    internal class Card : INPCBase
    {
        [NotMapped]
        private string _Title { get; set; }
        [NotMapped]
        private string _Description { get; set; }
        [NotMapped]
        private string _ImageAddress { get; set; }
        [NotMapped]
        private string _SoundAddress { get; set; }
        [NotMapped]
        private ObservableCollection<Tag> _Tags { get; set; }



        public int Id { get; set; }
        public string Title { get { return _Title; } set { _Title = value; OnPropertyChanged("Title"); } }
        public string Description { get { return _Description; } set { _Description = value; OnPropertyChanged("Description"); } }
        public string ImageAddress { get { return _ImageAddress; } set { _ImageAddress = value; OnPropertyChanged("ImageAddress"); } }
        public string SoundAddress { get { return _SoundAddress; } set { _SoundAddress = value; OnPropertyChanged("SoundAddress"); } }




        public ObservableCollection<Tag> Tags { get { return _Tags; } set { _Tags = value; OnPropertyChanged("Tags"); } }

    }
}
