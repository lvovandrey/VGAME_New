using CardsEditor.Tools;
using CardsEditor.View;
using CardsGameNewDBRepository;
using CardsGameNewDBRepository.Model;
using MVVMRealization;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Forms;

namespace CardsEditor.ViewModel
{
    public class VM : INPCBase
    { 
        #region Constructors
        public VM(MainWindow _mainWindow)
        {
            mainWindow = _mainWindow;
            mainWindow.DataContext = this;

            ttsSettingsWindow = new TTSSettingsWindow();
            ttsSettingsWindow.Owner = mainWindow;
            ttsSettingsWindow.DataContext = this;


            aboutWindow = new AboutWindow();
            aboutWindow.Owner = mainWindow;
            aboutWindow.DataContext = this;

            Settings.SettingsChanged += Settings_SettingsChanged;
            Settings.GetInstance().SetTTSVoices();
            Settings.GetInstance().ImportSettingsToXML(ConfigurationTools.SettingsFilename);
        }




        #endregion

        #region Fields
        MainWindow mainWindow;
        ObservableCollection<Card> _cards
        {
            get
            {
                if (IsDBLoaded(null))
                    return new ObservableCollection<Card>(DBTools.Context.Cards.ToList());
                else return new ObservableCollection<Card>();
            }
        }
        ObservableCollection<Level> _levels
        {
            get
            {
                if (IsDBLoaded(null))
                    return new ObservableCollection<Level>(DBTools.Context.Levels.ToList());
                else return new ObservableCollection<Level>();
            }
        }



        ObservableCollection<LevelPassing> _levelPassings
        {
            get
            {
                if (IsDBLoaded(null))
                    return new ObservableCollection<LevelPassing>(DBTools.Context.LevelPassings.ToList());
                else return new ObservableCollection<LevelPassing>();
            }
        }
        TTSSettingsWindow ttsSettingsWindow;
        AboutWindow aboutWindow;
        #endregion

        #region Properties

        public string DBFilename
        {
            get
            {
                return Path.GetFileName(DBTools.DBFilename);
            }
        }

        private ObservableCollection<CardVM> _cardvms { get; set; }
        public ObservableCollection<CardVM> CardVMs
        {
            get
            {
                if (_cards == null) return null;
                _cardvms = new ObservableCollection<CardVM>(from l in _cards select new CardVM(l, this));
                return _cardvms;
            }
        }


        private ObservableCollection<LevelVM> _Levelvms { get; set; }
        public ObservableCollection<LevelVM> LevelVMs
        {
            get
            {
                _Levelvms = new ObservableCollection<LevelVM>(from t in _levels select new LevelVM(t, this));
                return _Levelvms;
            }
        }

        private LevelVM _SelectedLevelVM;
        public LevelVM SelectedLevelVM
        {
            get
            { return _SelectedLevelVM; }
            set
            {
                _SelectedLevelVM = value;
                OnPropertyChanged("SelectedLevelVM");
            }
        }

        private CardVM _SelectedCardVM;
        public CardVM SelectedCardVM
        {
            get
            { return _SelectedCardVM; }
            set
            {
                _SelectedCardVM = value;
                OnPropertyChanged("SelectedCardVM");
                OnPropertyChanged("SelectedCardLevelVMs");
            }

        }

