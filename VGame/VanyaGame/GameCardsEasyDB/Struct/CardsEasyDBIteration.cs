﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Threading;
using VanyaGame.GameNumber.Units;
using VanyaGame.Struct;
using VanyaGame.Struct.Components;
using VanyaGame.Units;
using VanyaGame.Units.Components;

namespace VanyaGame.GameCardsEasyDB.Struct
{
    public class CardsEasyDBIteration: Iteration
    {
        public CardsEasyDBIteration(Scene _Scene): base(new IterationSets(), _Scene, "Iteration1")///ЗАТЫЧКА получилась
        {
            UnitsCollection<Number> units = new UnitsCollection<Number>("Units", this);
        }
    }
}
