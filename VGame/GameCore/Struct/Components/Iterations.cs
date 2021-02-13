using System.Collections.Generic;
using VanyaGame.Abstract;

namespace VanyaGame.Struct.Components
{
    public class Iterations : Component
    {
        #region constructors
        public Iterations(string name, IComponentContainer container) : base(name, container)
        {
        }
        #endregion

        #region variables
        #endregion

        #region properties
        public List<Iteration> Val { get; set; }
        public Iteration CurIteration { get; set; }
        #endregion

        #region methods
        #endregion

    }
}
