using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using VanyaGame.Units.Components;
using VanyaGame.GameNumber.Units.Components;
using VanyaGame.Struct;

namespace VanyaGame.GameNumberDB.Units
{
    public enum NumberState {Hidden, ShowingOnGrid, Showed,  ShowingInStack, InStack};
    


    public class Number : VanyaGame.Units.Unit
    {
        public Panel Box { get; set; }

        public Number(string s, Scene Scene)
        {
            
            HaveBody B;
            HaveBox HB;
            CheckedSymbol ChS;
            DragAndDrop DaD;
            Moveable M;
            HiderShower ShowComp;
            NumberShower numberShower;
            OLDInGameStruct InGS;
            UState uState;
            Hit hit;

            string path = System.IO.Path.Combine(Game.Sets.InterfaceDir, @"Numbers/units", s + ".png");

            B = new HaveBody("HaveBody", this, new NumberElement());

            if (System.IO.File.Exists(path))
            {
                ((NumberElement)B.Body).Img.Source = new BitmapImage(new Uri(@path));
                ((NumberElement)B.Body).Img.Width = 400;                
            }
            else
            {
                MessageBox.Show("Файл изображения рыбы не найден:  " + path);
            }

            HB = new HaveBox("HaveBox", Game.Owner, Game.Owner.GridMain, this);
            M = new Moveable("Moveable", this);
            

            ChS = new CheckedSymbol("CheckedSymbol", this, s);
            DaD = new DragAndDrop("DragAndDrop", this);
            ShowComp = new HiderShower("HiderShower", this);
            numberShower = new NumberShower("NumberShower", this);
            InGS = new OLDInGameStruct("InGameStruct", this, Scene);
            hit = new Hit("Hit", this);
            uState = new UState("UState", this);
            uState.newOld = NewOld.New;
            ShowComp.Hide();
        }

        /// <summary>
        /// Юнит исчезает из поля и появляется в Box. По завершении вызывается метод Complete
        /// </summary>
        /// <param name="Complete"></param>
        public void RemoveToBox( VoidDelegate Complete)
        {
            if (Box==null)
                throw new Exception("In game Unit Number inner Box (Panel) is not initialized. See NumberUnit module");
            NumberShower HS = this.GetComponent<NumberShower>();
           // HS.Complete += () =>
            HS.Hide(()=>{
                //ToolsTimer.Delay(() => 
                //{ 
                    this.GetComponent<HaveBox>().AddInBox(Box);
                    Panel.SetZIndex(Box, 0);
                   
                
                //Complete();
                HS.Show(()=> { Complete();});
                //HS.Show(1, TimeSpan.FromSeconds(1), new Thickness(0), TimeSpan.FromSeconds(0.3),20000);
                //}
                //, TimeSpan.FromSeconds(0.1));
            });

              //HS.Show(0, TimeSpan.FromSeconds(1), new Thickness(0), TimeSpan.FromSeconds(0.3));
        }



    }
}
