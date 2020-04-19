using CardsEditor.Abstract;
using CardsEditor.Model;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardsEditor.ViewModel
{
    public class CardVM:INPCBase
    {
        #region Constructors
        public CardVM(Card card)
        {
            _card = card;
        }
        #endregion

        #region Fields
        Card _card;
        #endregion

        #region Properties
        public Card Card
        {
            get { return _card; }
            set { _card = value; OnPropertyChanged("Card"); }
        }

        public int Id { get { return Card.Id; }  }
        public string Title { get { return Card.Title; } set { Card.Title = value; OnPropertyChanged("Title"); } }
        public string SoundedText { get { return Card.SoundedText; } set { Card.SoundedText = value; OnPropertyChanged("SoundedText"); } }
        public string Description { get { return Card.Description; } set { Card.Description = value; OnPropertyChanged("Description"); } }
        public string ImageAddress { get { return Card.ImageAddress; } set { Card.ImageAddress = value; OnPropertyChanged("ImageAddress"); OnPropertyChanged("ImageAdressURI"); } }
        public string SoundAddress { get { return Card.SoundAddress; } set { Card.SoundAddress = value; OnPropertyChanged("SoundAddress"); } }

        
        public Uri ImageAdressURI
        {
            get
            {
                if (Card.ImageAddress==null)
                    return new Uri(@"C:\1.jpg");
                else
                    return new Uri(Card.ImageAddress);
            }
        }


        #endregion

        #region Methods
        #endregion

        #region Commands


        private RelayCommand openImageFileCommand;
        public RelayCommand OpenImageFileCommand
        {
            get
            {
                return openImageFileCommand ?? (openImageFileCommand = new RelayCommand(obj =>
                {
                    OpenFileDialog openFileDialog = new OpenFileDialog();
                    openFileDialog.Filter = "Файлы изображений (*.bmp, *.jpg, *.png)|*.bmp;*.jpg;*.png";
                    openFileDialog.Title = "Открыть свое изображение для превью видео";
                    if (openFileDialog.ShowDialog() == true)
                        ImageAddress = @openFileDialog.FileName;
                    
                }));
            }
        }


        #endregion

    }
}
