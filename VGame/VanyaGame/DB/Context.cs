using System.Data.Entity;
using VanyaGame.DB.DBCardsRepositoryModel;
using VanyaGame.DB.DBLevelsRepositoryModel;

namespace VanyaGame.DB
{

    public class Context : DbContext
    {
        public Context()
            : base("LEVELSDBConnectionString")
        { }

        public DbSet<Level> Levels { get; set; }
        public DbSet<VideoInfo> VideoInfoes { get; set; }
        public DbSet<Preview> Previews { get; set; }
        public DbSet<Scene> Scenes { get; set; }
        public DbSet<VideoSegment> VideoSegments { get; set; }

    }


    public class ContextCards : DbContext
    {
        public ContextCards()
            : base("CardsDBConnectionString")
        { }

        public DbSet<Card> Cards { get; set; }
        public DbSet<Tag> Tags { get; set; }

    }

}
