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

namespace CardsEditor.DB
{
    public class DBTools
    {
        public static bool LoadDB(VM vm, ObservableCollection<Card> _cards, ObservableCollection<Level> _levels, Context context)
        {
            bool error = false;
            try
            {
                _cards = new ObservableCollection<Card>();
                _levels = new ObservableCollection<Level>();
                context = new Context();

                IEnumerable<Level> levels = context.Levels.Include(p => p.Cards).ToList();
                IEnumerable<Card> cards = context.Cards.ToList();

                foreach (Card c in cards)
                    _cards.Add(c);

                foreach (Level t in levels)
                    _levels.Add(t);

                vm.init(_cards, _levels, context);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                error = true;
            }
            return !error;
        }

    }
}
