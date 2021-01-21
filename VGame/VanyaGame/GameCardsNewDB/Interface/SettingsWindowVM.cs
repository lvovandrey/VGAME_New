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
            Settings.GetInstance().SettingsChanged += SettingsWindowVM_SettingsChanged;
            Settings.GetInstance().ImportSettingsFromXML(ConfigurationTools.SettingsFilename);
            
            settingsWindow = _settingsWindow;
            musicFilenamesListView = _musicFilenamesListView;
            settingsWindow.DataContext = this;
            
            RefreshAllDependencyProperties();

            settingsWindow.Loaded += SettingsWindowView_Loaded;
            settingsWindow.Activated += SettingsWindowView_Activated;
            
        }

        private void SettingsWindowVM_SettingsChanged()
        {
            RefreshAllDependencyProperties();
        }
        #endregion

        #region PROPERTIES




        public bool VisualHintEnable
        {
            get { return Settings.GetInstance().VisualHintEnable; }
            set { Settings.GetInstance().VisualHintEnable = value; OnPropertyChanged("VisualHintEnable"); }
        }

        public double SpeakAgainCardNameDelay
        {
            get { return Settings.GetInstance().SpeakAgainCardNameDelay; }
            set { Settings.GetInstance().SpeakAgainCardNameDelay = value; OnPropertyChanged("SpeakAgainCardNameDelay"); }
        }

        public bool EducationModeEnable
        {
            get { return Settings.GetInstance().EducationModeEnable; }
            set { Settings.GetInstance().EducationModeEnable = value; OnPropertyChanged("EducationModeEnable"); }
        }

        public double SpeakAgainCardNameTimePeriod
        {
            get { return Settings.GetInstance().SpeakAgainCardNameTimePeriod; }
            set { Settings.GetInstance().SpeakAgainCardNameTimePeriod = value; OnPropertyChanged("SpeakAgainCardNameTimePeriod"); }
        }

        public double VisualHintDelay
        {
            get { return Settings.GetInstance().VisualHintDelay; }
            set { Settings.GetInstance().VisualHintDelay = value; OnPropertyChanged("VisualHintDelay"); }
        }

        public double VisualHintTimePeriod
        {
            get { return Settings.GetInstance().VisualHintTimePeriod; }
            set { Settings.GetInstance().VisualHintTimePeriod = value; OnPropertyChanged("VisualHintTimePeriod"); }
        }

        public double VisualHintDuration
        {
            get { return Settings.GetInstance().VisualHintDuration; }
            set { Settings.GetInstance().VisualHintDuration = value; OnPropertyChanged("VisualHintDuration"); }
        }

        public double EducationVisualHintDelay
        {
            get { return Settings.GetInstance().EducationVisualHintDelay; }
            set { Settings.GetInstance().EducationVisualHintDelay = value; OnPropertyChanged("EducationVisualHintDelay"); }
        }

        public double EducationVisualHintTimePeriod
        {
            get { return Settings.GetInstance().EducationVisualHintTimePeriod; }
            set { Settings.GetInstance().EducationVisualHintTimePeriod = value; OnPropertyChanged("EducationVisualHintTimePeriod"); }
        }

        public double EducationVisualHintDuration
        {
            get { return Settings.GetInstance().EducationVisualHintDuration; }
            set { Settings.GetInstance().EducationVisualHintDuration = value; OnPropertyChanged("EducationVisualHintDuration"); }
        }



        public string FirstQuestionText
        {
            get { return Settings.GetInstance().FirstQuestionText; }
            set { Settings.GetInstance().FirstQuestionText = value; OnPropertyChanged("FirstQuestionText"); }
        }

        public string HintQuestionText
        {
            get { return Settings.GetInstance().HintQuestionText; }
            set { Settings.GetInstance().HintQuestionText = value; OnPropertyChanged("HintQuestionText"); }
        }

        public string SuccessTestText
        {
            get { return Settings.GetInstance().SuccessTestText; }
            set { Settings.GetInstance().SuccessTestText = value; OnPropertyChanged("SuccessTestText"); }
        }

        public string FallTestText
        {
            get { return Settings.GetInstance().FallTestText; }
            set { Settings.GetInstance().FallTestText = value; OnPropertyChanged("FallTestText"); }
        }


        public double CardSize
        {
            get { return Settings.GetInstance().CardSize; }
            set { Settings.GetInstance().CardSize = value; OnPropertyChanged("CardSize"); }
        }
        public double CardSuccesSize
        {
            get { return Settings.GetInstance().CardSuccesSize; }
            set { Settings.GetInstance().CardSuccesSize = value; OnPropertyChanged("CardSuccesSize"); }
        }
        public double CardSuccesTime
        {
            get { return Settings.GetInstance().CardSuccesTime; }
            set { Settings.GetInstance().CardSuccesTime = value; OnPropertyChanged("CardSuccesTime"); }
        }

        public double CardWrongPauseTime
        {
            get { return Settings.GetInstance().CardWrongPauseTime; }
            set { Settings.GetInstance().CardWrongPauseTime = value; OnPropertyChanged("CardWrongPauseTime"); }
        }
        public double CardSuccesSpeakAgainTime
        {
            get { return Settings.GetInstance().CardSuccesSpeakAgainTime; }
            set { Settings.GetInstance().CardSuccesSpeakAgainTime = value; OnPropertyChanged("CardSuccesSpeakAgainTime"); }
        }

        public string AttachedDBCardsFilename
        {
            get { return Settings.GetInstance().AttachedDBCardsFilename; }
            set { Settings.GetInstance().AttachedDBCardsFilename = value; OnPropertyChanged("AttachedDBCardsFilename"); }
        }



        public ObservableCollection<string> TextToSpeachVoicesNames
        {
            get
            {
                var voices = Settings.GetInstance().TextToSpeachVoices;
                return new ObservableCollection<string>(from v in voices select v.VoiceInfo.Name);
            }
        }


        public string TTSVoiceName
        {
            get
            {
                return Settings.GetInstance().TTSVoiceName;
            }
            set
            {
                if (TextToSpeachVoicesNames.Contains(value))
                {
                    Settings.GetInstance().TTSVoiceName = value;
                    OnPropertyChanged("TTSVoiceName");
                }
            }
        }


        public int TTSVoiceRate
        {
            get { return Settings.GetInstance().TTSVoiceRate; }
            set { Settings.GetInstance().TTSVoiceRate = value; OnPropertyChanged("TTSVoiceRate"); }
        }

        public int TTSVoiceSlowRate
        {
            get { return Settings.GetInstance().TTSVoiceSlowRate; }
            set { Settings.GetInstance().TTSVoiceSlowRate = value; OnPropertyChanged("TTSVoiceSlowRate"); }
        }

        public int TTSVoiceVolume
        {
            get { return Settings.GetInstance().TTSVoiceVolume; }
            set { Settings.GetInstance().TTSVoiceVolume = value; OnPropertyChanged("TTSVoiceVolume"); }
        }


        public string BackgroundFilename
        {
            get { return Settings.GetInstance().BackgroundFilename; }
            set { Settings.GetInstance().BackgroundFilename = value; OnPropertyChanged("BackgroundFilename"); }
        }

        public string BackgroundGameOverFilename
        {
            get { return Settings.GetInstance().BackgroundGameOverFilename; }
            set { Settings.GetInstance().BackgroundGameOverFilename = value; OnPropertyChanged("BackgroundGameOverFilename"); }
        }

        public string BackgroundMenuFilename
        {
            get { return Settings.GetInstance().BackgroundMenuFilename; }
            set { Settings.GetInstance().BackgroundMenuFilename = value; OnPropertyChanged("BackgroundMenuFilename"); }
        }

        public string BackgroundStartFilename
        {
            get { return Settings.GetInstance().BackgroundStartFilename; }
            set { Settings.GetInstance().BackgroundStartFilename = value; OnPropertyChanged("BackgroundStartFilename"); }
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
                var musicInfos = new ObservableCollection<MusicInfo>();
                if (MusicFilenames?.Count>0)
                    musicInfos = new ObservableCollection<MusicInfo>(from m in MusicFilenames select new MusicInfo(m));
                return musicInfos;
            }
        }

        public ObservableCollection<string> MusicFilenames
        {
            get
            {
                return Settings.GetInstance().MusicFilenames;
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
            get { return Settings.GetInstance().ShuffleMusic; }
            set { Settings.GetInstance().ShuffleMusic = value; OnPropertyChanged("ShuffleMusic"); }
        }

        public bool RepeatMusicPlaylist
        {
            get { return Settings.GetInstance().RepeatMusicPlaylist; }
            set { Settings.GetInstance().RepeatMusicPlaylist = value; OnPropertyChanged("RepeatMusicPlaylist"); }
        }
        
        #endregion

        #region METHODS
        private void SettingsWindowView_Activated(object sender, EventArgs e)
        {
            RefreshAllDependencyProperties();
        }

        private void SettingsWindowView_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            
        }

        public void RefreshAllDependencyProperties()
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
                      Settings.GetInstance().SaveAllSettings();
                  }));
            }
        }

        //private RelayCommand restoreSettingsCommand;
        //public RelayCommand RestoreSettingsCommand
        //{
        //    get
        //    {
        //        return restoreSettingsCommand ??
        //          (restoreSettingsCommand = new RelayCommand(obj =>
        //          {
        //              Settings.GetInstance().RestoreAllSettings();
        //              RefreshAllDependencyProperties();
        //          }));
        //    }
        //}

        private RelayCommand importSettingsFromXMLCommand;
        public RelayCommand ImportSettingsFromXMLCommand
        {
            get
            {
                return importSettingsFromXMLCommand ??
                  (importSettingsFromXMLCommand = new RelayCommand(obj =>
                  {
                      OpenFileDialog openFileDialog = new OpenFileDialog
                      {
                          Multiselect = false,
                          Filter = "Файлы настроек xml (*.xml)|*.xml"
                      };
                      if (openFileDialog.ShowDialog() != DialogResult.OK) return;

                      try
                      {
                          Settings.GetInstance().ImportSettingsFromXML(@openFileDialog.FileName);
                          RefreshAllDependencyProperties();
                      }
                      catch
                      {
                          System.Windows.MessageBox.Show("Ошибка открытия файла настроек");
                      }

                  }));
            }
        }

        private RelayCommand exportSettingsToXMLCommand;
        public RelayCommand ExportSettingsToXMLCommand
        {
            get
            {
                return exportSettingsToXMLCommand ??
                  (exportSettingsToXMLCommand = new RelayCommand(obj =>
                  {
                      SaveFileDialog saveFileDialog = new SaveFileDialog
                      {
                          Filter = "Файлы настроек xml (*.xml)|*.xml"
                      };
                      if (saveFileDialog.ShowDialog() != DialogResult.OK) return;
                      if (File.Exists(@saveFileDialog.FileName))
                      {
                          MessageBoxResult res = System.Windows.MessageBox.Show("Перезаписать файл?", "Файл существует", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                          if (res == MessageBoxResult.No) return;
                      }
                      Settings.GetInstance().ExportSettingsToXML(@saveFileDialog.FileName);
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
                      Settings.GetInstance()._MusicFilenames = new ObservableCollection<string>(mf);
                      
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
                      Settings.GetInstance()._MusicFilenames = new ObservableCollection<string>(mf);
                      
                      RefreshAllDependencyProperties();
                      musicFilenamesListView.SelectedIndex = oldPos + 1;
                  }));
            }
        }

        private RelayCommand restoreDefaultSettingsCommand;
        public RelayCommand RestoreDefaultSettingsCommand
        {
            get
            {
                return restoreDefaultSettingsCommand ??
                  (restoreDefaultSettingsCommand = new RelayCommand(obj =>
                  {
                      var res = System.Windows.MessageBox.Show("Будут восстановлены все настройки по умолчанию. " +
                          "Текущие настройки будут утрачены (рекомендуется предварительно экспортировать текущие настройки). \n \n" +
                          "Вы уверены, что хотите сбросить все настройки?","Сброс настроек", (MessageBoxButton)MessageBoxButtons.YesNo, (MessageBoxImage)MessageBoxIcon.Warning);
                      if (res == MessageBoxResult.No) return;
                      Settings.GetInstance().RestoreDefaultSettings();
                      SaveSettingsCommand.Execute(null);
                      RefreshAllDependencyProperties();
                  }));
            }
        }
        

        #endregion

    }
}
