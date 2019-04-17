using LevelSetsEditor.DB;
using LevelSetsEditor.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LevelSetsEditor.ViewModel
{
    public class VM : INotifyPropertyChanged
    {
        private LevelSet selectedLevelSet { get; set; }
        private ObservableCollection<LevelSet> LevelSets { get; set; }

        private ObservableCollection<LevelSetVM> _LevelSetVMs;
        public ObservableCollection<LevelSetVM> LevelSetVMs
        {
            get
            {
         //       LevelSetsVMsUpdate();
                return this._LevelSetVMs;
            }
            set
            {
                this._LevelSetVMs = value;
                OnPropertyChanged("LevelSetVMs");
               // LevelSetsVMsUpdate();
            }
        }
        public LevelSetContext db;

        public VM() 
        {
            LevelSets = new ObservableCollection<LevelSet>();
            using (LevelSetContext context = new LevelSetContext())
            {
                //LevelSet A = new LevelSet();
                //A.Name = "New __ " + (context.LevelSets.Count()+1).ToString(); ;
                //context.LevelSets.Add(A);
                //context.SaveChanges();

                if (context.LevelSets.Count() > 0)
                {
                    List<LevelSet> temp = context.LevelSets.ToList();
                    foreach (var item in temp)
                    {
                        LevelSets.Add(item);
                    }
                }
                
            }
            LevelSetsVMsUpdate();
        }

        public void LevelSetsVMsUpdate()
        {
            _LevelSetVMs = new ObservableCollection<LevelSetVM>();
            foreach (LevelSet levelSet in LevelSets)
            {
                _LevelSetVMs.Add(new LevelSetVM(levelSet));
            }
        }

        private LevelSetVM _SelectedLevelSet;
        public LevelSetVM SelectedLevelSet
        {
            get
            { return _SelectedLevelSet; }
            set
            {
                _SelectedLevelSet = value;
                OnPropertyChanged("SelectedLevelSet");
            }
                    
        }
        //{
        //    get
        //    {
        //        return new LevelSetVM(selectedLevelSet);
        //    }
        //    set
        //    {
        //        selectedLevelSet = value.GetLevelSet();

        //        OnPropertyChanged("SelectedLevelSet");
        //        using (LevelSetContext Context = new LevelSetContext())
        //        {
        //            Context.SaveChanges();
        //            foreach (Model.LevelSet L in Context.LevelSets)
        //            {
        //            }
        //        }
        //    }
        //}

        // команда добавления нового объекта
        private RelayCommand addCommand;
        public RelayCommand AddCommand
        {
            get
            {
                return addCommand ??
                  (addCommand = new RelayCommand(obj =>
                  {
                      LevelSet ls = new LevelSet();
                      ls.Name = "New Book";
                      LevelSets.Insert(0, ls);

                      selectedLevelSet = ls;
                      using (LevelSetContext context = new LevelSetContext())
                      {
                          context.LevelSets.Add(selectedLevelSet);
                          context.SaveChanges();
                      }
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
