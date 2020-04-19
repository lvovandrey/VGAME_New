using CardsEditor.Model;
using CardsEditor.ViewModel;
using LevelSetsEditor.DB;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardsEditor.DB
{
    public class DBTools
    {
        public static bool LoadDB(VM vm, ObservableCollection<Card> _cards, Context context)
        {
            bool error = false;
            try
            {
                _cards = new ObservableCollection<Card>();

                context = new Context();

                //IEnumerable<Tag> tags = context.Tags.OfType<Tag>().Where(n => n.Id < 10000);
                //List<Tag> TagList = tags.ToList();

                //IEnumerable<TagGroup> tagGroups = context.TagGroups.OfType<TagGroup>().Where(n => n.Id < 10000);
                //List<TagGroup> TagGroupList = tagGroups.ToList();

                int cardNumber = 0;
                int cardCount = 1;
                IEnumerable<Card> cards = context.Cards.OfType<Card>().Where(n => n.Id < 10000);
                cardCount = cards.Count();

                foreach (Card c in cards)
                {
                    cardNumber++;
                    _cards.Add(c);
                }

                vm.init(_cards, context);
            }
            catch
            {
                error = true;
            }
            return !error;
        }

        //public static bool loadDB(VM vm, ObservableCollection<Level> _levels, Context context)
        //{
        //    IInfoUI emptyInfoUI = new EmptyInfoUi();
        //    return LoadDB(vm, _levels, context, emptyInfoUI);
        //}

        //private static void SetInfoUI(IInfoUI infoUI)
        //{
        //    infoUI.Clear();
        //    infoUI.Title = "Загрузка БД";
        //    infoUI.Progress = 0;
        //    infoUI.Message = "Загрузка ...%";
        //    infoUI.Show();

        //}
    }
}
