using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using System.Xml.Serialization;

namespace VanyaGame.GameCardsNewDB.Tools
{

    [Serializable]
    public class Settings
    {
        //Сделаем этот класс синглтоном
        private static Settings instance;
        private static object syncRoot = new object();

        private Settings()
        {
            _MusicFilenames = new ObservableCollection<string>();
            _RecentlyOpenFilenames = new ObservableCollection<string>();
            SetTTSVoices();
        }

        public static Settings GetInstance()
        {
            if (instance == null)
            {
                lock (syncRoot)
                {
                    if (instance == null)
                        instance = new Settings();
                }
            }
            return instance;
        }

        /// <summary>
        /// Событие возникает когда изменяются настройки. На него рекомендуется вешать все изменения которые завязаны с настройками.
        /// </summary>
        public event Action SettingsChanged;

        [XmlIgnore]
        public ObservableCollection<string> _RecentlyOpenFilenames;
        public ObservableCollection<string> RecentlyOpenFilenames
        {
            get
            {
                return _RecentlyOpenFilenames;
            }
        }

        [XmlIgnore]
        public string visualHintEnable = "False";
        public bool VisualHintEnable
        {
            get
            {
                if (visualHintEnable == "True")
                    return true;
                else return false;
            }
            set
            {
                if (value)
                    visualHintEnable = "True";
                else
                    visualHintEnable = "False";
            }
        }

        [XmlIgnore]
        public string educationModeEnable = "False";
        public bool EducationModeEnable
        {
            get
            {
                if (educationModeEnable == "True")
                    return true;
                else return false;
            }
            set
            {
                if (value)
                    educationModeEnable = "True";
                else
                    educationModeEnable = "False";
            }
        }

        [XmlIgnore]
        public string speakAgainCardNameDelay = "8";
        public double SpeakAgainCardNameDelay
        {
            get
            {
                double s = 8;
                if (double.TryParse(speakAgainCardNameDelay, out s)) return s;
                else return 8;
            }
            set
            {
                if (value <= 0 || value > 100)
                {
                    InfoWindow.Show("Через сколько секунд после первого озвучивания должно быть положительным числом от 0.001 до 100");
                    return;
                }
                speakAgainCardNameDelay = value.ToString();
            }
        }

        [XmlIgnore]
        public string speakAgainCardNameTimePeriod = "5";
        public double SpeakAgainCardNameTimePeriod
        {
            get
            {
                double s = 5;
                if (double.TryParse(speakAgainCardNameTimePeriod, out s)) return s;
                else return 5;
            }
            set
            {
                if (value <= 0 || value > 100)
                {
                    InfoWindow.Show("Интервал повторений, в секундах (после второго и последующих) должно быть положительным числом от 0.001 до 100");
                    return;
                }
                speakAgainCardNameTimePeriod = value.ToString();
            }
        }

        [XmlIgnore]
        public string visualHintDelay = "15";
        public double VisualHintDelay
        {
            get
            {
                double s = 15;
                if (double.TryParse(visualHintDelay, out s)) return s;
                else return 15;
            }
            set
            {
                if (value <= 0 || value > 100)
                {
                    InfoWindow.Show("Через сколько секунд после предъявления карточки должна появиться первая визуальная подсказка должно быть положительным числом от 0.001 до 100");
                    return;
                }
                visualHintDelay = value.ToString();
            }
        }

        [XmlIgnore]
        public string visualHintTimePeriod = "10";
        public double VisualHintTimePeriod
        {
            get
            {
                double s = 10;
                if (double.TryParse(visualHintTimePeriod, out s)) return s;
                else return 10;
            }
            set
            {
                if (value <= 0 || value > 100)
                {
                    InfoWindow.Show("Интервал повторений визуальной подсказки, в секундах (после второго и последующих) должно быть положительным числом от 0.001 до 100");
                    return;
                }
                visualHintTimePeriod = value.ToString();
            }
        }

        [XmlIgnore]
        public string visualHintDuration = "1";
        public double VisualHintDuration
        {
            get
            {
                double s = 1;
                if (double.TryParse(visualHintDuration, out s)) return s;
                else return 1;
            }
            set
            {
                if (value <= 0 || value > 100)
                {
                    InfoWindow.Show("Длительность предъявления визуальной подсказки (сколько секунд она будет видна) должно быть положительным числом от 0.001 до 100");
                    return;
                }
                visualHintDuration = value.ToString();
            }
        }

