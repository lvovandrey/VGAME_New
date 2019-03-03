using VanyaGame.Abstract;

namespace VanyaGame.Units.Components
{
    /// <summary>
    /// Представляет собой компонент, который проверяет на соответствие символ 
    /// </summary>
    public class CheckedSymbol: Component
    {
        private string _symbol = "";
        public string Symbol { get => _symbol; set => _symbol = value; }

        public CheckedSymbol(string name, IComponentContainer container, string symbol) : base(name, container)
        {
            Container = container;
            Symbol = symbol;
        }

        public bool IsPrintedSimbolMatch(string symbol)
        {
            if (symbol == Symbol)
                return true;
            else
                return false;
        }
    }

    
}
