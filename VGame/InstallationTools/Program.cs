using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace InstallationTools
{
    class Program
    {
        static Dictionary<string, string> ValidAppSettings = new Dictionary<string, string>();
        static string AppConfigFilename;
        static string AppDir;
        static void Main(string[] args)
        {
            if (args.Length < 1) Console.WriteLine("Настройка конфигурационных файлов невозможна... придется Вам самим настраивать приложение (картинки фона и т.п. выбирать)");
           
            AppConfigFilename = args[0];
            AppDir = args[1];
            ValidAppSettings.Add("BackgroundStartFilename", Path.Combine(AppDir,"Images", "NewBack.jpg"));
            ValidAppSettings.Add("BackgroundMenuFilename", Path.Combine(AppDir, "Images", "back.jpg"));
            ValidAppSettings.Add("BackgroundGameOverFilename", Path.Combine(AppDir, "Images", "back.jpg"));
            ValidAppSettings.Add("MusicFilenames", Path.Combine(AppDir, "Music", "Music.mp3"));
            ValidAppSettings.Add("AttachedDBCardsFilename", Path.Combine(AppDir, "Data", "Fruits.db"));

            Console.WriteLine("Настройка конфигурационных файлов...");
//           
                ConfigurateFiles(@"c:\1.config", "");
        }

        static void ConfigurateFiles(string filename, string appDir)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(filename);
            XmlElement xRoot = xDoc.DocumentElement;
            // обход всех узлов в корневом элементе
            foreach (XmlNode xnode in xRoot)
            {
                // получаем атрибут name
                if (xnode.Name== "appSettings")
                {
                    foreach (XmlNode childnode in xnode.ChildNodes)
                    {
                        if (childnode.Attributes.Count > 0)
                        {
                            XmlNode attrkey = childnode.Attributes.GetNamedItem("key");
                            if (attrkey != null && attrkey.Value == "BackgroundFilename")
                            {
                                XmlNode attrval = childnode.Attributes.GetNamedItem("value");
                                if (attrval != null)
                                    attrval.Value = @"C:\Users\Professional\Pictures\Юниты\vrayr-vapovao-oapoaplpr-uchebnik.jpg";
                            }
                        }
                    }
                }
                Console.WriteLine();
            }
            
            Console.Read();
            xDoc.Save(filename);
        }
    }
}
