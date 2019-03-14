using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace LevelSetsEditor.Model
{
	public class LevelSet
	{
        public LevelSet()
        {
            SceneSets = new List<SceneSet>();
            VideoInfo = new VideoInfo();
        }
        public VideoInfo VideoInfo
		{ get; set; }

		public List<SceneSet> SceneSets { get; set; }

        public string Name { get; set; }

        public string SegregateScenes()
        {

            TimeSpan Dur = VideoInfo.Duration;
            int NumScenes =(int)Math.Ceiling(VideoInfo.Duration.TotalMinutes / 2);

            Name = "OPPPS";
            VideoInfo.Title = "JQJKJL";
            //SceneSets.Add()
            return "someShit";

        }
    }
}
