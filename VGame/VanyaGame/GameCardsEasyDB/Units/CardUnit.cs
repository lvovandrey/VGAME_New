using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using VanyaGame.Units.Components;
using VanyaGame.Struct;
using System.IO;
using VanyaGame.DB.DBCardsRepositoryModel;
using VanyaGame.GameCardsEasyDB.Units.Components;

namespace VanyaGame.GameCardsEasyDB.Units
{
    public enum CardUnitState { Hidden, ShowingOnGrid, Showed, ShowingInStack, InStack };



    public class CardUnit : VanyaGame.Units.Unit
    {
        public Panel Box { get; set; }
        public Card Card { get; set; }
        public event Action MouseClicked;
        public bool readyToReactionOnMouseDown= false;

        public CardUnit(Scene Scene, Card card)
        {
            Card = card;

            HaveBody B;
            HaveBox HB;
            CheckedSymbol ChS;
            DragAndDrop DaD;
            Moveable M;
            HiderShower ShowComp;
            CardShower cardShower;
            OLDInGameStruct InGS;
            UState uState;
            Hit hit;

            B = new HaveBody("HaveBody", this, new CardUnitElement());

            if (System.IO.File.Exists(card.ImageAddress))
            {
                ((CardUnitElement)B.Body).Img.Source = new BitmapImage(new Uri(card.ImageAddress));
                ((CardUnitElement)B.Body).Img.Width = 150;
            }
            else
            {
                MessageBox.Show("Файл изображения не найден:  " + card.ImageAddress);
            }

            HB = new HaveBox("HaveBox", Game.Owner, Game.Owner.WrapPanelMain, this);
            M = new Moveable("Moveable", this);


            DaD = new DragAndDrop("DragAndDrop", this);
            ShowComp = new HiderShower("HiderShower", this);
            cardShower = new CardShower("CardShower", this);
            InGS = new OLDInGameStruct("InGameStruct", this, Scene);
            hit = new Hit("Hit", this);
            uState = new UState("UState", this);
            uState.newOld = NewOld.New;
            ShowComp.Hide();

            B.Body.PreviewMouseLeftButtonDown += Body_PreviewMouseLeftButtonDown;
        }

        private void Body_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (readyToReactionOnMouseDown)
            {
                this.GetComponent<Hit>().IsHited = true;
                MouseClicked?.Invoke();
            }
        }

        
        ///// <summary>
        ///// Юнит исчезает из поля и появляется в Box. По завершении вызывается метод Complete
        ///// </summary>
        ///// <param name="Complete"></param>
        //public void RemoveToBox(VoidDelegate Complete)
        //{
        //    if (Box == null)
        //        throw new Exception("In game Unit Number inner Box (Panel) is not initialized. See NumberUnit module");
        //    CardShower HS = this.GetComponent<CardShower>();
        //    // HS.Complete += () =>
        //    HS.Hide(() =>
        //    {
        //        this.GetComponent<HaveBox>().AddInBox(Box);
        //        Panel.SetZIndex(Box, 0);
        //        HS.Show(() => { Complete(); });
        //    });

        //}



    }
}
