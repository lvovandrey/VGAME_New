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
        ObservableCollection<Tag> _tags = new ObservableCollection<Tag>();

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


        private ObservableCollection<TagVM> _tagvms { get; set; }
        public ObservableCollection<TagVM> TagVMs
        {
            get
            {
                _tagvms = new ObservableCollection<TagVM>(from t in _tags select new TagVM(t));
                return _tagvms;
            }
        }

        private TagVM _SelectedTagVM;
        public TagVM SelectedTagVM
        {
            get
            { return _SelectedTagVM; }
            set
            {
                _SelectedTagVM = value;
                OnPropertyChanged("SelectedTagVM");
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
                OnPropertyChanged("SelectedCardTagVMs");
            }

        }

        private TagVM _SelectedOnCardTagVM;
        public TagVM SelectedOnCardTagVM
        {
            get
            { return _SelectedOnCardTagVM; }
            set
            {
                _SelectedOnCardTagVM = value;
                OnPropertyChanged("SelectedOnCardTagVM");

            }
        }

        public ObservableCollection<TagVM> SelectedCardTagVMs
        {
            get
            {
                if (SelectedCardVM == null) return null;
                var _selcardTags = new ObservableCollection<TagVM>();
                Card card = SelectedCardVM.Card;
                if (card.Tags == null) return null;
                foreach (var tag in _tags)
                {
                    if (card.Tags.Contains(tag))
                    {
                        var t = (from tt in TagVMs where tt.Tag == tag select tt).First();
                        if (t == null) continue;
                        _selcardTags.Add(t);
                    }
                }
                return _selcardTags;
            }
        }

        #endregion

        #region Methods
        //позорный костыль для загрузки БД - так и не разобрался почему коллекция после выхода из статического метода не изменяется. а внутри меняется вроде.
        public void init(ObservableCollection<Card> cards, ObservableCollection<Tag> tags, Context Context)
        {
            _cards = cards;
            _tags = tags;
            context = Context;
        }

        //создаем и добавляем в БД новую каточку
        private Card Add(object obj)
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
        private Tag AddTag(object obj)
        {
            if (SelectedCardVM == null) return null;

            Tag t = new Tag() { Id = _tags.Count + 1, Name = "Tag " + (_tags.Count + 1).ToString(), Cards = new ObservableCollection<Card>() { SelectedCardVM.Card } };
            t.SoundedText = "Пустой текст";
            _tags.Add(t);
            context.Tags.Add(t);
            context.SaveChanges();
            OnPropertyChanged("TagVMs");
            OnPropertyChanged("SelectedCardTagVMs");
            return t;
        }

        //удаляем из БД тег
        private void RemoveTag(object obj)
        {
            Tag t = ((TagVM)obj).Tag;
            _tags.Remove(t);
            context.Entry(t).State = EntityState.Deleted;
            context.SaveChanges();
//            _tagvms.Remove((TagVM)obj);

            OnPropertyChanged("TagVMs");
            OnPropertyChanged("SelectedCardTagVMs");
        }

        private void AttachTagOnCard()
        {
            if (SelectedTagVM == null) return;
            if (SelectedCardVM == null) return;
            if (SelectedCardVM.Card.Tags == null) return;
            if (SelectedCardVM.Card.Tags.Contains(SelectedTagVM.Tag)) return;
            SelectedCardVM.Card.Tags.Add(SelectedTagVM.Tag);

            context.SaveChanges();
            OnPropertyChanged("SelectedCardTagVMs");
        }


        private void AttachAllTagsOnCard()
        {
            if (SelectedCardVM == null) return;
            foreach (var tvm in TagVMs)
            {
                if (SelectedCardVM.Card.Tags.Contains(tvm.Tag)) continue;
                SelectedCardVM.Card.Tags.Add(tvm.Tag);
            }

            context.SaveChanges();
            OnPropertyChanged("SelectedCardTagVMs");
        }
        private void DetachAllTagsFromCard()
        {
            

        }
        private void DetachTagFromCard()
        {
            if (SelectedOnCardTagVM == null) return;
            if (SelectedCardVM == null) return;
            if (!SelectedCardVM.Card.Tags.Contains(SelectedOnCardTagVM.Tag)) return;
            SelectedCardVM.Card.Tags.Remove(SelectedOnCardTagVM.Tag);
            context.Entry(SelectedCardVM.Card).State = EntityState.Modified;
            context.SaveChanges();
            OnPropertyChanged("SelectedCardTagVMs");
            OnPropertyChanged("SelectedOnCardTagVM");
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
                      bool res = DBTools.LoadDB(this, _cards, _tags, context);
                      if (!res)
                      {
                          MessageBox.Show("Ошибка загрузки базы данных", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                          return;
                      }
                      mainWindow.DataContext = this;
                      //context.SaveChanges();
                      //CardsVMs.Add(new CardVM(new Card()));
                      OnPropertyChanged("CardVMs");
                      OnPropertyChanged("TagVMs");
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
                              context.Entry(c).State = EntityState.Modified;
                          foreach (Tag t in _tags)
                              context.Entry(t).State = EntityState.Modified;
                          context.SaveChanges();
                          OnPropertyChanged("CardVMs");
                          OnPropertyChanged("TagVMs");
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



        private RelayCommand addTagCommand;
        public RelayCommand AddTagCommand
        {
            get
            {
                return addTagCommand ?? (addTagCommand = new RelayCommand(obj =>
                {
                    AddTag(obj);
                }));
            }
        }

        private RelayCommand removeTagCommand;
        public RelayCommand RemoveTagCommand

        {
            get
            {
                return removeTagCommand ?? (removeTagCommand = new RelayCommand(obj =>
                {
                    RemoveTag(SelectedTagVM);
                }));
            }
        }




        private RelayCommand attachTagOnCardCommand;
        public RelayCommand AttachTagOnCardCommand
        {
            get
            {
                return attachTagOnCardCommand ?? (attachTagOnCardCommand = new RelayCommand(obj =>
                {
                    AttachTagOnCard();
                }));
            }
        }



        private RelayCommand attachAllTagsOnCardCommand;
        public RelayCommand AttachAllTagsOnCardCommand
        {
            get
            {
                return attachAllTagsOnCardCommand ?? (attachAllTagsOnCardCommand = new RelayCommand(obj =>
                {
                    AttachAllTagsOnCard();
                }));
            }
        }

        private RelayCommand detachTagFromCardCommand;
        public RelayCommand DetachTagFromCardCommand
        {
            get
            {
                return detachTagFromCardCommand ?? (detachTagFromCardCommand = new RelayCommand(obj =>
                {
                    DetachTagFromCard();
                }));
            }
        }

        private RelayCommand detachAllTagsFromCardCommand;
        public RelayCommand DetachAllTagsFromCardCommand
        {
            get
            {
                return detachAllTagsFromCardCommand ?? (detachAllTagsFromCardCommand = new RelayCommand(obj =>
                {
                    DetachAllTagsFromCard();
                }));
            }
        }









        #endregion


    }
}
