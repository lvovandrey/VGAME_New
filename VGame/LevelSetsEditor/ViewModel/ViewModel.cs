using LevelSetsEditor.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LevelSetsEditor.ViewModel
{
    public class MyViewModel : INotifyPropertyChanged
    {
        

        public static LevelsFromDB LevelsFromDB { get; set; }
        public LevelSetContext db;

        public void OpenDb()
        {
            db = new LevelSetContext();
        }
        public void CloseDb()
        {
            db.Dispose();
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