        [XmlIgnore]
        public string EducationvisualHintDelay = "3";
        public double EducationVisualHintDelay
        {
            get
            {
                double s = 3;
                if (double.TryParse(EducationvisualHintDelay, out s)) return s;
                else return 3;
            }
            set
            {
                if (value <= 0 || value > 100)
                {
                    InfoWindow.Show("Через сколько секунд после предъявления карточки должна появиться первая визуальная подсказка должно быть положительным числом от 0.001 до 100");
                    return;
                }
                EducationvisualHintDelay = value.ToString();
            }
        }

        [XmlIgnore]
        public string EducationvisualHintTimePeriod = "2";
        public double EducationVisualHintTimePeriod
        {
            get
            {
                double s = 2;
                if (double.TryParse(EducationvisualHintTimePeriod, out s)) return s;
                else return 2;
            }
            set
            {
                if (value <= 0 || value > 100)
                {
                    InfoWindow.Show("Интервал повторений, в секундах (после второго и последующих) должно быть положительным числом от 0.001 до 100");
                    return;
                }
                EducationvisualHintTimePeriod = value.ToString();
            }
        }

        [XmlIgnore]
        public string EducationvisualHintDuration = "1";
        public double EducationVisualHintDuration
        {
            get
            {
                double s = 1;
                if (double.TryParse(EducationvisualHintDuration, out s)) return s;
                else return 1;
            }
            set
            {
                if (value <= 0 || value > 100)
                {
                    InfoWindow.Show("Длительность предъявления визуальной подсказки (сколько секунд она будет видна) должно быть положительным числом от 0.001 до 100");
                    return;
                }
                EducationvisualHintDuration = value.ToString();
            }
        }

        [XmlIgnore]
        private string firstQuestionText = "";
        public string FirstQuestionText
        {
            get
            {
                if (firstQuestionText == "" || firstQuestionText == "Not Found")
                    firstQuestionText = "Покажи где - ";
                return firstQuestionText;
            }
            set
            {
                if (value == "Not Found") firstQuestionText = " ";
                firstQuestionText = value;
            }
        }

        [XmlIgnore]
        private string hintQuestionText = "";
        public string HintQuestionText
        {
            get
            {
                if (hintQuestionText == "Not Found")
                    hintQuestionText = "";
                return hintQuestionText;
            }
            set
            {
                if (value == "Not Found") hintQuestionText = " ";
                hintQuestionText = value;
            }
        }

        [XmlIgnore]
        private string successTestText = "";
        public string SuccessTestText
        {
            get
            {
                if (successTestText == "" || successTestText == "Not Found")
                    successTestText = "Молодец! Это -  ";
                return successTestText;
            }
            set
            {
                if (value == "Not Found") successTestText = " ";
                successTestText = value;
            }
        }

        [XmlIgnore]
        private string fallTestText = "";
        public string FallTestText
        {
            get
            {
                if (fallTestText == "" || fallTestText == "Not Found")
                    fallTestText = "Не правильно! Попробуй еще раз.";
                return fallTestText;
            }
            set
            {
                if (value == "Not Found") fallTestText = " ";
                fallTestText = value;
            }
        }

        [XmlIgnore]
        public string cardSize = "250";
        public double CardSize
        {
            get
            {
                double s = 250;
                if (double.TryParse(cardSize, out s)) return s;
                else return 250;
            }
            set
            {
                if (value <= 0 || value > 1000)
                {
                    InfoWindow.Show("Размер карточек должно быть положительным числом от 0.001 до 1000");
                    return;
                }
                cardSize = value.ToString();
            }
        }


        [XmlIgnore]
        public string cardMargin = "30";
        public double CardMargin
        {
            get
            {
                double s = 30;
                if (double.TryParse(cardMargin, out s)) return s;
                else return 30;
            }
            set
            {
                if (value <= 0 || value > 300)
                {
                    InfoWindow.Show("Зазор между карточками должен быть положительным числом от 0.001 до 300");
                    return;
                }
                cardMargin = value.ToString();
            }
        }

