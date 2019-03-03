using System;
using VanyaGame.Abstract;

namespace VanyaGame.Struct
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
                if (System.IO.Directory.Exists(Game.Sets.MainDir + value + @"\"))
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
    }
}
