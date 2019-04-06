using System;
using System.Collections.Generic;
using System.Text;

namespace LevelSetsEditor.Model
{

    public class SceneSet
	{
        public SceneSet()
        { VideoSegment = new VideoSegment(); }

        public int Id { get; set; }
        public VideoSegment VideoSegment { get; set; }
		public int UnitsCount { get; set; }		
	}
}
