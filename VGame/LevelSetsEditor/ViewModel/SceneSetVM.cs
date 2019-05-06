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
    public class SceneSetVM : INotifyPropertyChanged
    {
        private SceneSet sceneSet;

        public SceneSetVM(SceneSet _sceneSet)
        {
            this.sceneSet = _sceneSet;
        }


        public TimeSpan VideoSegment_TimeBegin
        {
            get
            {
                return sceneSet.VideoSegment.TimeBegin;
            }
            set
            {
                sceneSet.VideoSegment.TimeBegin = value;
                OnPropertyChanged("VideoSegment_TimeBegin");
            }
        }

        public TimeSpan VideoSegment_TimeEnd
        {
            get
            {
                return sceneSet.VideoSegment.TimeEnd;
            }
            set
            {
                sceneSet.VideoSegment.TimeEnd = value;
                OnPropertyChanged("VideoSegment_TimeEnd");
            }
        }

        public Uri VideoSegment_Source
        {
            get
            {
                return sceneSet.VideoSegment.Source;
            }
            set
            {
                sceneSet.VideoSegment.Source = value;
                OnPropertyChanged("VideoSegment_Source");
            }
        }

        public int UnitsCount
        {
            get
            {
                return sceneSet.UnitsCount;
            }
            set
            {
                sceneSet.UnitsCount = value;
                OnPropertyChanged("UnitsCount");
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
