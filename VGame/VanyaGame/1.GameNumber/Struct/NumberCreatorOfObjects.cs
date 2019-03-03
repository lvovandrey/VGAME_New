using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using VanyaGame.Behaviour;
using VanyaGame.GameNumber.Units;
using VanyaGame.GameNumber.Struct;
using VanyaGame.Units;
using VanyaGame.Units.Components;
using VanyaGame.Struct;

namespace VanyaGame.GameNumber.Behaviour
{

    /// <summary>
    /// Конкретный класс в котором создаются объекты на отображение и заполняются очереди итераций (в общем конкретная работа по субуровню)
    /// </summary>
    public class NumberCreatorOfObjects : CreatorOfObjects
    {
       public int IterationNumber { get; set; }

       public  NumberCreatorOfObjects() : base()
        {
            IterationNumber = 0;
        }
        

        /// <summary>
        /// Конкретный метод создания объекта на отображение и вставки его в список объектов на отображение. В данном случае создаются пары объектов Unit и Shadow.
        /// При этом Shadow аггрегирован (упакован по ссылке) в объект Unit.
        /// </summary>
        /// <param name="Objects">Список объектов на отображение в который нужно вставить созданный объект</param>
        /// <param name="filepath">Путь к файлу картинки, которая будет отображаться</param>
        /// <param name="_Scene">Экземпляр субуровня (TScene) в котором находится список объектов на отображение</param>
        public override void CreateUnitAndShadow(ref List<Unit> Objects, string filepath, Scene _Scene)
        {
            //Number Obj = new Number(filepath, _Scene);
            //Objects.Add(Obj);
        }

        public override void CreateNextIteration ///CreateFirstIteration32132132
            (ref Queue<Iteration> Iterations, Scene Scene, Iteration PrevIteration, int curIterationNumber)
        {
            //IterationNumber=curIterationNumber+1;
            //NumberIteration It = new NumberIteration(Scene);
            //It.IterationNumber = IterationNumber;
            //It.Behaviour = new NumberIterationBehaviour();


            //It.Behaviour.AllUnits = new List<Unit>();
            //It.Behaviour.AllSceneUnits = new List<Unit>();
            //It.Behaviour.NewUnits = new List<Unit>();


            //List<Unit> Units = new List<Unit>();
            //for (int i = 0; i < Scene.Objects_ToShow.Count; i++)
            //{
            //    Unit O = Scene.Objects_ToShow[i];

            //    if (i > 1)
            //    {
            //        if (O.GetType().Name.ToString() == "Number")
            //        {
            //            Panel.SetZIndex((O.GetComponent<HaveBody>().Body), 3);
            //            Units.Add(O);
            //        }
            //    }
            //}

            //////////////////////// Много явных преобразований типов - плохой код значит! ПЕРЕДЕЛАТЬ!

            ////var KeyBoards = from O in Scene.Objects_ToShow // Вот решил попробовать LINQ
            ////                where O.type == "KeyBoard"
            ////                select O;
            ////if (KeyBoards.Count() > 0)
            ////{
            ////    ((NumberIterationBehaviour)(It.Behaviour)).Keyboard = ((GameNumber.Interface.Keyboard)KeyBoards.First());
            ////}

            //////////////////////////////

            ////Собрали все юниты которые есть в подуровне
            //foreach (Unit O in Scene.Objects_ToShow)
            //{
            //    if (O.GetType().Name.ToString() == "Number")
            //    {
            //        It.Behaviour.AllSceneUnits.Add(O);
            //    }
            //}

            ////Определяем набор юнитов участвующих в этой итерации - AllUnits
            //foreach (Unit O in Scene.Objects_ToShow)
            //{
            //    if ((O.GetType().Name.ToString() == "Number") && (It.Behaviour.AllUnits.Count < Scene.Sets.CountUnitsTogether))
            //    {
            //        if (!O.GetComponent<Hit>().IsHited)
            //            It.Behaviour.AllUnits.Add(O);
            //    }
            //}

            ////Поиск новых юнитов     
            //foreach (Unit O in It.Behaviour.AllUnits)
            //{
            //    bool flag_new = true;

            //    if (PrevIteration != null)
            //        foreach (Unit O_old in PrevIteration.Behaviour.AllUnits)
            //        {
            //            bool isEqual = O.Equals(O_old);
            //            if (isEqual)
            //            { flag_new = false; break; }
            //        }
            //    if (flag_new)
            //        It.Behaviour.NewUnits.Add((Unit)O);
            //}


            //List<Unit> ControlObjects = new List<Unit>();

            //foreach (Unit O in ControlObjects)
            //    It.ToShow.Add(O);

            //List<Unit> TargetObjects = new List<Unit>();
            //foreach (Unit U in It.Behaviour.AllUnits)
            //{
            //    TargetObjects.Add(U);
            //    It.ToShow.Add(U);
            //}

            //It.Behaviour.RegisterBehaviour(It, ControlObjects, TargetObjects);

            //It.Behaviour.PrevIteration = PrevIteration;
            //if (It.Behaviour.AllUnits.Count == 0)
            //{
            //    It.IsLastIteration = true;
            //}
            //else
            //{
            //    Iterations.Enqueue(It);
            //}
        }

        /// <summary>
        /// Создает очередь из итераций (конкретным способом - 1 юнит и сколько в настройках субуровня теней)
        /// </summary>
        /// <param name="Iterations">Ссылка на очередь итераций которую надо заполнить</param>
        /// <param name="Scene">Ссылка на субуровень где эта очердь находится</param>
        public override void AutoCreateIterations(ref Queue<Iteration> Iterations, Scene Scene)
        {
            //Iterations.Clear();
            //IterationNumber = 0; //дублирующийся код в Scene
            //CreateNextIteration(ref Iterations, Scene, null,0);
        }


    }
}
