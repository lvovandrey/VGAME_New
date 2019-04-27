using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Windows;

namespace LevelSetsEditor.Model
{
    public enum PreviewType
    {
        local,
        youtube,
        net
    }

    public class Preview : INotifyPropertyChanged
    {
        public Preview()
        {
            MultiplePrevSources = new List<Uri>();
        }

        public int Id { get; set; }

        [NotMapped]
        public Uri Source
        {
            get
            {
                if (SourceDb == null) SourceDb = "http://localhost/";
                return new Uri(SourceDb);
            }
            set
            {
                SourceDb = value.ToString();
                OnPropertyChanged("Source");
            }
        }
        [Column("Source")]
        public string SourceDb { get; set; }

        [NotMapped]
        public List<Uri> MultiplePrevSources
        {
            get
            {
                List<Uri> URIS = new List<Uri>(from u in MultiplePrevSourcesDB select new Uri(u));
                return URIS;
            }
            set
            {
                MultiplePrevSourcesDB = new List<string>(from u in value select u.ToString());
                OnPropertyChanged("MultiplePrevSources");
            }
        }
        public List<string> MultiplePrevSourcesDB { get; set; }



        public PreviewType Type { get { return _Type; } set { _Type = value; OnPropertyChanged("Type"); } }
        private PreviewType _Type { get; set; }


        [NotMapped]
        public System.Drawing.Size Size
        {
            get { return new System.Drawing.Size(Width, Height); }
            set { Width = value.Width; Height = value.Height; OnPropertyChanged("Size"); }
        }
        public int Height { get { return _Height; } set { _Height = value; OnPropertyChanged("Size"); } }
        public int Width { get { return _Width; } set { _Width = value; OnPropertyChanged("Size"); } }
        [NotMapped]
        private int _Height { get; set; }
        [NotMapped]
        private int _Width { get; set; }


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
