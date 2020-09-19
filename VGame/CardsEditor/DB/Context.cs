using CardsEditor.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LevelSetsEditor.DB
{

    public class Context : DbContext
    {
        public Context()
            : base("CardsEditor.Properties.Settings.CardsDBInsectsConnectionString_OLD")
        { }

        public DbSet<Card> Cards { get; set; }
        public DbSet<Tag> Tags { get; set; }

    }
}
