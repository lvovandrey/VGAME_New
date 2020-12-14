using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Speech.Synthesis;
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
        System.Windows.Controls.ListView musicFilenamesListView;
        #region CONSTRUCTOR
        public SettingsWindowVM(SettingsWindow _settingsWindow, System.Windows.Controls.ListView _musicFilenamesListView)
        {
            settingsWindow = _settingsWindow;
            musicFilenamesListView = _musicFilenamesListView;
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



        public ObservableCollection<string> TextToSpeachVoicesNames
        {
            get
            {
                var voices = Settings.TextToSpeachVoices;
                return new ObservableCollection<string>(from v in voices select v.VoiceInfo.Name);
            }
        }


        public string TTSVoiceName
        {
            get
            {
                return Settings.TTSVoiceName;
            }
            set
            {
                if (TextToSpeachVoicesNames.Contains(value))
                {
                    Settings.TTSVoiceName = value;
                    OnPropertyChanged("TTSVoiceName");
                }
            }
        }


        public int TTSVoiceRate
        {
            get { return Settings.TTSVoiceRate; }
            set { Settings.TTSVoiceRate = value; OnPropertyChanged("TTSVoiceRate"); }
        }

        public int TTSVoiceSlowRate
        {
            get { return Settings.TTSVoiceSlowRate; }
            set { Settings.TTSVoiceSlowRate = value; OnPropertyChanged("TTSVoiceSlowRate"); }
        }

        public int TTSVoiceVolume
        {
            get { return Settings.TTSVoiceVolume; }
            set { Settings.TTSVoiceVolume = value; OnPropertyChanged("TTSVoiceVolume"); }
        }


        public string BackgroundFilename
        {
            get { return Settings.BackgroundFilename; }
            set { Settings.BackgroundFilename = value; OnPropertyChanged("BackgroundFilename"); }
        }

        public string BackgroundGameOverFilename
        {
            get { return Sets.Settings.BackgroundGameOverFilename; }
            set { Sets.Settings.BackgroundGameOverFilename = value; OnPropertyChanged("BackgroundGameOverFilename"); }
        }

        public string BackgroundMenuFilename
        {
            get { return Sets.Settings.BackgroundMenuFilename; }
            set { Sets.Settings.BackgroundMenuFilename = value; OnPropertyChanged("BackgroundMenuFilename"); }
        }

        public string BackgroundStartFilename
        {
            get { return Sets.Settings.BackgroundStartFilename; }
            set { Sets.Settings.BackgroundStartFilename = value; OnPropertyChanged("BackgroundStartFilename"); }
        }



        public MusicInfo SelectedMusicInfo
        {
            get;
            set;
        }

        public ObservableCollection<MusicInfo> MusicInfos
        {
            get
            {
                return new ObservableCollection<MusicInfo>(from m in MusicFilenames select new MusicInfo(m));
            }
        }

        public ObservableCollection<string> MusicFilenames
        {
            get
            {
                return Settings.MusicFilenames;
            }
        }

        public class MusicInfo
        {
            public MusicInfo(string fullfilename)
            {
                FullFileName = fullfilename;
                IsExist = File.Exists(fullfilename);
                ShortFileName = Path.GetFileName(fullfilename);
            }
            public bool IsExist { get; private set; }
            public string ShortFileName { get; private set; }
            public string FullFileName { get; set; }
        }


        public bool ShuffleMusic
        {
            get { return Settings.ShuffleMusic; }
            set { Settings.ShuffleMusic = value; OnPropertyChanged("ShuffleMusic"); }
        }

        public bool RepeatMusicPlaylist
        {
            get { return Settings.RepeatMusicPlaylist; }
            set { Settings.RepeatMusicPlaylist = value; OnPropertyChanged("RepeatMusicPlaylist"); }
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
            OnPropertyChanged("BackgroundFilename");

            OnPropertyChanged("BackgroundGameOverFilename");
            OnPropertyChanged("BackgroundMenuFilename");
            OnPropertyChanged("BackgroundStartFilename");

            OnPropertyChanged("AttachedDBCardsFilename");

            OnPropertyChanged("TextToSpeachVoicesNames");
            OnPropertyChanged("TTSVoiceName");
            OnPropertyChanged("TTSVoiceRate");
            OnPropertyChanged("TTSVoiceSlowRate");
            OnPropertyChanged("TTSVoiceVolume");

            OnPropertyChanged("MusicFilenames");
            OnPropertyChanged("MusicInfos");
            OnPropertyChanged("ShuffleMusic");
            OnPropertyChanged("RepeatMusicPlaylist");
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
                          openFileDialog.Filter = "Файлы базы данных(*.db)|*.db";
                          openFileDialog.ValidateNames = false;
                          if (openFileDialog.ShowDialog() == DialogResult.OK)
                          {
                              AttachedDBCardsFilename = @openFileDialog.FileName;
                          }
                      }
                  }));
            }
        }


        private string ChooseFilename(string CurFilename, string filter, string title)
        {
            string NewFilename = CurFilename;
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = filter;
                openFileDialog.Title = title;
                openFileDialog.ValidateNames = false;
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    NewFilename = @openFileDialog.FileName;
                }
            }
            return NewFilename;
        }

        private RelayCommand chooseBackgroundFilename;
        public RelayCommand ChooseBackgroundFilename
        {
            get
            {
                return chooseBackgroundFilename ??
                  (chooseBackgroundFilename = new RelayCommand(obj =>
                  {
                      BackgroundFilename = ChooseFilename(BackgroundFilename, "Файлы изображений (*.bmp, *.jpg, *.png)|*.bmp;*.jpg;*.png", "Выбор файла для фона");
                  }));
            }
        }

        private RelayCommand chooseBackgroundGameOverFilename;
        public RelayCommand ChooseBackgroundGameOverFilename
        {
            get
            {
                return chooseBackgroundGameOverFilename ??
                  (chooseBackgroundGameOverFilename = new RelayCommand(obj =>
                  {
                      BackgroundGameOverFilename = ChooseFilename(BackgroundGameOverFilename, "Файлы изображений (*.bmp, *.jpg, *.png)|*.bmp;*.jpg;*.png", "Выбор файла для конечного фона");
                  }));
            }
        }

        private RelayCommand сhooseBackgroundMenuFilename;
        public RelayCommand ChooseBackgroundMenuFilename
        {
            get
            {
                return сhooseBackgroundMenuFilename ??
                  (сhooseBackgroundMenuFilename = new RelayCommand(obj =>
                  {
                      BackgroundMenuFilename = ChooseFilename(BackgroundMenuFilename, "Файлы изображений (*.bmp, *.jpg, *.png)|*.bmp;*.jpg;*.png", "Выбор файла для фона меню");
                  }));
            }
        }

        private RelayCommand сhooseBackgroundStartFilename;
        public RelayCommand ChooseBackgroundStartFilename
        {
            get
            {
                return сhooseBackgroundStartFilename ??
                  (сhooseBackgroundStartFilename = new RelayCommand(obj =>
                  {
                      BackgroundStartFilename = ChooseFilename(BackgroundStartFilename, "Файлы изображений (*.bmp, *.jpg, *.png)|*.bmp;*.jpg;*.png", "Выбор файла для первичного фона");
                  }));
            }
        }



        private RelayCommand addMusicFilenameCommand;
        public RelayCommand AddMusicFilenameCommand
        {
            get
            {
                return addMusicFilenameCommand ??
                  (addMusicFilenameCommand = new RelayCommand(obj =>
                  {
                      MusicFilenames.Add(ChooseFilename(BackgroundStartFilename, "Файлы .mp3 (*.mp3)|*.mp3", "Выбор файлов для музыкального сопровождения"));
                      OnPropertyChanged("MusicFilenames");
                      OnPropertyChanged("MusicInfos");
                  }));
            }
        }

        private RelayCommand removeMusicFilenameCommand;
        public RelayCommand RemoveMusicFilenameCommand
        {
            get
            {
                return removeMusicFilenameCommand ??
                  (removeMusicFilenameCommand = new RelayCommand(obj =>
                  {
                      if (SelectedMusicInfo == null) return;
                      if (MusicFilenames.Contains(SelectedMusicInfo.FullFileName))
                          MusicFilenames.Remove(SelectedMusicInfo.FullFileName);
                      OnPropertyChanged("MusicFilenames");
                      OnPropertyChanged("MusicInfos");
                  }));
            }
        }

        private RelayCommand openMusicFileInExplorerCommand;
        public RelayCommand OpenMusicFileInExplorerCommand
        {
            get
            {
                return openMusicFileInExplorerCommand ??
                  (openMusicFileInExplorerCommand = new RelayCommand(obj =>
                  {
                      if (SelectedMusicInfo == null) return;
                      Miscellanea.OpenFileInExplorer(SelectedMusicInfo.FullFileName);
                  }));
            }
        }

        private RelayCommand upMusicFilenameCommand;
        public RelayCommand UpMusicFilenameCommand
        {
            get
            {
                return upMusicFilenameCommand ??
                  (upMusicFilenameCommand = new RelayCommand(obj =>
                  {
                      if (SelectedMusicInfo == null) return;
                      var selected = SelectedMusicInfo.FullFileName;
                      List<string> mf = new List<string>(MusicFilenames);
                      var oldPos = mf.IndexOf(selected);
                      if (oldPos == 0) return;
                      string prevElement = mf[oldPos - 1];
                      mf[oldPos - 1] = selected;
                      mf[oldPos] = prevElement;
                      Settings._MusicFilenames = new ObservableCollection<string>(mf);
                      
                      RefreshAllDependencyProperties();
                      musicFilenamesListView.SelectedIndex = oldPos - 1;
                  }));
            }
        }

        private RelayCommand downMusicFilenameCommand;
        public RelayCommand DownMusicFilenameCommand
        {
            get
            {
                return downMusicFilenameCommand ??
                  (downMusicFilenameCommand = new RelayCommand(obj =>
                  {
                      if (SelectedMusicInfo == null) return;
                      var selected = SelectedMusicInfo.FullFileName;
                      List<string> mf = new List<string>(MusicFilenames);
                      var oldPos = mf.IndexOf(selected);
                      if (oldPos >= mf.Count-1) return;
                      string nextElement = mf[oldPos + 1];
                      mf[oldPos + 1] = selected;
                      mf[oldPos] = nextElement;
                      Settings._MusicFilenames = new ObservableCollection<string>(mf);
                      
                      RefreshAllDependencyProperties();
                      musicFilenamesListView.SelectedIndex = oldPos + 1;
                  }));
            }
        }


        #endregion

    }
}
