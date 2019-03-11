using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace LevelSetsEditor.Model
{
	public class LevelSet
	{
        public LevelSet()
        {
            Test = new List<int>() {1,2,3 };
        }
        public VideoInfo VideoInfo
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		public ObservableCollection<SceneSet> SceneSets
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

        public string Name { get; set; }

        public List<int> Test { get; set; }
    }
}
