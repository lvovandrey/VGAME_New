using CardsEditor.Model;
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
        public static Context Context;

        public static bool LoadDB(string _DBFilename)
        {
            bool error = false;
            try
            {
                Context = new Context(@"Data Source=" + _DBFilename);
                
                IEnumerable<Level> levels = Context.Levels.Include(p => p.Cards).ToList();
                IEnumerable<Card> cards = Context.Cards.ToList();
                IEnumerable<LevelPassing> levelPassings = Context.LevelPassings.ToList();
                IEnumerable<CardPassing> cardPassings = Context.CardPassings.ToList();

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
    }
}
