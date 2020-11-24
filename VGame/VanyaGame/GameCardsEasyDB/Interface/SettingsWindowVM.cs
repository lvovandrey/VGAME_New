using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VanyaGame.Abstract;
using VanyaGame.GameCardsEasyDB.Tools;

namespace VanyaGame.GameCardsEasyDB.Interface
{
    public class SettingsWindowVM : INPCBase
    {
        SettingsWindow settingsWindow;
        #region CONSTRUCTOR
        public SettingsWindowVM(SettingsWindow _settingsWindow)
        {
            settingsWindow = _settingsWindow;
            RestoreSettingsCommand.Execute(null);
            settingsWindow.DataContext = this;

            settingsWindow.Loaded += SettingsWindowView_Loaded;
            settingsWindow.Activated += SettingsWindowView_Activated;

        }
        #endregion

        #region PROPERTIES

        
            

        public bool VisualHintEnable
        {
            get { return Settings.VisualHintEnable; }
            set { Settings.VisualHintEnable = value; OnPropertyChanged("VisualHintEnable"); }
        }

        public double SpeakAgainCardNameDelay
        {
            get { return Settings.SpeakAgainCardNameDelay; }
            set { Settings.SpeakAgainCardNameDelay = value; OnPropertyChanged("SpeakAgainCardNameDelay"); }
        }

        public bool EducationModeEnable
        {
            get { return Settings.EducationModeEnable; }
            set { Settings.EducationModeEnable = value; OnPropertyChanged("EducationModeEnable"); }
        }

        public double SpeakAgainCardNameTimePeriod
        {
            get { return Settings.SpeakAgainCardNameTimePeriod; }
            set { Settings.SpeakAgainCardNameTimePeriod = value; OnPropertyChanged("SpeakAgainCardNameTimePeriod"); }
        }

        public double VisualHintDelay
        {
            get { return Settings.VisualHintDelay; }
            set { Settings.VisualHintDelay = value; OnPropertyChanged("VisualHintDelay"); }
        }

        public double VisualHintTimePeriod
        {
            get { return Settings.VisualHintTimePeriod; }
            set { Settings.VisualHintTimePeriod = value; OnPropertyChanged("VisualHintTimePeriod"); }
        }

        public double VisualHintDuration
        {
            get { return Settings.VisualHintDuration; }
            set { Settings.VisualHintDuration = value; OnPropertyChanged("VisualHintDuration"); }
        }

        public double EducationVisualHintDelay
        {
            get { return Settings.EducationVisualHintDelay; }
            set { Settings.EducationVisualHintDelay = value; OnPropertyChanged("EducationVisualHintDelay"); }
        }

        public double EducationVisualHintTimePeriod
        {
            get { return Settings.EducationVisualHintTimePeriod; }
            set { Settings.EducationVisualHintTimePeriod = value; OnPropertyChanged("EducationVisualHintTimePeriod"); }
        }

        public double EducationVisualHintDuration
        {
            get { return Settings.EducationVisualHintDuration; }
            set { Settings.EducationVisualHintDuration = value; OnPropertyChanged("EducationVisualHintDuration"); }
        }





        

        #endregion

        #region METHODS
        private void SettingsWindowView_Activated(object sender, EventArgs e)
        {
            //           RestoreSettingsCommand.Execute(null);
        }

        private void SettingsWindowView_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            RestoreSettingsCommand.Execute(null);
        }
        #endregion

        #region COMMANDS
        private RelayCommand saveSettingsCommand;
        public RelayCommand SaveSettingsCommand
        {
            get
            {
                return saveSettingsCommand ??
                  (saveSettingsCommand = new RelayCommand(obj =>
                  {
                      Settings.SaveAllSettings();
                  }));
            }
        }

        private RelayCommand restoreSettingsCommand;
        public RelayCommand RestoreSettingsCommand
        {
            get
            {
                return restoreSettingsCommand ??
                  (restoreSettingsCommand = new RelayCommand(obj =>
                  {
                      Settings.RestoreAllSettings();


                       OnPropertyChanged("VisualHintEnable");
                       OnPropertyChanged("EducationModeEnable");
                       OnPropertyChanged("SpeakAgainCardNameDelay");
                       OnPropertyChanged("SpeakAgainCardNameTimePeriod");
                       OnPropertyChanged("VisualHintDelay");
                       OnPropertyChanged("VisualHintTimePeriod");
                       OnPropertyChanged("VisualHintDuration");
                       OnPropertyChanged("EducationVisualHintDelay");
                       OnPropertyChanged("EducationVisualHintTimePeriod");
                       OnPropertyChanged("EducationVisualHintDuration");
                  }));
            }
        }
        #endregion

    }
}
