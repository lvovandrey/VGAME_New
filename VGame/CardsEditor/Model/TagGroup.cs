using CardsEditor.Abstract;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardsEditor.Model
{
    internal class TagGroup : INPCBase
    {
        [NotMapped]
        private string _Name { get; set; }
     
        public int Id { get; set; }
        public string Name { get { return _Name; } set { _Name = value; OnPropertyChanged("Name"); } }
    }
}