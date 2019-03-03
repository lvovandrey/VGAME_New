﻿using System;
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

namespace VanyaGame.GameNumber.Struct
{
    public class NumberScene : Scene
    {
        private UnitsCollection<Number> UnitsCol; //For easy call this component 
        private bool ReadyToNextUnit;

        private KeyboardElement keyboardElement;

        public NumberScene(Level _Level, string _SceneDir, string name) : base(_Level, name)
        {
            Sets = new SceneSets(_Level, this);
            Sets.Directory = _SceneDir;
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
            string filenameXML = Game.Sets.MainDir + Level.Sets.Directory + Sets.Directory + @"\SceneSets.xml";
            //VanyaGame.XMLTools.LoadSetsFromXML(this, filenameXML);
        }

        private void LoadContent()
        {
            //LoadBackground();
            //LoadPreview();
            int prevNum = Game.RandomGenerator.Next(1, 9);

            for (int i = 1; i < 2; i++)
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
            Game.Video.MediaGUI.UIMediaHide();
            
            //TDrawEffects.SlowDifferVolume(Game.Music.player, 1, 4, (sender, e) => { });
            Game.Music.player.Volume = 0;

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
                Game.Music.player.Volume = 0;
                Game.Music.Pause();
                Game.Owner.StackPanelGame.Children.Clear();

            //});

            Game.Video.ShowVideoPlayer();
            Game.Music.MediaGUI.UIMediaHide();
            ToolsTimer.Delay(() =>
            {
                Game.Video.MediaGUI.UIMediaShow();

            }, new TimeSpan(0, 0, 1));

            string MediaName = SL.Sets.GetComponent<InnerVideoSets>().VideoFileName;


            Game.Video.Play(MediaName, SL.Sets.GetComponent<InnerVideoSets>().VideoTimeBegin, SL.Sets.GetComponent<InnerVideoSets>().VideoTimeEnd);
            Game.Owner.VideoTimeSlider.Maximum = SL.Sets.GetComponent<InnerVideoSets>().VideoTimeEnd.TotalSeconds;
            Game.Owner.VideoTimeSlider.Minimum = SL.Sets.GetComponent<InnerVideoSets>().VideoTimeBegin.TotalSeconds;

            Game.Video.ClearOnEndedEvent();
            Game.Video.OnEnded += ((NumberLevel)Level).NextScene;
            Game.Video.OnEnded += () =>
            {
                Game.Video.ClearOnEndedEvent();
                Game.Video.HideVideoPlayer();
                Game.Video.MediaGUI.UIMediaHide();
            };
            

        }


    }
}
