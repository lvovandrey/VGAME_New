using CardsEditor.Model;

using LevelSetsEditor.DB;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using LevelSetsEditor.ViewModel;

namespace CardsEditor.DB
{
    public class DBTools
    {
        public static bool LoadDB(VM vm, ObservableCollection<Card> _cards, ObservableCollection<Tag> _tags, ContextCards context)
        {
            bool error = false;
            try
            {
                _cards = new ObservableCollection<Card>();
                _tags = new ObservableCollection<Tag>();
                context = new ContextCards();

                IEnumerable<Tag> tags = context.Tags.Include(p => p.Cards).ToList();

                IEnumerable<Card> cards = context.Cards.Include(p => p.Tags).ToList();

                foreach (Card c in cards)
                    _cards.Add(c);

                foreach (Tag t in tags)
                    _tags.Add(t);

                vm.initCards(_cards,_tags, context);
            }
            catch
            {
                error = true;
            }
            return !error;
        }

    }
}