        private LevelVM _SelectedOnCardLevelVM;
        public LevelVM SelectedOnCardLevelVM
        {
            get
            { return _SelectedOnCardLevelVM; }
            set
            {
                _SelectedOnCardLevelVM = value;
                OnPropertyChanged("SelectedOnCardLevelVM");

            }
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

        public int TTSVoiceVolume
        {
            get { return Settings.GetInstance().TTSVoiceVolume; }
            set { Settings.GetInstance().TTSVoiceVolume = value; OnPropertyChanged("TTSVoiceVolume"); }
        }



        #endregion

        #region Methods
        private void Settings_SettingsChanged()
        {
            OnPropertyChanged("TextToSpeachVoicesNames");
            OnPropertyChanged("TTSVoiceName");
            OnPropertyChanged("TTSVoiceRate");
            OnPropertyChanged("TTSVoiceVolume");
        }


        //позорный костыль для загрузки БД - так и не разобрался почему коллекция после выхода из статического метода не изменяется. а внутри меняется вроде.
        //public void init(ObservableCollection<Card> cards, ObservableCollection<Level> levels, ObservableCollection<LevelPassing> levelPassings, Context Context)
        //{
        //    _cards = cards;
        //    _levels = levels;
        //    _levelPassings = levelPassings;
        //    context = Context;
        //}

        //создаем и добавляем в БД новую каточку
        private Card AddCard(object obj)
        {
            Random random = new Random();
            Card c = new Card() { Id = _cards.Count + 1, Title = "Card " + (_cards.Count + 1).ToString(), ImageAddress = @"C:\1.jpg" };
            c.Description = "Описание";
            c.SoundedText = "Пустой текст";
            _cards.Add(c);
            DBTools.Context.Cards.Add(c);
            DBTools.Context.SaveChanges();
            OnPropertyChanged("CardVMs");
            return c;
        }

        private Card AddCard(string title, string soundedText, string imageAddress, string description)
        {
            Random random = new Random();
            Card c = new Card() { Id = _cards.Count + 1, Title = title, SoundedText = soundedText, ImageAddress = imageAddress, Description = description };
            _cards.Add(c);
            DBTools.Context.Cards.Add(c);
            DBTools.Context.SaveChanges();
            OnPropertyChanged("CardVMs");
            return c;
        }

        //создаем и добавляем в БД новый тег
        private Level AddLevel(object obj)
        {
            // if (SelectedCardVM == null) return null;

            Level t = new Level() { Id = _levels.Count + 1, Name = "Level " + (_levels.Count + 1).ToString(), Cards = new ObservableCollection<Card>() };
            _levels.Add(t);
            DBTools.Context.Levels.Add(t);
            DBTools.Context.SaveChanges();
            OnPropertyChanged("LevelVMs");
            OnPropertyChanged("SelectedCardLevelVMs");
            return t;
        }

        //удаляем из БД тег
        private void RemoveLevel(object obj)
        {
            if (obj == null) return;
            ((LevelVM)obj).LevelStatisticVM.ClearLevelPassingsStatistic();
            Level t = ((LevelVM)obj).Level;
            _levels.Remove(t);
            DBTools.Context.Entry(t).State = EntityState.Deleted;
            DBTools.Context.SaveChanges();

            OnPropertyChanged("LevelVMs");
            OnPropertyChanged("SelectedCardLevelVMs");
        }

        private void CreateCardsFromImageFiles(object obj)
        {
            string[] Filenames = new string[] { };
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Файлы изображений и видео (*.bmp, *.jpg, *.png, *.gif, *.wmv, *.avi)|*.bmp;*.jpg;*.png;*.gif;*.wmv;*.avi";
                openFileDialog.Title = "Открыть свое изображение для карточки";
                openFileDialog.Multiselect = true;
                string dir = obj as string;
                if (dir != null && Directory.Exists(dir))
                    openFileDialog.InitialDirectory = dir;
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    Filenames = @openFileDialog.FileNames;
                    CreateCardsFromArrayOfImgFilenames(Filenames);
                }
            }
        }

        private void CreateCardsFromArrayOfImgFilenames(string[] Filenames)
        {
            if (Filenames.Length < 1) return;
            if (!IsBigImageFilesCheckOk(Filenames) || !IsBigVideoFilesCheckOk(Filenames)) return;
            //добавление выбранных файлов
            foreach (var filename in Filenames)
            {
                AddCard(Path.GetFileNameWithoutExtension(filename),
                    Path.GetFileNameWithoutExtension(filename),
                    filename, "Нет описания");
            }
            OnPropertyChanged("CardVMs");
        }

        private static BrowserWindow browserWindow;
        private void CreateCardsFromInternet(object obj)
        {
            if (browserWindow == null)
            {
                browserWindow = new BrowserWindow();
                browserWindow.Owner = mainWindow;
                browserWindow.INNERBROWSER.InitBrowser(Path.Combine(Settings.GetInstance().LocalAppDataDir, "BrowserCacheFolder"));
                browserWindow.INNERBROWSER.ChoiceUrl += Browser_ChoiceUrl;
            }
            browserWindow.Show();
            browserWindow.Activate();
        }

