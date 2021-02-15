using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace VGameCore.Sets
{
    public enum VideoPlayerType 
    { 
        wpf,
        vlc
    }

    public class Settings
    {
        //Сделаем этот класс синглтоном
        private static Settings instance;
        private static object syncRoot = new object();

        private Settings()
        {
            RestoreAllSettings();
        }

        public static Settings GetInstance()
        {
            if (instance == null)
            {
                lock (syncRoot)
                {
                    if (instance == null)
                        instance = new Settings();
                }
            }
            return instance;
        }


        public event Action SettingsChanged;

        string backgroundGameOverFilename = "";
        public string BackgroundGameOverFilename
        {
            get
            {
                if (backgroundGameOverFilename == "" || backgroundGameOverFilename == "Not Found")
                    backgroundGameOverFilename = LocalAppDataDir + @"\Images\back.jpg";
                return backgroundGameOverFilename;
            }
            set
            {
                if (!File.Exists(value))
                {
                    MessageBox.Show("Нужно выбрать существующий файл изображения для фона окончания игры");
                    return;
                }
                backgroundGameOverFilename = value;
            }
        }

        string backgroundMenuFilename = "";
        public string BackgroundMenuFilename
        {
            get
            {
                if (backgroundMenuFilename == "" || backgroundMenuFilename == "Not Found")
                    backgroundMenuFilename = LocalAppDataDir + @"\Images\back.jpg";
                return backgroundMenuFilename;
            }
            set
            {
                if (!File.Exists(value))
                {
                    MessageBox.Show("Нужно выбрать существующий файл изображения для фона меню");
                    return;
                }
                backgroundMenuFilename = value;
            }
        }

        string backgroundStartFilename = "";
        public string BackgroundStartFilename
        {
            get
            {
                if (backgroundStartFilename == "" || backgroundStartFilename == "Not Found")
                    backgroundStartFilename = LocalAppDataDir + @"\Images\NewBack.jpg";
                return backgroundStartFilename;
            }
            set
            {
                if (!File.Exists(value))
                {
                    MessageBox.Show("Нужно выбрать существующий файл изображения для первоначального фона");
                    return;
                }
                backgroundStartFilename = value;
            }
        }

        #region GeneralSettings
        string videoPlayerType = "wpf";
        public VideoPlayerType VideoPlayerType 
        {
            get
            {
                if (videoPlayerType == "" || videoPlayerType == "Not Found")
                    videoPlayerType = "wpf";
                VideoPlayerType tmpvideoPlayerType = VideoPlayerType.wpf;
                Enum.TryParse(videoPlayerType, out tmpvideoPlayerType);
                return tmpvideoPlayerType;
            }
            set
            {
                videoPlayerType = value.ToString("G");
            }
        }

        string localAppDataDir = "";
        public string LocalAppDataDir
        {
            get
            {
                if (localAppDataDir == "" || localAppDataDir == "Not Found")
                    localAppDataDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "VGame");
                return localAppDataDir;
            }
            set
            {
                if (!Directory.Exists(value))
                {
                    MessageBox.Show("Нужно выбрать существующую директорию размещения основной программы");
                    return;
                }
                localAppDataDir = value;
            }
        }

        string appDir = "";
        public string AppDir
        {
            get
            {
                if (appDir == "" || appDir == "Not Found")
                    appDir = Environment.CurrentDirectory;
                return appDir;
            }
            set
            {
                if (!Directory.Exists(value))
                {
                    MessageBox.Show("Нужно выбрать существующую директорию размещения основной программы");
                    return;
                }
                appDir = value;
            }
        }


        string defaultVideo = "";
        public string DefaultVideo
        {
            get
            {
                if (defaultVideo == "" || defaultVideo == "Not Found")
                    defaultVideo = Path.Combine(LocalAppDataDir,"Video", "default.wmv");
                return defaultVideo;
            }
            set
            {
                if (!File.Exists(value) && Path.GetExtension(value)!=".wmv")
                {
                    MessageBox.Show("Нужно выбрать существующий видеофайл с расширением .wmv для указания видеофайла, проигрываемого по умолчанию");
                    return;
                }
                defaultVideo = value;
            }
        }

        string defaultImage = "";
        public string DefaultImage
        {
            get
            {
                if (defaultImage == "" || defaultImage == "Not Found")
                    defaultImage = Path.Combine(LocalAppDataDir, @"Images\default.jpg");
                return defaultImage;
            }
            set
            {
                if (!File.Exists(value) && Path.GetExtension(value) != ".jpg")
                {
                    MessageBox.Show("Нужно выбрать существующий файл изображения с расширением .jpg для указания изображения по умолчанию (отображается если нет другого изображения)");
                    return;
                }
                defaultImage = value;
            }
        }
        
        
        #endregion


        public void SaveAllSettings()
        {
            ConfigurationTools.AddUpdateAppSettings("BackgroundGameOverFilename", backgroundGameOverFilename);
            ConfigurationTools.AddUpdateAppSettings("BackgroundMenuFilename", backgroundMenuFilename);
            ConfigurationTools.AddUpdateAppSettings("BackgroundStartFilename", backgroundStartFilename);

            ConfigurationTools.AddUpdateAppSettings("VideoPlayerType", videoPlayerType);
            ConfigurationTools.AddUpdateAppSettings("AppDir", localAppDataDir);
            ConfigurationTools.AddUpdateAppSettings("DefaultVideo", defaultVideo);
            ConfigurationTools.AddUpdateAppSettings("DefaultImage", defaultImage);

            SettingsChanged?.Invoke();
        }

        public void RestoreAllSettings()
        {
            backgroundGameOverFilename = ConfigurationTools.ReadSetting("BackgroundGameOverFilename");
            backgroundMenuFilename = ConfigurationTools.ReadSetting("BackgroundMenuFilename");
            backgroundStartFilename = ConfigurationTools.ReadSetting("BackgroundStartFilename");

            videoPlayerType = ConfigurationTools.ReadSetting("VideoPlayerType");
            localAppDataDir = ConfigurationTools.ReadSetting("AppDir");
            defaultVideo = ConfigurationTools.ReadSetting("DefaultVideo");
            defaultImage = ConfigurationTools.ReadSetting("DefaultImage");

            SettingsChanged?.Invoke();
        }
    }
}
