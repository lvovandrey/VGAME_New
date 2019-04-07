using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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

        public int Id { get; set; }
        public TimeSpan TimeEnd { get; set; }

        [NotMapped]
        public Uri Source
        {
            get
            {
                return new Uri(this.SourceDb);
            }
            set
            {
                this.SourceDb = value.ToString();
            }
        }

        [Column("Source")]
        public string SourceDb { get; set; }
    }
}
