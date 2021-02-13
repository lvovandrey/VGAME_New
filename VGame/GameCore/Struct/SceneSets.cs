using System;
using VanyaGame.Abstract;
using VanyaGame.Sets;

namespace VanyaGame.Struct
{
    public class SceneSets: ComponentContainer
    {
        private Scene Scene;
        private Level Level;
        private string dir = @"\Scene1";
        public string Directory
        {
            get
            {
                return dir;
            }
            set
            {
                string path = Settings.GetInstance().LocalAppDataDir + Level.Sets.Directory + value + @"\";
                if (System.IO.Directory.Exists(path))
                {
                    dir = value;
                }
                else
                {
                    throw new Exception("Cant initialize Scene " + Scene.Name + " directory. Directory " + value + "not existed.");
                }
            }
        }

        public SceneSets(Level level, Scene scene)
        {
            Scene = scene;
            Level = level;
        }
    }
}