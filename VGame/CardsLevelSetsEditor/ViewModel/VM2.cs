using LevelSetsEditor.DB;
using LevelSetsEditor.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LevelSetsEditor.ViewModel
{
    public class VM : INotifyPropertyChanged
    {

        private ObservableCollection<LevelSet> _levelsets { get; set; }
        private ObservableCollection<LevelSetVM> _levelsetvms { get; set; }

        public ObservableCollection<LevelSetVM> LevelSetVMs
        {
            get
            {
                _levelsetvms = new ObservableCollection<LevelSetVM>(from l in _levelsets select new LevelSetVM(l));
                return _levelsetvms;
            }
        }

        private LevelSetVM _SelectedLevelSetVM;
        public LevelSetVM SelectedLevelSetVM
        {
            get
            { return _SelectedLevelSetVM; }
            set
            {
                _SelectedLevelSetVM = value;
                OnPropertyChanged("SelectedLevelSetVM");
            }

        }

        LevelSetContext context;
        public VM()
        {
            context = new LevelSetContext();
            _levelsets = new ObservableCollection<LevelSet>();
            _levelsets = new ObservableCollection<LevelSet>();
            using (LevelSetContext context = new LevelSetContext())
            {
                var temp = Repository.Select<LevelSet>().Where(c => true == true).ToList();

                if (context.LevelSets.Count() > 0)
                {
                    foreach (var item in temp)
                    {
                        _levelsets.Add(item);
                    }
                }

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
                      _levelsets.First().VideoInfo.Title = "NEW TITLE";
                      _levelsets.First().Name = "New name";
                      context.Entry(_levelsets.First()).State = EntityState.Modified;
                    //  context.Entry(_levelsets).State = EntityState.Modified;
                      context.SaveChanges();
                      foreach (LevelSet vl in context.LevelSets)
                      { }
                      foreach (LevelSet vl in _levelsets)
                      { }
                      OnPropertyChanged("LevelSetVMs");

                  }));
            }
        }


        //        private LevelSet selectedLevelSet { get; set; }
        //        private ObservableCollection<LevelSet> LevelSets { get; set; }

        //        private ObservableCollection<LevelSetVM> _LevelSetVMs;
        //        public ObservableCollection<LevelSetVM> LevelSetVMs
        //        {
        //            get
        //            {
        //         //       LevelSetsVMsUpdate();
        //                return this._LevelSetVMs;
        //            }
        //            set
        //            {
        //                this._LevelSetVMs = value;
        //                OnPropertyChanged("LevelSetVMs");
        //               // LevelSetsVMsUpdate();
        //            }
        //        }
        //        public LevelSetContext db;

        //        public VM() 
        //        {
        //            LevelSets = new ObservableCollection<LevelSet>();
        //            using (LevelSetContext context = new LevelSetContext())
        //            {
        //                //LevelSet A = new LevelSet();
        //                //A.Name = "New __ " + (context.LevelSets.Count()+1).ToString(); ;
        //                //context.LevelSets.Add(A);
        //                //context.SaveChanges();
        //                var temp = Repository.Select<LevelSet>().Where(c => c.Id > 0).ToList();

        //                if (context.LevelSets.Count() > 0)
        //                {
        ////                    List<LevelSet> temp = context.LevelSets.ToList();
        //                    foreach (var item in temp)
        //                    {
        //                        LevelSets.Add(item);
        //                    }
        //                }

        //            }

        //        }



        //        private LevelSetVM _SelectedLevelSet;
        //        public LevelSetVM SelectedLevelSet
        //        {
        //            get
        //            { return _SelectedLevelSet; }
        //            set
        //            {
        //                _SelectedLevelSet = value;
        //                OnPropertyChanged("SelectedLevelSet");
        //            }

        //        }

        // команда добавления нового объекта
        private RelayCommand addCommand;
        public RelayCommand AddCommand
        {
            get
            {
                return addCommand ??
                  (addCommand = new RelayCommand(obj =>
                  {
                      //LevelSet ls = new LevelSet();
                      //ls.Name = "New Book";
                      //LevelSets.Insert(0, ls);

                      //selectedLevelSet = ls;
                      //using (LevelSetContext context = new LevelSetContext())
                      //{
                      //    context.LevelSets.Add(selectedLevelSet);
                      //    context.SaveChanges();
                      //}
                  }));
            }
        }



        #region mvvm
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

    }
}
