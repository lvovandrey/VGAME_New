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
        private CardPassing CurCardPassing;
        private bool CurUnitAlreadyTasked = false; // Показывает в первый раз или нет данный юнит предъявляется для угадывания
        private bool IsAborted = false;

        SpeechSynthesizer speaker = new SpeechSynthesizer();

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
            Settings.GetInstance().RestoreAllSettings();
        }

        private void LoadContent()
        {
            IEnumerable<Card> cards;

            var cardsList = numberDBLevel.DbLevelRecord.Cards.ToList();
            cardsList = new ListShuffle<Card>().Shuffle(cardsList);

            for (int i = 0; i < cardsList.Count; i++)
            {
                CardUnit c = new CardUnit(this, cardsList[i], Settings.GetInstance().CardSize);
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
            if (IsAborted) return;
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
                Speak(Settings.GetInstance().FirstQuestionText + CurUnit.Card.SoundedText);

                needSpeakAgain = true;
                needFlash = true;

                speakAgainTimer = new Timer(SpeakAgain, null, (int)(1000 * Settings.GetInstance().SpeakAgainCardNameDelay), (int)(1000 * Settings.GetInstance().SpeakAgainCardNameTimePeriod));

                if (Settings.GetInstance().VisualHintEnable)
                    if (Settings.GetInstance().EducationModeEnable)
                        flashTimer = new Timer(Flash, null, (int)(1000 * Settings.GetInstance().EducationVisualHintDelay), (int)(1000 * Settings.GetInstance().EducationVisualHintTimePeriod));
                    else
                        flashTimer = new Timer(Flash, null, (int)(1000 * Settings.GetInstance().VisualHintDelay), (int)(1000 * Settings.GetInstance().VisualHintTimePeriod));

                
                SetNewCurCardPassing();

                Game.Owner.TextForCardTag.Text = "Тема: " + this.Name + ".  Надо показать: " + CurUnit.Card.Title;

                
            }
            else
            {
                SceneEnded(this, (CardsNewDBLevel)Level);
            }
        }


        bool UnitRepeat = false; // юнит уже предъявлялся (нужно чтобы понять что юнит уже предъявлялся)

        private void SetNewCurCardPassing()
        {
            if (CurUnitAlreadyTasked) return;
            CurCardPassing = new CardPassing() { DateAndTime = DateTime.Now.ToString(), AttemptsNumber = 0 };
            CurUnit.Card.CardPassings.Add(CurCardPassing);
            ((CardsNewDBLevel)Level).CurLevelPassing.CardPassings.Add(CurCardPassing);
            DBTools.Context.CardPassings.Add(CurCardPassing);
            DBTools.Context.SaveChanges();
        }



        bool needSpeakAgain = false;
        Timer speakAgainTimer;
        private void SpeakAgain(object obj)
        {
            if (needSpeakAgain)
            {
                SpeakSlow(Settings.GetInstance().HintQuestionText + CurUnit.Card.SoundedText);
            }
        }

        bool needFlash = false;
        Timer flashTimer;
        private void Flash(object obj)
        {
            if (IsAborted) return;
            if (needFlash)
            {
                var body = CurUnit.GetComponent<HaveBody>().Body as CardUnitElement;

                TimeSpan period;
                if (Settings.GetInstance().EducationModeEnable)
                    period = TimeSpan.FromSeconds(Settings.GetInstance().EducationVisualHintDuration);
                else
                    period = TimeSpan.FromSeconds(Settings.GetInstance().VisualHintDuration);

                body.Dispatcher.Invoke(() => { body.Flash(period, Settings.GetInstance().CardSize); });
            }
        }


        private void U_MouseClicked()
        {
            if (IsAborted) return;
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

            CurCardPassing.AttemptsNumber++;
            DBTools.Context.SaveChanges();

            if (IsHitSuccess)
            {
                CurUnitAlreadyTasked = false;
                Panel.SetZIndex(CurUnittmp.GetComponent<HaveBody>().Body, 1110);
                CurUnittmp.GetComponent<CardShower>().Hide(() => { });

                CurUnit.GetComponent<UState>().newOld = NewOld.Old;
                Speak(Settings.GetInstance().SuccessTestText + CurUnit.Card.SoundedText);
                ToolsTimer.Delay(() =>
                {
                    SpeakSlow(CurUnittmp.Card.SoundedText);
                }, TimeSpan.FromSeconds(Settings.GetInstance().CardSuccesSpeakAgainTime));

                CurUnit = null;

                Panel.SetZIndex(Game.Owner.WrapPanelBigCards, 30001);
                var cu = CurUnittmp.DeepCopy();
                cu.GetComponent<HaveBody>().Body.Height = Settings.GetInstance().CardSuccesSize;
                cu.GetComponent<HaveBody>().Body.Width = Settings.GetInstance().CardSuccesSize;

                HaveBox HB = new HaveBox("HaveBox", Game.Owner, Game.Owner.WrapPanelBigCards, cu);
                cu.GetComponent<HiderShower>().Show(1, TimeSpan.FromSeconds(1), new Thickness(100), TimeSpan.FromSeconds(1));

            }
            else
            {
                CurUnitAlreadyTasked = true;
                Speak(Settings.GetInstance().FallTestText);
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
                CardPauseTime = TimeSpan.FromSeconds(Settings.GetInstance().CardSuccesTime);
            else
                CardPauseTime = TimeSpan.FromSeconds(Settings.GetInstance().CardWrongPauseTime);

            ToolsTimer.Delay(() =>
            {
                if (IsAborted) return;
                Panel.SetZIndex(CurUnittmp.GetComponent<HaveBody>().Body, 1110);
                CurUnittmp.GetComponent<CardShower>().Hide(() => { });
                ToolsTimer.Delay(() =>
                {
                    if (IsAborted) return;
                    Game.Owner.WrapPanelBigCards.Children.Clear();
                    NextNumber();
                }, TimeSpan.FromSeconds(1));
            }, CardPauseTime);
        }

        private void Speak(string text)
        {
            if (text == null) return;
            speaker?.Pause();
            speaker?.Resume();
            speaker = new SpeechSynthesizer();
            if (Settings.GetInstance().TextToSpeachVoices.Count == 0) return;
            else speaker.SelectVoice(Settings.GetInstance().TTSVoice.VoiceInfo.Name);
            speaker.Rate = Settings.GetInstance().TTSVoiceRate;
            speaker.Volume = Settings.GetInstance().TTSVoiceVolume;
            speaker.SpeakAsync(text);
        }

        private void SpeakSlow(string text)
        {
            if (text == null) return;
            speaker?.Pause();
            speaker?.Resume();
            speaker = new SpeechSynthesizer();
            if (Settings.GetInstance().TextToSpeachVoices.Count == 0) return; 
            else speaker.SelectVoice(Settings.GetInstance().TTSVoice.VoiceInfo.Name);
            speaker.Rate = Settings.GetInstance().TTSVoiceSlowRate;
            speaker.Volume = Settings.GetInstance().TTSVoiceVolume;
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

            speakAgainTimer?.Dispose();
            flashTimer?.Dispose();

            level.NextScene();


        }

        public override void Abort()
        {
            Game.Owner.WrapPanelBigCards.Children.Clear();
            speaker?.Pause();
            speaker?.Resume();
            IsAborted = true;
            SceneEnded(this,(CardsNewDBLevel)Level);
        }

    }
}
