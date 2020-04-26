using System;
using System.Linq;
using System.Speech.Synthesis;
using System.Windows.Controls;
using System.Windows.Input;
using VanyaGame.GameCardsEasyDB.Units;
using VanyaGame.GameCardsEasyDB.Units.Components;
using VanyaGame.Struct;
using VanyaGame.Struct.Components;
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
            //Выбираем случайным образом текущий тег
            var tags = Game.DBTools.Tags;
            int rand = Game.RandomGenerator.Next(0, tags.Count-1);
            var tag = tags[rand];

            var cards = from c in Game.DBTools.Cards where c.Tags.Contains(tag) select c;
            foreach (var card in cards)
            {
                CardUnit c = new CardUnit(this, card);
                UnitsCol.AddUnit(c);
            }
        }

        private void Start()
        {
            CardUnit N = UnitsCol.GetFirstUnit();

            foreach (var u in UnitsCol.GetAllUnits())
                u.MouseClicked += U_MouseClicked;
            

            NextNumber();
            SceneStarted(this, Level);
            Game.UserActivity.UserDoSomethingEvent += UserDoSomething;
        }

        private void UserDoSomething(MouseEventArgs mouse, MouseButtonEventArgs mousebutton, KeyEventArgs key)
        {
           
        }
        

        private void NextNumber()
        {
            if (UnitsCol.GetNewUnits().Count > 0)
            {


                foreach (var u in UnitsCol.GetAllUnits())
                {
                    u.GetComponent<CardShower>().Show(() => { ReadyToNextUnit = true; });
                    u.GetComponent<Hit>().IsHited = false;
                    u.readyToReactionOnMouseDown = true;
                    Panel.SetZIndex(u.GetComponent<HaveBody>().Body, 33000);
                }
                CurUnit = UnitsCol.GetNewUnits().First();
                Panel.SetZIndex(CurUnit.GetComponent<HaveBox>().InnerBox, 30000);

                Speak("Ваня! Покажи " + CurUnit.Card.SoundedText);// + ". Ваня! Где " + CurUnit.Card.Title + "?");
            }
            else
            {
                SceneEnded(this, Level);
            }
        }

        private void U_MouseClicked()
        {
            bool IsHitSuccess = false;
            foreach (var u in UnitsCol.GetAllUnits())
            {
                u.GetComponent<CardShower>().Show(() => { ReadyToNextUnit = true; });
                u.readyToReactionOnMouseDown = false;
                if ((u.GetComponent<Hit>().IsHited) && (CurUnit.Card.Title == u.Card.Title))
                    IsHitSuccess = true;
            }
            if (IsHitSuccess)
            {
                CurUnit.GetComponent<UState>().newOld = NewOld.Old;
                Speak("Молодец, Ваня!");// Умница! Ты показал " + CurUnit.Card.SoundedText);
            }
            else
            {
                Speak("Не правильно! Попробуй ещё раз.");
            }

            foreach (var u in UnitsCol.GetAllUnits())
            {
                Panel.SetZIndex(u.GetComponent<HaveBody>().Body, 0);
                u.GetComponent<CardShower>().Hide(() => { });
            }

            ToolsTimer.Delay(() =>
            {
                NextNumber();
            }, TimeSpan.FromSeconds(3));
        }

        private void Speak(string text)
        {
            if (text == null) return;
            SpeechSynthesizer speaker = new SpeechSynthesizer();
            speaker.Rate = 1;
            speaker.Volume = 100;
            speaker.SpeakAsync(text);
        }


        public void SceneStarted(Scene SL, Level Level)
        {

            Game.Music.Play();
            Game.Music.MediaGUI.UIMediaShow();
            Game.CurVideo.MediaGUI.UIMediaHide();
            Game.Owner.Cursor = Cursors.Arrow;
        }



        public void SceneEnded(Scene SL, Level Level)
        {

            foreach (var u in UnitsCol.GetAllUnits())
                u.MouseClicked -= U_MouseClicked;


            Game.UserActivity.UserDoSomethingEvent -= UserDoSomething;

                Game.Music.Pause();
                Game.Owner.StackPanelGame.Children.Clear();

            ((CardsEasyDBLevel)Level).NextScene();

            //VideoType videoType = SL.Sets.GetComponent<InnerVideoSets>().VideoFileType;
            //switch (videoType)
            //{
            //    case VideoType.local: Game.VideoPlayerSet(Game.VideoWpf); break;
            //    case VideoType.youtube: Game.VideoPlayerSet(Game.VideoVlc); break;
            //}

            //Game.Owner.TextVideoDescription.Text = Level.Sets.Description;

            //Game.CurVideo.ShowVideoPlayer();
            //Game.Music.MediaGUI.UIMediaHide();
            //Game.Owner.LabelWait.Visibility = System.Windows.Visibility.Visible;
            //TDrawEffects.BlurShow(Game.Owner.LabelWait,0.2, 0, () => { });
            //ToolsTimer.Delay(() =>
            //{
            //    Game.CurVideo.MediaGUI.UIMediaShow();

            //}, TimeSpan.FromSeconds(1));


            //ToolsTimer.Delay(() =>
            //{
            //    string MediaName = SL.Sets.GetComponent<InnerVideoSets>().VideoFileName;


            //    Game.CurVideo.Play(MediaName, SL.Sets.GetComponent<InnerVideoSets>().VideoTimeBegin, SL.Sets.GetComponent<InnerVideoSets>().VideoTimeEnd, SL.Sets.GetComponent<InnerVideoSets>().VideoFileType);
            //    Game.Owner.VideoTimeSlider.Maximum = SL.Sets.GetComponent<InnerVideoSets>().VideoTimeEnd.TotalSeconds;
            //    Game.Owner.VideoTimeSlider.Minimum = SL.Sets.GetComponent<InnerVideoSets>().VideoTimeBegin.TotalSeconds;

            //    TDrawEffects.BlurHide(Game.Owner.LabelWait, 1, 0, () => { Game.Owner.LabelWait.Visibility = System.Windows.Visibility.Hidden; });

            //    Game.CurVideo.ClearOnEndedEvent();
            //    Game.CurVideo.OnEnded += ((CardsEasyDBLevel)Level).NextScene;
            //    Game.CurVideo.OnEnded += () =>
            //    {
            //        Game.CurVideo.ClearOnEndedEvent();
            //        Game.CurVideo.HideVideoPlayer();
            //        Game.CurVideo.MediaGUI.UIMediaHide();
            //        Game.Owner.LabelWait.Visibility = System.Windows.Visibility.Hidden;
            //        Game.Owner.TextVideoDescription.Text = "Ничего...";
            //    };
            //}, TimeSpan.FromSeconds(1.3));

        }


    }
}