        private void Browser_ChoiceUrl(string filename)
        {
            if (!(Miscellanea.ExstentionCheck(filename, new string[] { ".jpg", ".png", ".gif", ".bmp", ".avi", ".wmv" })))
            {
                var res = System.Windows.MessageBox.Show(@"Надо выбрать прямой путь к картинке с расширением "+
                            ".jpg, .png, .gif или .bmp либо видеофайлу с расширением .avi или .wmv. "+ 
                            "Т.е. чтобы в адресной строке браузера адрес заканчивался этим расширением: "+ 
                            @"например так http:\\www.somesite.ru\image.jpg.",
                            "Неверное расширение", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if(res == MessageBoxResult.Yes)
                return;
            }
            if (!Miscellanea.UrlExists(filename))
            {
                System.Windows.MessageBox.Show(@"По указанному адресу файл не обнаружен.",
    "Файл не найден", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            CreateCardsFromArrayOfImgFilenames(new string[] { filename });
        }

        public static bool IsBigVideoFilesCheckOk(string[] Filenames)
        {

            //определение наличия файлов большого размера
            Dictionary<string, double> BigBitrateFiles = new Dictionary<string, double>();
            foreach (var filename in Filenames)
            {
                var ext = Path.GetExtension(filename);
                if (!File.Exists(filename) && (ext != ".avi" || ext != ".wmv")) continue;
                var Bitrate = Miscellanea.GetVideoBitRate(filename);
                if (Bitrate > Settings.GetInstance().MaxVideoFileBitrate) BigBitrateFiles.Add(Path.GetFileName(filename), Bitrate / 1024);
            }
            if (BigBitrateFiles.Count > 0)
            {
                string BigLenghtFilesString = "";
                foreach (var BigLenghtFile in BigBitrateFiles)
                {
                    BigLenghtFilesString += BigLenghtFile.Key + " : " + BigLenghtFile.Value.ToString("0") + "kbps\n";
                }
                if (System.Windows.MessageBox.Show("Некоторые файлы видео имеют очень высокий битрейт (большое разрешение, частоту и т.п.)," +
                                                   " что может замедлить работу, " +
                                                   "вызвать недостаток оперативной памяти, в том числе в ходе " +
                                                   "игры при выборе уровня с большим количеством таких файлов:\n\n" +
                                                   BigLenghtFilesString + "\nВсе равно открыть указанные файлы?",
                                                   "Очень большие файлы",
                                                   (MessageBoxButton)MessageBoxButtons.YesNo,
                                                   (MessageBoxImage)MessageBoxIcon.Information) == MessageBoxResult.No) return false;
            }
            return true;
        }

        public static bool IsBigImageFilesCheckOk(string[] Filenames)
        {

            //определение наличия файлов большого размера
            Dictionary<string, double> BigLenghtFiles = new Dictionary<string, double>();
            foreach (var filename in Filenames)
            {
                var ext = Path.GetExtension(filename);
                if (!File.Exists(filename) || ext == ".avi" || ext == ".wmv") continue;
                var lenghtMb = ((double)(new FileInfo(filename)).Length) / (1024 * 1024);
                if (lenghtMb > Settings.GetInstance().MaxImageFileLenghtInMb) BigLenghtFiles.Add(Path.GetFileName(filename), lenghtMb);
            }
            if (BigLenghtFiles.Count > 0)
            {
                string BigLenghtFilesString = "";
                foreach (var BigLenghtFile in BigLenghtFiles)
                {
                    BigLenghtFilesString += BigLenghtFile.Key + " : " + BigLenghtFile.Value.ToString("0.0") + " Мб\n";
                }
                if (System.Windows.MessageBox.Show("Некоторые файлы изображений очень большие, что может замедлить работу, " +
                                                   "вызвать недостаток оперативной памяти, в том числе в ходе " +
                                                   "игры при выборе уровня с большим количеством крупных файлов:\n\n" +
                                                   BigLenghtFilesString + "\nВсе равно открыть указанные файлы?",
                                                   "Очень большие файлы",
                                                   (MessageBoxButton)MessageBoxButtons.YesNo,
                                                   (MessageBoxImage)MessageBoxIcon.Information) == MessageBoxResult.No) return false;
            }
            return true;
        }

        private bool IsDBLoaded(object obj)
        {
            return DBTools.IsDBLoaded;
        }

        public void OnPropertyChangedCardVMs()
        {
            OnPropertyChanged("CardVMs");
        }

        public void OnWindowClosing()
        {
            Settings.GetInstance().ExportSettingsToXML(ConfigurationTools.SettingsFilename);

        }
        #endregion

        #region Commands

        private RelayCommand сreateBDCommand;
        public RelayCommand CreateBDCommand
        {
            get
            {
                return сreateBDCommand ??
                  (сreateBDCommand = new RelayCommand(obj =>
                  {

                      string DBFilename = "";
                      using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                      {
                          saveFileDialog.Filter = "Файлы базы данных(*.db)|*.db";
                          saveFileDialog.ValidateNames = false;
                          if (saveFileDialog.ShowDialog() == DialogResult.OK)
                          {
                              DBFilename = saveFileDialog.FileName;
                              bool res = DBTools.CreateDBOnCodeFirst(DBFilename);
                              if (!res)
                              {
                                  System.Windows.MessageBox.Show("Ошибка загрузки базы данных " + DBFilename, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                                  return;
                              }
                              mainWindow.DataContext = this;
                              OnPropertyChanged("CardVMs");
                              OnPropertyChanged("LevelVMs");
                              OnPropertyChanged("DBFilename");
                              Settings.GetInstance().AddRecentlyDBFilename(DBFilename);
                              mainWindow.RefreshRecentlyFilesMenu();
                          }
                      }



                  }));
            }
        }

        private RelayCommand easyCreateBDCommand;
        public RelayCommand EasyCreateBDCommand
        {
            get
            {
                return easyCreateBDCommand ??
                  (easyCreateBDCommand = new RelayCommand(obj =>
                  {

                      string DBFilename = "";
                      using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                      {
                          saveFileDialog.Filter = "Файлы базы данных(*.db)|*.db";
                          saveFileDialog.ValidateNames = false;
                          if (saveFileDialog.ShowDialog() == DialogResult.OK)
                          {
                              DBFilename = saveFileDialog.FileName;
                              bool res = DBTools.CreateDBOnCodeFirst(DBFilename);
                              if (!res)
                              {
                                  System.Windows.MessageBox.Show("Ошибка загрузки базы данных " + DBFilename, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                                  return;
                              }
                              mainWindow.DataContext = this;
                              OnPropertyChanged("CardVMs");
                              OnPropertyChanged("LevelVMs");
                              OnPropertyChanged("DBFilename");
                              Settings.GetInstance().AddRecentlyDBFilename(DBFilename);
                              mainWindow.RefreshRecentlyFilesMenu();
                          }
                      }



                  }));
            }
        }

        private RelayCommand сreateCardsFromImageFilesCommand;
        public RelayCommand CreateCardsFromImageFilesCommand
        {
            get
            {
                return сreateCardsFromImageFilesCommand ??
                  (сreateCardsFromImageFilesCommand = new RelayCommand(CreateCardsFromImageFiles, IsDBLoaded));
            }
        }

        private RelayCommand openBDCommand;
        public RelayCommand OpenBDCommand
        {
            get
            {
                return openBDCommand ??
                  (openBDCommand = new RelayCommand(obj =>
                  {

                      string DBFilename = "";
                      using (OpenFileDialog openFileDialog = new OpenFileDialog())
                      {
                          openFileDialog.Filter = "Файлы базы данных(*.db)|*.db";
                          openFileDialog.ValidateNames = false;
                          if (openFileDialog.ShowDialog() == DialogResult.OK)
                          {
                              DBFilename = @openFileDialog.FileName;
                              bool res = DBTools.LoadDB(DBFilename);
                              if (!res)
                              {
                                  System.Windows.MessageBox.Show("Ошибка загрузки базы данных " + DBFilename, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                                  return;
                              }

                              mainWindow.DataContext = this;
                              OnPropertyChanged("CardVMs");
                              OnPropertyChanged("LevelVMs");
                              OnPropertyChanged("DBFilename");
                              Settings.GetInstance().AddRecentlyDBFilename(DBFilename);
                              mainWindow.RefreshRecentlyFilesMenu();
                          }
                      }



                  }));
            }
        }

        private RelayCommand openRecentlyBDCommand;
        public RelayCommand OpenRecentlyBDCommand
        {
            get
            {
                return openRecentlyBDCommand ??
                  (openRecentlyBDCommand = new RelayCommand(obj =>
                  {

                      string DBFilename = "";

                      DBFilename = obj.ToString();
                      if (!File.Exists(DBFilename) || Path.GetExtension(DBFilename) != ".db") return;
                      bool res = DBTools.LoadDB(DBFilename);
                      if (!res)
                      {
                          System.Windows.MessageBox.Show("Ошибка загрузки базы данных " + DBFilename, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                          return;
                      }

                      mainWindow.DataContext = this;
                      OnPropertyChanged("CardVMs");
                      OnPropertyChanged("LevelVMs");
                      OnPropertyChanged("DBFilename");
                      Settings.GetInstance().AddRecentlyDBFilename(DBFilename);
                      mainWindow.RefreshRecentlyFilesMenu();
                  }));
            }
        }

        private RelayCommand clearRecentlyBDCommand;
        public RelayCommand ClearRecentlyBDCommand
        {
            get
            {
                return clearRecentlyBDCommand ??
                  (clearRecentlyBDCommand = new RelayCommand(obj =>
                  {
                      mainWindow.DataContext = this;
                      OnPropertyChanged("CardVMs");
                      OnPropertyChanged("LevelVMs");
                      OnPropertyChanged("DBFilename");
                      Settings.GetInstance().ClearRecentlyDBFilename();
                      mainWindow.RefreshRecentlyFilesMenu();
                  }));
            }
        }

        private RelayCommand addCardCommand;
        public RelayCommand AddCardCommand
        {
            get
            {
                return addCardCommand ??
                  (addCardCommand = new RelayCommand(obj =>
                  {
                      AddCard(obj);
                  }, IsDBLoaded));
            }
        }

        private RelayCommand saveCommand;
        public RelayCommand SaveCommand
        {
            get
            {
                return saveCommand ??
                  (saveCommand = new RelayCommand(obj =>
                  {
                      {

                          foreach (Card c in _cards)
                              DBTools.Context.Entry(c).State = EntityState.Modified;
                          foreach (Level t in _levels)
                              DBTools.Context.Entry(t).State = EntityState.Modified;
                          DBTools.Context.SaveChanges();
                          OnPropertyChanged("CardVMs");
                          OnPropertyChanged("LevelVMs");
                      }
                  }, IsDBLoaded));
            }
        }

        private RelayCommand removeCardCommand;
        public RelayCommand RemoveCardCommand
        {
            get
            {
                return removeCardCommand ??
                  (removeCardCommand = new RelayCommand(obj =>
                  {
                      {
                          if (SelectedCardVM == null) return;

                          Card card = SelectedCardVM.Card;

                          Card.DeleteCardFromDB(card);
                          _cards.Remove(card);
                          OnPropertyChanged("CardVMs");
                          SelectedCardVM = CardVMs.FirstOrDefault();
                      }
                  }, IsDBLoaded));
            }
        }

        private RelayCommand addLevelCommand;
        public RelayCommand AddLevelCommand
        {
            get
            {
                return addLevelCommand ?? (addLevelCommand = new RelayCommand(obj =>
                {
                    AddLevel(obj);
                }, IsDBLoaded));
            }
        }

        private RelayCommand removeLevelCommand;
        public RelayCommand RemoveLevelCommand

        {
            get
            {
                return removeLevelCommand ?? (removeLevelCommand = new RelayCommand(obj =>
                {
                    RemoveLevel(SelectedLevelVM);
                }, IsDBLoaded));
            }
        }

        private RelayCommand tTSChangeCommand;
        public RelayCommand TTSChangeCommand

        {
            get
            {
                return tTSChangeCommand ?? (tTSChangeCommand = new RelayCommand(obj =>
                {
                    ttsSettingsWindow.Show();
                    ttsSettingsWindow.Activate();
                }));
            }
        }

        private RelayCommand tTSSettingsSaveCommand;
        public RelayCommand TTSSettingsSaveCommand

        {
            get
            {
                return tTSSettingsSaveCommand ?? (tTSSettingsSaveCommand = new RelayCommand(obj =>
                {
                    Settings.GetInstance().ExportSettingsToXML(ConfigurationTools.SettingsFilename);
                }));
            }
        }

        private RelayCommand tTSSettingsLoadCommand;
        public RelayCommand TTSSettingsLoadCommand

        {
            get
            {
                return tTSSettingsLoadCommand ?? (tTSSettingsLoadCommand = new RelayCommand(obj =>
                {
                    Settings.GetInstance().ImportSettingsToXML(ConfigurationTools.SettingsFilename);
                }));
            }
        }

        private RelayCommand openDefaultDBCommand;
        public RelayCommand OpenDefaultDBCommand

        {
            get
            {
                return openDefaultDBCommand ?? (openDefaultDBCommand = new RelayCommand(obj =>
                {
                    string DBFilename = "";
                    if (IsDBLoaded(null))
                        if (System.Windows.MessageBox.Show("Вы уверены что хотите открыть БД по умолчанию " +
                            "(несохраненные данные в текущей БД будут потеряны)?",
                            "Открыть БД по умолчанию",
                            (MessageBoxButton)MessageBoxButtons.YesNo,
                            (MessageBoxImage)MessageBoxIcon.Warning) == MessageBoxResult.No) return;


                    DBFilename = Settings.GetInstance().DefaultDBCardsFilename;
                    bool res = DBTools.LoadDB(DBFilename);
                    if (!res)
                    {
                        System.Windows.MessageBox.Show("Ошибка загрузки базы данных " + DBFilename, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    mainWindow.DataContext = this;
                    OnPropertyChanged("CardVMs");
                    OnPropertyChanged("LevelVMs");
                    OnPropertyChanged("DBFilename");
                    Settings.GetInstance().AddRecentlyDBFilename(DBFilename);
                    mainWindow.RefreshRecentlyFilesMenu();
                }));
            }
        }

        private RelayCommand openAppDataFolderCommand;
        public RelayCommand OpenAppDataFolderCommand

        {
            get
            {
                return openAppDataFolderCommand ?? (openAppDataFolderCommand = new RelayCommand(obj =>
                {
                    if (Directory.Exists(Settings.GetInstance().LocalAppDataDir))
                        Process.Start("explorer.exe", Settings.GetInstance().LocalAppDataDir);
                    else
                        System.Windows.MessageBox.Show("Папка с данными программы не найдена: "
                            + Settings.GetInstance().LocalAppDataDir,
                            "Директория отуствует",
                            MessageBoxButton.OK,
                            (MessageBoxImage)MessageBoxIcon.Error);
                }));
            }
        }

        private RelayCommand createCardsFromImagesOfDefaultDBCommand;
        public RelayCommand CreateCardsFromImagesOfDefaultDBCommand

        {
            get
            {
                return createCardsFromImagesOfDefaultDBCommand ?? (createCardsFromImagesOfDefaultDBCommand = new RelayCommand
                    (obj =>
                {
                    CreateCardsFromImageFilesCommand.Execute(Settings.GetInstance().LocalAppDataDir);
                }, IsDBLoaded));
            }
        }

        private RelayCommand createCardsFromInternetCommand;
        public RelayCommand CreateCardsFromInternetCommand

        {
            get
            {
                return createCardsFromInternetCommand ?? (createCardsFromInternetCommand = new RelayCommand
                    (obj =>
                    {
                        CreateCardsFromInternet(null);
                    }, IsDBLoaded));
            }
        }
        private RelayCommand openManualCommand;
        public RelayCommand OpenManualCommand
        {
            get
            {
                return openManualCommand ??
                  (openManualCommand = new RelayCommand(obj =>
                  {
                      Process.Start(Settings.GetInstance().ManualFilename);
                  }));
            }
        }

        private RelayCommand openLicenseInfoFileCommand;
        public RelayCommand OpenLicenseInfoFileCommand
        {
            get
            {
                return openLicenseInfoFileCommand ??
                  (openLicenseInfoFileCommand = new RelayCommand(obj =>
                  {
                      Process.Start(Settings.GetInstance().LicenseInfoFilename);
                  }));
            }
        }

        private RelayCommand openAboutWindowCommand;
        public RelayCommand OpenAboutWindowCommand
        {
            get
            {
                return openAboutWindowCommand ??
                  (openAboutWindowCommand = new RelayCommand(obj =>
                  {
                      aboutWindow.Show();
                      aboutWindow.Activate();
                  }));
            }
        }

                private RelayCommand copyTextToClipboard;
        public RelayCommand CopyTextToClipboard
        {
            get
            {
                return copyTextToClipboard ??
                  (copyTextToClipboard = new RelayCommand(obj =>
                  {
                      if (obj is string)
                          System.Windows.Clipboard.SetText(obj as string);
                  }));
            }
        }

        #endregion


    }
}
