using System.Collections.Generic;
using VGameCore.Abstract;

namespace VGameCore.Units
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
