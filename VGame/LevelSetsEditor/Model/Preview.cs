using System;
using System.Collections.Generic;
using System.Text;

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
		public Uri Source
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		public PreviewType Type
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}
	}
}
