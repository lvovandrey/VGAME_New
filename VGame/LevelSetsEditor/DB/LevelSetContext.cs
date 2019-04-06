using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LevelSetsEditor.Model;

namespace LevelSetsEditor.DB
{

    class LevelSetContext : DbContext
    {
        public LevelSetContext()
            : base("DbConnection")
        { }

        public DbSet<LevelSet> LevelSets { get; set; }
    }
}
