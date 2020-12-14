using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;


namespace VanyaGame.GameCardsNewDB.Tools
{


    public static class Settings
    {
        /// <summary>
        /// Событие возникает когда изменяются настройки. На него рекомендуется вешать все изменения которые завязаны с настройками.
        /// </summary>
        public static event Action SettingsChanged;

        public static string visualHintEnable = "True";
        public static bool VisualHintEnable
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

        public static string educationModeEnable = "True";
        public static bool EducationModeEnable
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


        public static string speakAgainCardNameDelay = "8";
        public static double SpeakAgainCardNameDelay
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


        public static string speakAgainCardNameTimePeriod = "5";
        public static double SpeakAgainCardNameTimePeriod
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

        public static string visualHintDelay = "15";
        public static double VisualHintDelay
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

        public static string visualHintTimePeriod = "10";
        public static double VisualHintTimePeriod
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

        public static string visualHintDuration = "1";
        public static double VisualHintDuration
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

        public static string EducationvisualHintDelay = "3";
        public static double EducationVisualHintDelay
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

        public static string EducationvisualHintTimePeriod = "2";
        public static double EducationVisualHintTimePeriod
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

        public static string EducationvisualHintDuration = "1";
        public static double EducationVisualHintDuration
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



        private static string firstQuestionText = "";
        public static string FirstQuestionText
        {
            get
            {
                if (firstQuestionText == "" || firstQuestionText == "Not Found")
                    firstQuestionText = "Покажи где - ";
                return firstQuestionText;
            }
            set
            {
                firstQuestionText = value;
            }
        }

        private static string hintQuestionText = "";
        public static string HintQuestionText
        {
            get
            {
                return hintQuestionText;
            }
            set
            {
                hintQuestionText = value;
            }
        }

        private static string successTestText = "";
        public static string SuccessTestText
        {
            get
            {
                if (successTestText == "" || successTestText == "Not Found")
                    successTestText = "Молодец! Это -  ";
                return successTestText;
            }
            set
            {
                successTestText = value;
            }
        }

        private static string fallTestText = "";
        public static string FallTestText
        {
            get
            {
                if (fallTestText == "" || fallTestText == "Not Found")
                    fallTestText = "Не правильно! Попробуй еще раз.";
                return fallTestText;
            }
            set
            {
                fallTestText = value;
            }
        }


        public static string cardSize = "250";
        public static double CardSize
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

        public static string cardSuccesSize = "500";
        public static double CardSuccesSize
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

        public static string cardSuccesTime = "3";
        public static double CardSuccesTime
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


