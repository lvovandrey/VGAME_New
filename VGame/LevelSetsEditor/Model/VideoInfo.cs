using LevelSetsEditor.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Windows;

namespace LevelSetsEditor.Model
{
    public enum VideoType {local, youtube, net}
	public class VideoInfo : INotifyPropertyChanged
    {

        public int Id { get; set; }

        [NotMapped]
        private string _Title { get; set; }

        public string Title { get { return _Title; } set { _Title = value; OnPropertyChanged("Title"); } }
		


















		//public TimeSpan Duration { get; set; }

  //      public string Description { get; set; }
  //      [NotMapped]
  //      public System.Drawing.Size Resolution
  //      {
  //          get { return new System.Drawing.Size(ResolutionWidth, ResolutionHeight); }
  //          set { ResolutionWidth = value.Width; ResolutionHeight = value.Height; }
  //      }

  //      public int ResolutionHeight { get; set; }
  //      public int ResolutionWidth { get; set; }


  //      [NotMapped]
  //      public Uri Source
  //      {
  //          get
  //          {
  //              if (SourceDb == null) SourceDb = "http://localhost/";
  //              return new Uri(SourceDb);
  //          }
  //          set
  //          {
  //              SourceDb = value.ToString();
  //          }
  //      }
  //      [Column("Source")]
  //      public string SourceDb { get; set; }

  //      public string Address { get; set; }

  //      public VideoType Type { get; set; }

  //      public Preview Preview { get; set; }

        //public VideoInfo()
        //{ Preview = new Preview(); }

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