        [XmlIgnore]
        public string cardSuccesSize = "500";
        public double CardSuccesSize
        {
            get
            {
                double s = 500;
                if (double.TryParse(cardSuccesSize, out s)) return s;
                else return 500;
            }
            set
            {
                if (value <= 0 || value > 2000)
                {
                    InfoWindow.Show("Размер увеличенной карточки после правильного ответа должно быть положительным числом от 0.001 до 2000");
                    return;
                }
                cardSuccesSize = value.ToString();
            }
        }

        [XmlIgnore]
        public string cardSuccesTime = "3";
        public double CardSuccesTime
        {
            get
            {
                double s = 3;
                if (double.TryParse(cardSuccesTime, out s)) return s;
                else return 3;
            }
            set
            {
                if (value <= 0 || value > 100)
                {
                    InfoWindow.Show("Время предъявления увеличенной карточки после правильного ответа должно быть положительным числом от 0.001 до 100");
                    return;
                }
                cardSuccesTime = value.ToString();
            }
        }

        [XmlIgnore]
        private string defaultDBCardsFilename => VanyaGame.Sets.Settings.GetInstance().LocalAppDataDir + @"\Data\Fruits.db";
        [XmlIgnore]
        string attachedDBCardsFilename = "";
        public string AttachedDBCardsFilename
        {
            get
            {
                if (attachedDBCardsFilename == "" || attachedDBCardsFilename == "Not Found")
                    attachedDBCardsFilename = VanyaGame.Sets.Settings.GetInstance().LocalAppDataDir + @"\Data\Fruits.db";
                return attachedDBCardsFilename;
            }
            set
            {
                if (!File.Exists(value) || Path.GetExtension(value) != ".db")
                {
                    InfoWindow.Show("Нужно выбрать существующий файл типа *.db (SQLite-бд) для базы данных уровней");
                    return;
                }
                attachedDBCardsFilename = value;
            }
        }

        [XmlIgnore]
        public string cardWrongPauseTime = "3";
        public double CardWrongPauseTime
        {
            get
            {
                double s = 3;
                if (double.TryParse(cardWrongPauseTime, out s)) return s;
                else return 3;
            }
            set
            {
                if (value <= 0 || value > 100)
                {
                    InfoWindow.Show("Время паузы после НЕправильного ответа должно быть положительным числом от 0.001 до 100");
                    return;
                }
                cardWrongPauseTime = value.ToString();
            }
        }

        [XmlIgnore]
        public string cardSuccesSpeakAgainTime = "3";
        public double CardSuccesSpeakAgainTime
        {
            get
            {
                double s = 3;
                if (double.TryParse(cardSuccesSpeakAgainTime, out s)) return s;
                else return 3;
            }
            set
            {
                if (value <= 0 || value > 100)
                {
                    InfoWindow.Show("Время повторного озвучивания карточки после правильного ответа должно быть положительным числом от 0.001 до 100");
                    return;
                }
                cardSuccesSpeakAgainTime = value.ToString();
            }
        }

        [XmlIgnore]
        private ReadOnlyCollection<InstalledVoice> _TextToSpeachVoices;
        [XmlIgnore]
        public ObservableCollection<InstalledVoice> TextToSpeachVoices
        {
            get
            {
                return new ObservableCollection<InstalledVoice>(_TextToSpeachVoices);
            }
        }


        public string TTSVoiceName
        {
            get
            {
                string name = "";
                if (_TTSVoice != null)
                    name = _TTSVoice.VoiceInfo.Name;
                return name;
            }
            set
            {
                var TextToSpeachVoicesNames = from v in TextToSpeachVoices select v.VoiceInfo.Name;
                if (TextToSpeachVoicesNames.Contains(value))
                {
                    TTSVoice = (from v in TextToSpeachVoices where (v.VoiceInfo.Name == value) select v).First();
                }
            }
        }

        [XmlIgnore]
        private InstalledVoice _TTSVoice;
        [XmlIgnore]
        public InstalledVoice TTSVoice
        {
            get { return _TTSVoice; }
            set
            {
                _TTSVoice = value;
            }
        }

        [XmlIgnore]
        public string _TTSVoiceRate = "0";
        public int TTSVoiceRate
        {
            get
            {
                int s = 0;
                if (int.TryParse(_TTSVoiceRate, out s)) return s;
                else return 0;
            }
            set
            {
                if (value < -10 || value > 10)
                {
                    InfoWindow.Show("Темп речи должен быть целым числом от -10 до 10");
                    return;
                }
                _TTSVoiceRate = value.ToString();
            }
        }

