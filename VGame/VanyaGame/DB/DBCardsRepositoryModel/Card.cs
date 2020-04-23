using VanyaGame.Abstract;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VanyaGame.DB.DBCardsRepositoryModel
{
    public class Card : INPCBase
    {
        //public Card()
        //{
        //    Tags = new ObservableCollection<Tag>();
        //}
        [NotMapped]
        private string _Title { get; set; }
        [NotMapped]
        private string _SoundedText { get; set; }
        [NotMapped]
        private string _Description { get; set; }
        [NotMapped]
        private string _ImageAddress { get; set; }
        [NotMapped]
        private string _SoundAddress { get; set; }
        [NotMapped]
        private ObservableCollection<Tag> _Tags { get; set; }



        public int Id { get; set; }
        public ObservableCollection<Tag> Tags { get { return _Tags; } set { _Tags = value; OnPropertyChanged("Tags"); } }
        public string Title { get { return _Title; } set { _Title = value; OnPropertyChanged("Title"); } }
        public string SoundedText { get { return _SoundedText; } set { _SoundedText = value; OnPropertyChanged("SoundedText"); } }
        public string Description { get { return _Description; } set { _Description = value; OnPropertyChanged("Description"); } }
        public string ImageAddress { get { return _ImageAddress; } set { _ImageAddress = value; OnPropertyChanged("ImageAddress"); } }
        public string SoundAddress { get { return _SoundAddress; } set { _SoundAddress = value; OnPropertyChanged("SoundAddress"); } }


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
                //проверим изменился ли тип ссылки на 
            }
        }

    }
}
