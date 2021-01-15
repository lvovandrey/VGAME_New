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
        static string AppDataDir;
        static string DefaultDbFilename;
        static string DBCardsImagesDir;
        static void Main(string[] args)
        {
            AppDataDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "VGame");
            AppConfigFilename = Path.Combine(AppDataDir, "VGame","VGame.Config.xml");
            CardsEditorConfigFilename = Path.Combine(AppDataDir, "VGame", "CardsEditor.Config.xml");
            DefaultDbFilename = Path.Combine(AppDataDir, "VGame", "Data", "Fruits.db");
            DBCardsImagesDir = Path.Combine(AppDataDir, "Images", "Fruits");

            Console.WriteLine("Файл настроек VGame: {0} \n Файл настроек CardsEditor: {1} \n Папка хранения данных: {2} \n Тестовая БД: {3}",
                AppConfigFilename, CardsEditorConfigFilename, AppDataDir, DefaultDbFilename);
            Console.ReadKey();
           

            ValidAppSettings.Add("BackgroundFilename", Path.Combine(AppDataDir, "Images", "back.jpg"));
            ValidAppSettings.Add("BackgroundStartFilename", Path.Combine(AppDataDir, "Images", "NewBack.jpg"));
            ValidAppSettings.Add("BackgroundMenuFilename", Path.Combine(AppDataDir, "Images", "back.jpg"));
            ValidAppSettings.Add("BackgroundGameOverFilename", Path.Combine(AppDataDir, "Images", "back.jpg"));
            ValidAppSettings.Add("AttachedDBCardsFilename", Path.Combine(AppDataDir, "Data", "Fruits.db"));

            Console.WriteLine("Настройка конфигурационных файлов...");
            ConfigurateFiles(AppConfigFilename, ValidAppSettings);

            Console.WriteLine("Настройка записей о музыкальных файлах...");
            ConfigurateMusicFiles(AppConfigFilename, Path.Combine(AppDataDir, "Music", "Music.mp3"), "_MusicFilenames");

            Console.WriteLine("Настройка тестовой базы данных с примерами карточек...");
            DBRewrite(DefaultDbFilename, DBCardsImagesDir, DBCardsImagesDir);

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

                foreach (XmlNode xnode in xRoot)
                {
                    foreach (var setting in validAppSettings)
                    {
                        if (xnode.Name == setting.Key)
                        {
                            xnode.InnerText = setting.Value;
                        }
                    }
                }
                xDoc.Save(filename);
            }
            catch (Exception e)
            {
                Console.WriteLine("Ошибка настройки конфигурационного файла: " + e.Message);
            }
        }

        static void ConfigurateMusicFiles(string configFilename, string musicFilename, string musicFilesNode)
        {
            try
            {
                XmlDocument xDoc = new XmlDocument();
                xDoc.Load(configFilename);
                XmlElement xRoot = xDoc.DocumentElement;

                foreach (XmlNode xnode in xRoot)
                {
                    if (xnode.Name == musicFilesNode)
                    {
                        xnode.RemoveAll();
                        XmlElement stringElem = xDoc.CreateElement("string");
                        XmlText stringText = xDoc.CreateTextNode(musicFilename);
                        stringElem.AppendChild(stringText);
                        xnode.AppendChild(stringElem);
                        break;
                    }
                }
                xDoc.Save(configFilename);
            }
            catch (Exception e)
            {
                Console.WriteLine("Ошибка настройки конфигурационного файла: " + e.Message);
            }
        }
    }
}
