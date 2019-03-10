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
    public class VideoDataVM : INotifyPropertyChanged
    {
        private VideoData videoData;
        public VideoDataVM(VideoData _videoData)
        { videoData = _videoData; }

        private ObservableCollection<SceneDataVM> sceneDatas;
        public ObservableCollection<SceneDataVM> SceneDatas
        {
            get { return sceneDatas; }
            set
            {
                sceneDatas = value;
                OnPropertyChanged("SceneDatas");
            }
        }


        public SceneDataVM selectedSceneData;
        public SceneDataVM SelectedSceneData
        {
            get { return selectedSceneData; }
            set
            {
                selectedSceneData = value;
                OnPropertyChanged("SelectedSceneData");
            }
        }

        public string Title
        {
            get { return  videoData.Title; }
            set
            {
                videoData.Title = value;
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
