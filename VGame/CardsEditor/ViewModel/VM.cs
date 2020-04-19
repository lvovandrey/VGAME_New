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

namespace CardsEditor.ViewModel
{
    public class VM: INPCBase
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
        #endregion

        #region Properties
        private ObservableCollection<CardVM> _cardvms { get; set; }
        public ObservableCollection<CardVM> CardVMs
        {
            get
            {
                if (_cards == null) return null;
                _cardvms = new ObservableCollection<CardVM>(from l in _cards select new CardVM(l));
                return _cardvms;
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
            }

        }
        #endregion

        #region Methods
        //позорный костыль для загрузки БД - так и не разобрался почему коллекция после выхода из статического метода не изменяется. а внутри меняется вроде.
        public void init(ObservableCollection<Card> cards, Context Context)
        {
            _cards = cards;
            context = Context;
        }

        //создаем и добавляем в БД новую каточку
        private Card Add(object obj)
        {
            Random random = new Random();
            Card c = new Card() { Id = _cards.Count + 1, Title = "Card " + (_cards.Count + 1).ToString(), ImageAddress=@"C:\1.jpg" };
            c.Description = "Описание";
            _cards.Add(c);
            context.Cards.Add(c);
            context.SaveChanges();
            OnPropertyChanged("CardVMs");
            return c;
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
                      bool res = DBTools.LoadDB(this, _cards, context);
                      if (!res)
                      {
                          MessageBox.Show("Ошибка загрузки базы данных", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                          return;
                      }
                      mainWindow.DataContext = this;
                      OnPropertyChanged("CardsVMs");
                  }));
            }
        }

        private RelayCommand addCommand;
        public RelayCommand AddCommand
        {
            get
            {
                return addCommand ??
                  (addCommand = new RelayCommand(obj =>
                  {
                      Add(obj);
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
                          {
                              context.Entry(c).State = EntityState.Modified;
                          }
                          context.SaveChanges();
                          OnPropertyChanged("CardVMs");
                      }
                  }));
            }
        }



        private RelayCommand removeCommand;
        public RelayCommand RemoveCommand
        {
            get
            {
                return removeCommand ??
                  (removeCommand = new RelayCommand(obj =>
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
        #endregion


    }
}
