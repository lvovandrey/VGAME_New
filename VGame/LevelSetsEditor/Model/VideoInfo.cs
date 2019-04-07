using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Windows;

namespace LevelSetsEditor.Model
{
    public enum VideoType {local, youtube, net}
	public class VideoInfo
	{
        public VideoInfo()
        { Preview = new Preview(); }

        public int Id { get; set; }

        public string Title { get; set; }
		
		public TimeSpan Duration { get; set; }

        public string Description { get; set; }
        [NotMapped]
        public System.Drawing.Size Resolution
        {
            get { return new System.Drawing.Size(ResolutionWidth, ResolutionHeight); }
            set { ResolutionWidth = value.Width; ResolutionHeight = value.Height; }
        }

        public int ResolutionHeight { get; set; }
        public int ResolutionWidth { get; set; }


        [NotMapped]
        public Uri Source
        {
            get
            {
                return new Uri(SourceDb);
            }
            set
            {
                SourceDb = value.ToString();
            }
        }
        [Column("Source")]
        public string SourceDb { get; set; }

        public string Address { get; set; }

        public VideoType Type { get; set; }

        public Preview Preview { get; set; }
    }
}
