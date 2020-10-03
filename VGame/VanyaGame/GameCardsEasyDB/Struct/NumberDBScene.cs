using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Speech.Synthesis;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using VanyaGame.DB.DBCardsRepositoryModel;
using VanyaGame.GameCardsEasyDB.Units;
using VanyaGame.GameCardsEasyDB.Units.Components;
using VanyaGame.Struct;
using VanyaGame.Struct.Components;
using VanyaGame.ToolsShuffle;
using VanyaGame.Units.Components;
using DBModel = VanyaGame.DB.DBLevelsRepositoryModel;

namespace VanyaGame.GameCardsEasyDB.Struct
{
    public class CardsEasyDBScene : VanyaGame.Struct.Scene
    {
        private UnitsCollection<CardUnit> UnitsCol; //For easy call this component 
        private CardUnit CurUnit; //Текущая карточка, которую нужно озвучить и отгадать
        private bool ReadyToNextUnit;
        private CardsEasyDBLevel numberDBLevel;
        private DBModel.Scene DBSceneRecord;

        public string tag;


        public CardsEasyDBScene(Level _Level, DBModel.Scene scene, string name) : base(_Level, name)
        {
            this.Level = _Level;
            this.DBSceneRecord = scene;
            Sets = new SceneSets(_Level, this);
            //Sets.Directory = "NONE SCENE DIR!!!";

            InnerVideoSets IVC = new InnerVideoSets("InnerVideoSets", Sets);
            Level = _Level;

            GetComponent<Loader>().LoadSets = LoadSets;
            GetComponent<Loader>().LoadContent = LoadContent;

            GetComponent<Starter>().StartElements.Add(Start);

            UnitsCol = new UnitsCollection<CardUnit>("UnitsCollection", this);
            ReadyToNextUnit = true;

        }

        private void LoadSets()
        {

        }

        private void LoadContent()
        {
            IEnumerable<DB.DBCardsRepositoryModel.Card> cards;
            string LevelTag = ((CardsEasyDBLevel)this.Level).Tag;
            if (LevelTag == null || LevelTag == "")
            {
                MessageBox.Show("Тег уровня - пустой!");
                throw new Exception("Тег уровня - пустой!");
            }

            var tags = Game.DBTools.Tags;
            DB.DBCardsRepositoryModel.Tag TAG = new DB.DBCardsRepositoryModel.Tag();
            foreach (var tagDB in Game.DBTools.Tags)
            {
                if (tagDB.Name == LevelTag)
                {
                    this.tag = tagDB.Name;
                    TAG = tagDB;
                    break;
                }
            }
            if (tag == null || tag == "")
            {
                MessageBox.Show("Тег уровня не найден в БД!");
                throw new Exception("Тег уровня не найден в БД!");
            }
            cards = from c in Game.DBTools.Cards where c.Tags.Contains(TAG) select c;
            var cardsList = cards.ToList();


            cardsList = new ListShuffle<Card>().Shuffle(cardsList); 

            for ( int i=0; i<cardsList.Count;i++)// var card in cards)
            {
                CardUnit c = new CardUnit(this, cardsList[i]);
                UnitsCol.AddUnit(c);
            }
        }



        private void Start()
        {

            //foreach (var u in UnitsCol.GetAllUnits())
            //{
            //    HaveBox HB = new HaveBox("HaveBox", Game.Owner, Game.Owner.WrapPanelMain, u);
            //}
            //UnitsCol.Shuffle();
            // CardUnit N = UnitsCol.GetFirstUnit();

            foreach (var u in UnitsCol.GetAllUnits())
                u.MouseClicked += U_MouseClicked;
            Game.Owner.TextForCardTag.Text = this.tag;

            NextNumber();
            SceneStarted(this, Level);
            Game.UserActivity.UserDoSomethingEvent += UserDoSomething;
        }

        private void UserDoSomething(MouseEventArgs mouse, MouseButtonEventArgs mousebutton, KeyEventArgs key)
        {

        }


        private void NextNumber()
        {
            Panel.SetZIndex(Game.Owner.WrapPanelMain, 30000);

            if (UnitsCol.GetNewUnits().Count > 0)
            {

                Game.Owner.WrapPanelMain.Children.Clear();
                UnitsCol.Shuffle();
                var AllUnitsShuffled= new ListShuffle<CardUnit>().Shuffle(UnitsCol.GetAllUnits());
                foreach (var u in AllUnitsShuffled)
                {
                    if (u.GetComponent<HaveBox>() != null)
                    {
                        u.GetComponent<HaveBox>().RemoveFromBox();
                        u.Components.Remove("HaveBox");
                    }
                    HaveBox HB = new HaveBox("HaveBox", Game.Owner, Game.Owner.WrapPanelMain, u);
                }



                foreach (var u in UnitsCol.GetAllUnits())
                {
                    u.GetComponent<CardShower>().Show(() => { ReadyToNextUnit = true; });
                    u.GetComponent<Hit>().IsHited = false;
                    u.readyToReactionOnMouseDown = true;
                    Panel.SetZIndex(u.GetComponent<HaveBody>().Body, 33000);
                }

                if (CurUnit == null)
                {
                    List<CardUnit> newUnitsShuffled = new ListShuffle<CardUnit>().Shuffle(UnitsCol.GetNewUnits());
                    CurUnit = newUnitsShuffled[0]; //UnitsCol.GetNewUnits().First();
                }
                Speak("Ваня! Покажи где " + CurUnit.Card.SoundedText);// + ". Ваня! Где " + CurUnit.Card.Title + "?");
                Game.Owner.TextForCardTag.Text = "Тема: " + this.tag + ".  Надо показать:" + CurUnit.Card.Title;
            }
            else
            {
                SceneEnded(this, Level);
            }
        }

