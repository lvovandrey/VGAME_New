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
        public VideoDataVM VideoData
        {
            get { return V; }
            set
            {
                title = value;
                OnPropertyChanged("Title");
            }
        }

        public MainViewModel()
        {
            VideoDataVM VideoData = new VideoDataVM(new VideoData() { Title = "Заголовок1" });
            VideoData.SceneDatas = new ObservableCollection<SceneDataVM>();
            VideoData.SceneDatas.Add(new SceneDataVM() { Num = 1 });
            VideoData.SceneDatas.Add(new SceneDataVM() { Num = 2 });
            VideoData.SceneDatas.Add(new SceneDataVM() { Num = 3 });


        }
        public MainViewModel(List<VideoData> videoDatas)
        {




            // VideoDataList = new ObservableCollection<VideoDataVM>(videoDatas.Select(vd => new VideoDataVM(vd)));
        }
        string title = "123";
        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                OnPropertyChanged("Title");
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
