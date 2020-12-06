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
        private Context()
            : base("CardsAndLevelsConnection")
        { }

        public Context(string ConnectionName)
            : base(ConnectionName)
        { }


        public DbSet<Card> Cards { get; set; }
        public DbSet<Level> Levels { get; set; }

    }
}
