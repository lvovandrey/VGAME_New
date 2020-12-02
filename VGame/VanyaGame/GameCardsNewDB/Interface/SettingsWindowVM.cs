using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VanyaGame.Abstract;
using VanyaGame.GameCardsNewDB.Tools;

namespace VanyaGame.GameCardsNewDB.Interface
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



        public string FirstQuestionText
        {
            get { return Settings.FirstQuestionText; }
            set { Settings.FirstQuestionText = value; OnPropertyChanged("FirstQuestionText"); }
        }

        public string HintQuestionText
        {
            get { return Settings.HintQuestionText; }
            set { Settings.HintQuestionText = value; OnPropertyChanged("HintQuestionText"); }
        }

        public string SuccessTestText
        {
            get { return Settings.SuccessTestText; }
            set { Settings.SuccessTestText = value; OnPropertyChanged("SuccessTestText"); }
        }

        public string FallTestText
        {
            get { return Settings.FallTestText; }
            set { Settings.FallTestText = value; OnPropertyChanged("FallTestText"); }
        }


        public double CardSize
        {
            get { return Settings.CardSize; }
            set { Settings.CardSize = value; OnPropertyChanged("CardSize"); }
        }
        public double CardSuccesSize
        {
            get { return Settings.CardSuccesSize; }
            set { Settings.CardSuccesSize = value; OnPropertyChanged("CardSuccesSize"); }
        }
        public double CardSuccesTime
        {
            get { return Settings.CardSuccesTime; }
            set { Settings.CardSuccesTime = value; OnPropertyChanged("CardSuccesTime"); }
        }

        public double CardWrongPauseTime
        {
            get { return Settings.CardWrongPauseTime; }
            set { Settings.CardWrongPauseTime = value; OnPropertyChanged("CardWrongPauseTime"); }
        }
        public double CardSuccesSpeakAgainTime
        {
            get { return Settings.CardSuccesSpeakAgainTime; }
            set { Settings.CardSuccesSpeakAgainTime = value; OnPropertyChanged("CardSuccesSpeakAgainTime"); }
        }
        
        public string AttachedDBCardsFilename
        {
            get { return Settings.AttachedDBCardsFilename; }
            set { Settings.AttachedDBCardsFilename = value; OnPropertyChanged("AttachedDBCardsFilename"); }
        }

        public string AttachedDBLevelsFilename
        {
            get { return Settings.AttachedDBLevelsFilename; }
            set { Settings.AttachedDBLevelsFilename = value; OnPropertyChanged("AttachedDBLevelsFilename"); }
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

        private void RefreshAllDependencyProperties()
        {
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

            OnPropertyChanged("FirstQuestionText");
            OnPropertyChanged("HintQuestionText");
            OnPropertyChanged("SuccessTestText");
            OnPropertyChanged("FallTestText");

            OnPropertyChanged("CardSize");
            OnPropertyChanged("CardSuccesSize");
            OnPropertyChanged("CardSuccesTime");
            OnPropertyChanged("CardWrongPauseTime");
            OnPropertyChanged("CardSuccesSpeakAgainTime");

            OnPropertyChanged("AttachedDBCardsFilename");
            OnPropertyChanged("AttachedDBLevelsFilename");
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
                      RefreshAllDependencyProperties();
                  }));
            }
        }

        private RelayCommand chooseAttachedDBCardsFilename;
        public RelayCommand ChooseAttachedDBCardsFilename
        {
            get
            {
                return chooseAttachedDBCardsFilename ??
                  (chooseAttachedDBCardsFilename = new RelayCommand(obj =>
                  {
                      using (OpenFileDialog openFileDialog = new OpenFileDialog())
                      {
                          openFileDialog.Filter = "Файлы базы данных(*.mdf)|*.mdf";
                          openFileDialog.ValidateNames = false;
                          if (openFileDialog.ShowDialog() == DialogResult.OK)
                          {
                              AttachedDBCardsFilename = @openFileDialog.FileName;
                          }
                      }
                  }));
            }
        }

        private RelayCommand chooseAttachedDBLevelsFilename;
        public RelayCommand ChooseAttachedDBLevelsFilename
        {
            get
            {
                return chooseAttachedDBLevelsFilename ??
                  (chooseAttachedDBLevelsFilename = new RelayCommand(obj =>
                  {
                      using (OpenFileDialog openFileDialog = new OpenFileDialog())
                      {
                          openFileDialog.Filter = "Файлы базы данных(*.mdf)|*.mdf";
                          openFileDialog.ValidateNames = false;
                          if (openFileDialog.ShowDialog() == DialogResult.OK)
                          {
                              AttachedDBLevelsFilename = @openFileDialog.FileName;
                          }
                      }
                  }));
            }
        }
        #endregion

    }
}
