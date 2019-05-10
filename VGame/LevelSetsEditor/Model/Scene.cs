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
        [NotMapped]
        private int _Position { get; set; }


        public VideoSegment VideoSegment { get { return _VideoSegment; } set { _VideoSegment = value; OnPropertyChanged("VideoSegment"); } }
        public int UnitsCount { get { return _UnitsCount; } set { _UnitsCount = value; OnPropertyChanged("UnitsCount"); } }
        public int Position { get { return _Position; } set { _Position = value; OnPropertyChanged("Position"); } }

        #region reserve
        //Резервные поля для базы данных
        [NotMapped]
        public string _GameInfo1 { get; set; }
        [NotMapped]
        public string _GameInfo2 { get; set; }
        [NotMapped]
        public string _GameInfo3 { get; set; }
        [NotMapped]
        public string _GameInfo4 { get; set; }
        [NotMapped]
        public string _GameInfo5 { get; set; }

        public string GameInfo1 { get { return _GameInfo1; } set { _GameInfo1 = value; OnPropertyChanged("GameInfo1"); } }
        public string GameInfo2 { get { return _GameInfo2; } set { _GameInfo2 = value; OnPropertyChanged("GameInfo2"); } }
        public string GameInfo3 { get { return _GameInfo3; } set { _GameInfo3 = value; OnPropertyChanged("GameInfo3"); } }
        public string GameInfo4 { get { return _GameInfo4; } set { _GameInfo4 = value; OnPropertyChanged("GameInfo4"); } }
        public string GameInfo5 { get { return _GameInfo5; } set { _GameInfo5 = value; OnPropertyChanged("GameInfo5"); } }
        #endregion
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
