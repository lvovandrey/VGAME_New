using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Threading;
using VanyaGame.GameCardsNewDB.Units;
using VGameCore.Struct;
using VGameCore.Struct.Components;
using VGameCore.Units;
using VGameCore.Units.Components;
using VGameCore.Abstract;

namespace VanyaGame.GameCardsNewDB.Struct
{
    public class CardsNewDBIteration: Iteration
    {
        public CardsNewDBIteration(Scene _Scene): base(new IterationSets(), _Scene, "Iteration1")///ЗАТЫЧКА получилась
        {
            UnitsCollection<CardUnit> units = new UnitsCollection<CardUnit>("Units", this);
        }
    }
}
