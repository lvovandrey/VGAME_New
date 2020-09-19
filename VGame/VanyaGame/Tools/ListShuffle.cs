using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VanyaGame.ToolsShuffle
{
    public  class ListShuffle<T>
    {
        public List<T> Shuffle(List<T> list)
        {
            List<T> newList = new List<T>(list);
            for (int x = 0; x < 1000; x++)
            {
                int i = Game.RandomGenerator.Next(0, newList.Count);
                int j = Game.RandomGenerator.Next(0, newList.Count);
                var temp = newList[j];
                newList[j] = newList[i];
                newList[i] = temp;
            }

            return newList;
        }
    }
}
