
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using VanyaGame.GameNumberDB.Tools;

namespace VanyaGame.GameNumberDB.Model
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
            MultiplePrevSources = new ObservableCollection<Uri>();
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
                //проверим изменился ли тип ссылки на 
                PreviewType newType = Type;
                if ((value.ToString().Contains("youtube.com")) && (value.ToString().StartsWith("http"))) { newType = PreviewType.youtube; }
                else if (value.ToString().StartsWith("http")) { newType = PreviewType.net; }
                else if (value.ToString().StartsWith("file")) { newType = PreviewType.local; }
                if (newType != Type) { Type = newType; OnPropertyChanged("Type"); }

                //пересчитаем превью
                if ((SourceDb == "http://localhost/") && (SourceDb == "")) { Size = new System.Drawing.Size(0, 0); return; }
                //if (Type == PreviewType.local) PictHelper.GetLocalPictureSize(SourceDb);
                //if ((Type == PreviewType.net)|| (Type == PreviewType.youtube))
                   Size = PictHelper.GetPictureSize(Source);
                 OnPropertyChanged("Size");
            }
        }
        [Column("Source")]
        public string SourceDb { get; set; }

        [NotMapped]
        public ObservableCollection<Uri> MultiplePrevSources
        {
            get
            {
                ObservableCollection<Uri> URIS = new ObservableCollection<Uri>();

                string[] separators = new string[] { "[stop]" };

                string[] struris = MultiplePrevSourcesDB.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                foreach (string s in struris)
                    URIS.Add(new Uri(s));
                return URIS;
            }
            set
            {
                MultiplePrevSourcesDB = "";
                foreach (Uri u in value)
                    MultiplePrevSourcesDB += u.ToString() + "[stop]";
                OnPropertyChanged("MultiplePrevSources");
            }
        }


        [NotMapped]
        private string _MultiplePrevSourcesDB { get; set; }

        public string MultiplePrevSourcesDB
        {
            get { return _MultiplePrevSourcesDB; }
            set { _MultiplePrevSourcesDB = value; OnPropertyChanged("MultiplePrevSourcesDB"); }
        }



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
