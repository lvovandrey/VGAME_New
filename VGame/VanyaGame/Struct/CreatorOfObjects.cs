using System.Collections.Generic;
using VanyaGame.Struct;
using VanyaGame.Units;

namespace VanyaGame.Behaviour
{

    /// <summary>
    /// Конкретный класс в котором создаются объекты на отображение и заполняются очереди итераций (в общем конкретная работа по субуровню)
    /// </summary>
    public abstract class CreatorOfObjects
    {
        int IterationNumber { get; set; }

        protected CreatorOfObjects() : base()
        {

        }

        /// <summary>
        /// Конкретный метод создания объекта на отображение и вставки его в список объектов на отображение. В данном случае создаются пары объектов Unit и Shadow.
        /// При этом Shadow аггрегирован (упакован по ссылке) в объект Unit.
        /// </summary>
        /// <param name="Objects">Список объектов на отображение в который нужно вставить созданный объект</param>
        /// <param name="filepath">Путь к файлу картинки, которая будет отображаться</param>
        /// <param name="_Scene">Экземпляр субуровня (TScene) в котором находится список объектов на отображение</param>
        public abstract void CreateUnitAndShadow(ref List<Unit> Objects, string filepath, Scene _Scene); //вариант метода создания объекта на отображение

        public abstract void CreateNextIteration ///CreateFirstIteration32132132
            (ref Queue<Iteration> Iterations, Scene Scene, Iteration PrevIteration, int curIterationNumber);

        /// <summary>
        /// Создает очередь из итераций (конкретным способом - 1 юнит и сколько в настройках субуровня теней)
        /// </summary>
        /// <param name="Iterations">Ссылка на очередь итераций которую надо заполнить</param>
        /// <param name="Scene">Ссылка на субуровень где эта очердь находится</param>
        public abstract void AutoCreateIterations(ref Queue<Iteration> Iterations, Scene Scene);
    }
}
