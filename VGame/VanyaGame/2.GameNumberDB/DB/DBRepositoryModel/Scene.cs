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
        private int _TasksCount { get; set; }
        [NotMapped]
        private int _Position { get; set; }


        public VideoSegment VideoSegment { get { return _VideoSegment; } set { _VideoSegment = value; OnPropertyChanged("VideoSegment"); } }
        public int UnitsCount { get { return _UnitsCount; } set { _UnitsCount = value; OnPropertyChanged("UnitsCount"); } }
        public int TasksCount { get { return _TasksCount; } set { _TasksCount = value; OnPropertyChanged("TasksCount"); } }
        public int Position { get { return _Position; } set { _Position = value; OnPropertyChanged("Position"); } }


        /// <summary>
        /// Копирует состояние данного объекта Scene и вложенного объекта VideoSegment в объект передаваемый в аргументе
        /// </summary>
        /// <param name="scene">Объект куда копируются данные</param>
        public void Copy(Scene scene)
        {
            scene._UnitsCount = this._UnitsCount;
            scene._Position = this._Position;

            scene._GameInfo1 = this._GameInfo1;
            scene._GameInfo2 = this._GameInfo2;
            scene._GameInfo3 = this._GameInfo3;
            scene._GameInfo4 = this._GameInfo4;
            scene._GameInfo5 = this._GameInfo5;

            this.VideoSegment.Copy(scene.VideoSegment);
        }


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
