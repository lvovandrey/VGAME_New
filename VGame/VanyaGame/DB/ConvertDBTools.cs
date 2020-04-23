using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VanyaGame.Struct.Components;
using DBModel = VanyaGame.DB.DBLevelsRepositoryModel;

namespace VanyaGame.DB
{
    public static class ConvertDBTools
    {
        public static VideoType VideoTypeConvertFromDB(DBModel.VideoType videoType)
        {
            VideoType newType; 
            switch (videoType)
            {
                case DBModel.VideoType.local: newType = VideoType.local; break;
                case DBModel.VideoType.net: newType = VideoType.net; break;
                case DBModel.VideoType.youtube: newType = VideoType.youtube; break;
                case DBModel.VideoType.none: newType = VideoType.unknown; break;
                default: newType = VideoType.unknown; break;
            }
            return newType;
        }
    }
}
