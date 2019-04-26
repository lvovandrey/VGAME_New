using LevelSetsEditor.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
//using System.Windows;
using System.Drawing;

namespace LevelSetsEditor.Model
{
    public enum VideoType {local, youtube, net}
    public class VideoInfo : INotifyPropertyChanged
    {

        public int Id { get; set; }

        [NotMapped]
        private string _Title { get; set; }
        [NotMapped]
        private string _Description { get; set; }
        [NotMapped]
        private TimeSpan _Duration { get; set; }
        [NotMapped]
        private string _Address { get; set; }
        [NotMapped]
        private VideoType _Type { get; set; }


        [NotMapped]
        private Preview _Preview { get; set; }
//        [NotMapped]
//        private Uri _Source { get; set; }
        [NotMapped]
        private Size _Resolution { get; set; }



        public string Title { get { return _Title; } set { _Title = value; OnPropertyChanged("Title"); } }
        public string Description { get { return _Description; } set { _Description = value; OnPropertyChanged("Description"); } }
        public TimeSpan Duration { get { return _Duration; } set { _Duration = value; OnPropertyChanged("Duration"); } }
        public string Address { get { return _Address; } set { _Address = value; OnPropertyChanged("Address"); } }
        public VideoType Type { get { return _Type; } set { _Type = value; OnPropertyChanged("Type"); } }

        public Preview Preview { get { return _Preview; } set { _Preview = value; OnPropertyChanged("Preview"); } }


        [Column("Source")]
        public string _Source { get; set; }
        [NotMapped]
        public Uri Source
        {
            get
            {
                if (_Source == null) _Source = "http://localhost/";
                return new Uri(_Source);
            }
            set
            {
                _Source = value.ToString();
                OnPropertyChanged("Source");
            }
        }




        //     
        //      [NotMapped]
        //      public System.Drawing.Size Resolution
        //      {
        //          get { return new System.Drawing.Size(ResolutionWidth, ResolutionHeight); }
        //          set { ResolutionWidth = value.Width; ResolutionHeight = value.Height; }
        //      }

        //      public int ResolutionHeight { get; set; }
        //      public int ResolutionWidth { get; set; }





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
