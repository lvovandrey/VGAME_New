using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LevelSetsEditor.Model
{
	public class VideoSegment
	{


        public int Id { get; set; }

        [NotMapped]
        TimeSpan timeBegin;
        public TimeSpan TimeBegin
        {
            get
            {
                return timeBegin;
            }
            set
            {
                timeBegin = value;
            }
        }

        [NotMapped]
        TimeSpan timeEnd;
        public TimeSpan TimeEnd
        {
            get
            {
                return timeEnd;
            }
            set
            {
                timeEnd = value;
            }
        }

        [NotMapped]
        public Uri Source
        {
            get
            {
                if (SourceDb == null) SourceDb = "http://localhost/";
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
