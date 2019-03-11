using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LevelSetsEditor.Model;

namespace LevelSetsEditor.ViewModel
{
    public class LevelSetVM: INotifyPropertyChanged
    {
        public LevelSet LevelSet;

        public LevelSetVM(LevelSet levelSet)
        {
            this.LevelSet = levelSet;
            Test = new ObservableCollection<int>(levelSet.Test); 
        }

        public string Name
        {
            get { return LevelSet.Name; }
            set
            {
                LevelSet.Name = value;
                OnPropertyChanged("Name");
            }
        }

        public ObservableCollection<int> Test { get; set; }

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
