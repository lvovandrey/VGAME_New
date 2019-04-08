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
    public class LevelsFromDB : INotifyPropertyChanged
    {
        private ObservableCollection<LevelSetVM> levelSetVMs;
        private List<LevelSet> levelSets;
        LevelSetContext db;
        public LevelsFromDB(LevelSetContext db) // В этом конструкторе заполняем тестовыми данными свойства ойства...
        {
            levelSetVMs = new ObservableCollection<LevelSetVM>();
            levelSets = new List<LevelSet>();

            var LS = db.LevelSets;
            foreach (LevelSet L in LS)
            {
                levelSets.Add(L);
            }
            foreach (LevelSet L in levelSets)
            {
                LevelSetVM levelSetVM = new LevelSetVM(L);
                levelSetVMs.Add(levelSetVM);
            }
        }

        

        public ObservableCollection<LevelSetVM> LevelSetVMs
        {
            get { return levelSetVMs; }
            set { levelSetVMs = value; OnPropertyChanged("SceneSetVMs"); }
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