        [XmlIgnore]
        public string _TTSVoiceSlowRate = "-2";
        public int TTSVoiceSlowRate
        {
            get
            {
                int s = -2;
                if (int.TryParse(_TTSVoiceSlowRate, out s)) return s;
                else return -2;
            }
            set
            {
                if (value < -10 || value > 10)
                {
                    InfoWindow.Show("Темп речи должен быть целым числом от -10 до 10");
                    return;
                }
                _TTSVoiceSlowRate = value.ToString();
            }
        }

        [XmlIgnore]
        public string _TTSVoiceVolume = "100";
        public int TTSVoiceVolume
        {
            get
            {
                int s = 100;
                if (int.TryParse(_TTSVoiceVolume, out s)) return s;
                else return 100;
            }
            set
            {
                if (value < 0 || value > 100)
                {
                    InfoWindow.Show("Громкость должна быть целым числом от 0 до 100");
                    return;
                }
                _TTSVoiceVolume = value.ToString();
            }
        }

        [XmlIgnore]
        string backgroundFilename = "";
        public string BackgroundFilename
        {
            get
            {
                if (backgroundFilename == "" || backgroundFilename == "Not Found")
                    backgroundFilename = VanyaGame.Sets.Settings.GetInstance().LocalAppDataDir + @"\Images\back.jpg";
                return backgroundFilename;
            }
            set
            {
                if (!File.Exists(value))
                {
                    InfoWindow.Show("Нужно выбрать существующий файл изображения для фона");
                    return;
                }
                backgroundFilename = value;
            }
        }

        [XmlIgnore]
        public ObservableCollection<string> _MusicFilenames;
        //[XmlIgnore]
        public ObservableCollection<string> MusicFilenames
        {
            get
            {
                return _MusicFilenames;
            }
        }

        [XmlIgnore]
        public string shuffleMusic = "True";
        public bool ShuffleMusic
        {
            get
            {
                if (shuffleMusic == "True")
                    return true;
                else return false;
            }
            set
            {
                if (value)
                    shuffleMusic = "True";
                else
                    shuffleMusic = "False";
            }
        }

        [XmlIgnore]
        public string repeatMusicPlaylist = "True";
        public bool RepeatMusicPlaylist
        {
            get
            {
                if (repeatMusicPlaylist == "True")
                    return true;
                else return false;
            }
            set
            {
                if (value)
                    repeatMusicPlaylist = "True";
                else
                    repeatMusicPlaylist = "False";
            }
        }

        public string BackgroundGameOverFilename
        {
            get
            {
                return VanyaGame.Sets.Settings.GetInstance().BackgroundGameOverFilename;
            }
            set
            {
                VanyaGame.Sets.Settings.GetInstance().BackgroundGameOverFilename = value;
            }
        }

        public string BackgroundMenuFilename
        {
            get
            {
                return VanyaGame.Sets.Settings.GetInstance().BackgroundMenuFilename;
            }
            set
            {
                VanyaGame.Sets.Settings.GetInstance().BackgroundMenuFilename = value;
            }
        }

        public string BackgroundStartFilename
        {
            get
            {
                return VanyaGame.Sets.Settings.GetInstance().BackgroundStartFilename;
            }
            set
            {
                VanyaGame.Sets.Settings.GetInstance().BackgroundStartFilename = value;
            }
        }

        [XmlIgnore]
        public string ManualFilename
        {
            get
            {
                return Path.Combine(VanyaGame.Sets.Settings.GetInstance().LocalAppDataDir, "Manual.pdf");
            }           
        }

        [XmlIgnore]
        public string LicenseInfoFilename
        {
            get
            {
                return Path.Combine(VanyaGame.Sets.Settings.GetInstance().LocalAppDataDir, "License.pdf");
            }
        }
        
        [XmlIgnore]
        public string LicenseFilename
        {
            get
            {
                return Path.Combine(VanyaGame.Sets.Settings.GetInstance().LocalAppDataDir, "License GNU GPL v3.rtf");
            }
        }
        

        public void ExportSettingsToXML(string filename)
        {
            if (!Directory.Exists(Path.GetDirectoryName(filename)))
                Directory.CreateDirectory(Path.GetDirectoryName(filename));

            XmlSerializer formatter = new XmlSerializer(typeof(Settings));
            using (FileStream fs = new FileStream(filename, FileMode.Create))
            {
                formatter.Serialize(fs, this);
            }

        }

