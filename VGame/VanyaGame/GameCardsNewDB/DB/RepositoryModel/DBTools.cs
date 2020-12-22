using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Data.Entity;
using VanyaGame.GameCardsNewDB.DB.RepositoryModel;
using VanyaGame.DB;

namespace VanyaGame.GameCardsNewDB.DB
{
    public class DBTools
    {
        public static ObservableCollection<Level> Levels = new ObservableCollection<Level>();
        public static ObservableCollection<Card> Cards = new ObservableCollection<Card>();
        public static ObservableCollection<LevelPassing> LevelPassings = new ObservableCollection<LevelPassing>();
        public static Context Context;

        //позорный костыль для загрузки БД - так и не разобрался почему коллекция после выхода из статического метода не изменяется. а внутри меняется вроде.
        public void init(ObservableCollection<Level> levels, ObservableCollection<Card> cards, ObservableCollection<LevelPassing> levelPassings, Context context)
        {
            Levels = levels;
            Cards = cards;
            LevelPassings = levelPassings;
            Context = context;
        }

        public static bool LoadDB(ObservableCollection<Card> _cards, ObservableCollection<Level> _levels, ObservableCollection<LevelPassing> _levelPassings, string AttachDbFilename)
        {
            bool error = false;
            try
            {
                _cards = new ObservableCollection<Card>();
                _levels = new ObservableCollection<Level>();
                _levelPassings = new ObservableCollection<LevelPassing>();
                Context = new Context(@"Data Source=" + AttachDbFilename);

                IEnumerable<Level> levels = Context.Levels.Include(p => p.Cards).ToList();
                IEnumerable<Card> cards = Context.Cards.ToList();
                IEnumerable<LevelPassing> levelPassings = Context.LevelPassings.ToList();
                IEnumerable<CardPassing> cardPassings = Context.CardPassings.ToList();


                foreach (Card c in cards)
                    _cards.Add(c);

                foreach (LevelPassing l in levelPassings)
                    _levelPassings.Add(l);


                foreach (Level t in levels)
                {
                    if (t.LevelPassings == null) t.LevelPassings = new ObservableCollection<LevelPassing>();
                    _levels.Add(t);
                }

                new DBTools().init( _levels, _cards, _levelPassings, Context);
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
