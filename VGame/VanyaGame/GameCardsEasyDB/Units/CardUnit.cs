using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using VanyaGame.Units.Components;
using VanyaGame.GameNumber.Units.Components;
using VanyaGame.Struct;
using System.IO;
using VanyaGame.DB.DBCardsRepositoryModel;

namespace VanyaGame.GameCardsEasyDB.Units
{
    public enum CardUnitState { Hidden, ShowingOnGrid, Showed, ShowingInStack, InStack };



    public class CardUnit : VanyaGame.Units.Unit
    {
        public Panel Box { get; set; }
        public Card Card { get; set; }

        public CardUnit(Scene Scene, Card card)
        {
            Card = card;

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

            B = new HaveBody("HaveBody", this, new CardUnitElement());

            if (System.IO.File.Exists(card.ImageAddress))
            {
                ((CardUnitElement)B.Body).Img.Source = new BitmapImage(new Uri(card.ImageAddress));
                ((CardUnitElement)B.Body).Img.Width = 100;
            }
            else
            {
                MessageBox.Show("Файл изображения не найден:  " + card.ImageAddress);
            }

            HB = new HaveBox("HaveBox", Game.Owner, Game.Owner.GridMain, this);
            M = new Moveable("Moveable", this);


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
        public void RemoveToBox(VoidDelegate Complete)
        {
            if (Box == null)
                throw new Exception("In game Unit Number inner Box (Panel) is not initialized. See NumberUnit module");
            NumberShower HS = this.GetComponent<NumberShower>();
            // HS.Complete += () =>
            HS.Hide(() =>
            {
                this.GetComponent<HaveBox>().AddInBox(Box);
                Panel.SetZIndex(Box, 0);
                HS.Show(() => { Complete(); });
            });

        }



    }
}
