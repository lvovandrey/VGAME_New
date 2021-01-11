
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Windows;
using VanyaGame.Tools;


namespace VanyaGame.DB.DBLevelsRepositoryModel
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

        [NotMapped]
        public string _Tag { get; set; }
        public string Tag { get { return _Tag; } set { _Tag = value; OnPropertyChanged("Tag"); } }




        public Level()
        {
            Scenes = new ObservableCollection<Scene>();
            
            VideoInfo = new VideoInfo();
        }

        public string SegregateScenes()
        {

            TimeSpan Dur = VideoInfo.Duration;
            int NumScenes = (int)Math.Ceiling(VideoInfo.Duration.TotalMinutes / 2);
            //Scenes = new ObservableCollection<Scene>();
            Scenes.Clear();
            for (int i = 1; i <= NumScenes; i++)
            {
                Scene s = new Scene();
                int nextid = 0;
                if (Scenes.Count > 0)
                    nextid = Scenes.Max(a => a.Id);
                s.Id = ++nextid;
                s.Position = i;
                s.UnitsCount = i;
                s.VideoSegment.TimeBegin = TimeSpan.FromSeconds((i - 1) * 120);
                if (i < NumScenes)
                    s.VideoSegment.TimeEnd = TimeSpan.FromSeconds(i * 120);
                else
                    s.VideoSegment.TimeEnd = VideoInfo.Duration - TimeSpan.FromSeconds(0.5);
                s.VideoSegment.Source = VideoInfo.Source;
                Scenes.Add(s);
            }

            return "someShit";
        }

        /// <summary>
        /// Разбивает видеозапись на сцены по равным отрезкам времени. Последний отрезок - по остаточному принципу (может быть коротеньким совсем)
        /// </summary>
        /// <param name="segrTime">Время одной сцены</param>
        /// <returns></returns>
        public string SegregateScenes(TimeSpan segrTime, TimeSpan OverlapSegregateTime)
        {

            TimeSpan Dur = VideoInfo.Duration;
            int NumScenes = (int)Math.Ceiling(VideoInfo.Duration.TotalMinutes / segrTime.TotalMinutes);
            //Scenes = new ObservableCollection<Scene>();
            Scenes.Clear();
            for (int i = 1; i <= NumScenes; i++)
            {
                Scene s = new Scene();
                int nextid = 0;
                if (Scenes.Count > 0)
                    nextid = Scenes.Max(a => a.Id);
                s.Id = ++nextid;
                s.Position = i;
                s.UnitsCount = i;
                s.VideoSegment.TimeBegin = TimeSpan.FromSeconds((i - 1) * segrTime.TotalSeconds);
                if (i < NumScenes)
                    s.VideoSegment.TimeEnd = TimeSpan.FromSeconds((i * segrTime.TotalSeconds) + OverlapSegregateTime.TotalSeconds);
                else
                    s.VideoSegment.TimeEnd = VideoInfo.Duration - TimeSpan.FromSeconds(0.1);
                s.VideoSegment.Source = VideoInfo.Source;
                Scenes.Add(s);
            }

            return "someShit";
        }

        /// <summary>
        /// Разбивает видеозапись заданное количество сцен с равной длительностью 
        /// </summary>
        /// <param name="scenesCount">Нужное количество сцен</param>
        /// <returns></returns>
        public string SegregateScenes(int scenesCount, TimeSpan OverlapSegregateTime)
        {

            TimeSpan Dur = VideoInfo.Duration;
            TimeSpan segrTime = TimeSpan.FromSeconds((Dur.TotalSeconds - 0.1)/ scenesCount);

            int NumScenes = scenesCount;
            //Scenes = new ObservableCollection<Scene>();
            Scenes.Clear();
            for (int i = 1; i <= NumScenes; i++)
            {
                Scene s = new Scene();

                int nextid = 0;
                if (Scenes.Count > 0)
                    nextid = Scenes.Max(a => a.Id);
                s.Id = ++nextid;
                s.Position = i;
                s.UnitsCount = i;
                s.VideoSegment.TimeBegin = TimeSpan.FromSeconds((i - 1) * segrTime.TotalSeconds);
                if (i < NumScenes)
                    s.VideoSegment.TimeEnd = TimeSpan.FromSeconds((i * segrTime.TotalSeconds) + OverlapSegregateTime.TotalSeconds);
                else
                    s.VideoSegment.TimeEnd = VideoInfo.Duration - TimeSpan.FromSeconds(0.1); 
                s.VideoSegment.Source = VideoInfo.Source;
                Scenes.Add(s);
            }

            return "someShit";
        }

        /// <summary>
        /// Разделение сцены на 2 части по времени указанному в параметере
        /// </summary>
        /// <param name="parameter">Время разделения сцен</param>
        public void SplitScene(TimeSpan splitTime)
        {
            foreach (Scene s in _Scenes)
            {
                if ((s.VideoSegment.TimeBegin < splitTime) && (s.VideoSegment.TimeEnd > splitTime))
                {
                    Scene newS = new Scene();
                    s.Copy(newS);

                    newS.VideoSegment.TimeBegin = splitTime;
                    s.VideoSegment.TimeEnd = splitTime;

                    ObservableCollection<Scene> newScenes = new ObservableCollection<Scene>();
                   
                    foreach (Scene ss in _Scenes)
                    {

                        if (ss.Equals(s))
                        {
                            newScenes.Add(ss);
                            newScenes.Add(newS);
                        }
                        else
                        {
                            newScenes.Add(ss);
                        }
                    }
                    _Scenes.Clear();
                    _Scenes = newScenes;
                    break;
                }

            }
            //приводим в порядок очередность сцен
            int pos = 0;
            foreach (Scene s in _Scenes)
            {
                pos++; s.Position = pos;
            }
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
