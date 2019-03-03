using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VanyaGame.Media
{
    public class Volume : INotifyPropertyChanged // Наследуемся от нужного интерфеса
    {
        // Ваши поля 
        private double volume;
        public event PropertyChangedEventHandler PropertyChanged; // Событие, которое нужно вызывать при изменении
        // Для удобства обернем событие в метод с единственным параметром - имя изменяемого свойства
        public void RaisePropertyChanged(string propertyName)
        {
            // Если кто-то на него подписан, то вызывем его
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        // А тут будут свойства, в которые мы обернем поля
        public double level
        {
            get { return volume; }
            set
            {
                // Устанавливаем новое значение
                volume = value;
                // Сообщаем всем, кто подписан на событие PropertyChanged, что поле изменилось Name
                RaisePropertyChanged("level");
            }
        }
    }
}
