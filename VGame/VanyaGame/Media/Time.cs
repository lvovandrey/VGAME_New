using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VanyaGame.Media
{
    public class Time : INotifyPropertyChanged // Наследуемся от нужного интерфеса
    {
        // Ваши поля 
        private double _timeInSec;
        public event PropertyChangedEventHandler PropertyChanged; // Событие, которое нужно вызывать при изменении
        // Для удобства обернем событие в метод с единственным параметром - имя изменяемого свойства
        public void RaisePropertyChanged(string propertyName)
        {
            // Если кто-то на него подписан, то вызывем его
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        // А тут будут свойства, в которые мы обернем поля
        public double timeInSec
        {
            get { return _timeInSec; }
            set
            {
                // Устанавливаем новое значение
                _timeInSec = value;
                // Сообщаем всем, кто подписан на событие PropertyChanged, что поле изменилось Name
                RaisePropertyChanged("timeInSec");
            }
        }
    }
}
