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
        

        public LevelSetVM() // В этом конструкторе заполняем тестовыми данными свойства ойства...
        {
            this.LevelSet = new LevelSet() { Name = nameof(LevelSet.Name) };

        
            SceneSets = new ObservableCollection<SceneSetVM>();
            for (int i = 1; i <= 3; i++)
            {
                SceneSetVM sceneSetVM = new SceneSetVM(new Model.SceneSet()
                {
                    UnitsCount = 153 * i,
                    VideoSegment = new VideoSegment()
                    {
                        TimeBegin = TimeSpan.FromSeconds(i * 66),
                        TimeEnd = TimeSpan.FromSeconds(i * 88),
                        Source = new Uri("C:/test.avi")
                    }
                });
                SceneSets.Add(sceneSetVM);
            }

            videoInfoVM = new VideoInfoVM(new VideoInfo()
            {
                Title = "Айболит - серия 1",
                Duration = TimeSpan.FromSeconds(356),
                Description = "Мультфильм про доктора Айболита. СССР",
                Preview = new Preview()
                {
                    Size = new System.Drawing.Size(480,360),
                    Type = PreviewType.youtube,
                    Source = new Uri(@"https://img.youtube.com/vi/dDRqhwHdpe8/hqdefault.jpg")
                },
                Resolution = new System.Drawing.Size(720, 480),
                Source = new Uri(@"https://www.youtube.com/watch?v=dDRqhwHdpe8"),
                Type = VideoType.youtube
            });
        }


        public LevelSetVM(LevelSet levelSet)
        {
            this.LevelSet = levelSet;

        }

        private SceneSetVM selectedSceneSetVM;
        public SceneSetVM SelectedSceneSetVM
        {
            get { return selectedSceneSetVM; }
            set
            {
                selectedSceneSetVM = value;
                OnPropertyChanged("SelectedSceneSetVM");
            }
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

        private VideoInfoVM videoInfoVM;
        public VideoInfoVM VideoInfoVM
        {
            get { return videoInfoVM; }
            set
            {
                videoInfoVM = value;
                OnPropertyChanged("VideoInfoVM");
            }
        }



        public ObservableCollection<SceneSetVM> SceneSets { get; set; }

        // команда добавления нового объекта
        private RelayCommand autoSegregateVideoToScenes;
        public RelayCommand AutoSegregateVideoToScenes
        {
            get
            {
                return autoSegregateVideoToScenes ??
                  (autoSegregateVideoToScenes = new RelayCommand(obj =>
                  {
                      int i = 555;
                      SceneSetVM sceneSetVM = new SceneSetVM(new Model.SceneSet()
                      {
                          UnitsCount = 153 * i,
                          VideoSegment = new VideoSegment()
                          {
                              TimeBegin = TimeSpan.FromSeconds(i * 66),
                              TimeEnd = TimeSpan.FromSeconds(i * 88),
                              Source = new Uri("C:/test.avi")
                          }
                      });
                      SceneSets.Add(sceneSetVM);
                  }));
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
