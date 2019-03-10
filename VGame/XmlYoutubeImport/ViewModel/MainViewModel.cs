using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XmlYoutubeImport.Model;

namespace XmlYoutubeImport.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<VideoDataVM> VideoDataList { get; set; }

        public MainViewModel()
        { }
        public MainViewModel(List<VideoData> videoDatas)
        {
            VideoDataList = new ObservableCollection<VideoDataVM>(videoDatas.Select(vd => new VideoDataVM(vd)));
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
