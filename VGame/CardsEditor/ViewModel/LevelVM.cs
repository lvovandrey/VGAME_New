using CardsEditor.Abstract;
using CardsEditor.Model;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardsEditor.ViewModel
{
    public class LevelVM : INPCBase
    {
        #region Constructors
        public LevelVM(Level level, VM vm)
        {
            _level = level;
            _vm = vm;
        }
        #endregion

        #region Fields
        Level _level;
        VM _vm;
        #endregion

        #region Properties
        public Level Level
        {
            get { return _level; }
            set { _level = value; OnPropertyChanged("Level"); }
        }

        public int Id { get { return Level.Id; } }

        public string Name
        {
            get { return Level.Name; }
            set
            {
                Level.Name = value;
                OnPropertyChanged("Name");
            }
        }

        public string ImageAddress { get { return Level.ImageAddress; } set { Level.ImageAddress = value; OnPropertyChanged("ImageAddress"); OnPropertyChanged("ImageAdressURI"); } }
      
        public Uri ImageAdressURI
        {
            get
            {
                if (Level.ImageAddress == null)
                    return new Uri(@"C:\1.jpg");
                else
                    return new Uri(Level.ImageAddress);
            }
        }


        private ObservableCollection<Card> _cards { get { return _level.Cards; } set { _level.Cards = value; OnPropertyChanged("CardVMs"); } }
        private ObservableCollection<CardVM> _cardsvm { get; set; }

        public ObservableCollection<CardVM> AttachedCardVMs
        {
            get
            {
                _cardsvm = new ObservableCollection<CardVM>(from l in _cards select new CardVM(l,_vm));
                return _cardsvm;
            }
        }

        #endregion

        #region Methods
        public void DetachCardToSelectedLevel(CardVM cardVM)
        {
            if (!_cards.Contains(cardVM.Card)) return;
            _cards.Remove(cardVM.Card);
            OnPropertyChanged("AttachedCardVMs");
        }

        public void AttachCardToSelectedLevel(CardVM cardVM)
        {
            if (_cards.Contains(cardVM.Card)) return;
            _cards.Add(cardVM.Card);
            OnPropertyChanged("AttachedCardVMs");
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
                    openFileDialog.Filter = "Файлы изображений (*.bmp, *.jpg, *.png)|*.bmp;*.jpg;*.png";
                    openFileDialog.Title = "Открыть свое изображение для карточки";
                    if (openFileDialog.ShowDialog() == true)
                        ImageAddress = @openFileDialog.FileName;

                }));
            }
        }
        #endregion

    }
}
