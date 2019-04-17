using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace LevelSetsEditor.Model
{
	public class LevelSet : INotifyPropertyChanged
    {
        public LevelSet()
        {
            SceneSets = new List<SceneSet>();
            VideoInfo = new VideoInfo();
        }

        public int Id { get; set; }
        public VideoInfo VideoInfo
		{ get; set; }

		public List<SceneSet> SceneSets { get; set; }

        public string Name { get; set; }

        public string SegregateScenes()
        {

            TimeSpan Dur = VideoInfo.Duration;
            int NumScenes =(int)Math.Ceiling(VideoInfo.Duration.TotalMinutes / 2);
            SceneSets.Clear();
            for (int i = 1; i <= NumScenes; i++)
            {
                SceneSet s = new SceneSet();
                s.UnitsCount = i;
                s.VideoSegment.TimeBegin = TimeSpan.FromSeconds((i - 1) * 120);
                if (i < NumScenes)
                    s.VideoSegment.TimeEnd = TimeSpan.FromSeconds(i * 120);
                else
                    s.VideoSegment.TimeEnd = VideoInfo.Duration - TimeSpan.FromSeconds(0.5);
                s.VideoSegment.Source = VideoInfo.Source;
                SceneSets.Add(s);
            }

            Name = "OPPPS";
            VideoInfo.Title = "JQJKJL";
            //SceneSets.Add()
            return "someShit";
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
