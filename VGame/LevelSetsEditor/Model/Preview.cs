using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Windows;

namespace LevelSetsEditor.Model
{
    public enum PreviewType
    {
        local,
        youtube,
        net
    }

    public class Preview
    {
        public Preview()
        {
            MultiplePrevSources = new List<Uri>();
        }

        public int Id { get; set; }

        [NotMapped]
        public Uri Source
        {
            get
            {
                if (SourceDb == null) SourceDb = "http://localhost/";
                return new Uri(SourceDb);
            }
            set
            {
                SourceDb = value.ToString();
            }
        }
        [Column("Source")]
        public string SourceDb { get; set; }

        [NotMapped]
        public List<Uri> MultiplePrevSources
        {
            get
            {
                List<Uri> URIS = new List<Uri>(from u in MultiplePrevSourcesDB select new Uri(u));
                return URIS;
            }
            set
            {
                MultiplePrevSourcesDB = new List<string>(from u in value select u.ToString());
            }
        }
        public List<string> MultiplePrevSourcesDB { get; set; }


        public PreviewType Type { get; set; }
        
     
        [NotMapped]
        public System.Drawing.Size Size
        {
            get { return new System.Drawing.Size(SizeWidth, SizeHeight); }
            set { SizeWidth = value.Width; SizeHeight = value.Height; }
        }
        public int SizeHeight { get; set; }
        public int SizeWidth { get; set; }
    }
}
