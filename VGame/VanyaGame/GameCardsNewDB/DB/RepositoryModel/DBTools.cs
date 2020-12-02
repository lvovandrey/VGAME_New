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
        public static Context Context;

        //позорный костыль для загрузки БД - так и не разобрался почему коллекция после выхода из статического метода не изменяется. а внутри меняется вроде.
        public void init(ObservableCollection<Level> levels, ObservableCollection<Card> cards, Context context)
        {
            Levels = levels;
            Cards = cards;
            Context = context;
        }

        public static bool LoadDB(ObservableCollection<Card> _cards, ObservableCollection<Level> _levels, string AttachDbFilename)
        {
            bool error = false;
            try
            {
                _cards = new ObservableCollection<Card>();
                _levels = new ObservableCollection<Level>();
                Context = new Context(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + AttachDbFilename + @";Integrated Security=True;Connect Timeout=30");

                IEnumerable<Level> levels = Context.Levels.Include(p => p.Cards).ToList();
                IEnumerable<Card> cards = Context.Cards.ToList();

                foreach (Card c in cards)
                    _cards.Add(c);

                foreach (Level t in levels)
                    _levels.Add(t);

               new DBTools().init( _levels, _cards, Context);
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
