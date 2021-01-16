using LevelSetsEditor.DB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Speech.Synthesis;
using System.Globalization;

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
            AppConfigFilename = Path.Combine(AppDataDir, "VGame.Config.xml");
            CardsEditorConfigFilename = Path.Combine(AppDataDir, "CardsEditor.Config.xml");
            DefaultDbFilename = Path.Combine(AppDataDir, "Data", "Fruits.db");
            DBCardsImagesDir = Path.Combine(AppDataDir, "Images", "Fruits");

            Console.WriteLine("Файл настроек VGame: {0} \n Файл настроек CardsEditor: {1} \n Папка хранения данных: {2} \n Тестовая БД: {3} \n Нажмите любую клавишу",
                AppConfigFilename, CardsEditorConfigFilename, AppDataDir, DefaultDbFilename);
            Console.ReadKey();
           

            ValidAppSettings.Add("BackgroundFilename", Path.Combine(AppDataDir, "Images", "back.jpg"));
            ValidAppSettings.Add("BackgroundStartFilename", Path.Combine(AppDataDir, "Images", "NewBack.jpg"));
            ValidAppSettings.Add("BackgroundMenuFilename", Path.Combine(AppDataDir, "Images", "back.jpg"));
            ValidAppSettings.Add("BackgroundGameOverFilename", Path.Combine(AppDataDir, "Images", "back.jpg"));
            ValidAppSettings.Add("AttachedDBCardsFilename", Path.Combine(AppDataDir, "Data", "Fruits.db"));

            Console.Write("Настройка конфигурационных файлов...          ");
            ConfigurateFiles(AppConfigFilename, ValidAppSettings);

            Console.Write("Настройка записей о музыкальных файлах...          ");
            ConfigurateMusicFiles(AppConfigFilename, Path.Combine(AppDataDir, "Music", "Music.mp3"), "MusicFilenames");

            Console.Write("Настройка тестовой базы данных с примерами карточек...");
            DBRewrite(DefaultDbFilename, DBCardsImagesDir, DBCardsImagesDir);

            Console.Write("Проверка наличия движка Text-to-Speech (преобразования текста в речь) и голосов..");
            VoicesCheck();

            Console.WriteLine("Если не было сообщений об ошибках (красным цветом), все в порядке. Если такие сообщения были - " +
                "сделайте скриншот экрана или фотографию, или запишите сообщение об ошибки и отошлите разработчику на почту lvovandrey@mail.ru   \n " +
                " Для продолжения установки нажмите любую клавишу)");
            Console.ReadKey();
        }

        private static void VoicesCheck()
        {
            try
            {
                SpeechSynthesizer speaker = new SpeechSynthesizer();
                var _TextToSpeachVoices = speaker.GetInstalledVoices(new CultureInfo("ru-RU"));
                if (_TextToSpeachVoices == null || _TextToSpeachVoices.Count == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("В Windows не установлены голоса для синтеза речи на русском языке. Установите пожалуйста, " +
                        "а то ничего не будет слышно. Гуглить по запросам TTS SAPI 5, MS Speach Platform. " +
                        "Развернутая справка по установке голосов - в руководстве программы");
                    Console.ResetColor();
                    return;
                }

                if (_TextToSpeachVoices.Count > 0)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("ОК.  В системе установлено {0} голосов. По умолчанию используется голос {1}", _TextToSpeachVoices.Count, _TextToSpeachVoices[0].VoiceInfo.Name);
                    Console.ResetColor();
                    return;
                }
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Не удалось определить наличие средств синтеза речи в вашей Windows. Ошибка:"+ e.Message);
                Console.WriteLine("Гуглить по запросам TTS SAPI 5, MS Speach Platform. Развернутая справка по установке голосов - в руководстве программы.");
                Console.ResetColor();
            }
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
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("OK");
                Console.ResetColor();
            }
            catch(Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Ошибка перезаписи базы данных: "+ e.Message+ "   InnerException: " + e.InnerException.Message);
                Console.ResetColor();
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
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("OK"); 
                Console.ResetColor();
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Ошибка настройки конфигурационного файла: " + e.Message);
                Console.ResetColor();
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
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("OK");
                Console.ResetColor();
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Ошибка настройки конфигурационного файла: " + e.Message);
                Console.ResetColor();
            }
        }
    }
}
