using CardsEditor.Abstract;
using CardsEditor.DB;
using CardsEditor.Model;
using LevelSetsEditor.DB;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
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
        }
        #endregion

        #region Fields
        Context context;
        MainWindow mainWindow;
        ObservableCollection<Card> _cards = new ObservableCollection<Card>();
        ObservableCollection<Level> _levels = new ObservableCollection<Level>();

        #endregion

        #region Properties
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



        #endregion

        #region Methods
        //позорный костыль для загрузки БД - так и не разобрался почему коллекция после выхода из статического метода не изменяется. а внутри меняется вроде.
        public void init(ObservableCollection<Card> cards, ObservableCollection<Level> levels, Context Context)
        {
            _cards = cards;
            _levels = levels;
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
            Level t = ((LevelVM)obj).Level;
            _levels.Remove(t);
            context.Entry(t).State = EntityState.Deleted;
            context.SaveChanges();
//            _Levelvms.Remove((LevelVM)obj);

            OnPropertyChanged("LevelVMs");
            OnPropertyChanged("SelectedCardLevelVMs");
        }

 
        #endregion

        #region Commands
        private RelayCommand loadDBCommand;
        public RelayCommand LoadDBCommand
        {
            get
            {
                return loadDBCommand ??
                  (loadDBCommand = new RelayCommand(obj =>
                  {

                      string DBFilename = "";
                      using (OpenFileDialog openFileDialog = new OpenFileDialog())
                      {
                          openFileDialog.Filter = "Файлы базы данных(*.db)|*.db";
                          openFileDialog.ValidateNames = false;
                          if (openFileDialog.ShowDialog() == DialogResult.OK)
                          {
                              DBFilename = @openFileDialog.FileName;
                              bool res = DBTools.LoadDB(this, _cards, _levels, context, DBFilename);
                              if (!res)
                              {
                                  System.Windows.MessageBox.Show("Ошибка загрузки базы данных "+ DBFilename, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                                  return;
                              }
                              mainWindow.DataContext = this;
                              OnPropertyChanged("CardVMs");
                              OnPropertyChanged("LevelVMs");
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
                  }));
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
                  }));
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
                          context.Entry(card).State = EntityState.Deleted;
                          context.SaveChanges();

                          _cards.Remove(card);
                          OnPropertyChanged("CardVMs");
                          SelectedCardVM = CardVMs.FirstOrDefault();
                      }
                  }));
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
                }));
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
                }));
            }
        }




        #endregion


    }
}
