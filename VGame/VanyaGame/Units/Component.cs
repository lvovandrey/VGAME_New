using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VanyaGame.Units
{
    public abstract class Component: IComponent, IDisposable
    {
        public string Name { get; set; }
        public IComponentContainer Container { get; set; }

        public Component(string name, IComponentContainer container)
        {
            Name = name;
            Container = container;
            Container.Components.Add(name, this);
        }

        public void Dispose()
        {
            
        }
    }
}
