using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VGameCore.Abstract;
using VGameCore.Struct.Components;

namespace VGameCore.Struct
{
    public abstract class Iteration : StructComponentContainer
    {

        protected Scene Scene;  //Родительская сцена 
        public string Name { get; private set; }
        public IterationSets Sets { get; private set; }

        public Iteration(IterationSets sets, Scene scene, string name)
        {
            Sets = sets;
            Scene = scene;
            Name = name;
            State state = new State("State", this);
            Loader loader = new Loader("Loader", this);
            Starter starter = new Starter("Starter", this);
        }

    }
}
