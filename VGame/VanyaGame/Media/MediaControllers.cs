using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace VanyaGame.Media
{
    /// <summary>
    /// Содержит коллекцию UI-контролов для управления медиаплеером и средства для работы с ними
    /// Всякие плюшки типа появления при движении мышей или 
    /// </summary>
    public class MediaControllers
    { 
        public List<FrameworkElement> UICollection; // коллекция элементов для управления медиа-проигрывателем



        public MediaControllers()
        {
            UICollection = new List<FrameworkElement>();
        }


        /// <summary>
        /// Добавляем в коллекцию элементов медиа-проигрывателем управления еще один
        /// </summary>
        /// <param name="UI">Элемент управления медиа-проигрывателем</param>
        public void Add(FrameworkElement UI)
        {
            UICollection.Add(UI);
        }

        /// <summary>
        /// Очищаем коллекцию элементов медиа-проигрывателем управления
        /// </summary>
        public void Clear()
        {
            UICollection.Clear();
        }

        /// <summary>
        /// Показывает все элементы пользовательского интерфейса для управления медиа-проигрывателем (из коллекции MediaUI)
        /// </summary>
        public void UIMediaShow()
        {
            foreach (FrameworkElement UI in UICollection)
            {
                UI.Visibility = Visibility.Visible;
            }
            foreach (FrameworkElement UI in UICollection)
            {
                ToolsTimer.Delay(() => { TDrawEffects.UIOpacityChange(UI, 0, 0, 1); }, new TimeSpan(0, 0, 0, 1, 500));
            }

        }
        /// <summary>
        /// Скрывает все элементы пользовательского интерфейса для управления медиа-проигрывателем (из коллекции MediaUI)
        /// </summary>
        public void UIMediaHide()
        {
            foreach (FrameworkElement UI in UICollection)
            {
                TDrawEffects.UIOpacityChange(UI, 1, 0, 0.01);
                UI.Visibility = Visibility.Hidden;
            }
        }
        /// <summary>
        /// Показывает все элементы пользовательского интерфейса для управления медиа-проигрывателем (из коллекции MediaUI) до полной непрозрачности
        /// </summary>
        public void UIMediaShowFull()
        {
            foreach (FrameworkElement UI in UICollection)
            {
                TDrawEffects.UIOpacityChange(UI, UI.Opacity, 1, 0.3);
            }

        }
        /// <summary>
        /// Делает полупрозрачными все элементы пользовательского интерфейса для управления медиа-проигрывателем (из коллекции MediaUI) 
        /// </summary>
        public void UIMediaHideNotFull()
        {
            ToolsTimer.Delay(() =>
            {
                if (!IsMouseOverUI())
                    foreach (FrameworkElement UI in UICollection)
                    {
                        TDrawEffects.UIOpacityChange(UI, UI.Opacity, 0, 2);
                    }
            }, new TimeSpan(0, 0, 2));
        }
        /// <summary>
        /// Показывает все элементы пользовательского интерфейса, а затем их прячет         
        /// </summary>
        public void UIMediaShowAndHideFull()
        {
            foreach (FrameworkElement UI in UICollection)
            {
                UI.BeginAnimation(FrameworkElement.OpacityProperty, null);
                TDrawEffects.UIOpacityChange(UI, UI.Opacity, 1, 0.3);
                ToolsTimer.Delay(() =>
                {
                    if (!IsMouseOverUI())
                        UIMediaHideNotFull();
                }, new TimeSpan(0, 0, 2));
            }

        }
        public bool IsMouseOverUI()
        {
            bool result = false;
            foreach (FrameworkElement UI in UICollection)
                if (UI.IsMouseOver) result = true;
            return result;
        }

    }
}
