using CardsGameNewDBRepository.Model;
using SQLite.CodeFirst;
using System.Data.Entity;
using System.Data.SQLite;

namespace CardsGameNewDBRepository
{
    public class Context : DbContext
    {
        public Context(string connectionString) : base(new SQLiteConnection(connectionString), true) { }

        public DbSet<Card> Cards { get; set; }
        public DbSet<Level> Levels { get; set; }
        public DbSet<LevelPassing> LevelPassings { get; set; }
        public DbSet<CardPassing> CardPassings { get; set; }
        public DbSet<Attempt> Attempts { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var sqliteConnectionInitializer = new SqliteCreateDatabaseIfNotExists<Context>(modelBuilder);
            Database.SetInitializer(sqliteConnectionInitializer);
        }
    }
}
