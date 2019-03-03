﻿using System.Collections.Generic;

namespace VanyaGame.Abstract
{

    public interface IComponentContainer
    {
        Dictionary<string,IComponent> Components { get; set; }

        T GetComponent<T>() where T : IComponent;
        IComponent GetComponent(string name);
    }

}
