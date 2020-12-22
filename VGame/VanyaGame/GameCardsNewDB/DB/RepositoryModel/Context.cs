using System.Data.Entity;
using System.Data.SQLite;
using VanyaGame.GameCardsNewDB.DB.RepositoryModel;

namespace VanyaGame.GameCardsNewDB.DB
{

    public class Context : DbContext
    {
        public Context(string connectionString) : base(new SQLiteConnection(connectionString), true) { }

        public DbSet<Card> Cards { get; set; }
        public DbSet<Level> Levels { get; set; }
        public DbSet<LevelPassing> LevelPassings { get; set; }
    }


}
