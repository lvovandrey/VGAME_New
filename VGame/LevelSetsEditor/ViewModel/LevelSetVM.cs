﻿using System;
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

            Test = new ObservableCollection<int>(LevelSet.Test);
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
        }


        public LevelSetVM(LevelSet levelSet)
        {
            this.LevelSet = levelSet;
            Test = new ObservableCollection<int>(levelSet.Test);

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

        public ObservableCollection<int> Test { get; set; }
        public ObservableCollection<SceneSetVM> SceneSets { get; set; }

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