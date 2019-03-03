using System.Collections.Generic;
using VanyaGame.Abstract;

namespace VanyaGame.Units
{
    public class Unit : ComponentContainer
    {
        public Unit() : base()
        {
        }
        public Unit(Dictionary<string, IComponent> components) : base(components)
        {
        }

    }
}
