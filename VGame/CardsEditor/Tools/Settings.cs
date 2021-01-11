using CardsEditor.Abstract;
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
using System.Xml.Serialization;

namespace CardsEditor.Tools
{
    [Serializable]
    public class Settings: INPCBase
    {

        //Сделаем этот класс синглтоном
        private static Settings instance;
        private static object syncRoot = new object();

        private Settings(bool NeedRestore)
        {
            _TextToSpeachVoices = new SpeechSynthesizer().GetInstalledVoices(new CultureInfo("ru-RU"));
            TTSVoice = _TextToSpeachVoices.First();
            if (NeedRestore) RestoreAllSettings();
        }

        private Settings()
        {
            _TextToSpeachVoices = new SpeechSynthesizer().GetInstalledVoices(new CultureInfo("ru-RU"));
            TTSVoice = _TextToSpeachVoices.First();
        }

        public static Settings GetInstance()
        {
            if (instance == null)
            {
                lock (syncRoot)
                {
                    if (instance == null)
                        instance = new Settings(true);
                }
            }
            return instance;
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
                    MessageBox.Show("Громкость должна быть целым числом от 0 до 100");
                    return;
                }
                _TTSVoiceVolume = value.ToString();
            }
        }

       
        public void ExportSettingsToXML(string filename)
        {
            XmlSerializer formatter = new XmlSerializer(typeof(Settings));
            using (FileStream fs = new FileStream(filename, FileMode.Create))
            {
                formatter.Serialize(fs, this);
            }
        }

        public void ImportSettingsToXML(string filename)
        {
            XmlSerializer formatter = new XmlSerializer(typeof(Settings));
            if (!File.Exists(filename)) { MessageBox.Show("Файл настроек " + filename + " не найден.", "Ошибка"); return; }
            using (FileStream fs = new FileStream(filename, FileMode.Open))
            {
                var settings = (Settings)formatter.Deserialize(fs);
                instance = settings;
            }
            if (instance == null) { MessageBox.Show("Ошибка открытия файла настроек. Десериализатор вернул null."); return; }
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
    }
}
