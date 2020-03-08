using VanyaGame.GameNumberDB.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace VanyaGame.GameNumberDB.DB
{
    public class DBTools
    {
        public static bool LoadDB(ObservableCollection<Level> _levels, Context context)
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


            }
            catch
            {
                error = true;

            }
            return !error;
        }

        public static bool loadDB(ObservableCollection<Level> _levels, Context context)
        {
            return LoadDB( _levels, context);
        }

    }
}
