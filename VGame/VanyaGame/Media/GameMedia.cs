using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using VanyaGame.Media.Abstract;
using VanyaGame.Struct.Components;
using VanyaGame.Tools;

namespace VanyaGame.Media
{
    public delegate void MediaEvent();


    public class GameMedia
    {
        public event MediaEvent onStateChangeEvent;
        public event MediaEvent OnEnded;
        private MediaTimer timer_;
        public Volume volume;
        public Time timer;
        public Player player;
        public MediaControllers MediaGUI;

        public string FilePath;
        /// <summary>
        /// Словарь медиафайлов - ключ-"название", значение - путь к файлу на диске
        /// </summary>
        public Dictionary<string, string> Media = new Dictionary<string, string>();
        /// <summary>
        /// Упорядоченная синхронизированная с Media коллекция. Для информации об очередности треков. Не очень как-то... ну ладно.
        /// </summary>
        public List<string> OrderedMediaKeys = new List<string>();

        public bool IsPlaying = false;
        public bool IsPaused = false;
        public bool IsShuffle = false;
        public bool IsPlaylistRepeat = true;


        public GameMedia(ref Player _player)
        {
            player = _player;
            player.Ended += OnMediaEnded;

            volume = new Volume();
            volume.PropertyChanged += volume_PropertyChanged;
            volume.level = 50;

            MediaGUI = new MediaControllers();
            timer = new Time();
            timer.PropertyChanged += timer_PropertyChanged;
            timer.timeInSec = 0;
        }

        void timer_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {

        }

        void volume_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.player.Volume = (volume.level) / 100;
        }


        private void OnTimerTick(object sender, EventArgs e)
        {
            Game.Owner.Label1_Copy3.Content = player.Position.ToString("g");
            timer.timeInSec = player.Position.TotalSeconds;
            if (player.Position > timer_.TimeEnd)
            {
                if (timer_ != null) timer_.Stop();
                Stop();
                OnEnded();
            }
        }

        void OnMediaEnded()
        {
            if (OnEnded != null)
            {
                OnEnded();
                if (timer_ != null) timer_.Stop();
            }
            End();
        }
        public void ClearOnEndedEvent()
        {
            OnEnded = null;
        }


        public void Play(string MediaName)
        {
            if (!Media.ContainsKey(MediaName)) return;
            player.Source = @Media[MediaName];
            player.Play();
            IsPaused = false;
            IsPlaying = true;

            if (onStateChangeEvent != null) onStateChangeEvent();
        }
        public void Play()
        {
            if (IsPlaying)
            {
                if (timer_ != null) timer_.Start(OnTimerTick);

                player.UnPaused();
                IsPaused = false;

                if (onStateChangeEvent != null) onStateChangeEvent();
            }
        }
        public void PlayRandom(bool isPlaylistRepeat)
        {
            IsShuffle = true;
            IsPlaylistRepeat = isPlaylistRepeat;
            int c = Media.Count;
            int x = Game.RandomGenerator.Next(1, c + 1);
            int y = 1;
            string s = null;
            foreach (var M in Media)
            {
                y++;
                s = M.Key;
                if (y == x) { break; }
            }
            IsPlaying = true;
            Play(s);

        }

        public void PlayInOrder(bool isPlaylistRepeat)
        {
            IsShuffle = false;
            IsPlaylistRepeat = isPlaylistRepeat;
            string s = player.Source;
            string mkey = "";
            if (s == "" || s == null) mkey = OrderedMediaKeys.First();
            else
            {
                foreach (var item in OrderedMediaKeys)
                {
                    if (Media[item] == s)
                    {
                        if (OrderedMediaKeys.IndexOf(item) + 1 < OrderedMediaKeys.Count)
                            mkey = OrderedMediaKeys[OrderedMediaKeys.IndexOf(item) + 1];
                        else
                        {
                            if (IsPlaylistRepeat) mkey = OrderedMediaKeys.First();
                            else
                            {
                                IsPlaying = false; return;
                            }
                        }
                        break;
                    }
                }

            }
            IsPlaying = true;
            Play(mkey);
        }

        public void End()
        {
            if (IsPlaying)
            {
                if (IsShuffle) PlayRandom(IsPlaylistRepeat);
                else PlayInOrder(IsPlaylistRepeat);
            }
        }

        public void Start()
        {
            IsPlaying = true;
            PlayRandom(IsPlaylistRepeat);
        }

        public void Pause()
        {
            if (IsPlaying)
            {
                player.Pause();
                IsPaused = true;
                if (timer_ != null) timer_.Stop();
                if (onStateChangeEvent != null) onStateChangeEvent();
            }

        }
        public void Stop()
        {
            IsPlaying = false;
            IsPaused = false;
            player.Stop();
            if (onStateChangeEvent != null) onStateChangeEvent();
        }

        public virtual void Rewind(TimeSpan span, bool reverse)
        {
            TimeSpan NewTime = new TimeSpan();
            if (reverse) NewTime = player.Position - span;
            else NewTime = player.Position + span;
            player.Position = NewTime;
        }

        /// <summary>
        /// Загружает в словарь Media названия и пути к медиафайлам с указанным расширением из директории. 
        /// Берутся все подходящие файлы. Названия выделяются из имени файла 
        /// </summary>
        /// <param name="Dir">Путь к директории</param>
        /// <param name="file_extension">Расширения нужных медиафайлов без точки (например "avi")</param>
        protected void LoadMediaFilesFromDir(string Dir, string file_extension)
        {
            Media.Clear();
            string[] MediaPaths = Directory.GetFiles(Dir, "*." + file_extension, SearchOption.TopDirectoryOnly);
            foreach (string s in MediaPaths)
            {
                string tmp = System.IO.Path.GetFileNameWithoutExtension(s);
                Media.Add(tmp, s);
                OrderedMediaKeys.Add(tmp); //вот и получается что тут надо синхронно коллекции заполнять - не очень хорошо. Забуду где-нибудь и будут проблемы.
            }
        }

