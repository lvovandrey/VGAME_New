using CardsEditor.Abstract;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardsEditor.Model
{
    internal class Tag : INPCBase
    {
        [NotMapped]
        private string _Name { get; set; }
        [NotMapped]
        private ObservableCollection<TagGroup> _TagGroups { get; set; }

        public int Id { get; set; }
        public string Name { get { return _Name; } set { _Name = value; OnPropertyChanged("Name"); } }
        public ObservableCollection<TagGroup> TagGroups { get { return _TagGroups; } set { _TagGroups = value; OnPropertyChanged("TagGroups"); } }
    }
}