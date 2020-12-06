﻿using CardsEditor.Model;
using System.Data.Entity;
using System.Data.SQLite;


namespace LevelSetsEditor.DB
{

    public class Context : DbContext
    {
        public Context(string connectionString) : base(new SQLiteConnection(connectionString), true) { }

        public DbSet<Card> Cards { get; set; }
        public DbSet<Level> Levels { get; set; }

    }
}