        /// <summary>
        /// Загружает в словарь Media названия и пути к медиафайлам. 
        /// </summary>
        /// <param name="filenames">Имена файлов</param>
        public virtual void LoadMediaFiles(List<string> filenames)
        {
            Media.Clear();
            foreach (string f in filenames)
            {
                string tmp = System.IO.Path.GetFileNameWithoutExtension(f);
                if (!Media.ContainsKey(tmp))
                    Media.Add(tmp, f);
                OrderedMediaKeys.Add(tmp);
            }
        }

        /// <summary>
        /// Загружает в словарь Media названия и пути к медиафайлам с расширениями AVI WMV MKV. 
        /// Берутся все подходящие файлы. Названия выделяются из имени файла 
        /// </summary>
        /// <param name="Dir">Путь к директории</param>
        public virtual void LoadMediaFilesFromDir(string Dir) //какой-то дурацкий метод.
        {
            Media.Clear();
            string[] MediaPaths = Directory.GetFiles(Dir, "*.avi", SearchOption.TopDirectoryOnly);
            foreach (string s in MediaPaths)
            {
                string tmp = System.IO.Path.GetFileName(s);
                Media.Add(tmp, s);
                OrderedMediaKeys.Add(tmp);
            }
            MediaPaths = Directory.GetFiles(Dir, "*.wmv", SearchOption.TopDirectoryOnly);
            foreach (string s in MediaPaths)
            {
                string tmp = System.IO.Path.GetFileName(s);
                Media.Add(tmp, s);
                OrderedMediaKeys.Add(tmp);
            }
            MediaPaths = Directory.GetFiles(Dir, "*.mkv", SearchOption.TopDirectoryOnly);
            foreach (string s in MediaPaths)
            {
                string tmp = System.IO.Path.GetFileName(s);
                Media.Add(tmp, s);
                OrderedMediaKeys.Add(tmp);
            }
        }

    }




    public class GameSound : GameMedia
    {

        public GameSound(ref Player _player) : base(ref _player)
        { }

        /// <summary>
        /// Загружает в словарь Media названия и пути к аудиофайлам (звукам) с расширением mp3. 
        /// Берутся все подходящие файлы. Названия выделяются из имени файла 
        /// </summary>
        /// <param name="Dir">Путь к директории</param>
        public override void LoadMediaFilesFromDir(string Dir)
        {
            base.LoadMediaFilesFromDir(Dir, "mp3");
        }

    }
    public class GameMusic : GameMedia
    {
        public GameMusic(ref Player _player) : base(ref _player)
        {
        }

        /// <summary>
        /// Загружает в словарь Media названия и пути к аудиофайлам (музыке) с расширением mp3. 
        /// Берутся все подходящие файлы. Названия выделяются из имени файла 
        /// </summary>
        /// <param name="Dir">Путь к директории</param>
        public override void LoadMediaFilesFromDir(string Dir)
        {
            base.LoadMediaFilesFromDir(Dir, "mp3");
        }
    }
    public class GameVideo : GameMedia
    {
        public Grid OwnGrid;

        public Slider SliderTimer;
        public Image PlayBtn;

        public GameVideo(ref Player _player)
            : base(ref _player)
        {
            OwnGrid = Game.Owner.GridVideo;
            onStateChangeEvent += OnStateChange;
        }

        /// <summary>
        /// Загружает в словарь Media названия и пути к видеофайлам с расширением avi. 
        /// Берутся все подходящие файлы. Названия выделяются из имени файла 
        /// </summary>
        /// <param name="Dir">Путь к директории</param>
        public override void LoadMediaFilesFromDir(string Dir)
        {
            base.LoadMediaFilesFromDir(Dir);
        }
        public virtual void ShowVideoPlayer()
        {
            player.Show();
        }
        public virtual void HideVideoPlayer()
        {
            player.Hide();
        }

        public void PlayBtnClick()
        {
            if (IsPlaying)
            {
                if (!IsPaused)
                {
                    Pause();

                }
                else
                {
                    Play();
                }
            }
        }
        void OnStateChange()
        {
            if (IsPlaying && !IsPaused)
                PlayBtn.Source = PictHelper.GetBitmapImage(new Uri("pack://application:,,,/Images/BtnStop.png"));
            else
                PlayBtn.Source = PictHelper.GetBitmapImage(new Uri("pack://application:,,,/Images/BtnPlay.png"));
        }

        /// <summary>
        /// Осуществляет перемотку на определенный интервал вперед или назад
        /// </summary>
        /// <param name="span">Интервал смещения при перемотке</param>
        /// <param name="reverse">если true - то перематываем назад</param>
        public override void Rewind(TimeSpan span, bool reverse)
        {


            TimeSpan NewTime = new TimeSpan();
            if (reverse) NewTime = player.Position - span;
            else NewTime = player.Position + span;

            TimeSpan MinTime = new TimeSpan(0, 0, (int)SliderTimer.Minimum);
            TimeSpan MaxTime = new TimeSpan(0, 0, (int)SliderTimer.Maximum);

            if (NewTime > MinTime)
            {
                if (NewTime < MaxTime)
                {
                    player.Position = NewTime;
                }
                else
                {
                    player.Position = MaxTime;
                }
            }
            else player.Position = MinTime;
        }

    }
}
