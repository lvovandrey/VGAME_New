using System;

namespace VanyaGame.Abstract
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

        public void Remove(string name, IComponentContainer container)
        {
            Container.Components.Remove(name);
        }

        public void Dispose()
        {
            
        }
    }
}
