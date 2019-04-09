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
        private LevelSet selectedLevelSet;
        public ObservableCollection<LevelSet> LevelSets { get; set; }
        public LevelSetContext db;

        public VM() 
        {
            LevelSets = new ObservableCollection<LevelSet>();
            using (LevelSetContext context = new LevelSetContext())
            {
                List<LevelSet> temp = context.LevelSets.ToList();
                foreach (var item in temp)
                {
                    LevelSets.Add(item);
                }
            }
        }


        public LevelSetVM SelectedLevelSet
        {
            get
            {
                return new LevelSetVM(selectedLevelSet);
            }
            set
            {
                selectedLevelSet = value.GetLevelSet();
            }
        }

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
                          context.LevelSets.Add(SelectedLevelSet);
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
