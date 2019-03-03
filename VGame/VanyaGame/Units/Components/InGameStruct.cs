using VanyaGame.LevelsStruct;

namespace VanyaGame.Units.Components
{
    public class InGameStruct: Component
    {
        #region constructors
        public InGameStruct(string name, IComponentContainer container, SubLevel SubLevel) : base(name, container)
        {
            this.SubLevel = SubLevel;
        }
        #endregion

        #region variables
        #endregion

        #region properties
        public SubLevel SubLevel { get; set; }   // родительский субуровень
        #endregion

        #region methods
        #endregion

    }
}
