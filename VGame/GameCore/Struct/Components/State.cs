using VGameCore.Abstract;

namespace VGameCore.Struct.Components
{
    // НА БУДУЩЕЕ ПОДУМАТЬ ЧТОБЫ РЕАЛИЗОВАТЬ ПРИ НЕОБХОДИМОСТИ ТУТ ПАТТЕРН "СОСТОЯНИЕ"


    public enum StructState { Empty, Loaded, Started }

    public class State : Component
    {

        #region constructors
        public State(string name, IComponentContainer container) : base(name, container)
        {
            value = StructState.Empty;
        }
        #endregion

        #region variables
        #endregion

        #region properties
        public StructState value { get; set; }
        #endregion

        #region methods
        #endregion

    }
}
