using VGameCore.Abstract;

namespace VGameCore.Units.Components
{
    /// <summary>
    /// Определяет "попадание" в юнит. В принципе здесь нужно только свойство - сейчас юнит "прострелен" или нет. Также может использоваться как маркер
    /// любой ситуации где схожая логика (например - правильная цифра нажата для юнита или нет в связке с CheckedSymbol)
    /// </summary>
    public class Hit : Component
    {
        #region constructors

        public Hit(string name, IComponentContainer container) : base(name, container)
        {
            IsHited = false;
        }

        public Hit(string name, IComponentContainer container, bool isHited) : this(name, container)
        {
            IsHited = isHited;
        }
        #endregion

        #region variables
        #endregion

        #region properties
        public bool IsHited { get; set; }
        #endregion

        #region methods
        #endregion

    }
}
