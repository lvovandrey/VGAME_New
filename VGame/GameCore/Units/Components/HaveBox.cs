using System;
using System.Windows;
using System.Windows.Controls;
using VGameCore.Abstract;

namespace VGameCore.Units.Components
{
    /// <summary>
    /// Определяет UI-контейнер в который может помещаться элемент
    /// Внутри него обязательно должен находиться UI типа Panel, на который будут добавляться
    /// упаковывающиеся элементы
    /// </summary>
    public class HaveBox: Component
    {
        public FrameworkElement Element { get; set; }
        public Panel InnerBox { get; set; }

       

        public HaveBox(string name, IComponentContainer container) : base(name, container)
        {
            if (Container.GetComponent<HaveBody>() == null)
                throw new Exception("Have no component HaveBody in contaier" );
        }
        /// <summary>
        /// Создает UI-контейнер
        /// </summary>
        /// <param name="element">Весь UI контейнера </param>
        /// <param name="box">Внутренний копмоновочный контейнер типа Panel, куда собственно добавляет нужные элементы </param>
        public HaveBox(string name, FrameworkElement element, Panel box, IComponentContainer container) :this(name, container)
        {
            Element = element;
            InnerBox = box;
            this.AddInBox(Element, InnerBox);
        }

        /// <summary>
        /// Добавить элемент в визуальный контейнер
        /// </summary>
        /// <param name="element"></param>
        /// <param name="box"></param>
        /// <param name="innerElement"></param>
        private void AddInBox(FrameworkElement element, Panel box, FrameworkElement innerElement)
        {
            Element = element;
            InnerBox = box;
            if (innerElement.Parent != null)
                if (innerElement.Parent is Panel)
                    ((Panel)innerElement.Parent).Children.Remove(innerElement);

            InnerBox.Children.Add(innerElement);
        }

        /// <summary>
        /// Добавить элемент в визуальный контейнер
        /// </summary>
        /// <param name="element">Весь UI контейнера </param>
        /// <param name="box">Внутренний копмоновочный контейнер типа Panel, куда собственно добавляет нужные элементы </param>
        public void AddInBox(FrameworkElement element, Panel box)
        {
            FrameworkElement innerElement = Container.GetComponent<HaveBody>().Body;
            this.AddInBox(element, box, innerElement);
        }

        /// <summary>
        /// Добавить элемент в визуальный контейнер
        /// </summary>
        /// <param name="box">Внутренний копмоновочный контейнер типа Panel, куда собственно добавляет нужные элементы </param>
        public void AddInBox(Panel box)
        {
            FrameworkElement innerElement = Container.GetComponent<HaveBody>().Body;
            if (((FrameworkElement)box.Parent != null) )// && (box.Parent is Panel))
            {
                this.AddInBox((FrameworkElement)box.Parent, box, innerElement);
            }
            else
            {
                this.AddInBox(null, box, innerElement);
            }
        }

        /// <summary>
        /// Удалить элемент из текущего визуального контейнера
        /// </summary>
        public void RemoveFromBox()
        {
            InnerBox.Children.Remove(Container.GetComponent<HaveBody>().Body);
        }



    }
}
