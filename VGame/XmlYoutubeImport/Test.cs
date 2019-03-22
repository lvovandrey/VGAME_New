using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XmlYoutubeImport
{
    public class Test : INotifyPropertyChanged
    {
        private string name = "Press button ...";
        public string Name
        {
            get { return name; }

            set
            {
                if (value == name) return;
                name = value;
                OnPropertyChanged("Name");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }



        private DelegateCommand _buttonClick;
        public DelegateCommand ButtonClick
        {
            get
            {
                if (_buttonClick == null)
                    _buttonClick = new DelegateCommand(o =>
                    {
                        Name = "Well done!!!";
                    });
                return _buttonClick;
            }
        }

    }
}
