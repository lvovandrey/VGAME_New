using CardsEditor.Abstract;
using CardsEditor.Model;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;
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
        }
        #endregion

        #region Fields
        Card _card;
        VM _vm;
        #endregion

        #region Properties
        public Card Card
        {
            get { return _card; }
            set { _card = value; OnPropertyChanged("Card"); }
        }

        public int Id { get { return Card.Id; } }
        public string Title { get { return Card.Title; } set { Card.Title = value; OnPropertyChanged("Title"); } }
        public string SoundedText { get { return Card.SoundedText; } set { Card.SoundedText = value; OnPropertyChanged("SoundedText"); } }
        public string Description { get { return Card.Description; } set { Card.Description = value; OnPropertyChanged("Description"); } }
        public string ImageAddress { get { return Card.ImageAddress; } set { Card.ImageAddress = value; OnPropertyChanged("ImageAddress"); OnPropertyChanged("ImageAdressURI"); } }
        public string SoundAddress { get { return Card.SoundAddress; } set { Card.SoundAddress = value; OnPropertyChanged("SoundAddress"); } }


        public Uri ImageAdressURI
        {
            get
            {
                if (Card.ImageAddress == null)
                    return new Uri(@"C:\1.jpg");
                else
                    return new Uri(Card.ImageAddress);
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

        #endregion

        #region Methods
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
                    openFileDialog.Filter = "Файлы изображений (*.bmp, *.jpg, *.png)|*.bmp;*.jpg;*.png";
                    openFileDialog.Title = "Открыть свое изображение для карточки";
                    if (openFileDialog.ShowDialog() == true)
                        ImageAddress = @openFileDialog.FileName;

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
                    if(_vm.TTSVoice == null) MessageBox.Show("Не выбран голос для синтеза речи на русском языке. Выберите пожалуйста, а то ничего не будет слышно.");
                    speaker.SelectVoice(_vm.TTSVoice.VoiceInfo.Name);
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
                    //var cardvm = obj as CardVM;
                    //if (cardvm == null) return;
                    _vm.SelectedLevelVM.DetachCardToSelectedLevel(this);
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
                    //var cardvm = obj as CardVM;
                    //if (cardvm == null) return;
                    _vm.SelectedLevelVM.AttachCardToSelectedLevel(this);
                }));
            }
        }


        #endregion

    }
}
