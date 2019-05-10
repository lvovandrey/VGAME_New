using LevelSetsEditor.Model;
using LevelSetsEditor.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LevelSetsEditor.DB
{
    public class DBTools
    {
        public static bool loadDB(VM vm,  ObservableCollection<Level> _levels, Context context)
        {
            bool error = false;
            try
            {
                _levels = new ObservableCollection<Level>();

                context = new Context();

                IEnumerable<VideoInfo> VI = context.VideoInfoes.OfType<VideoInfo>().Where(n => n.Id < 1000);
                List<VideoInfo> VList = VI.ToList();

                IEnumerable<Preview> Pr = context.Previews.OfType<Preview>().Where(n => n.Id < 1000);
                List<Preview> PList = Pr.ToList();

                IEnumerable<Scene> Sc = context.Scenes.OfType<Scene>().Where(n => n.Id < 1000);
                List<Scene> ScList = Sc.ToList();

                IEnumerable<VideoSegment> Vss = context.VideoSegments.OfType<VideoSegment>().Where(n => n.Id < 1000);
                List<VideoSegment> VssList = Vss.ToList();

                ////То же самое с помощью операции OfType
                IEnumerable<Level> LLL = context.Levels.OfType<Level>().Where(n => n.Id < 1000);
                foreach (Level l in LLL)
                {
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

                vm.init(_levels, context);
            }
            catch
            {
                error = true;
            }
            return !error;
        }

    }
}
