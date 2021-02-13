using VGameCore.Abstract;

namespace VGameCore.Units.Components
{
    public enum UnitType {Number }

    /// <summary>
    /// Информация о контейнере компонента (например о юните)
    /// </summary>
    class UInfo: Component
    {
        #region constructors
        public UInfo(string name, IComponentContainer container) : base(name, container)
        {
        }
        public UInfo(string name, IComponentContainer container, UnitType type, string UnitName) : this(name, container)
        {
            Type = type;
            this.UnitName = UnitName;
        }
        #endregion

        #region variables
        #endregion

        #region properties
        public UnitType Type { get; set; }
        public string UnitName { get; set; }
        #endregion

        #region methods
        #endregion
    }
}
