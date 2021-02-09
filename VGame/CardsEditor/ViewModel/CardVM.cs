using CardsEditor.Tools;
using CardsGameNewDBRepository.Model;
using Microsoft.Win32;
using MVVMRealization;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Speech.Synthesis;
using System.Windows;

namespace CardsEditor.ViewModel
{
    public class CardVM : INPCBase
    {
        #region Constructors
        public CardVM(Card card, VM vm)
        {
            _card = card;
            _vm = vm;
            _cardStatisticVM = new CardStatisticVM(card, this, vm);
        }
        #endregion

        #region Fields
        Card _card;
        VM _vm;
        #endregion

        #region Properties
        private CardStatisticVM _cardStatisticVM;
        public CardStatisticVM CardStatisticVM
        {
            get { return _cardStatisticVM; }
            set { _cardStatisticVM = value; OnPropertyChanged("CardStatisticVM"); }
        }

        public Card Card
        {
            get { return _card; }
            set { _card = value; OnPropertyChanged("Card"); }
        }

        public int Id { get { return Card.Id; } }
        public string Title { get { return Card.Title; } set { Card.Title = value; OnPropertyChanged("Title"); } }
        public string SoundedText { get { return Card.SoundedText; } set { Card.SoundedText = value; OnPropertyChanged("SoundedText"); } }
        public string Description { get { return Card.Description; } set { Card.Description = value; OnPropertyChanged("Description"); } }
        public string ImageAddress { get { return Card.ImageAddress; } set { Card.ImageAddress = value; OnPropertyChanged("ImageAddress"); 
                OnPropertyChanged("ImageAdressURI");
                OnPropertyChanged("VideoAdressURI");
                OnPropertyChanged("VideoVisibility");
            } }
        public string SoundAddress { get { return Card.SoundAddress; } set { Card.SoundAddress = value; OnPropertyChanged("SoundAddress"); } }


        public Uri ImageAdressURI
        {
            get
            {
                string extention = Path.GetExtension(Card.ImageAddress);
                if (Card.ImageAddress != null && (extention == ".jpg" || extention == ".bmp" || extention == ".png" || extention == ".gif"))
                    return new Uri(Card.ImageAddress);
                else if (Card.ImageAddress != null && (extention == ".avi" || extention == ".wmv"))
                    return null;
                else
                    return new Uri(Settings.GetInstance().DefaultImageFilename);
            }
        }

        public Uri VideoAdressURI
        {
            get
            {
                string extention = Path.GetExtension(Card.ImageAddress);

                if (Card.ImageAddress != null && (extention == ".avi" || extention == ".wmv"))
                    return new Uri(Card.ImageAddress);
                else
                    return null;
            }
        }
        public Visibility VideoVisibility 
        {
            get 
            {
                if (VideoAdressURI != null) return Visibility.Visible;
                else return Visibility.Collapsed;
            }
        }

        private ObservableCollection<Level> _levels { get { return _card.Levels; } set { _card.Levels = value; OnPropertyChanged("AttachedLevelsVMs"); } }
        private ObservableCollection<LevelVM> _levelsvm { get; set; }

        public ObservableCollection<LevelVM> AttachedLevelsVMs
        {
            get
            {
                if (_levels == null) return null;
                _levelsvm = new ObservableCollection<LevelVM>(from l in _levels select new LevelVM(l, _vm));
                return _levelsvm;
            }
        }

        public int CountCardPassings
        {
            get
            {
                if (Card.CardPassings != null)
                    return Card.CardPassings.Count;
                else return 0;
            }
        }

        #endregion

        #region Methods
        internal void OnClearCardStatisticsVM()
        {
            CardStatisticVM = new CardStatisticVM(_card, this, _vm);
        }
        #endregion

        #region Commands


        private RelayCommand openImageFileCommand;
        public RelayCommand OpenImageFileCommand
        {
            get
            {
                return openImageFileCommand ?? (openImageFileCommand = new RelayCommand(obj =>
                {
                    OpenFileDialog openFileDialog = new OpenFileDialog();
                    openFileDialog.Filter = "Файлы изображений и видео (*.bmp, *.jpg, *.png, *.gif, *.wmv, *.avi)|*.bmp;*.jpg;*.png;*.gif;*.wmv;*.avi";
                    openFileDialog.Title = "Открыть свое изображение для карточки";
                    if (openFileDialog.ShowDialog() == true)
                    {
                        if (!VM.IsBigFilesCheckOk(new string[] { @openFileDialog.FileName }))
                            return;
                        ImageAddress = @openFileDialog.FileName;
                    }
                }));
            }
        }



        private RelayCommand soundedTextSpeakCommand;
        public RelayCommand SoundedTextSpeakCommand
        {
            get
            {
                return soundedTextSpeakCommand ?? (soundedTextSpeakCommand = new RelayCommand(obj =>
                {
                    if (SoundedText == null) return;
                    SpeechSynthesizer speaker = new SpeechSynthesizer();
                    if (_vm.TTSVoiceName == "" || _vm.TTSVoiceName == null) MessageBox.Show("Не выбран голос для синтеза речи на русском языке. Выберите пожалуйста, а то ничего не будет слышно.");
                    speaker.SelectVoice(_vm.TTSVoiceName);
                    speaker.Rate = _vm.TTSVoiceRate;
                    Console.WriteLine(_vm.TTSVoiceRate);
                    speaker.Volume = _vm.TTSVoiceVolume;
                    speaker.SpeakAsync(SoundedText);
                }));
            }
        }

        private RelayCommand copyTitleToSoundedTextCommand;
        public RelayCommand CopyTitleToSoundedTextCommand
        {
            get
            {
                return copyTitleToSoundedTextCommand ?? (copyTitleToSoundedTextCommand = new RelayCommand(obj =>
                {
                    SoundedText = Title;
                }));
            }
        }

        private RelayCommand detachCardToSelectedLevelCommand;
        public RelayCommand DetachCardToSelectedLevelCommand
        {
            get
            {
                return detachCardToSelectedLevelCommand ?? (detachCardToSelectedLevelCommand = new RelayCommand(obj =>
                {
                    _vm?.SelectedLevelVM?.DetachCardToSelectedLevel(this);
                }));
            }
        }

        private RelayCommand attachCardToSelectedLevelCommand;
        public RelayCommand AttachCardToSelectedLevelCommand
        {
            get
            {
                return attachCardToSelectedLevelCommand ?? (attachCardToSelectedLevelCommand = new RelayCommand(obj =>
                {
                    _vm?.SelectedLevelVM?.AttachCardToSelectedLevel(this);
                }));
            }
        }


        #endregion

    }
}
