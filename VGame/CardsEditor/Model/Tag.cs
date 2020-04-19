using CardsEditor.Abstract;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardsEditor.Model
{
    public class Tag : INPCBase
    {
        [NotMapped]
        private string _Name { get; set; }
        [NotMapped]
        private string _SoundedText { get; set; }
        [NotMapped]
        private ObservableCollection<TagGroup> _TagGroups { get; set; }

        public int Id { get; set; }
        public string Name { get { return _Name; } set { _Name = value; OnPropertyChanged("Name"); } }
        public string SoundedText { get { return _SoundedText; } set { _SoundedText = value; OnPropertyChanged("SoundedText"); } }
        public ObservableCollection<TagGroup> TagGroups { get { return _TagGroups; } set { _TagGroups = value; OnPropertyChanged("TagGroups"); } }
    }
}