        static string attachedDBCardsFilename = "";
        public static string AttachedDBCardsFilename
        {
            get
            {
                if (attachedDBCardsFilename == "" || attachedDBCardsFilename == "Not Found")
                    attachedDBCardsFilename = @"C:\Users\Professional\TestBackupDB\CardsDBInsects.mdf";
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




        public static string cardWrongPauseTime = "3";
        public static double CardWrongPauseTime
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

        public static string cardSuccesSpeakAgainTime = "3";
        public static double CardSuccesSpeakAgainTime
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


        private static ReadOnlyCollection<InstalledVoice> GetTTSVoices() 
        {
            var voices = new SpeechSynthesizer().GetInstalledVoices(new CultureInfo("ru-RU"));
            TTSVoice = voices.First();

           
            return voices;
        }


        private static ReadOnlyCollection<InstalledVoice> _TextToSpeachVoices = GetTTSVoices();
        public static ObservableCollection<InstalledVoice> TextToSpeachVoices
        {
            get
            {
                return new ObservableCollection<InstalledVoice>(_TextToSpeachVoices);
            }
        }


        public static string TTSVoiceName
        {
            get
            {
                return _TTSVoice.VoiceInfo.Name;
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

        private static InstalledVoice _TTSVoice;
        public static InstalledVoice TTSVoice
        {
            get { return _TTSVoice; }
            set
            {
                _TTSVoice = value;
            }
        }


        public static string _TTSVoiceRate = "0";
        public static int TTSVoiceRate
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

        public static string _TTSVoiceSlowRate = "-2";
        public static int TTSVoiceSlowRate
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

        public static string _TTSVoiceVolume = "100";
        public static int TTSVoiceVolume
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



        static string backgroundFilename = "";
        public static string BackgroundFilename
        {
            get
            {
                if (backgroundFilename == "" || backgroundFilename == "Not Found")
                    backgroundFilename = @"J:\1.VANYA GAME\LEVELS\VanjaGame\interface\back.jpg";
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



        public static ObservableCollection<string> _MusicFilenames;
        public static ObservableCollection<string> MusicFilenames
        {
            get
            {
                return _MusicFilenames;
            }
        }


        private static string PackObservableCollectionToString(ObservableCollection<string> collection)
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

        private static ObservableCollection<string>  UnpackObservableCollectionFromString(string str)
        {
            return new ObservableCollection<string>(str.Split('|'));
        }

        public static string shuffleMusic = "True";
        public static bool ShuffleMusic
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

        public static string repeatMusicPlaylist = "True";
        public static bool RepeatMusicPlaylist
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
        static public void SaveAllSettings()
        {
            ConfigurationTools.AddUpdateAppSettings("VisualHintEnable", visualHintEnable);
            ConfigurationTools.AddUpdateAppSettings("EducationModeEnable", educationModeEnable);
            ConfigurationTools.AddUpdateAppSettings("SpeakAgainCardNameDelay", speakAgainCardNameDelay);
            ConfigurationTools.AddUpdateAppSettings("SpeakAgainCardNameTimePeriod", speakAgainCardNameTimePeriod);
            ConfigurationTools.AddUpdateAppSettings("VisualHintDelay", visualHintDelay);
            ConfigurationTools.AddUpdateAppSettings("VisualHintTimePeriod", visualHintTimePeriod);
            ConfigurationTools.AddUpdateAppSettings("VisualHintDuration", visualHintDuration);
            ConfigurationTools.AddUpdateAppSettings("EducationVisualHintDelay", EducationvisualHintDelay);
            ConfigurationTools.AddUpdateAppSettings("EducationVisualHintTimePeriod", EducationvisualHintTimePeriod);
            ConfigurationTools.AddUpdateAppSettings("EducationVisualHintDuration", EducationvisualHintDuration);

            ConfigurationTools.AddUpdateAppSettings("FirstQuestionText", FirstQuestionText);
            ConfigurationTools.AddUpdateAppSettings("HintQuestionText", HintQuestionText);
            ConfigurationTools.AddUpdateAppSettings("SuccessTestText", SuccessTestText);
            ConfigurationTools.AddUpdateAppSettings("FallTestText", FallTestText);

            ConfigurationTools.AddUpdateAppSettings("CardSize", cardSize);
            ConfigurationTools.AddUpdateAppSettings("CardSuccesSize", cardSuccesSize);
            ConfigurationTools.AddUpdateAppSettings("CardSuccesTime", cardSuccesTime);
            ConfigurationTools.AddUpdateAppSettings("CardWrongPauseTime", cardWrongPauseTime);
            ConfigurationTools.AddUpdateAppSettings("CardSuccesSpeakAgainTime", cardSuccesSpeakAgainTime);
            ConfigurationTools.AddUpdateAppSettings("BackgroundFilename", backgroundFilename);

            ConfigurationTools.AddUpdateAppSettings("AttachedDBCardsFilename", attachedDBCardsFilename);

            ConfigurationTools.AddUpdateAppSettings("TTSVoiceName", TTSVoiceName);
            ConfigurationTools.AddUpdateAppSettings("TTSVoiceRate", _TTSVoiceRate);
            ConfigurationTools.AddUpdateAppSettings("TTSVoiceSlowRate", _TTSVoiceSlowRate);
            ConfigurationTools.AddUpdateAppSettings("TTSVoiceVolume", _TTSVoiceVolume);

            ConfigurationTools.AddUpdateAppSettings("MusicFilenames", PackObservableCollectionToString(_MusicFilenames));
            ConfigurationTools.AddUpdateAppSettings("ShuffleMusic", shuffleMusic);
            ConfigurationTools.AddUpdateAppSettings("RepeatMusicPlaylist", repeatMusicPlaylist);
  
            VanyaGame.Sets.Settings.SaveAllSettings();

            SettingsChanged?.Invoke();
        }


        static public void RestoreAllSettings()
        {


            visualHintEnable = ConfigurationTools.ReadSetting("VisualHintEnable");
            educationModeEnable = ConfigurationTools.ReadSetting("EducationModeEnable");
            speakAgainCardNameDelay = ConfigurationTools.ReadSetting("SpeakAgainCardNameDelay");
            speakAgainCardNameTimePeriod = ConfigurationTools.ReadSetting("SpeakAgainCardNameTimePeriod");
            visualHintDelay = ConfigurationTools.ReadSetting("VisualHintDelay");
            visualHintTimePeriod = ConfigurationTools.ReadSetting("VisualHintTimePeriod");
            visualHintDuration = ConfigurationTools.ReadSetting("VisualHintDuration");
            EducationvisualHintDelay = ConfigurationTools.ReadSetting("EducationVisualHintDelay");
            EducationvisualHintTimePeriod = ConfigurationTools.ReadSetting("EducationVisualHintTimePeriod");
            EducationvisualHintDuration = ConfigurationTools.ReadSetting("EducationVisualHintDuration");

            FirstQuestionText = ConfigurationTools.ReadSetting("FirstQuestionText");
            HintQuestionText = ConfigurationTools.ReadSetting("HintQuestionText");
            SuccessTestText = ConfigurationTools.ReadSetting("SuccessTestText");
            FallTestText = ConfigurationTools.ReadSetting("FallTestText");

            cardSize = ConfigurationTools.ReadSetting("CardSize");
            cardSuccesSize = ConfigurationTools.ReadSetting("CardSuccesSize");
            cardSuccesTime = ConfigurationTools.ReadSetting("CardSuccesTime");
            cardWrongPauseTime = ConfigurationTools.ReadSetting("CardWrongPauseTime");
            cardSuccesSpeakAgainTime = ConfigurationTools.ReadSetting("CardSuccesSpeakAgainTime");
            backgroundFilename = ConfigurationTools.ReadSetting("BackgroundFilename");
   
            attachedDBCardsFilename = ConfigurationTools.ReadSetting("AttachedDBCardsFilename");

            TTSVoiceName = ConfigurationTools.ReadSetting("TTSVoiceName");
            _TTSVoiceRate = ConfigurationTools.ReadSetting("TTSVoiceRate");
            _TTSVoiceSlowRate = ConfigurationTools.ReadSetting("TTSVoiceSlowRate");
            _TTSVoiceVolume = ConfigurationTools.ReadSetting("TTSVoiceVolume");

            _MusicFilenames =  UnpackObservableCollectionFromString(ConfigurationTools.ReadSetting("MusicFilenames"));
            shuffleMusic = ConfigurationTools.ReadSetting("ShuffleMusic");
            repeatMusicPlaylist = ConfigurationTools.ReadSetting("RepeatMusicPlaylist");

            VanyaGame.Sets.Settings.RestoreAllSettings();

            SettingsChanged?.Invoke();
        }
    }
}
