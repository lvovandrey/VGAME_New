using MVVMRealization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VanyaGame.GameCardsNewDB.Interface
{
    public class MainWindowVM : INPCBase
    {
        public MainWindowVM(SettingsWindowVM settingsWindowVM)
        {
            SettingsWindowVM = settingsWindowVM;

        }

        private SettingsWindowVM _settingsWindowVM;
        public SettingsWindowVM SettingsWindowVM
        {
            get
            { return _settingsWindowVM; }
            set
            { _settingsWindowVM = value; OnPropertyChanged("SettingsWindowVM"); }
        }


        private RelayCommand openSettingsWindowOnDBSettings;
        public RelayCommand OpenSettingsWindowOnDBSettings
        {
            get
            {
                return openSettingsWindowOnDBSettings ??
                  (openSettingsWindowOnDBSettings = new RelayCommand(obj =>
                  {
                      Game.ShowSettingsWindow();
                      Game.SettingsWindow.SettingsElementShow("База данных");
                  }));
            }
        }
    }
}
