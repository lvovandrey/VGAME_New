using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace LevelSetsEditor.Model
{
    public enum VideoType {local, youtube, net}
	public class VideoInfo
	{
        public VideoInfo()
        { Preview = new Preview(); }
		public string Title { get; set; }
		
		public TimeSpan Duration { get; set; }

        public string Description { get; set; }

        public System.Drawing.Size Resolution { get; set; }

        public Uri Source { get; set; }

        public string Address { get; set; }

        public VideoType Type { get; set; }

        public Preview Preview { get; set; }
    }
}
