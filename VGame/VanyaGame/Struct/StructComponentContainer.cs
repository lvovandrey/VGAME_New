using System;
using System.Collections.Generic;
using VanyaGame.Abstract;

namespace VanyaGame.Struct
{
    public class StructComponentContainer : IComponentContainer, IDisposable
    {
        public virtual Dictionary<string, IComponent> Components { get; set; }

        public StructComponentContainer()
        {
            Components = new Dictionary<string, IComponent>();

        }
        public StructComponentContainer(Dictionary<string, IComponent> components)
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
