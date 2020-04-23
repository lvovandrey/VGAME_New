using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using VanyaGame.GameNumber.Behaviour;
using VanyaGame.GameNumber.Interface;
using VanyaGame.GameNumber.Units;
using VanyaGame.GameNumber.Units.Components;
using VanyaGame.Struct;
using VanyaGame.Struct.Components;
using VanyaGame.Units;
using VanyaGame.Units.Components;
using DBModel = VanyaGame.DB.DBLevelsRepositoryModel;

namespace VanyaGame.GameNumberDB.Struct
{
    public class NumberDBScene : VanyaGame.Struct.Scene
    {
        private UnitsCollection<Number> UnitsCol; //For easy call this component 
        private bool ReadyToNextUnit;

        private KeyboardElement keyboardElement;
        private NumberDBLevel numberDBLevel;
        private DBModel.Scene DBSceneRecord;



        public NumberDBScene(Level _Level, DBModel.Scene scene, string name) : base(_Level, name)
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

            UnitsCol = new UnitsCollection<Number>("UnitsCollection", this);
            ReadyToNextUnit = true;

        }

        private void LoadSets()
        {
            //string filenameXML = Game.Sets.MainDir + Level.Sets.Directory + Sets.Directory + @"\SceneSets.xml";
            //VanyaGame.XMLTools.LoadSetsFromXML(this, filenameXML);
        }

        private void LoadContent()
        {
            //LoadBackground();
            //LoadPreview();
            int prevNum = Game.RandomGenerator.Next(1, 9);

            for (int i = 1; i <= DBSceneRecord.TasksCount; i++)
            {

                int ii = Game.RandomGenerator.Next(1, 9);
                while (prevNum == ii)
                {
                    System.Threading.Thread.Sleep(1);
                    ii = Game.RandomGenerator.Next(1, 9);
                }
                prevNum = ii;
                Number N = new Number(ii.ToString(), this);
                UnitsCol.AddUnit(N);
            }
        }

        private void Start()
        {
            Number N = UnitsCol.GetFirstUnit();
            N.GetComponent<NumberShower>().Show(()=> { ReadyToNextUnit = true; });
            




            SceneStarted(this, Level);
            //LoadMedia();
            //LoadScenes();
            //CurScene.GetComponent<Starter>().Start();

            foreach (Number NN in UnitsCol.GetAllUnits())
            {
                PaperGrid P = new PaperGrid();
                P.Width = 100; P.Height = 100;
                Game.Owner.StackPanelGame.Children.Add(P);
                NN.Box = P.PaperGird;
            }

            Game.UserActivity.UserDoSomethingEvent += UserDoSomething;
            keyboardElement = new KeyboardElement();
            Game.Owner.GridMain.Children.Add(keyboardElement);
            keyboardElement.Show(N.GetComponent<CheckedSymbol>().Symbol);
        }

        private void UserDoSomething(MouseEventArgs mouse, MouseButtonEventArgs mousebutton, KeyEventArgs key)
        {
            if (key != null)
            {
                Number N = UnitsCol.GetNewUnits().First();
                string s;
                s = key.Key.ToString();
                s = s.Replace("D", "");
                if (N.GetComponent<CheckedSymbol>().IsPrintedSimbolMatch(s) && ReadyToNextUnit == true)
                {
                    ReadyToNextUnit = false;
                    N.GetComponent<UState>().newOld = NewOld.Old;
                    N.RemoveToBox(() => 
                    {
                        
                        NextNumber(); }
                    );
                    keyboardElement.Hide();
                }
                if (s == "Q")
                {
                    keyboardElement.Show(N.GetComponent<CheckedSymbol>().Symbol);
                }
                if (s == "M")
                {
                    keyboardElement.Show(s);
                }

            }
        }



        private void NextNumber()
        {
            if (UnitsCol.GetNewUnits().Count > 0)
            {
                Number N = UnitsCol.GetNewUnits().First();
                N.GetComponent<NumberShower>().Show(()=> { ReadyToNextUnit = true; });

                keyboardElement.Show(N.GetComponent<CheckedSymbol>().Symbol);
            }
            else
            {
                SceneEnded(this, Level);
            }
        }

        public void SceneStarted(Scene SL, Level Level)
        {

            Game.Music.Play();
            Game.Music.MediaGUI.UIMediaShow();
            Game.CurVideo.MediaGUI.UIMediaHide();
            
            //TDrawEffects.SlowDifferVolume(Game.Music.player, 1, 4, (sender, e) => { });
            //Game.Music.player.Volume = 0;

            Game.Owner.StackPanelGame.Children.Clear();

            Game.Owner.Cursor = Cursors.Arrow;
        }



        public void SceneEnded(Scene SL, Level Level)
        {
            Game.Owner.GridMain.Children.Remove(keyboardElement);


            Game.UserActivity.UserDoSomethingEvent -= UserDoSomething;

            Game.Owner.StackPanelGame.Children.Clear();
           // TDrawEffects.SlowDifferVolume(Game.Music.player, 0, 2, (sender, e) =>
           // {
              //  Game.Music.player.Volume = 0;
                Game.Music.Pause();
                Game.Owner.StackPanelGame.Children.Clear();

            //});

            VideoType videoType = SL.Sets.GetComponent<InnerVideoSets>().VideoFileType;
            switch (videoType)
            {
                case VideoType.local: Game.VideoPlayerSet(Game.VideoWpf); break;
                case VideoType.youtube: Game.VideoPlayerSet(Game.VideoVlc); break;
            }

            Game.Owner.TextVideoDescription.Text = Level.Sets.Description;

            Game.CurVideo.ShowVideoPlayer();
            Game.Music.MediaGUI.UIMediaHide();
            Game.Owner.LabelWait.Visibility = System.Windows.Visibility.Visible;
            TDrawEffects.BlurShow(Game.Owner.LabelWait,0.2, 0, () => { });
            ToolsTimer.Delay(() =>
            {
                Game.CurVideo.MediaGUI.UIMediaShow();

            }, TimeSpan.FromSeconds(1));


            ToolsTimer.Delay(() =>
            {
                string MediaName = SL.Sets.GetComponent<InnerVideoSets>().VideoFileName;


                Game.CurVideo.Play(MediaName, SL.Sets.GetComponent<InnerVideoSets>().VideoTimeBegin, SL.Sets.GetComponent<InnerVideoSets>().VideoTimeEnd, SL.Sets.GetComponent<InnerVideoSets>().VideoFileType);
                Game.Owner.VideoTimeSlider.Maximum = SL.Sets.GetComponent<InnerVideoSets>().VideoTimeEnd.TotalSeconds;
                Game.Owner.VideoTimeSlider.Minimum = SL.Sets.GetComponent<InnerVideoSets>().VideoTimeBegin.TotalSeconds;

                TDrawEffects.BlurHide(Game.Owner.LabelWait, 1, 0, () => { Game.Owner.LabelWait.Visibility = System.Windows.Visibility.Hidden; });
               
                Game.CurVideo.ClearOnEndedEvent();
                Game.CurVideo.OnEnded += ((NumberDBLevel)Level).NextScene;
                Game.CurVideo.OnEnded += () =>
                {
                    Game.CurVideo.ClearOnEndedEvent();
                    Game.CurVideo.HideVideoPlayer();
                    Game.CurVideo.MediaGUI.UIMediaHide();
                    Game.Owner.LabelWait.Visibility = System.Windows.Visibility.Hidden;
                    Game.Owner.TextVideoDescription.Text = "Ничего...";
                };
            }, TimeSpan.FromSeconds(1.3));

        }


    }
}
