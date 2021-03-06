﻿using System;
using System.Collections.Generic;
using VGameCore.Abstract;
using VGameCore.Struct.Components;

namespace VGameCore.Struct
{



    public abstract class Level: StructComponentContainer 
    {
        public LevelSets Sets { get; protected set; }
        public string Name { get; private set; }
        public Dictionary<string, Scene> Scenes { get; protected set; }
        public Scene CurScene { get; protected set; }
        public bool IsAborted { get; set; }

        public Level()
        {
            Scenes = new Dictionary<string, Scene>();
            State state = new State("State", this);
            Loader loader = new Loader("Loader", this);
            Starter starter = new Starter("Starter", this);
        }

        public void AddScene(string sceneName, Scene scene)
        {
            Scenes.Add(sceneName, scene);
        }

        public virtual void Abort()
        { 
            
        }

    }
}
