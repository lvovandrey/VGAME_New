using LevelSetsEditor.DB;
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
        static string CardsEditorConfigFilename;
        static string AppDir;
        static string DbFilename;
        static void Main(string[] args)
        {
            if (args.Length < 3)
            {
                Console.WriteLine("Настройка конфигурационных файлов невозможна... придется Вам самим настраивать приложение (картинки фона и т.п. выбирать, базу данных создавать новую). Нажмите любую клавишу для продолжения.");
                Console.ReadKey();
                return;
            }
            AppConfigFilename = args[0];
            AppDir = args[1];
            CardsEditorConfigFilename = args[2];
            DbFilename = args[3];

            Console.WriteLine(AppConfigFilename + " " + AppDir + " " + DbFilename);
            Console.ReadKey();
            string DBCardsImagesDir = Path.Combine(AppDir, "Images", "Fruits");

            //ValidAppSettings.Add("BackgroundFilename", Path.Combine(AppDir, "Images", "back.jpg"));
            //ValidAppSettings.Add("BackgroundStartFilename", Path.Combine(AppDir, "Images", "NewBack.jpg"));
            //ValidAppSettings.Add("BackgroundMenuFilename", Path.Combine(AppDir, "Images", "back.jpg"));
            //ValidAppSettings.Add("BackgroundGameOverFilename", Path.Combine(AppDir, "Images", "back.jpg"));
            //ValidAppSettings.Add("MusicFilenames", Path.Combine(AppDir, "Music", "Music.mp3"));
            //ValidAppSettings.Add("AttachedDBCardsFilename", Path.Combine(AppDir, "Data", "Fruits.db"));

            //Console.WriteLine("Настройка конфигурационных файлов...");
            //ConfigurateFiles(AppConfigFilename, ValidAppSettings);

            Console.WriteLine("Настройка базы данных по-умолчанию...");
            DBRewrite(DbFilename, DBCardsImagesDir, DBCardsImagesDir);

        }

        private static void DBRewrite(string filename, string dBCardsImagesDir, string dBLevelsImagesDir)
        {
            try
            {
                CardsEditor.DB.DBTools.LoadDB(filename);
                foreach (var card in CardsEditor.DB.DBTools.Context.Cards)
                {
                    card.ImageAddress = Path.Combine(dBCardsImagesDir, Path.GetFileName(card.ImageAddress));
                }

                foreach (var level in CardsEditor.DB.DBTools.Context.Levels)
                {
                    level.ImageAddress = Path.Combine(dBLevelsImagesDir, Path.GetFileName(level.ImageAddress));
                }

                CardsEditor.DB.DBTools.Context.SaveChanges();
            }
            catch 
            {
                Console.WriteLine("Ошибка перезаписи базы данных.");
            }
        }

        static void ConfigurateFiles(string filename, Dictionary<string, string> validAppSettings)
        {
            try
            {
                XmlDocument xDoc = new XmlDocument();
                xDoc.Load(filename);
                XmlElement xRoot = xDoc.DocumentElement;
                // обход всех узлов в корневом элементе
                foreach (XmlNode xnode in xRoot)
                {
                    // получаем атрибут name
                    if (xnode.Name == "appSettings")
                    {
                        foreach (XmlNode childnode in xnode.ChildNodes)
                        {
                            if (childnode.Attributes.Count > 0)
                            {
                                XmlNode attrkey = childnode.Attributes.GetNamedItem("key");

                                foreach (var appsettings in validAppSettings)
                                {
                                    if (attrkey != null && attrkey.Value == appsettings.Key)
                                    {
                                        XmlNode attrval = childnode.Attributes.GetNamedItem("value");
                                        if (attrval != null)
                                            attrval.Value = appsettings.Value;
                                    }
                                }

                            }
                        }
                    }
                }
                xDoc.Save(filename);
            }
            catch (Exception e) 
            {
                Console.WriteLine("Ошибка настройки конфигурационного файла: "+ e.Message);
            }
        }
    }
}
