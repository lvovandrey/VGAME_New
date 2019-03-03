using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VanyaGame.Units
{

    public interface IComponentContainer
    {
        Dictionary<string,IComponent> Components { get; set; }

        T GetComponent<T>() where T : IComponent;
        IComponent GetComponent(string name);
    }

}
