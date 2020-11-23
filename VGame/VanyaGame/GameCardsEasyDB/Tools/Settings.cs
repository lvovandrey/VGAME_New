using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VanyaGame.GameCardsEasyDB.Tools
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

            SettingsChanged?.Invoke();
        }
    }
}
