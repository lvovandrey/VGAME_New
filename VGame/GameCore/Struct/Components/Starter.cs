using System;
using System.Collections.Generic;
using VanyaGame.Abstract;

namespace VanyaGame.Struct.Components
{
    /// <summary>
    /// Определяет порядок и подключает методы для старта структурного элемента игры (уровня). Абстрактный стартер "уровня"
    /// </summary>
    public class Starter : Component
    {
        #region constructors
        public Starter(string name, IComponentContainer container) : base(name, container)
        {
            if (Container.GetComponent<State>() == null)
                throw new Exception("Have no component State in contaier");
            StartElements = new List<VoidDelegate>();
        }
        #endregion

        #region variables
        #endregion

        #region properties
        /// <summary>
        /// Набор директив для последовательного старта множества абстрактных элементов.
        /// </summary>
        public List<VoidDelegate> StartElements { get; set; }
        #endregion

        #region methods
        /// <summary>
        /// Шаблонный метод старта уровня
        /// </summary>
        public virtual void Start() //ПОХОЖЕ НА ПАТТЕРН ШАБЛОННЫЙ МЕТОД
        {
            if (StartElements.Count < 1)
                throw new Exception("Have no Starts metods in Starter-compontent");

            foreach (VoidDelegate start in StartElements)
                start();
            Container.GetComponent<State>().value = StructState.Started;
        }
        #endregion

    }
}

