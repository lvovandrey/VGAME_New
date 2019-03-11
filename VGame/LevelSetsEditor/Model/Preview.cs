using System;
using System.Collections.Generic;
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
		public Uri Source { get; set; }

		public PreviewType Type { get; set; }
        
        public Size Size { get; set; }
    }
}
