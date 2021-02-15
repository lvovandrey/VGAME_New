using System;
using VGameCore.Abstract;

namespace VGameCore.Struct.Components
{
    /// <summary>
    /// Определяет методы загрузки настроек и содержимого. Абстрактный загрузчик "уровня"
    /// </summary>
    public class Loader : Component
    {
        #region constructors
        public Loader(string name, IComponentContainer container) : base(name, container)
        {
            if (Container.GetComponent<State>() == null)
                throw new Exception("Have no component State in contaier");
        }
        #endregion

        #region variables
        #endregion

        #region properties
        /// <summary>
        /// Загрузка настроек
        /// </summary>
        public Action LoadSets { get; set; }

        /// <summary>
        /// Загрузка элементов (контента, изображений и проч)
        /// </summary>
        public Action LoadContent { get; set; }

        #endregion

        #region methods
        /// <summary>
        /// Шаблонный метод загрузки 
        /// </summary>
        public virtual void Load() //ПОХОЖЕ НА ПАТТЕРН ШАБЛОННЫЙ МЕТОД
        {
            if ((LoadSets == null)||(LoadContent == null))
                throw new Exception("Have no Load metods in Loader-compontent");
            LoadSets();
            LoadContent();
            Container.GetComponent<State>().value = StructState.Loaded;
        }
        #endregion

    }
}
