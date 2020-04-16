using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VanyaGame.Struct.Components;
using DBMDL = LevelSetsEditor.Model;

namespace VanyaGame.GameNumberDB.Tools
{
    public static class ConvertDBTools
    {
        public static VideoType VideoTypeConvertFromDB(DBMDL.VideoType videoType)
        {
            VideoType newType; 
            switch (videoType)
            {
                case DBMDL.VideoType.local: newType = VideoType.local; break;
                case DBMDL.VideoType.net: newType = VideoType.net; break;
                case DBMDL.VideoType.youtube: newType = VideoType.youtube; break;
                case DBMDL.VideoType.none: newType = VideoType.unknown; break;
                default: newType = VideoType.unknown; break;
            }
            return newType;
        }
    }
}
