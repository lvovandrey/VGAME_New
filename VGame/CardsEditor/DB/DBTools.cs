using CardsEditor.Model;
using CardsEditor.ViewModel;
using LevelSetsEditor.DB;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.SQLite;
using System.Data.Common;
using System.Data;

namespace CardsEditor.DB
{
    public class DBTools
    {
        public static bool IsDBLoaded = false;
        public static string DBFilename = "";

        public static bool LoadDB(VM vm, ObservableCollection<Card> _cards, ObservableCollection<Level> _levels, ObservableCollection<LevelPassing> _levelPassings, Context context, string _DBFilename)
        {
            bool error = false;
            try
            {
                _cards = new ObservableCollection<Card>();
                _levels = new ObservableCollection<Level>();
                _levelPassings = new ObservableCollection<LevelPassing>();
                context = new Context(@"Data Source=" + _DBFilename);

                IEnumerable<Level> levels = context.Levels.Include(p => p.Cards).ToList();
                IEnumerable<Card> cards = context.Cards.ToList();
                IEnumerable<LevelPassing> levelPassings = context.LevelPassings.ToList();

                foreach (Card c in cards)
                    _cards.Add(c);

                foreach (Level t in levels)
                    _levels.Add(t);

                foreach (LevelPassing l in levelPassings)
                    _levelPassings.Add(l);


                vm.init(_cards, _levels, _levelPassings, context);
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


        public static bool CreateDB(VM vm, ObservableCollection<Card> _cards, ObservableCollection<Level> _levels, ObservableCollection<LevelPassing> _levelPassings, Context context, string _DBFilename)
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
    FOREIGN KEY([Level_Id]) REFERENCES [Levels]([Id]),
	FOREIGN KEY([Card_Id]) REFERENCES [Cards]([Id]),
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
	FOREIGN KEY([Level_Id]) REFERENCES [Levels]([Id])
)";
                        command.CommandType = CommandType.Text;
                        command.ExecuteNonQuery();
                    }
                }

                LoadDB(vm, _cards, _levels, _levelPassings, context, _DBFilename);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                error = true;
            }
            return !error;
        }
    }
}
