using LevelSetsEditor.Model;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LevelSetsEditor.ViewModel
{
    public class SceneVM : INotifyPropertyChanged
    {
        public Scene scene;
        private VideoPlayerVM videoPlayerVM;
        public int SceneId { get { return scene.Id; } }

        public SceneVM(Scene _scene, VideoPlayerVM _videoPlayerVM)
        {
            this.scene = _scene;
            videoPlayerVM = _videoPlayerVM;
            TrackTime = true;
        }


        public TimeSpan VideoSegment_TimeBegin
        {
            get
            {
                return scene.VideoSegment.TimeBegin;
            }
            set
            {
                scene.VideoSegment.TimeBegin = value;
                OnPropertyChanged("VideoSegment_TimeBegin");
            }
        }

        public TimeSpan VideoSegment_TimeEnd
        {
            get
            {
                return scene.VideoSegment.TimeEnd;
            }
            set
            {
                scene.VideoSegment.TimeEnd = value;
                OnPropertyChanged("VideoSegment_TimeEnd");
            }
        }

        public Uri VideoSegment_Source
        {
            get
            {
                return scene.VideoSegment.Source;
            }
            set
            {
                scene.VideoSegment.Source = value;
                OnPropertyChanged("VideoSegment_Source");
            }
        }

        private bool _TrackTime { get; set; }
        public bool TrackTime
        {
            get { return _TrackTime; }
            set { _TrackTime = value; OnPropertyChanged("TrackTime"); }
        }

        public int UnitsCount
        {
            get
            {
                return scene.UnitsCount;
            }
            set
            {
                scene.UnitsCount = value;
                OnPropertyChanged("UnitsCount");
            }
        }

        public int TasksCount
        {
            get
            {
                return scene.TasksCount;
            }
            set
            {
                scene.TasksCount = value;
                OnPropertyChanged("TasksCount");
            }
        }


        public int Position
        {
            get
            {
                return scene.Position;
            }
            set
            {
                scene.Position = value;
                OnPropertyChanged("Position");
            }
        }

        private RelayCommand incTimeCommand;
        public RelayCommand IncTimeCommand
        {
            get
            {
                return incTimeCommand ??
                    (incTimeCommand = new RelayCommand(obj =>
                    {
                        VideoSegment_TimeBegin += TimeSpan.FromSeconds(1);
                    }));
            }
        }


       


//        private DelegateCommand<MouseWheelEventArgs> yourCommand;
        public DelegateCommand<MouseWheelEventArgs> WheelTimeBeginCommand
        {
            get
            {
                return new DelegateCommand<MouseWheelEventArgs>(args =>
                {
                    if (args.Delta > 0)
                    {
                        VideoSegment_TimeBegin += TimeSpan.FromSeconds(1);
                    }
                    if (args.Delta < 0)
                    {
                        VideoSegment_TimeBegin -= TimeSpan.FromSeconds(1);
                    }

                    if(TrackTime) videoPlayerVM.CurTime = VideoSegment_TimeBegin;
                });
            }
        }
        public DelegateCommand<MouseWheelEventArgs> WheelTimeEndCommand
        {
            get
            {
                return new DelegateCommand<MouseWheelEventArgs>(args =>
                {
                    if (args.Delta > 0)
                    {
                        VideoSegment_TimeEnd += TimeSpan.FromSeconds(1);
                    }
                    if (args.Delta < 0)
                    {
                        VideoSegment_TimeEnd -= TimeSpan.FromSeconds(1);
                    }
                    if (TrackTime) videoPlayerVM.CurTime = VideoSegment_TimeEnd;
                });
            }
        }

        
        public DelegateCommand<MouseWheelEventArgs> WheelUnitsCommand
        {
            get
            {
                return new DelegateCommand<MouseWheelEventArgs>(args =>
                {
                    if (args.Delta > 0)
                    {
                        UnitsCount += 1;
                    }
                    if (args.Delta < 0)
                    {
                        UnitsCount -= 1;
                    }
                });
            }
        }

        public DelegateCommand<MouseWheelEventArgs> WheelTasksCommand
        {
            get
            {
                return new DelegateCommand<MouseWheelEventArgs>(args =>
                {
                    if (args.Delta > 0)
                    {
                        TasksCount += 1;
                    }
                    if (args.Delta < 0)
                    {
                        TasksCount -= 1;
                    }
                });
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
