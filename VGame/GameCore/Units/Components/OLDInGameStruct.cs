﻿using VGameCore.Abstract;
using VGameCore.Struct;

namespace VGameCore.Units.Components
{
    public class OLDInGameStruct: Component
    {
        #region constructors
        public OLDInGameStruct(string name, IComponentContainer container, Scene Scene) : base(name, container)
        {
            this.Scene = Scene;
        }
        #endregion

        #region variables
        #endregion

        #region properties
        public Scene Scene { get; set; }   // родительский субуровень
        #endregion

        #region methods
        #endregion

    }
}
