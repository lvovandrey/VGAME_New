﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VGameCore.Abstract;

//namespace VanyaGame.GameCardsNewDB.DB.RepositoryModel
//{
//    public class Card : INPCBase
//    {
//        [NotMapped]
//        private string _Title { get; set; }
//        [NotMapped]
//        private string _SoundedText { get; set; }
//        [NotMapped]
//        private string _Description { get; set; }
//        [NotMapped]
//        private string _ImageAddress { get; set; }
//        [NotMapped]
//        private string _SoundAddress { get; set; }
//        [NotMapped]
//        private ObservableCollection<Level> _Levels { get; set; }
//        [NotMapped]
//        private ObservableCollection<CardPassing> _CardPassings { get; set; } = new ObservableCollection<CardPassing>();


//        public int Id { get; set; }
//        public ObservableCollection<Level> Levels { get { return _Levels; } set { _Levels = value; OnPropertyChanged("Levels"); } }
//        public ObservableCollection<CardPassing> CardPassings { get { return _CardPassings; } set { _CardPassings = value; OnPropertyChanged("CardPassings"); } }
//        public string Title { get { return _Title; } set { _Title = value; OnPropertyChanged("Title"); } }
//        public string SoundedText { get { return _SoundedText; } set { _SoundedText = value; OnPropertyChanged("SoundedText"); } }
//        public string Description { get { return _Description; } set { _Description = value; OnPropertyChanged("Description"); } }
//        public string ImageAddress { get { return _ImageAddress; } set { _ImageAddress = value; OnPropertyChanged("ImageAddress"); } }
//        public string SoundAddress { get { return _SoundAddress; } set { _SoundAddress = value; OnPropertyChanged("SoundAddress"); } }


//        [NotMapped]
//        public Uri Source
//        {
//            get
//            {
//                if (ImageAddress == null) ImageAddress = "http://localhost/";
//                return new Uri(ImageAddress);
//            }
//            set
//            {
//                ImageAddress = value.ToString();
//                OnPropertyChanged("ImageAddress");
//                //проверим изменился ли тип ссылки на 
//            }
//        }
//    }

//    public class Level : INPCBase
//    {
//        [NotMapped]
//        private string _Name { get; set; }
//        [NotMapped]
//        private string _ImageAddress { get; set; }
//        [NotMapped]
//        private ObservableCollection<Card> _Cards { get; set; }
//        [NotMapped]
//        public ObservableCollection<LevelPassing> _LevelPassings { get; set; }


//        public int Id { get; set; }
//        public ObservableCollection<Card> Cards { get { return _Cards; } set { _Cards = value; OnPropertyChanged("Cards"); } }
//        public string Name { get { return _Name; } set { _Name = value; OnPropertyChanged("Name"); } }
//        public string ImageAddress { get { return _ImageAddress; } set { _ImageAddress = value; OnPropertyChanged("ImageAddress"); } }
//        public ObservableCollection<LevelPassing> LevelPassings { get { return _LevelPassings; } set { _LevelPassings = value; OnPropertyChanged("LevelPassings"); } }

//        [NotMapped]
//        public Uri Source
//        {
//            get
//            {
//                if (ImageAddress == null) ImageAddress = "http://localhost/";
//                return new Uri(ImageAddress);
//            }
//            set
//            {
//                ImageAddress = value.ToString();
//                OnPropertyChanged("ImageAddress");
//            }
//        }
//    }

//    public class LevelPassing : INPCBase
//    {
//        [NotMapped]
//        private string _DateAndTime { get; set; }
//        [NotMapped]
//        private bool _IsComplete { get; set; }
//        [NotMapped]
//        private ObservableCollection<CardPassing> _CardPassings { get; set; } = new ObservableCollection<CardPassing>();

//        public int Id { get; set; }
//        public ObservableCollection<CardPassing> CardPassings { get { return _CardPassings; } set { _CardPassings = value; OnPropertyChanged("CardPassings"); } }
//        public string DateAndTime { get { return _DateAndTime; } set { _DateAndTime = value; OnPropertyChanged("DateAndTime"); } }
//        public bool IsComplete { get { return _IsComplete; } set { _IsComplete = value; OnPropertyChanged("IsComplete"); } }
//    }

//    public class CardPassing : INPCBase
//    {
//        [NotMapped]
//        private string _DateAndTime { get; set; }
//        [NotMapped]
//        private int _AttemptsNumber { get; set; }

//        public int Id { get; set; }
//        public string DateAndTime { get { return _DateAndTime; } set { _DateAndTime = value; OnPropertyChanged("DateAndTime"); } }
//        public int AttemptsNumber { get { return _AttemptsNumber; } set { _AttemptsNumber = value; OnPropertyChanged("AttemptsNumber"); } }
//    }
//}
