using System;
using VGameCore.Abstract;
using VGameCore.Struct.Components;

namespace VGameCore.Struct
{
    
    public abstract class Scene: StructComponentContainer
    {
        
        protected Level Level;  //Родительский уровень 
        public string Name { get; private set; }
        public SceneSets Sets { get; protected set; }

        public Scene(Level level, string name)
        {
            Level = level;
            Name = name;
            State state = new State("State", this);
            Loader loader = new Loader("Loader", this);
            Starter starter = new Starter("Starter", this);

        }

        public virtual void Abort()
        { 
        
        }
    }
}