        private void U_MouseClicked()
        {
            bool IsHitSuccess = false;
            var CurUnittmp = CurUnit;

            foreach (var u in UnitsCol.GetAllUnits())
            {
                u.GetComponent<CardShower>().Show(() => { ReadyToNextUnit = true; });
                u.readyToReactionOnMouseDown = false;
                if ((u.GetComponent<Hit>().IsHited) && (CurUnit.Card.Title == u.Card.Title))
                    IsHitSuccess = true;
            }
            if (IsHitSuccess)
            {

                Panel.SetZIndex(CurUnittmp.GetComponent<HaveBody>().Body, 1110);
                CurUnittmp.GetComponent<CardShower>().Hide(() => { });

                CurUnit.GetComponent<UState>().newOld = NewOld.Old;
                Speak("Молодец! Это "+ CurUnit.Card.SoundedText);// Умница! Ты показал " + CurUnit.Card.SoundedText);
                ToolsTimer.Delay(() =>
                {
                    SpeakSlow( CurUnittmp.Card.SoundedText);
                }, TimeSpan.FromSeconds(3));

                CurUnit = null;

                Panel.SetZIndex(Game.Owner.WrapPanelBigCards, 30001);
                var cu = CurUnittmp.DeepCopy();
                cu.GetComponent<HaveBody>().Body.Height = 500;
                cu.GetComponent<HaveBody>().Body.Width = 500;

                HaveBox HB = new HaveBox("HaveBox", Game.Owner, Game.Owner.WrapPanelBigCards, cu);
                cu.GetComponent<HiderShower>().Show(1, TimeSpan.FromSeconds(1), new Thickness(100), TimeSpan.FromSeconds(1));

            }
            else
            {
                Speak("Не правильно! Попробуй ещё раз.");
            }

            foreach (var u in UnitsCol.GetAllUnits())
            {
                if (u != CurUnittmp || (!IsHitSuccess && u == CurUnittmp))
                {
                    Panel.SetZIndex(u.GetComponent<HaveBody>().Body, 1110);
                    u.GetComponent<CardShower>().Hide(() => { });
                }
            }

            ToolsTimer.Delay(() =>
            {
                Panel.SetZIndex(CurUnittmp.GetComponent<HaveBody>().Body, 1110);
                CurUnittmp.GetComponent<CardShower>().Hide(() => { });
                ToolsTimer.Delay(() =>
                {
                    Game.Owner.WrapPanelBigCards.Children.Clear();
                    NextNumber();
                }, TimeSpan.FromSeconds(1));
            }, TimeSpan.FromSeconds(4));
        }

        private void Speak(string text)
        {
            if (text == null) return;
            SpeechSynthesizer speaker = new SpeechSynthesizer();

            var voices = speaker.GetInstalledVoices(new CultureInfo("ru-RU"));

            if (voices.Count == 0) MessageBox.Show("В системе не установлены голоса для синтеза речи на русском языке. Установите пожалуйста, а то ничего не будет слышно.");
            else speaker.SelectVoice(voices[0].VoiceInfo.Name);
            speaker.Rate = 1;
            speaker.Volume = 100;
            speaker.SpeakAsync(text);
        }

        private void SpeakSlow(string text)
        {
            if (text == null) return;
            SpeechSynthesizer speaker = new SpeechSynthesizer();

            var voices = speaker.GetInstalledVoices(new CultureInfo("ru-RU"));

            if (voices.Count == 0) MessageBox.Show("В системе не установлены голоса для синтеза речи на русском языке. Установите пожалуйста, а то ничего не будет слышно.");
            else speaker.SelectVoice(voices[0].VoiceInfo.Name);
            speaker.Rate = -3;
            speaker.Volume = 100;
            speaker.SpeakAsync(text);
        }



        public void SceneStarted(Scene SL, Level Level)
        {

            Game.Music.Play();
            Game.Music.MediaGUI.UIMediaShow();
            Game.CurVideo.MediaGUI.UIMediaHide();
            //Game.Owner.Cursor = Cursors.Arrow;
        }



        public void SceneEnded(Scene SL, Level Level)
        {

            foreach (var u in UnitsCol.GetAllUnits())
                u.MouseClicked -= U_MouseClicked;

            Game.Owner.TextForCardTag.Text = "";
            Game.UserActivity.UserDoSomethingEvent -= UserDoSomething;

            Game.Music.Pause();
            Game.Owner.WrapPanelMain.Children.Clear();

            ((CardsEasyDBLevel)Level).NextScene();


        }


    }
}
