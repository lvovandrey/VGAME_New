
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Windows;
using YouTubeUrlSupplier;

namespace VanyaGame.GameNumberDB.Model
{
        

    public class Level : INotifyPropertyChanged
    {

        public int Id { get; set; }
        public int VideoInfoId { get;set;}
        [NotMapped]
        public VideoInfo _VideoInfo { get; set; }

        [NotMapped]
        public string _Name { get; set; }

        [NotMapped]
        public string YoutubePreview { get; private set; }

        public VideoInfo VideoInfo { get { return _VideoInfo; } set { _VideoInfo = value; OnPropertyChanged("VideoInfo"); } }
        public string Name { get { return _Name; } set { _Name = value; OnPropertyChanged("Name"); } }



        [NotMapped]
        private ObservableCollection<Scene> _Scenes { get; set; }

        public ObservableCollection<Scene> Scenes { get { return _Scenes; } set { _Scenes = value; OnPropertyChanged("Scenes"); } }



        public Level()
        {
            Scenes = new ObservableCollection<Scene>();
            
            VideoInfo = new VideoInfo();
        }

       

       

        public async void RefreshYoutubeLink()
        {
            if (this.VideoInfo.Type != VideoType.youtube) return;

            YoutubeVidInfo vidInfo = new YoutubeVidInfo(VideoInfo.Address);
            await vidInfo.NEWLIBRARY_GetVideoAsync();
            if (vidInfo.DirectURL == "")
            {
                MessageBox.Show("Невозможно получить прямую ссылку на это видео", "Ошибка получения ссылки", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            YoutubePreview = vidInfo.ImageUrl;

            this.VideoInfo.Source = new Uri(vidInfo.DirectURL);
            OnPropertyChanged("VideoInfo");
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
