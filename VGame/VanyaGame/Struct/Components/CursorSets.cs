using System;
using System.Windows.Input;
using VanyaGame.Abstract;

namespace VanyaGame.Struct.Components
{
    /// <summary>
    /// Инкапсулирует информацию и методы связанные с настройкой в сцене (или уровне) курсора
    /// </summary>
    public class CursorSets : Component
    {
        #region constructors
        public CursorSets(string name, IComponentContainer container, Cursor cursor) : base(name, container)
        {
            Cursor = cursor;
        }
        #endregion

        #region variables
        #endregion

        #region properties
        /// <summary>
        /// Курсор
        /// </summary>
        public Cursor Cursor { get; set; }

        #endregion

        #region methods

        #endregion
    }
}
