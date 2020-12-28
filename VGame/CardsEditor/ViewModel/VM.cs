using CardsEditor.Abstract;
using CardsEditor.DB;
using CardsEditor.Model;
using CardsEditor.Tools;
using CardsEditor.View;
using LevelSetsEditor.DB;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;
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

            SetTTSVoices();
            RestoreAllSettings();

            ttsSettingsWindow = new TTSSettingsWindow();
            ttsSettingsWindow.Owner = mainWindow;
            ttsSettingsWindow.DataContext = this;
        }


        #endregion

        #region Fields
        Context context;
        MainWindow mainWindow;
        ObservableCollection<Card> _cards = new ObservableCollection<Card>();
        ObservableCollection<Level> _levels = new ObservableCollection<Level>();
        ObservableCollection<LevelPassing> _levelPassings = new ObservableCollection<LevelPassing>();
        TTSSettingsWindow ttsSettingsWindow;
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
                _Levelvms = new ObservableCollection<LevelVM>(from t in _levels select new LevelVM(t,this));
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


        private ReadOnlyCollection<InstalledVoice> _TextToSpeachVoices;
        public ObservableCollection<InstalledVoice> TextToSpeachVoices

        {
            get
            {
                return new ObservableCollection<InstalledVoice>(_TextToSpeachVoices);
            }
        }

        private string TTSVoiceName 
        {
            get 
            {
               return _TTSVoice.VoiceInfo.Name;
            }
            set 
            {
                var TextToSpeachVoicesNames = from v in TextToSpeachVoices select v.VoiceInfo.Name;
                if(TextToSpeachVoicesNames.Contains(value))
                {
                    TTSVoice = (from v in TextToSpeachVoices where (v.VoiceInfo.Name == value) select v).First();
                }
            }
        }

        private InstalledVoice _TTSVoice;
        public InstalledVoice TTSVoice
        {
            get { return _TTSVoice; }
            set 
            { 
                _TTSVoice = value;
                OnPropertyChanged("TTSVoice");
                ConfigurationTools.AddUpdateAppSettings("TTSVoiceName", TTSVoiceName);
            }
        }

        private int _TTSVoiceRate = 0;
        public int TTSVoiceRate
        { 
            get { return _TTSVoiceRate; }
            set
            {
                if (value <= 10 && value >= -10)
                {
                    _TTSVoiceRate = value;
                    OnPropertyChanged("TTSVoiceRate");
                    ConfigurationTools.AddUpdateAppSettings("TTSVoiceRate", TTSVoiceRate.ToString());
                }
            }
        }

        private int _TTSVoiceVolume = 100;
        public int TTSVoiceVolume
        {
            get { return _TTSVoiceVolume; }
            set
            {
                if (value <= 100 && value >= 0)
                {
                    _TTSVoiceVolume = value;
                    OnPropertyChanged("TTSVoiceVolume");
                    ConfigurationTools.AddUpdateAppSettings("TTSVoiceVolume", TTSVoiceVolume.ToString());
                }
            }
        }

        #endregion

        #region Methods

        public void SaveAllSettings()
        {
            ConfigurationTools.AddUpdateAppSettings("TTSVoiceName", TTSVoiceName);
            ConfigurationTools.AddUpdateAppSettings("TTSVoiceRate", TTSVoiceRate.ToString());
            ConfigurationTools.AddUpdateAppSettings("TTSVoiceVolume", TTSVoiceVolume.ToString());
        }

        public void RestoreAllSettings()
        {
            TTSVoiceName = ConfigurationTools.ReadSetting("TTSVoiceName");
            int.TryParse(ConfigurationTools.ReadSetting("TTSVoiceRate"), out _TTSVoiceRate);
            int.TryParse(ConfigurationTools.ReadSetting("TTSVoiceVolume"), out _TTSVoiceVolume);
        }


        private void SetTTSVoices()
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

        //позорный костыль для загрузки БД - так и не разобрался почему коллекция после выхода из статического метода не изменяется. а внутри меняется вроде.
        public void init(ObservableCollection<Card> cards, ObservableCollection<Level> levels, ObservableCollection<LevelPassing> levelPassings, Context Context)
        {
            _cards = cards;
            _levels = levels;
            _levelPassings = levelPassings;
            context = Context;
        }

        //создаем и добавляем в БД новую каточку
        private Card AddCard(object obj)
        {
            Random random = new Random();
            Card c = new Card() { Id = _cards.Count + 1, Title = "Card " + (_cards.Count + 1).ToString(), ImageAddress = @"C:\1.jpg" };
            c.Description = "Описание";
            c.SoundedText = "Пустой текст";
            _cards.Add(c);
            context.Cards.Add(c);
            context.SaveChanges();
            OnPropertyChanged("CardVMs");
            return c;
        }

        private Card AddCard(string title, string soundedText, string imageAddress, string description)
        {
            Random random = new Random();
            Card c = new Card() { Id = _cards.Count + 1, Title = title, SoundedText= soundedText,  ImageAddress = imageAddress, Description = description };
            _cards.Add(c);
            context.Cards.Add(c);
            context.SaveChanges();
            OnPropertyChanged("CardVMs");
            return c;
        }

        //создаем и добавляем в БД новый тег
        private Level AddLevel(object obj)
        {
           // if (SelectedCardVM == null) return null;

            Level t = new Level() { Id = _levels.Count + 1, Name = "Level " + (_levels.Count + 1).ToString(), Cards = new ObservableCollection<Card>()};
            _levels.Add(t);
            context.Levels.Add(t);
            context.SaveChanges();
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
            context.Entry(t).State = EntityState.Deleted;
            context.SaveChanges();

            OnPropertyChanged("LevelVMs");
            OnPropertyChanged("SelectedCardLevelVMs");
        }

        private void CreateCardsFromImageFiles(object obj)
        {
            string[] Filenames = new string[] { };
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Файлы изображений (*.bmp, *.jpg, *.png)|*.bmp;*.jpg;*.png";
                openFileDialog.Title = "Открыть свое изображение для карточки";
                openFileDialog.Multiselect = true;
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    Filenames = @openFileDialog.FileNames;
                    if (Filenames.Length < 1) return;
                    foreach (var filename in Filenames)
                    {
                        AddCard(Path.GetFileNameWithoutExtension(filename),
                            Path.GetFileNameWithoutExtension(filename),
                            filename, "Нет описания");
                    }
                    OnPropertyChanged("CardVMs");
                }
            }

        }
        private bool IsDBLoaded(object obj)
        {
            return DBTools.IsDBLoaded;
        }

        public void OnPropertyChangedCardVMs()
        {
            OnPropertyChanged("CardVMs");
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
                              bool res = DBTools.CreateDB(this, _cards, _levels,_levelPassings, context, DBFilename);
                              if (!res)
                              {
                                  System.Windows.MessageBox.Show("Ошибка загрузки базы данных " + DBFilename, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                                  return;
                              }
                              mainWindow.DataContext = this;
                              OnPropertyChanged("CardVMs");
                              OnPropertyChanged("LevelVMs");
                              OnPropertyChanged("DBFilename");
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
                              bool res = DBTools.LoadDB(this, _cards, _levels, _levelPassings, context, DBFilename);
                              if (!res)
                              {
                                  System.Windows.MessageBox.Show("Ошибка загрузки базы данных "+ DBFilename, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                                  return;
                              }
                              mainWindow.DataContext = this;
                              OnPropertyChanged("CardVMs");
                              OnPropertyChanged("LevelVMs");
                              OnPropertyChanged("DBFilename");
                          }
                      }



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
                              context.Entry(c).State = EntityState.Modified;
                          foreach (Level t in _levels)
                              context.Entry(t).State = EntityState.Modified;
                          context.SaveChanges();
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




        #endregion


    }
}
