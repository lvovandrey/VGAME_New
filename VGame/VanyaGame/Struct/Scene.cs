using System;
using VanyaGame.Abstract;
using VanyaGame.Struct.Components;

namespace VanyaGame.Struct
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

    }
}
