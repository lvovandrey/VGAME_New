using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Speech.Synthesis;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using VanyaGame.GameCardsNewDB.Tools;
using VanyaGame.GameCardsNewDB.Units;
using VanyaGame.GameCardsNewDB.Units.Components;
using VanyaGame.GameCardsNewDB.DB;
using VanyaGame.Struct;
using VanyaGame.Struct.Components;
using VanyaGame.ToolsShuffle;
using VanyaGame.Units.Components;
using VanyaGame.GameCardsNewDB.DB.RepositoryModel;

namespace VanyaGame.GameCardsNewDB.Struct
{
    public class CardsNewDBScene : VanyaGame.Struct.Scene
    {
        private UnitsCollection<CardUnit> UnitsCol; //For easy call this component 
        private CardUnit CurUnit; //Текущая карточка, которую нужно озвучить и отгадать
        private bool ReadyToNextUnit;
        private CardsNewDBLevel numberDBLevel;


        public CardsNewDBScene(CardsNewDBLevel _Level) : base(_Level, _Level.Sets.Name)
        {
            this.Level = _Level;
            Sets = new SceneSets(_Level, this);
            numberDBLevel = _Level;
            GetComponent<Loader>().LoadSets = LoadSets;
            GetComponent<Loader>().LoadContent = LoadContent;

            GetComponent<Starter>().StartElements.Add(Start);

            UnitsCol = new UnitsCollection<CardUnit>("UnitsCollection", this);
            ReadyToNextUnit = true;
        }

        private void LoadSets()
        {
            Settings.RestoreAllSettings();
        }

        private void LoadContent()
        {
            IEnumerable<Card> cards;

            var cardsList = numberDBLevel.DbLevelRecord.Cards.ToList();
            cardsList = new ListShuffle<Card>().Shuffle(cardsList);

            for (int i = 0; i < cardsList.Count; i++)
            {
                CardUnit c = new CardUnit(this, cardsList[i], Settings.CardSize);
                UnitsCol.AddUnit(c);
            }
        }



        private void Start()
        {
           
            foreach (var u in UnitsCol.GetAllUnits())
                u.MouseClicked += U_MouseClicked;
            Game.Owner.TextForCardTag.Text = this.Name;

            NextNumber();
            SceneStarted(this);
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
                var AllUnitsShuffled = new ListShuffle<CardUnit>().Shuffle(UnitsCol.GetAllUnits());
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
                Speak(Settings.FirstQuestionText + CurUnit.Card.SoundedText);

                needSpeakAgain = true;
                needFlash = true;

                speakAgainTimer = new Timer(SpeakAgain, null, (int)(1000 * Settings.SpeakAgainCardNameDelay), (int)(1000 * Settings.SpeakAgainCardNameTimePeriod));

                if (Settings.VisualHintEnable)
                    if (Settings.EducationModeEnable)
                        flashTimer = new Timer(Flash, null, (int)(1000 * Settings.EducationVisualHintDelay), (int)(1000 * Settings.EducationVisualHintTimePeriod));
                    else
                        flashTimer = new Timer(Flash, null, (int)(1000 * Settings.VisualHintDelay), (int)(1000 * Settings.VisualHintTimePeriod));


                Game.Owner.TextForCardTag.Text = "Тема: " + this.Name + ".  Надо показать: " + CurUnit.Card.Title;

            }
            else
            {
                SceneEnded(this, (CardsNewDBLevel)Level);
            }
        }

        bool needSpeakAgain = false;
        Timer speakAgainTimer;
        private void SpeakAgain(object obj)
        {
            if (needSpeakAgain)
            {
                SpeakSlow(Settings.HintQuestionText + CurUnit.Card.SoundedText);
            }
        }

        bool needFlash = false;
        Timer flashTimer;
        private void Flash(object obj)
        {
            if (needFlash)
            {
                var body = CurUnit.GetComponent<HaveBody>().Body as CardUnitElement;

                TimeSpan period;
                if (Settings.EducationModeEnable)
                    period = TimeSpan.FromSeconds(Settings.EducationVisualHintDuration);
                else
                    period = TimeSpan.FromSeconds(Settings.VisualHintDuration);

                body.Dispatcher.Invoke(() => { body.Flash(period, Settings.CardSize); });
            }
        }


