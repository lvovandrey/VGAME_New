using CardsGameNewDBRepository.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.Linq;
using System.Data.Entity;
using System.Windows;
using System.IO;

namespace CardsGameNewDBRepository
{
    public delegate void DBInitCallback(ObservableCollection<Card> cards, ObservableCollection<Level> levels, ObservableCollection<LevelPassing> levelPassings, Context Context);

    public class DBTools
    {
        public static bool IsDBLoaded = false;
        public static string DBFilename = "";
        public static Context Context;

        public static bool LoadDB(string _DBFilename)
        {
            bool error = false;
            try
            {
                Context = new Context(@"Data Source=" + _DBFilename);

                if (!DBTools.IsDBStructureOK())
                {
                    var r = System.Windows.MessageBox.Show("Структура БД устарела или повреждена " + _DBFilename + "\n Попробовать исправить?", "Ошибка", MessageBoxButton.YesNo, MessageBoxImage.Error);
                    if (r == MessageBoxResult.Yes)
                    {
                        if (DBTools.UpdateAndAddTableAttemptToDB(_DBFilename))
                            System.Windows.MessageBox.Show("Структура БД успешно обновлена" + _DBFilename, "Обновление структуры БД", MessageBoxButton.OK, MessageBoxImage.Information);
                        else
                            System.Windows.MessageBox.Show("Структуру БД не удалось исправить " + _DBFilename, "Обновление структуры БД", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }


                IEnumerable<Level> levels = Context.Levels.Include(p => p.Cards).ToList();
                IEnumerable<Card> cards = Context.Cards.ToList();
                IEnumerable<LevelPassing> levelPassings = Context.LevelPassings.ToList();
                IEnumerable<CardPassing> cardPassings = Context.CardPassings.ToList();
                IEnumerable<Attempt> attempts = Context.Attempts.ToList();

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

        public static bool CreateDBOnCodeFirst(string _DBFilename)
        {
            bool error = false;
            try
            {


                Context = new Context(@"Data Source=" + _DBFilename);
                //Card card = new Card();
                //Context.Cards.Add(card);
                //Context.SaveChanges();
                //Context.Cards.Remove(card);
                Context.SaveChanges();

                LoadDB(_DBFilename);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                error = true;
            }
            return !error;
        }

        public static bool CreateDBOnSQLCommands(string _DBFilename)
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

                LoadDB(_DBFilename);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                error = true;
            }
            return !error;
        }

        public static bool IsDBStructureOK()
        {
            return IsDBTableExist("Cards")
            && IsDBTableExist("Levels")
            && IsDBTableExist("LevelPassings") 
            && IsDBTableExist("CardPassings")
            && IsDBTableExist("Attempts");
        }

        public static bool IsDBTableExist(string tableName)
        {
            try
            {
                return Context.Database
                     .SqlQuery<string>(@"SELECT name FROM sqlite_master WHERE type='table' AND name='" + tableName + "';")
                     .SingleOrDefault() != null;
            }
            catch 
            {
                MessageBox.Show("Ошибка обращения к базе данных " + DBFilename, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        public static bool UpdateAndAddTableAttemptToDB(string _DBFilename)
        {
            if (IsDBTableExist("Attempts")) return false;
            bool error = false;
            try
            {

                if (!File.Exists(_DBFilename)) return false;
                
                SQLiteFactory factory = (SQLiteFactory)DbProviderFactories.GetFactory("System.Data.SQLite");
                using (SQLiteConnection connection = (SQLiteConnection)factory.CreateConnection())
                {
                    connection.ConnectionString = "Data Source = " + _DBFilename;
                    connection.Open();

                    using (SQLiteCommand command = new SQLiteCommand(connection))
                    {
                        command.CommandText = @"CREATE TABLE [Attempts] 
                                             ([Id] INTEGER PRIMARY KEY, 
                                              [DateAndTimeBegin] nvarchar, 
                                              [DateAndTimeEnd] nvarchar, 
                                              [CardPassing_Id] int, 
                                              [AnswerCard_Id] int, 
                                              [AskedCard_Id] int, 
                                              FOREIGN KEY (CardPassing_Id) REFERENCES [CardPassings](Id), 
                                              FOREIGN KEY (AnswerCard_Id) REFERENCES [Cards](Id), 
                                              FOREIGN KEY (AskedCard_Id) REFERENCES [Cards](Id))";
                        command.CommandType = CommandType.Text;
                        command.ExecuteNonQuery();
                    }
                }
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

