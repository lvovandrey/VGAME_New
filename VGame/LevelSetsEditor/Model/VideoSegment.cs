using System;
using System.Collections.Generic;
using System.Text;

namespace LevelSetsEditor.Model
{
	public class VideoSegment
	{
        TimeSpan timeBegin;
		public TimeSpan TimeBegin
        {
            get
            {
                return timeBegin;
            }
            set
            { timeBegin = value; }
        }
		public TimeSpan TimeEnd { get; set; }
		public Uri Source { get; set; }
	}
}
