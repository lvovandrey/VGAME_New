using LevelSetsEditor.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace VanyaGame.GameNumberDB.DB
{

    public class Context : DbContext
    {
        public Context()
            : base("DBConnectionLevelSets")
        { }

        public DbSet<Level> Levels { get; set; }
        public DbSet<VideoInfo> VideoInfoes { get; set; }
        public DbSet<Preview> Previews { get; set; }
        public DbSet<Scene> Scenes { get; set; }
        public DbSet<VideoSegment> VideoSegments { get; set; }

    }
}
