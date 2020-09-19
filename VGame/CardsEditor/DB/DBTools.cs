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
        public static bool LoadDB(VM vm, ObservableCollection<Card> _cards, ObservableCollection<Tag> _tags, Context context)
        {
            bool error = false;
            try
            {
                _cards = new ObservableCollection<Card>();
                _tags = new ObservableCollection<Tag>();
                context = new Context();

                IEnumerable<Tag> tags = context.Tags.Include(p => p.Cards).ToList();

                IEnumerable<Card> cards = context.Cards.Include(p => p.Tags).ToList();

                foreach (Card c in cards)
                    _cards.Add(c);

                foreach (Tag t in tags)
                    _tags.Add(t);

                vm.init(_cards,_tags, context);
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
