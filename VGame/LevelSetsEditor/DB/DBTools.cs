using LevelSetsEditor.Model;
using LevelSetsEditor.Tools;
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
        public static bool LoadDB(VM vm,  ObservableCollection<Level> _levels, Context context, IInfoUI infoUI)
        {
            if (infoUI == null) infoUI = new EmptyInfoUi();
            bool error = false;
            try
            {
                SetInfoUI(infoUI);
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

                int levNumber = 0;
                int levCount = 1;
                IEnumerable<Level> LLL = context.Levels.OfType<Level>().Where(n => n.Id < 1000);
                levCount = LLL.Count();

                foreach (Level l in LLL)
                {
                    levNumber++;
                    infoUI.Progress = 100*levNumber /levCount; infoUI.Message = "Загрузка " + infoUI.Progress.ToString() + "%";

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
                infoUI.Hide();
            }
            catch
            {
                error = true;
                infoUI.Clear();
                infoUI.Hide();

            }
            return !error;
        }

        public static bool loadDB(VM vm, ObservableCollection<Level> _levels, Context context)
        {
            IInfoUI emptyInfoUI = new EmptyInfoUi();
            return LoadDB(vm, _levels, context, emptyInfoUI);
        }

        private static void SetInfoUI(IInfoUI infoUI)
        {
            infoUI.Clear();
            infoUI.Title = "Загрузка БД";
            infoUI.Progress = 0;
            infoUI.Message = "Загрузка ...%";
            infoUI.Show();
            
        }
    }
}
