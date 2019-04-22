using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LevelSetsEditor.Model;

namespace LevelSetsEditor.DB
{

    public class Context : DbContext
    {
        public Context()
            : base("DbConnection")
        { }

        public DbSet<Level> Levels { get; set; }
        public DbSet<VideoInfo> VideoInfos { get; set; }

    }
}
