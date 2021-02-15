using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using VanyaGame;
using VGameCore.Abstract;

namespace VGameCore.Units.Components
{
    /// <summary>
    /// Компонент отвечает за анимацию при появлении и исчезновении контейнера. 
    /// Должен быть подключен компонент HaveBody
    /// </summary>
    public class HiderShower : Component
    {
        #region constructors
        public HiderShower(string name, IComponentContainer container) : base(name, container)
        {
            if (Container.GetComponent<HaveBody>() == null)
                throw new Exception("Have no component HaveBody in contaier");
            Complete += HiderShower_Complete;
            CurOpacity = 1;
            CurMargin = new Thickness(0);
        }
        #endregion

        #region variables
        
        #endregion

        #region properties
        private double CurOpacity { get; set; }
        private Thickness CurMargin { get; set; }
        #endregion

        #region methods

        public event Action Complete;

        /// <summary>
        /// Очищаем подписку на событие Complete. При этом обработчик "по умолчанию" остается
        /// </summary>
        public void ResetCompleteSubscriptions()
        {
            Complete = null;
            Complete += HiderShower_Complete;
        }

        /// <summary>
        /// Запускаем анимацию появления/исчезновения (прозрачность и отступ изменяем плавно)
        /// </summary>
        /// <param name="opacity"></param>
        /// <param name="opacityDuration"></param>
        /// <param name="margin"></param>
        /// <param name="marginDuration"></param>
        public void Show(double opacity, TimeSpan opacityDuration, Thickness margin, TimeSpan marginDuration)
        {
            //вычисляем сколько займет операция
            TimeSpan fullTime = TimeSpan.FromMilliseconds(Math.Max(opacityDuration.TotalMilliseconds, marginDuration.TotalMilliseconds));

            CurMargin = margin;
            CurOpacity = opacity;
            FrameworkElement body = Container.GetComponent<HaveBody>().Body;
            //Удаляем анимацию если какая-то есть и включаем анимацию свою
            TDrawEffects.AllAnimationNull(body);
            body.BeginAnimation(FrameworkElement.OpacityProperty, new DoubleAnimation(opacity, opacityDuration));
            body.BeginAnimation(FrameworkElement.MarginProperty, new ThicknessAnimation(margin, marginDuration));
            //событие Complete вызывается когда все закончилось (это надежнее чем полагаться на complete встроенныый в Animation - тот может и не сработать)
            ToolsTimer.Delay(() => { Complete(); }, fullTime);
        }

        public void Show(double opacity, TimeSpan opacityDuration, Thickness margin, TimeSpan marginDuration, int ZIndex)
        {
            Show(opacity, opacityDuration, margin, marginDuration);
            FrameworkElement body = Container.GetComponent<HaveBody>().Body;
            Panel.SetZIndex(body, ZIndex);
        }

        public void Hide()
        {
            Show(0, TimeSpan.FromSeconds(0), Container.GetComponent<HaveBody>().Body.Margin, TimeSpan.FromSeconds(0));
        }

        /// <summary>
        /// Обработчик Complete по умолчанию
        /// </summary>
        private void HiderShower_Complete()
        {
            FrameworkElement body = Container.GetComponent<HaveBody>().Body;
            TDrawEffects.AllAnimationNull(body);
            body.Opacity = CurOpacity;
            body.Margin = CurMargin;
        }


        #endregion

    }
}
