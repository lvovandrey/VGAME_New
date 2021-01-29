using CardsGameNewDBRepository.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.Linq;
using System.Data.Entity;

namespace CardsGameNewDBRepository
{
    public delegate void DBInitCallback(ObservableCollection<Card> cards, ObservableCollection<Level> levels, ObservableCollection<LevelPassing> levelPassings, Context Context);

    public class DBTools
    {
        public static bool IsDBLoaded = false;
        public static string DBFilename = "";
        public static Context Context;

        public static bool LoadDB(DBInitCallback DBInitCallback, ObservableCollection<Card> _cards, ObservableCollection<Level> _levels, ObservableCollection<LevelPassing> _levelPassings, Context context, string _DBFilename)
        {
            bool error = false;
            try
            {
                _cards = new ObservableCollection<Card>();
                _levels = new ObservableCollection<Level>();
                _levelPassings = new ObservableCollection<LevelPassing>();
                context = new Context(@"Data Source=" + _DBFilename);
                Context = context;

                IEnumerable<Level> levels = context.Levels.Include(p => p.Cards).ToList();
                IEnumerable<Card> cards = context.Cards.ToList();
                IEnumerable<LevelPassing> levelPassings = context.LevelPassings.ToList();
                IEnumerable<CardPassing> cardPassings = context.CardPassings.ToList();


                foreach (Card c in cards)
                    _cards.Add(c);

                foreach (Level t in levels)
                    _levels.Add(t);

                foreach (LevelPassing l in levelPassings)
                    _levelPassings.Add(l);



                DBInitCallback(_cards, _levels, _levelPassings, context);
                IsDBLoaded = true;
                DBFilename = _DBFilename;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                error = true;
            }
            return !error;
        }

        public static bool LoadDB( ObservableCollection<Card> _cards, ObservableCollection<Level> _levels, ObservableCollection<LevelPassing> _levelPassings, Context context, string _DBFilename)
        {
            void DBInitCallbackMoq(ObservableCollection<Card> cards, ObservableCollection<Level> levels, ObservableCollection<LevelPassing> levelPassings, Context Context) { }
            return LoadDB(DBInitCallbackMoq, _cards, _levels, _levelPassings, Context,_DBFilename);
        }

        public static bool LoadDB(ObservableCollection<Card> _cards, ObservableCollection<Level> _levels, ObservableCollection<LevelPassing> _levelPassings, string _DBFilename)
        {
            void DBInitCallbackMoq(ObservableCollection<Card> cards, ObservableCollection<Level> levels, ObservableCollection<LevelPassing> levelPassings, Context Context) { }
            return LoadDB(DBInitCallbackMoq, _cards, _levels, _levelPassings, Context, _DBFilename);
        }

        public static bool CreateDB(DBInitCallback DBInitCallback, ObservableCollection<Card> _cards, ObservableCollection<Level> _levels, ObservableCollection<LevelPassing> _levelPassings, Context context, string _DBFilename)
        {
            bool error = false;
            try
            {

                SQLiteConnection.CreateFile(_DBFilename);

                SQLiteFactory factory = (SQLiteFactory)DbProviderFactories.GetFactory("System.Data.SQLite");
                using (SQLiteConnection connection = (SQLiteConnection)factory.CreateConnection())
                {
                    connection.ConnectionString = "Data Source = " + _DBFilename;
                    connection.Open();

                    using (SQLiteCommand command = new SQLiteCommand(connection))
                    {
                        command.CommandText = @"CREATE TABLE [Cards] (
	[Id]	INTEGER NOT NULL UNIQUE,
	[Title]	TEXT,
	[SoundedText]	TEXT,
	[Description]	TEXT,
	[ImageAddress]	TEXT,
	[SoundAddress]	TEXT,
	PRIMARY KEY([Id] AUTOINCREMENT)
)";
                        command.CommandType = CommandType.Text;
                        command.ExecuteNonQuery();
                    }
                    using (SQLiteCommand command = new SQLiteCommand(connection))
                    {
                        command.CommandText = @"CREATE TABLE [Levels] (
    [Id]    INTEGER NOT NULL UNIQUE, 
    [Name]  TEXT,
	[ImageAddress]  TEXT,
	PRIMARY KEY([Id] AUTOINCREMENT)
)";
                        command.CommandType = CommandType.Text;
                        command.ExecuteNonQuery();
                    }
                    using (SQLiteCommand command = new SQLiteCommand(connection))
                    {
                        command.CommandText = @"CREATE TABLE [LevelCards] (
    [Level_Id]  INTEGER,
	[Card_Id]   INTEGER,
	[Id]    INTEGER NOT NULL UNIQUE,
    FOREIGN KEY([Level_Id]) REFERENCES [Levels]([Id]) ON DELETE CASCADE,
	FOREIGN KEY([Card_Id]) REFERENCES [Cards]([Id]) ON DELETE CASCADE,
	PRIMARY KEY([Id] AUTOINCREMENT)
)";
                        command.CommandType = CommandType.Text;
                        command.ExecuteNonQuery();
                    }

                    using (SQLiteCommand command = new SQLiteCommand(connection))
                    {
                        command.CommandText = @"CREATE TABLE [LevelPassings] (
    [Id]    INTEGER NOT NULL UNIQUE,
    [DateAndTime]   TEXT NOT NULL,
	[IsComplete]    INTEGER NOT NULL,
	[Level_Id]  INTEGER NOT NULL,
	PRIMARY KEY([Id] AUTOINCREMENT),
	FOREIGN KEY([Level_Id]) REFERENCES [Levels]([Id]) ON DELETE CASCADE
)";
                        command.CommandType = CommandType.Text;
                        command.ExecuteNonQuery();
                    }

                    using (SQLiteCommand command = new SQLiteCommand(connection))
                    {
                        command.CommandText = @"CREATE TABLE [CardPassings] (
    [Id]    INTEGER NOT NULL UNIQUE,
    [AttemptsNumber]    INTEGER NOT NULL,
	[DateAndTime]   TEXT,
	[Card_Id]   INTEGER NOT NULL,
	[LevelPassing_Id]   INTEGER NOT NULL,
	PRIMARY KEY([Id] AUTOINCREMENT),
	FOREIGN KEY([Card_Id]) REFERENCES Cards(Id) ON DELETE CASCADE,
	FOREIGN KEY([LevelPassing_Id]) REFERENCES [LevelPassings]([Id]) ON DELETE CASCADE
)";
                        command.CommandType = CommandType.Text;
                        command.ExecuteNonQuery();
                    }
                }

                LoadDB(DBInitCallback,_cards, _levels, _levelPassings, context, _DBFilename);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                error = true;
            }
            return !error;
        }

        public static bool CreateDB(ObservableCollection<Card> _cards, ObservableCollection<Level> _levels, ObservableCollection<LevelPassing> _levelPassings, Context context, string _DBFilename)
        {
            void DBInitCallbackMoq(ObservableCollection<Card> cards, ObservableCollection<Level> levels, ObservableCollection<LevelPassing> levelPassings, Context Context) { }
            return CreateDB(DBInitCallbackMoq, _cards, _levels, _levelPassings, Context, _DBFilename);
        }
    }
}
