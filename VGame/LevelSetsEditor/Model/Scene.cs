using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LevelSetsEditor.Model
{

    public class Scene : INotifyPropertyChanged
    {
        public Scene()
        {
            _VideoSegment = new VideoSegment();
        }


        public int Id { get; set; }
        public int VideoSegmentId { get; set; }
        [NotMapped]
        private VideoSegment _VideoSegment { get; set; }
        [NotMapped]
        private int _UnitsCount { get; set; }

        public VideoSegment VideoSegment { get { return _VideoSegment; } set { _VideoSegment = value; OnPropertyChanged("VideoSegment"); } }
        public int UnitsCount { get { return _UnitsCount; } set { _UnitsCount = value; OnPropertyChanged("UnitsCount"); } }

        #region mvvm
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
