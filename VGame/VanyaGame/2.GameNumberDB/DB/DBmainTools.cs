using LevelSetsEditor.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VanyaGame.GameNumberDB.DB
{
    public class DBmainTools
    {
        public ObservableCollection<Level> Levels = new ObservableCollection<Level>();
        public Context Context;

        //позорный костыль для загрузки БД - так и не разобрался почему коллекция после выхода из статического метода не изменяется. а внутри меняется вроде.
        public void init(ObservableCollection<Level> levels, Context context)
        {
            Levels = levels;
            Context = context;
        }

        public bool LoadDB(ObservableCollection<Level> _levels, Context context)
        {
          
            bool error = false;
            try
            {
                _levels = new ObservableCollection<Level>();

                context = new Context();

                IEnumerable<VideoInfo> VI = context.VideoInfoes.OfType<VideoInfo>().Where(n => n.Id < 10000);
                List<VideoInfo> VList = VI.ToList();

                IEnumerable<Preview> Pr = context.Previews.OfType<Preview>().Where(n => n.Id < 10000);
                List<Preview> PList = Pr.ToList();

                IEnumerable<Scene> Sc = context.Scenes.OfType<Scene>().Where(n => n.Id < 10000);
                List<Scene> ScList = Sc.ToList();

                IEnumerable<VideoSegment> Vss = context.VideoSegments.OfType<VideoSegment>().Where(n => n.Id < 10000);
                List<VideoSegment> VssList = Vss.ToList();

                int levNumber = 0;
                int levCount = 1;
                IEnumerable<Level> LLL = context.Levels.OfType<Level>().Where(n => n.Id < 10000);
                levCount = LLL.Count();

                foreach (Level l in LLL)
                {
                    levNumber++;
               
                    l.VideoInfo = VList.Where(n => n.Id == l.VideoInfoId).FirstOrDefault();
                    l.VideoInfo.Preview = PList.Where(n => n.Id == l.VideoInfo.PreviewId).FirstOrDefault();
                    var newScenes = l.Scenes.OrderBy(i => i.Position);
                    l.Scenes = new ObservableCollection<Scene>();
                    foreach (Scene s in newScenes)
                    {
                        l.Scenes.Add(s);
                    }
                    foreach (Scene s in l.Scenes)
                    {
                        foreach (VideoSegment v in VssList)
                        {
                            if (s.VideoSegmentId == v.Id)
                            {
                                s.VideoSegment = v;
                            }
                        }
                    }
                    l.RefreshYoutubeLink();
                    _levels.Add(l);
                }

                init(_levels, context);
             
            }
            catch
            {
                error = true;
            }
            return !error;
        }

    }
}
