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
           
        }
        public VideoInfo VideoInfo
		{ get; set; }

		public List<SceneSet> SceneSets { get; set; }

        public string Name { get; set; }

        public string SegregateScenes()
        {

            TimeSpan Dur = VideoInfo.Duration;

            return "someShit";

        }
    }
}
