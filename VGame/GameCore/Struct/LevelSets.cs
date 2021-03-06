﻿using GameCore;
using System;
using System.IO;
using VGameCore.Abstract;


namespace VGameCore.Struct
{
    public class LevelSets : ComponentContainer
    {
        public string VideoDir = @"\video";
        public string InterfaceDir = @"\interface";
        public string SoundDir = @"\sound";
        public string MusicDir = @"\music";
        public GameType gameType = GameType.None;

        private Level Level;
        private string dir = @"\Level1";
        public string Directory
        {
            get
            {
                return dir;
            }
            set
            {
                if (System.IO.Directory.Exists(
                        Path.Combine(Environment.GetFolderPath(
                            Environment.SpecialFolder.LocalApplicationData
                            ), "VGame") + value + @"\"))
                {
                    dir = value;
                }
                else
                {
                    throw new Exception("Cant initialize Scene " + Level.Name + " directory. Directory " + value + "not existed.");
                }
            }
        }

        public LevelSets(Level level)
        {
            Level = level;
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public string PreviewType { get; set; }
        public string PreviewURL { get; set; }
        public string BackgroundType { get; set; }
        public string BaseVideoFilename { get; set; }
        
    }
}
