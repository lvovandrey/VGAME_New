using System.Data.Entity;
using System.Data.SqlClient;
using VanyaGame.GameCardsNewDB.DB.RepositoryModel;

namespace VanyaGame.GameCardsNewDB.DB
{

    public class Context : DbContext
    {
        public Context(string connectionString) : base(new SqlConnection(connectionString), true) { }

        public DbSet<Card> Cards { get; set; }
        public DbSet<Level> Levels { get; set; }
    }


}
