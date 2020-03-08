using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Threading;
using VanyaGame.GameNumber.Units;
using VanyaGame.Struct;
using VanyaGame.Struct.Components;
using VanyaGame.Units;
using VanyaGame.Units.Components;

namespace VanyaGame.GameNumberDB.Struct
{
    public class NumberIteration: Iteration
    {
        public NumberIteration(Scene _Scene): base(new IterationSets(), _Scene, "Iteration1")///ЗАТЫЧКА получилась
        {
            UnitsCollection<Number> units = new UnitsCollection<Number>("Units", this);
        }

        //private Iteration _Iteration;


        ///// <summary>
        ///// УБРАТЬ!!!!!!!!!!
        ///// </summary>
        //public bool IsCanPlay;


        //DispatcherTimer dispatcherTimer;




        //private void DispatcherTimerTick(object sender, EventArgs e)
        //{
        //    UserDoSomething(null, null, null);
        //}
        //public override void UserDoSomething (MouseEventArgs mouse, MouseButtonEventArgs mousebutton, KeyEventArgs key)
        //{
        //    if (key != null)
        //    {
        //        HitCheck(key);
        //    }
        //}

        //public override void RegisterBehaviour(Iteration Iteration, List<Unit> ObjectsToUserControl, List<Unit> TargetObjects)
        //{
        //    _Iteration = Iteration;
        //    _ObjectsToUserControl = ObjectsToUserControl;
        //    _TargetObjects = TargetObjects;
        //}


        //public void HitCheck(KeyEventArgs key)
        //{
        //    foreach (Unit U in AllUnits)
        //    {
        //        string s;
        //        s = key.Key.ToString();
        //        Game.Msg(s);
        //        if (s.Length > 1)
        //        {
        //            s = s.Replace("D", "");
        //            s = s.Replace("NumPad", "");
        //            Game.Msg(s);
        //        }
        //        if (U.GetComponent<CheckedSymbol>().IsPrintedSimbolMatch(s))
        //        {
        //            U.GetComponent<Hit>().IsHited = true;
        //            U.GetComponent<HiderShower>().Complete += () => { };
        //            U.GetComponent<HiderShower>().Show(0, TimeSpan.FromSeconds(1), new Thickness(0), TimeSpan.FromSeconds(1));

        //            ((Number)U).RemoveToBox(() => { ToolsTimer.Delay(() => { IterationEnd(); }, TimeSpan.FromSeconds(1)); });
        //            return;
        //        }
        //    }

        //    foreach (Unit U in AllUnits)
        //        if (!U.GetComponent<Hit>().IsHited)
        //            IsAllUnitsCathed = false;

        //    if (IsAllUnitsCathed)
        //        ToolsTimer.Delay(() => { IterationEnd(); }, TimeSpan.FromSeconds(3));

        //}






        //public override void IterationBegin()
        //{
        //    Game.HasCursor = true;
        //    FirstIterationBegin();
        //    SceneSets SLSets = this._Iteration.Scene.Sets;

        //    Tools.CreateAndStart_DispatcherTimer(ref dispatcherTimer, DispatcherTimerTick, new TimeSpan(0, 0, 0, 0, 200));

        //    if (_Iteration.IterationNumber == 1)
        //    {

        //        TDrawEffects.MoveTo(Game.Owner.Label1, 10, 10, 0.4, () =>
        //        {
        //            IsCanPlay = true;
        //        });
        //        PaperBoxes = new List<PaperGrid>();
        //        foreach (Unit U in AllSceneUnits)
        //        {
        //            PaperGrid PaperGrd = new PaperGrid();
        //            PaperGrd.Width = 120;
        //            PaperGrd.Height = 120;
        //            Game.Owner.StackPanelGame.Children.Add(PaperGrd);
        //            PaperBoxes.Add(new PaperGrid());
        //            ((Number)U).Box = PaperGrd.PaperGird;

        //        }
        //        foreach (Unit U in AllUnits)
        //        {
        //            Number F = (Number)U;
        //            double x = (-200) + _Iteration.Scene.Sets.UnitAppearenceZone.Left + _Iteration.Scene.Sets.UnitAppearenceZone.Width / 2;
        //            double y = (-300) + _Iteration.Scene.Sets.UnitAppearenceZone.Top + _Iteration.Scene.Sets.UnitAppearenceZone.Height / 2;
        //            F.GetComponent<Moveable>().MoveTo(x, y);
        //        }

        //    }
        //    else
        //    {
        //        if (_Iteration.IterationNumber > 1)
        //        {
        //            foreach (Unit U in NewUnits)
        //            {
        //                Number F = (Number)U;
        //                double x = (-200) + _Iteration.Scene.Sets.UnitAppearenceZone.Left + _Iteration.Scene.Sets.UnitAppearenceZone.Width / 2;
        //                double y = (-300) + _Iteration.Scene.Sets.UnitAppearenceZone.Top + _Iteration.Scene.Sets.UnitAppearenceZone.Height / 2;
        //                F.GetComponent<Moveable>().MoveTo(x, y);
        //            }
        //        }
        //    }

        //    Game.Msg("IterationBegin");

        //}

        //public override void IterationEnd()
        //{
        //    Game.HasCursor = true;
        //    NumberCreatorOfObjects Creator = new NumberCreatorOfObjects();
        //    Creator.CreateNextIteration(ref _Iteration.Scene.Iterations, _Iteration.Scene, this._Iteration, 1);
        //    Tools.StopAndNull_DispatcherTimer(ref dispatcherTimer);
        //    Game.UserActivity.UserDoSomethingEvent -= UserDoSomething;
        //    _Iteration.Scene.NextIteration();
        //    //IsCanPlay = true;
        //    //TDrawEffects.MoveTo(Game.Owner.Label1, 10, 10, 0.1, () => {
        //    //    IsCanPlay = true;
        //    //});
        //    if (_Iteration.IsLastIteration)
        //    {

        //    }


        //}
        //private void FirstIterationBegin()
        //{
        //    //НЕ ДОЛЖНО БЫТЬ ЭТОГО МЕТОДА ТУТ АЙ-ЯЙ-ЯЙ
        //    //tree.ShowMethod();
        //    //basketUp.ShowMethod();
        //    //Panel.SetZIndex(basketUp, 7);
        //    //basketDown.ShowMethod();
        //    //Panel.SetZIndex(basketDown, 0);
        //    //basketDown.ShowMethod();
        //    //Panel.SetZIndex(basketDown, 0);


        //}
    }
}
