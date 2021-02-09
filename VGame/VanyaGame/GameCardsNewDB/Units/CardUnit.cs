using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using VanyaGame.Units.Components;
using VanyaGame.Struct;
using System.IO;
using VanyaGame.GameCardsNewDB.Units.Components;
using VanyaGame.Tools;
using CardsGameNewDBRepository.Model;
using WpfAnimatedGif;

namespace VanyaGame.GameCardsNewDB.Units
{
    public enum CardUnitState { Hidden, ShowingOnGrid, Showed, ShowingInStack, InStack };



    public class CardUnit : VanyaGame.Units.Unit
    {
        public Panel Box { get; set; }
        public Card Card { get; set; }
        public Scene Scene { get; set; }
        public event Action MouseClicked;
        public bool readyToReactionOnMouseDown = false;

        private ImageAnimationController GifController;


        public CardUnit(Scene scene, Card card, double size) : this(scene, card)
        {
            ((CardUnitElement)this.GetComponent<HaveBody>().Body).Width = size;
            ((CardUnitElement)this.GetComponent<HaveBody>().Body).Height = size;
            ((CardUnitElement)this.GetComponent<HaveBody>().Body).CardUnit = this;
        }

        private CardUnit(Scene scene, Card card)
        {
            Card = card;
            Scene = scene;

            HaveBody B;
            HaveBox haveBox;
            VideoInCard videoInCard;
            CheckedSymbol ChS;
            DragAndDrop DaD;
            Moveable M;
            HiderShower ShowComp;
            CardShower cardShower;
            OLDInGameStruct InGS;
            UState uState;
            Hit hit;

            B = new HaveBody("HaveBody", this, new CardUnitElement());
            videoInCard = new VideoInCard("VideoInCard", this, ((CardUnitElement)B.Body).ContentGrid);

            if (System.IO.File.Exists(card.ImageAddress) || Miscellanea.UrlExists(card.ImageAddress))
            {
                if (Path.GetExtension(card.ImageAddress) == ".gif")
                {
                    var image = new BitmapImage();
                    image.BeginInit();
                    image.UriSource = new Uri(card.ImageAddress);
                    image.EndInit();
                    ImageBehavior.SetAnimatedSource(((CardUnitElement)B.Body).Img, image);
                    GifController = ImageBehavior.GetAnimationController(((CardUnitElement)B.Body).Img);
                }
                else if (Path.GetExtension(card.ImageAddress) == ".wmv")
                {
                    ((CardUnitElement)B.Body).Img.Visibility = System.Windows.Visibility.Collapsed;
                    videoInCard.Run(card.ImageAddress);
                }
                else
                    ((CardUnitElement)B.Body).Img.Source = PictHelper.GetBitmapImage(new Uri(card.ImageAddress));
            }
            else
            {
                if (System.IO.File.Exists(Sets.Settings.GetInstance().DefaultImage))
                {
                    ((CardUnitElement)B.Body).Img.Source =
                        PictHelper.GetBitmapImage(new Uri(Sets.Settings.GetInstance().DefaultImage));
                }
                else
                    MessageBox.Show("Файл изображения не найден:  " + card.ImageAddress);
            }

            ShowComp = new HiderShower("HiderShower", this);
            cardShower = new CardShower("CardShower", this);
            InGS = new OLDInGameStruct("InGameStruct", this, Scene);
            hit = new Hit("Hit", this);
            uState = new UState("UState", this);
            uState.newOld = NewOld.New;
            ShowComp.Hide();

            B.Body.PreviewMouseLeftButtonDown += Body_PreviewMouseLeftButtonDown;
        }

        public void UnloadImage()
        {
            if (Path.GetExtension(Card.ImageAddress) == ".gif")
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.UriSource = new Uri("pack://application:,,,/Images/simpleGif.gif");
                image.EndInit();
                ImageBehavior.SetAnimatedSource(((CardUnitElement)GetComponent<HaveBody>().Body).Img, image);
            }
            else if (Path.GetExtension(Card.ImageAddress) == ".wmv")
            {
                GetComponent<VideoInCard>().Delete();
            }
            else
                ((CardUnitElement)GetComponent<HaveBody>().Body).Img.Source = PictHelper.GetBitmapImage(new Uri("pack://application:,,,/Images/simpleGif.gif"));

            GifController?.Dispose();
            GC.Collect();
        }


        private void Body_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (readyToReactionOnMouseDown)
            {
                this.GetComponent<Hit>().IsHited = true;
                MouseClicked?.Invoke();
            }
        }


        public CardUnit DeepCopy()
        {
            var newcardunit = new CardUnit(Scene, Card);


            newcardunit.Components.Remove("HaveBody");
            newcardunit.Components.Remove("VideoInCard");
            newcardunit.Components.Remove("CheckedSymbol");
            newcardunit.Components.Remove("DragAndDrop");
            newcardunit.Components.Remove("Moveable");
            newcardunit.Components.Remove("HiderShower");
            newcardunit.Components.Remove("CardShower");

            newcardunit.Components.Remove("InGameStruct");
            newcardunit.Components.Remove("UState");
            newcardunit.Components.Remove("Hit");




            HaveBody B;
            VideoInCard videoInCard;
            CheckedSymbol ChS;
            DragAndDrop DaD;
            Moveable M;
            HiderShower ShowComp;
            CardShower cardShower;
            OLDInGameStruct InGS;
            UState uState;
            Hit hit;

            B = new HaveBody("HaveBody", newcardunit, new CardUnitElement());
            videoInCard = new VideoInCard("VideoInCard", newcardunit, ((CardUnitElement)B.Body).ContentGrid);

            if (System.IO.File.Exists(Card.ImageAddress) || Miscellanea.UrlExists(Card.ImageAddress))
            {
                if (Path.GetExtension(Card.ImageAddress) == ".gif")
                {
                    var image = new BitmapImage();
                    image.BeginInit();
                    image.UriSource = new Uri(Card.ImageAddress);
                    image.EndInit();
                    ImageBehavior.SetAnimatedSource(((CardUnitElement)B.Body).Img, image);
                }
                else if (Path.GetExtension(Card.ImageAddress) == ".wmv")
                {
                    ((CardUnitElement)B.Body).Img.Visibility = System.Windows.Visibility.Collapsed;
                    videoInCard.Run(Card.ImageAddress);
                }
                else
                    ((CardUnitElement)B.Body).Img.Source = PictHelper.GetBitmapImage(new Uri(Card.ImageAddress));
            }
            else
            {
                if (System.IO.File.Exists(Sets.Settings.GetInstance().DefaultImage))
                {
                    ((CardUnitElement)B.Body).Img.Source =
                        PictHelper.GetBitmapImage(new Uri(Sets.Settings.GetInstance().DefaultImage));
                }
                else
                    MessageBox.Show("Файл изображения не найден:  " + Card.ImageAddress);
            }

            ShowComp = new HiderShower("HiderShower", newcardunit);
            cardShower = new CardShower("CardShower", newcardunit);
            InGS = new OLDInGameStruct("InGameStruct", newcardunit, Scene);
            hit = new Hit("Hit", newcardunit);
            uState = new UState("UState", newcardunit);
            uState.newOld = NewOld.New;
            ShowComp.Hide();

            B.Body.PreviewMouseLeftButtonDown += newcardunit.Body_PreviewMouseLeftButtonDown;



            return newcardunit;
        }





    }
}