        private void U_MouseClicked()
        {
            needSpeakAgain = false;
            if (speakAgainTimer != null)
            {
                speakAgainTimer.Dispose();
            }

            needFlash = false;
            if (flashTimer != null)
            {
                flashTimer.Dispose();
            }

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
                Speak(Settings.SuccessTestText + CurUnit.Card.SoundedText);
                ToolsTimer.Delay(() =>
                {
                    SpeakSlow(CurUnittmp.Card.SoundedText);
                }, TimeSpan.FromSeconds(Settings.CardSuccesSpeakAgainTime));

                CurUnit = null;

                Panel.SetZIndex(Game.Owner.WrapPanelBigCards, 30001);
                var cu = CurUnittmp.DeepCopy();
                cu.GetComponent<HaveBody>().Body.Height = Settings.CardSuccesSize;
                cu.GetComponent<HaveBody>().Body.Width = Settings.CardSuccesSize;

                HaveBox HB = new HaveBox("HaveBox", Game.Owner, Game.Owner.WrapPanelBigCards, cu);
                cu.GetComponent<HiderShower>().Show(1, TimeSpan.FromSeconds(1), new Thickness(100), TimeSpan.FromSeconds(1));

            }
            else
            {
                Speak(Settings.FallTestText);

            }

            foreach (var u in UnitsCol.GetAllUnits())
            {
                if (u != CurUnittmp || (!IsHitSuccess && u == CurUnittmp))
                {
                    Panel.SetZIndex(u.GetComponent<HaveBody>().Body, 1110);
                    u.GetComponent<CardShower>().Hide(() => { });
                }
            }


            TimeSpan CardPauseTime;
            if (IsHitSuccess)
                CardPauseTime = TimeSpan.FromSeconds(Settings.CardSuccesTime);
            else
                CardPauseTime = TimeSpan.FromSeconds(Settings.CardWrongPauseTime);

            ToolsTimer.Delay(() =>
            {
                Panel.SetZIndex(CurUnittmp.GetComponent<HaveBody>().Body, 1110);
                CurUnittmp.GetComponent<CardShower>().Hide(() => { });
                ToolsTimer.Delay(() =>
                {
                    Game.Owner.WrapPanelBigCards.Children.Clear();
                    NextNumber();
                }, TimeSpan.FromSeconds(1));
            }, CardPauseTime);
        }

        private void Speak(string text)
        {
            if (text == null) return;
            SpeechSynthesizer speaker = new SpeechSynthesizer();
            if (Settings.TextToSpeachVoices.Count == 0)
            {
                MessageBox.Show("В системе не установлены голоса для синтеза речи на русском языке. Установите пожалуйста, а то ничего не будет слышно.");
                return;
            }
            else speaker.SelectVoice(Settings.TTSVoice.VoiceInfo.Name);
            speaker.Rate = Settings.TTSVoiceRate;
            speaker.Volume = Settings.TTSVoiceVolume;
            speaker.SpeakAsync(text);
        }

        private void SpeakSlow(string text)
        {
            if (text == null) return;
            SpeechSynthesizer speaker = new SpeechSynthesizer();
            if (Settings.TextToSpeachVoices.Count == 0)
            {
                MessageBox.Show("В системе не установлены голоса для синтеза речи на русском языке. Установите пожалуйста, а то ничего не будет слышно.");
                return;
            }
            else speaker.SelectVoice(Settings.TTSVoice.VoiceInfo.Name);
            speaker.Rate = Settings.TTSVoiceSlowRate;
            speaker.Volume = Settings.TTSVoiceVolume;
            speaker.SpeakAsync(text);
        }



        public void SceneStarted(Scene SL)
        {
            Game.Music.Play();
            Game.Music.MediaGUI.UIMediaShow();
            Game.CurVideo.MediaGUI.UIMediaHide();
        }



        public void SceneEnded(Scene SL, CardsNewDBLevel level)
        {

            foreach (var u in UnitsCol.GetAllUnits())
                u.MouseClicked -= U_MouseClicked;

            Game.Owner.TextForCardTag.Text = "";
            Game.UserActivity.UserDoSomethingEvent -= UserDoSomething;

            Game.Music.Pause();
            Game.Owner.WrapPanelMain.Children.Clear();

            level.NextScene();


        }


    }
}