        public void ImportSettingsFromXML(string filename)
        {
            XmlSerializer formatter = new XmlSerializer(typeof(Settings));
            if (!File.Exists(filename))
            {
                if (MessageBox.Show("Файл настроек " + filename + " не найден. Создать пустой файл настроек с этим именем?", "Ошибка",
                    MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes)
                {
                    ExportSettingsToXML(filename);
                    ConfigurateMusicFiles();
                    SettingsChanged?.Invoke();
                }
                return;
            }
            try
            {
                using (FileStream fs = new FileStream(filename, FileMode.Open))
                {
                    var settings = (Settings)formatter.Deserialize(fs);
                    instance = settings;
                    SettingsChanged?.Invoke();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Ошибка открытия файла настроек. Ошибка открытия файла." + e.Message);
            }
            finally
            {
                if (instance == null)
                { MessageBox.Show("Ошибка открытия файла настроек. Десериализатор вернул null."); }
                SettingsChanged?.Invoke();
            }
        }

        internal void OpenDefaultDB()
        {
            AttachedDBCardsFilename = defaultDBCardsFilename;
        }

        internal void RestoreDefaultSettings()
        {
            var settings = new Settings();
            instance = settings;
            instance.ConfigurateMusicFiles();
            SettingsChanged?.Invoke();
        }

        internal void SaveAllSettings()
        {
            ExportSettingsToXML(ConfigurationTools.SettingsFilename);
        }

        internal void RestoreAllSettings()
        {
            ImportSettingsFromXML(ConfigurationTools.SettingsFilename);
        }

        void ConfigurateMusicFiles()
        {
            try
            {
                string AppDataDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "VGame");
                string configFilename = Path.Combine(AppDataDir, "VGame.Config.xml");
                string musicFilename = Path.Combine(AppDataDir, "Music", "Music.mp3");
                string musicFilesNode = "MusicFilenames";

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
                if (MusicFilenames.Count == 0)
                    MusicFilenames.Add(musicFilename);
            }
            catch (Exception e)
            {
                MessageBox.Show("Файлы музыки не добавлены. Ошибка настройки конфигурационного файла: " + e.Message);
            }
        }

        public void SetTTSVoices()
        {
            SpeechSynthesizer speaker = new SpeechSynthesizer();
            _TextToSpeachVoices = speaker.GetInstalledVoices(new CultureInfo("ru-RU"));
            if (_TextToSpeachVoices.Count == 0)
            {
                System.Windows.MessageBox.Show("В Windows не установлены голоса для синтеза речи на русском языке. Установите пожалуйста, а то ничего не будет слышно. Гуглить по запросам TTS SAPI 5, MS Speach Platform ");
                _TTSVoice = null;
                return;
            }
            _TTSVoice = _TextToSpeachVoices[0];
        }

        private string PackObservableCollectionToString(ObservableCollection<string> collection)
        {
            string packedString = "";

            if (collection.Count() == 0) { return packedString; }
            packedString = collection.First();
            for (int i = 1; i < collection.Count(); i++)
            {
                packedString += "|" + collection[i];
            }
            return packedString;
        }

        private ObservableCollection<string> UnpackObservableCollectionFromString(string str)
        {
            var col = new ObservableCollection<string>(str.Split('|'));
            var colactual = new ObservableCollection<string>();
            foreach (var item in col)
            {
                if (item != "Not Found")
                    colactual.Add(item);
            }
            return colactual;
        }

        public void AddRecentlyDBFilename(string dBFilename)
        {
            if (_RecentlyOpenFilenames.Contains(dBFilename))
            {
                _RecentlyOpenFilenames.Remove(dBFilename);
                _RecentlyOpenFilenames.Add(dBFilename);
                return;
            }
            _RecentlyOpenFilenames.Add(dBFilename);
            while (_RecentlyOpenFilenames.Count > 10)
                _RecentlyOpenFilenames.Remove(_RecentlyOpenFilenames.First());
            ExportSettingsToXML(ConfigurationTools.SettingsFilename);
        }

        public void ClearRecentlyDBFilename()
        {
            _RecentlyOpenFilenames.Clear();
            ExportSettingsToXML(ConfigurationTools.SettingsFilename);
        }
    }
}
