﻿using System;
using VGameCore.Abstract;

namespace VGameCore.Struct.Components
{
    public enum VideoType {local, net, youtube, ipcam, unknown}

    /// <summary>
    /// Инкапсулирует информацию и методы связанные с наличием в сцене (или уровне) прикрепленного видео
    /// </summary>
    public class InnerVideoSets : Component
    {
        #region constructors
        public InnerVideoSets(string name, IComponentContainer container) : base(name, container)
        {
            VideoFileName = "";
            VideoFileType = VideoType.unknown;
        }
        #endregion

        #region variables
        #endregion

        #region properties
        /// <summary>
        /// Имя видеофайла
        /// </summary>
        public string VideoFileName { get; set; }

        /// <summary>
        /// Тип видеофайла
        /// </summary>
        public VideoType VideoFileType { get; set; }

        /// <summary>
        /// Номер отрезка (видеофайла)
        /// </summary>
        public string VideoFileNumber { get; set; }

        /// <summary>
        /// Начало проигрываемого отрезка видеофайла
        /// </summary>
        public TimeSpan VideoTimeBegin { get; set; }

        /// <summary>
        /// Конец проигрываемого отрезка видеофайла
        /// </summary>
        public TimeSpan VideoTimeEnd { get; set; }

        #endregion

        #region methods

        #endregion
    }
}
