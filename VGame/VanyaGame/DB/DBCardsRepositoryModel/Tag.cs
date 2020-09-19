using VanyaGame.Abstract;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace VanyaGame.DB.DBCardsRepositoryModel
{
    public class Tag : INPCBase
    {
        //public Tag()
        //{
        //  //  Cards = new ObservableCollection<Card>();
        //}
        [NotMapped]
        private string _Name { get; set; }
        [NotMapped]
        private string _SoundedText { get; set; }
        [NotMapped]
        private ObservableCollection<Card> _Cards { get; set; }

        public int Id { get; set; }
        public ObservableCollection<Card> Cards { get { return _Cards; } set { _Cards = value; OnPropertyChanged("Cards"); } }
        public string Name { get { return _Name; } set { _Name = value; OnPropertyChanged("Name"); } }
        public string SoundedText { get { return _SoundedText; } set { _SoundedText = value; OnPropertyChanged("SoundedText"); } }

        [NotMapped]
        private string _TagGroup { get; set; }
        public string TagGroup { get { return _TagGroup; } set { _TagGroup = value; OnPropertyChanged("TagGroup"); } }

    }
}