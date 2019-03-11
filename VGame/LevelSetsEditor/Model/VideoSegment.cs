using System;
using System.Collections.Generic;
using System.Text;

namespace LevelSetsEditor.Model
{
	public class VideoSegment
	{
		public TimeSpan TimeBegin { get; set; }
		public TimeSpan TimeEnd { get; set; }
		public Uri Source { get; set; }
	}
}
