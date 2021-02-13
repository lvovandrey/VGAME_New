using System;
using System.Collections.Generic;

namespace VanyaGame.Abstract
{
    public abstract class ComponentContainer: IComponentContainer,IDisposable
    {
        public virtual Dictionary<string, IComponent> Components { get; set; }
        public ComponentContainer()
        {
            Components = new Dictionary<string, IComponent>();

        }
        public ComponentContainer(Dictionary<string, IComponent> components)
        {
            Components = new Dictionary<string, IComponent>(components);
        }

        public virtual T GetComponent<T>() where T : IComponent
        {
            foreach (KeyValuePair<string, IComponent> C in Components)
            {
                if (C.Value is T)
                    return (T)(C.Value);
            }
            return (T)(IComponent)null;
        }
        public virtual IComponent GetComponent(string name)
        {
            return Components[name];
        }

        public void Dispose()
        {
            
        }
    }
}
