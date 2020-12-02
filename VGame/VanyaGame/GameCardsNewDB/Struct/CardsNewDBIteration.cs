using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Threading;
using VanyaGame.GameCardsNewDB.Units;
using VanyaGame.Struct;
using VanyaGame.Struct.Components;
using VanyaGame.Units;
using VanyaGame.Units.Components;

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
