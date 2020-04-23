using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace VanyaGame.DB.DBLevelsRepositoryModel
{
	public class VideoSegment:INotifyPropertyChanged
	{


        public int Id { get; set; }

        [NotMapped]
        TimeSpan timeBegin;
        public TimeSpan TimeBegin
        {
            get
            {
                return timeBegin;
            }
            set
            {
                timeBegin = value;
            }
        }

        [NotMapped]
        TimeSpan timeEnd;
        public TimeSpan TimeEnd
        {
            get
            {
                return timeEnd;
            }
            set
            {
                timeEnd = value;
            }
        }

        [NotMapped]
        public Uri Source
        {
            get
            {
                if (SourceDb == null) SourceDb = "http://localhost/";
                return new Uri(this.SourceDb);
            }
            set
            {
                this.SourceDb = value.ToString();
            }
        }

        [Column("Source")]
        public string SourceDb { get; set; }


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
        public string GameInfo2 { get { return _GameInfo1; } set { _GameInfo1 = value; OnPropertyChanged("GameInfo1"); } }
        public string GameInfo3 { get { return _GameInfo1; } set { _GameInfo1 = value; OnPropertyChanged("GameInfo1"); } }
        public string GameInfo4 { get { return _GameInfo1; } set { _GameInfo1 = value; OnPropertyChanged("GameInfo1"); } }
        public string GameInfo5 { get { return _GameInfo1; } set { _GameInfo1 = value; OnPropertyChanged("GameInfo1"); } }
        #endregion

        /// <summary>
        /// Копирует состояние данного объекта VideoSegment в объект передаваемый в аргументе
        /// </summary>
        /// <param name="videoSegment">Объект куда копируются данные</param>
        public void Copy(VideoSegment videoSegment)
        {
            videoSegment.timeBegin = this.timeBegin;
            videoSegment.timeEnd = this.timeEnd;
            videoSegment.SourceDb = this.SourceDb;

            videoSegment._GameInfo1 = this._GameInfo1;
            videoSegment._GameInfo2 = this._GameInfo2;
            videoSegment._GameInfo3 = this._GameInfo3;
            videoSegment._GameInfo4 = this._GameInfo4;
            videoSegment._GameInfo5 = this._GameInfo5;
        }

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
