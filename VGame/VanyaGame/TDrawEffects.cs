using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace VanyaGame
{
    //----------------------------------------------------------
    // БЫЛО БЫ НЕ ПЛОХО ЗДЕСЬ РЕАЛИЗОВАТЬ ОБОБЩЕНИЯ КАКИЕ-НИБУДЬ
    //    А зачем? уж и сам забыл
    //----------------------------------------------------------
    public partial class TDrawEffects
    {

        public static Point GeneratePosition(Rect Limits)
        {
            int x = Game.RandomGenerator.Next((int)Limits.Left, (int)Limits.Width + (int)Limits.Left);
            int y = Game.RandomGenerator.Next((int)Limits.Top, (int)Limits.Height + (int)Limits.Top);
            Point value = new Point(x, y);
            return value;
        }
        public static void Show(FrameworkElement UI)
        {
            UI.Visibility = System.Windows.Visibility.Visible;
        }
        public static void Hide(FrameworkElement UI)
        {
            UI.Visibility = System.Windows.Visibility.Collapsed;
        }

        #region методы анимационные 
        public static void BlurShow(FrameworkElement UI, double timeInSec)
        {
            BlurShow(UI,timeInSec,0, () => {});
        }
        
        
        public static void Delay(FrameworkElement UI, double delay, VoidDelegate EndingMethod)
        {
            DoubleAnimation buttonAnimation = new DoubleAnimation();
            buttonAnimation.From = UI.Opacity;
            buttonAnimation.To = UI.Opacity;
            buttonAnimation.Duration = TimeSpan.FromSeconds(delay);
            buttonAnimation.Completed += (s, _) =>
            {
                UI.BeginAnimation(FrameworkElement.OpacityProperty, null);
                EndingMethod(); 
            };
            UI.BeginAnimation(FrameworkElement.OpacityProperty, buttonAnimation);
        }
        
        public static void BlurShow(FrameworkElement UI, double timeInSec, double delay, VoidDelegate EndingMethod)
        {
            UI.Visibility = Visibility.Visible;
            Show(UI);
            UI.Opacity = 0;
            DoubleAnimation buttonAnimation = new DoubleAnimation();
            buttonAnimation.From = UI.Opacity;
            buttonAnimation.To = 1;
            buttonAnimation.Duration = TimeSpan.FromSeconds(timeInSec);
            buttonAnimation.BeginTime = TimeSpan.FromSeconds(delay);
            buttonAnimation.Completed += (s, _) =>
            {
                double CurValue = UI.Opacity;
                UI.BeginAnimation(FrameworkElement.OpacityProperty, null);
                UI.Opacity = 1;
                UI.Visibility = Visibility.Visible;
                EndingMethod(); 
            };
            UI.BeginAnimation(FrameworkElement.OpacityProperty, buttonAnimation);
        }
        public static void BlurHide(FrameworkElement UI, double timeInSec, double delay,  VoidDelegate EndingMethod)
        {
            DoubleAnimation buttonAnimation = new DoubleAnimation();
            buttonAnimation.From = UI.Opacity;
            buttonAnimation.To = 0;
            buttonAnimation.Duration = TimeSpan.FromSeconds(timeInSec);
            buttonAnimation.BeginTime = TimeSpan.FromSeconds(delay);
            buttonAnimation.Completed += (s, _) =>
            {   
                double CurValue = UI.Opacity;
                UI.BeginAnimation(FrameworkElement.OpacityProperty, null);
                UI.Opacity = CurValue;
                UI.Visibility = Visibility.Collapsed;
                EndingMethod(); 
            };
            UI.BeginAnimation(FrameworkElement.OpacityProperty, buttonAnimation);
        }
        public static void BlurHide(FrameworkElement UI, double timeInSec)
        {
            BlurHide(UI,timeInSec, 0, () => {});
        }
        
        

        public static void Zoom(FrameworkElement UI, double ZoomBeg, double ZoomEnd, double timeInSec)
        {
            DoubleAnimation A = new DoubleAnimation();
            A.From = UI.ActualWidth * ZoomBeg;
            A.To = UI.ActualWidth * ZoomEnd;
            A.Duration = TimeSpan.FromSeconds(timeInSec);
            A.Completed += (s, _) =>
            {
                double CurValue = UI.ActualWidth;
                
                UI.Width = CurValue;
                
            };

            DoubleAnimation A2 = new DoubleAnimation();
            A2.From = UI.ActualHeight * ZoomBeg;
            A2.To = UI.ActualHeight * ZoomEnd;
            A2.Duration = TimeSpan.FromSeconds(timeInSec);
            A2.Completed += (s, _) =>
            {
                double CurValue = UI.ActualHeight;
                UI.BeginAnimation(FrameworkElement.HeightProperty, null);
                UI.Height = CurValue;
                
            };
            // Анимация margin
            ThicknessAnimation ta = new ThicknessAnimation();
            double x = (double)((A.To - A.From) / 2);
            double y = (double)((A2.To - A2.From) / 2); ;

            Thickness topMargin = new Thickness(-x, -y, 0, 0);
            ta.By = topMargin;
            ta.Duration = TimeSpan.FromSeconds(timeInSec);
            ta.Completed += (s, _) =>
            {
                Thickness CurValue = UI.Margin;
                UI.BeginAnimation(FrameworkElement.MarginProperty, null);
                UI.Margin = CurValue;
                
            };
            
            UI.BeginAnimation(FrameworkElement.WidthProperty, A);
            UI.BeginAnimation(FrameworkElement.HeightProperty, A2);
            UI.BeginAnimation(FrameworkElement.MarginProperty, ta);
        }
        public static void Zoom(FrameworkElement UI, double ZoomBeg, double ZoomEnd, double timeInSec, VoidDelegate EndingMethod) 
        {
            DoubleAnimation A = new DoubleAnimation();
            A.From = UI.ActualWidth * ZoomBeg;
            A.To = UI.ActualWidth * ZoomEnd;
            A.Duration = TimeSpan.FromSeconds(timeInSec);
            A.Completed += (s, _) =>
            {
                double CurValue = UI.ActualWidth;

                UI.Width = CurValue;

            };

            DoubleAnimation A2 = new DoubleAnimation();
            A2.From = UI.ActualHeight * ZoomBeg;
            A2.To = UI.ActualHeight * ZoomEnd;
            A2.Duration = TimeSpan.FromSeconds(timeInSec);
            A2.Completed += (s, _) =>
            {
                double CurValue = UI.ActualHeight;
                UI.BeginAnimation(FrameworkElement.HeightProperty, null);
                UI.Height = CurValue;

            };
            // Анимация margin
            ThicknessAnimation ta = new ThicknessAnimation();
            double x = (double)((A.To - A.From) / 2);
            double y = (double)((A2.To - A2.From) / 2); ;

            Thickness topMargin = new Thickness(-x, -y, 0, 0);
            ta.By = topMargin;
            ta.Duration = TimeSpan.FromSeconds(timeInSec);
            ta.Completed += (s, _) =>
            {
                Thickness CurValue = UI.Margin;
                UI.BeginAnimation(FrameworkElement.MarginProperty, null);
                UI.Margin = CurValue;

                AllAnimationNull(UI);

                EndingMethod();
            };

            UI.BeginAnimation(FrameworkElement.WidthProperty, A);
            UI.BeginAnimation(FrameworkElement.HeightProperty, A2);
            UI.BeginAnimation(FrameworkElement.MarginProperty, ta);
        }
        public static void Zoom(FrameworkElement UI, double ZoomBeg, double ZoomEnd, double Xcenter, double Ycenter, double timeInSec)
        {
            DoubleAnimation A = new DoubleAnimation();
            A.From = UI.Width * ZoomBeg;
            A.To = UI.Width * ZoomEnd;
            A.Duration = TimeSpan.FromSeconds(timeInSec);
            A.Completed += (s, _) =>
            {
                double CurValue = UI.ActualWidth;
                UI.BeginAnimation(FrameworkElement.WidthProperty, null);
                UI.Width = CurValue;
                
            };
            

            DoubleAnimation A2 = new DoubleAnimation();
            A2.From = UI.Height * ZoomBeg;
            A2.To = UI.Height * ZoomEnd;
            A2.Duration = TimeSpan.FromSeconds(timeInSec);
            A2.Completed += (s, _) =>
            {
                double CurValue = UI.ActualHeight;
                UI.BeginAnimation(FrameworkElement.HeightProperty, null);
                UI.Height = CurValue;
                
            };
            
            //     UI.Margin.L = Xcenter - (buttonAnimation.From / 2);
            Thickness Pos = new Thickness();
            Pos.Left = (double)(Xcenter - (A.From / 2));
            Pos.Top = (double)(Ycenter - (A2.From / 2));
            UI.Margin = Pos;
            // Анимация margin
            ThicknessAnimation ta = new ThicknessAnimation();
            double x = (double)((A.To - A.From) / 2);
            double y = (double)((A2.To - A2.From) / 2); ;

            Thickness topMargin = new Thickness(0, 0, 0, 0);
            ta.By = topMargin;
            ta.Duration = TimeSpan.FromSeconds(timeInSec);
            ta.Completed += (s, _) =>
            {
                Thickness CurValue = UI.Margin;
                UI.BeginAnimation(FrameworkElement.MarginProperty, null);
                UI.Margin = CurValue;
                
            };

            UI.BeginAnimation(FrameworkElement.WidthProperty, A);
            UI.BeginAnimation(FrameworkElement.HeightProperty, A2);
            UI.BeginAnimation(FrameworkElement.MarginProperty, ta);
        }
        /// <summary>
        /// Смещает на определенный отрезок
        /// </summary>
        /// <param name="UI">Смещаемый элемент</param>
        /// <param name="dx">Смещение по горизонтали</param>
        /// <param name="dy">Смещение по вертикали</param>
        /// <param name="timeInSec">Время на перемещение</param>
        public static void MoveShift(FrameworkElement UI, double dx, double dy, double timeInSec)
        {
            // Анимация margin
            ThicknessAnimation ta = new ThicknessAnimation();
            Thickness topMargin = new Thickness(dx, dy, 0, 0);
            ta.By = topMargin;
            ta.Duration = TimeSpan.FromSeconds(timeInSec);
            ta.Completed += (s, _) =>
            {
                Thickness CurValue = UI.Margin;
                UI.BeginAnimation(FrameworkElement.MarginProperty, null);
                UI.Margin = CurValue;
                
            };
            UI.BeginAnimation(FrameworkElement.MarginProperty, ta);
        }
        /// <summary>
        /// Плавно перемещает элемент в определенную позицию
        /// </summary>
        /// <param name="UI">Смещаемый элемент</param>
        /// <param name="x">Новое положение по горизонтали</param>
        /// <param name="y">Новое положение по вертикали</param>
        /// <param name="timeInSec">Время на перемещение</param>
        public static void MoveTo(FrameworkElement UI, double x, double y, double timeInSec)
        {
            // Анимация margin
            ThicknessAnimation ta = new ThicknessAnimation();
            Thickness topMargin = new Thickness(x, y, 0, 0);
            ta.To = topMargin;
            ta.From = UI.Margin;
            ta.Duration = TimeSpan.FromSeconds(timeInSec);
            ta.Completed += (s, _) =>
            {
                Thickness CurValue = UI.Margin;
                UI.BeginAnimation(FrameworkElement.MarginProperty, null);
                UI.Margin = CurValue;
            };
            UI.BeginAnimation(FrameworkElement.MarginProperty, ta);
        }

        public static void MoveTo(FrameworkElement UI, double x, double y, double timeInSec, VoidDelegate EndingMethod)
        {
            // Анимация margin
            ThicknessAnimation ta = new ThicknessAnimation();
            Thickness topMargin = new Thickness(x, y, 0, 0);
            ta.To = topMargin;
            ta.From = UI.Margin;
            ta.Duration = TimeSpan.FromSeconds(timeInSec);
            ta.Completed += (s, _) =>
            {
                Thickness CurValue = UI.Margin;
                UI.BeginAnimation(FrameworkElement.MarginProperty, null);
                UI.Margin = CurValue;
                EndingMethod();
            };
            UI.BeginAnimation(FrameworkElement.MarginProperty, ta);
        }


        //static void EndMarginAnimation(FrameworkElement UI) 
        //{
        //    UI.BeginAnimation(FrameworkElement.MarginProperty, null);
        //}
        /// <summary>
        /// Перемещает элемент в определенную позицию без анимации
        /// </summary>
        /// <param name="UI">Смещаемый элемент</param>
        /// <param name="x">Новое положение по горизонтали</param>
        /// <param name="y">Новое положение по вертикали</param>
        public static void MoveTo(FrameworkElement UI, double x, double y)
        {
            Thickness Pos = new Thickness();
            Pos.Left = x;
            Pos.Top = y;
            UI.Margin = Pos;
        }
        /// <summary>
        /// Плавно изменяет громкость медиаэлемента
        /// </summary>
        /// <param name="Media">Медиаэлемент</param>
        /// <param name="volume">Конечный уровень громкости</param>
        /// <param name="timeInSec">Время за которое должна измениться громкость</param>
        /// <param name="AnimationCompleted">Обработчик события окончания анимации типа EventHandler </param>
        public static void SlowDifferVolume(MediaElement Media, double volume, double timeInSec, EventHandler AnimationCompleted)
        {
            DoubleAnimation A = new DoubleAnimation();
            A.From = Media.Volume;
            A.To = volume;
            A.Duration = TimeSpan.FromSeconds(timeInSec);
            A.Completed += AnimationCompleted;
            Media.BeginAnimation(MediaElement.VolumeProperty, A);
        }
        /// <summary>
        /// Плавно изменяет громкость медиаэлемента
        /// </summary>
        /// <param name="Media">Медиаэлемент</param>
        /// <param name="volume">Конечный уровень громкости</param>
        /// <param name="timeInSec">Время за которое должна измениться громкость</param>
        public static void SlowDifferVolume(MediaElement Media, double volume, double timeInSec)
        {
            SlowDifferOpacity(Media, volume, timeInSec,0, (sender, e) => { });
        }

        /// <summary>
        /// Плавно изменяет прозрачность элемента
        /// </summary>
        /// <param name="Media">Элемент</param>
        /// <param name="opacity">Конечный уровень прозрачности</param>
        /// <param name="timeInSec">Время за которое должна измениться прозрачность</param>
        /// <param name="AnimationCompleted">Обработчик события окончания анимации типа EventHandler </param>
        public static void SlowDifferOpacity(FrameworkElement UI, double opacity, double timeInSec, double beginTimeInSec, EventHandler AnimationCompleted)
        {
            DoubleAnimation A = new DoubleAnimation();
            A.From = UI.Opacity;
            A.To = opacity;
            A.Duration = TimeSpan.FromSeconds(timeInSec);
            A.BeginTime = TimeSpan.FromSeconds(beginTimeInSec);
            A.Completed += AnimationCompleted;
            A.Completed += (s, _) =>
            {
                double CurValue = UI.Opacity;
                UI.BeginAnimation(FrameworkElement.OpacityProperty, null);
                UI.Opacity = CurValue;
                
                //UI.Opacity = 1;
            };            
            UI.BeginAnimation(MediaElement.OpacityProperty, A);
        }
        public static void SlowDifferOpacity(FrameworkElement UI, double opacity, double timeInSec, double beginTimeInSec, VoidDelegate AnimationCompleted)
        {
            DoubleAnimation A = new DoubleAnimation();
            A.From = UI.Opacity;
            A.To = opacity;
            A.Duration = TimeSpan.FromSeconds(timeInSec);
            A.BeginTime = TimeSpan.FromSeconds(beginTimeInSec);
            A.Completed += (s, _) =>
            {
                double CurValue = UI.Opacity;
                UI.BeginAnimation(FrameworkElement.OpacityProperty, null);
                UI.Opacity = CurValue;
                AnimationCompleted();

                //UI.Opacity = 1;
            };
            UI.BeginAnimation(MediaElement.OpacityProperty, A);
        }
        /// <summary>
        /// Плавно изменяет прозрачность элемента
        /// </summary>
        /// <param name="Media">Элемент</param>
        /// <param name="opacity">Конечный уровень прозрачности</param>
        /// <param name="timeInSec">Время за которое должна измениться прозрачность</param>
        public static void SlowDifferOpacity(FrameworkElement UI, double opacity, double timeInSec)
        {
            SlowDifferOpacity(UI,opacity,timeInSec, 0,(sender, e) => { });
        }
        public static void SlowDifferOpacity(FrameworkElement UI, double opacity, double timeInSec, double beginTimeInSec)
        {
            SlowDifferOpacity(UI, opacity, timeInSec, beginTimeInSec, (sender, e) => { });
        }
        #endregion


        #region использование анимационных методов (вроде конкретных реализаций)
        public static void BlurWide_Show(FrameworkElement UI)
        {
            BlurShow(UI, 0.3);
            Zoom(UI, 1, 1, 0.3);
        }
        public static void BlurWide_Show(FrameworkElement UI, double EndWidth, double EndHeight, double Xcenter, double Ycenter, double timeInSec)
        {
            //UI.Height = UI.Width / (EndWidth/EndHeight);
            double k = EndWidth / UI.Width;
            BlurShow(UI, timeInSec);
            Zoom(UI, 1, k, Xcenter, Ycenter, timeInSec);
        }
        public static void BlurWide_Hide(FrameworkElement UI)
        {
            BlurHide(UI, 0.4);
            Zoom(UI, 1, 5, 1);
        }

        
        public static void HideMediaSlow(MediaElement Media , double timeInSec) 
        {
            SlowDifferOpacity(Media, 0, 2);
            SlowDifferVolume(Media, 0, 2);
        }
        public static void ShowMediaSlow(MediaElement Media, double timeInSec)
        {
            SlowDifferOpacity(Media, 1, 2);
            SlowDifferVolume(Media, 0.5, 2);
        }

        


        public static void HideHard(FrameworkElement UI) 
        {
            UI.BeginAnimation(FrameworkElement.OpacityProperty, null);
            UI.Opacity = 0;
        }
        #endregion

        public static void ChildrenRemoveAndAdd(FrameworkElement UI, Panel Container1, Panel Container2) 
        {
            
            //AllAnimationNull(UI);
            //Container1.Children.Remove(UI);
            //UI.Opacity = 0.3;
            //UI.Margin = new Thickness(5);
            //UI.Width = 40;
            //UI.Height = 40;
            //UI.VerticalAlignment = VerticalAlignment.Center;
            //UI.HorizontalAlignment = HorizontalAlignment.Center;

            //if (UI.MyBox != null)
            //{
            //    UI.MyBox.Add(UI); 
            //}
            //else
            //{
            //    MessageBox.Show("Не найден контейнер для юнита");
            //    return;
            //}
            //Panel.SetZIndex(UI, 5);
            //AllAnimationNull(UI);
            //TDrawEffects.Delay(UI, 0.1, () => { TDrawEffects.SlowDifferOpacity(UI, 1, 0.1); });
            
            //ToolsTimer.Delay(()=>
            //{
            //    AllAnimationNull(UI); 
            //    UI.Opacity = 1;
            //}, new TimeSpan(0,0,0,1,100));
            

            //if((UI is Unit) && (((Unit)UI).type=="Fruit"))
            //{
            //    ((Unit)UI).IsOnStackPanel = true;
            //}
        }


    
        public static void AllAnimationNull(FrameworkElement UI)
        {
            if (UI == null) return;
                UI.BeginAnimation(FrameworkElement.WidthProperty, null);
                UI.BeginAnimation(FrameworkElement.HeightProperty, null);
                UI.BeginAnimation(FrameworkElement.OpacityProperty, null);
                UI.BeginAnimation(FrameworkElement.MarginProperty, null);
        }


        #region Методы анимации пользовательского интерфейса

        public static void UIOpacityChange(FrameworkElement Img, double from, double to, double timeInSec)
        {

            Img.BeginAnimation(FrameworkElement.OpacityProperty, null);

            DoubleAnimation A = new DoubleAnimation();
            A.From = from;
            A.To = to;
            A.Duration = TimeSpan.FromSeconds(timeInSec);
            Img.BeginAnimation(FrameworkElement.OpacityProperty, A);
        }


        public static void ShowUI_MouseEnter(FrameworkElement Img)
        {

            Img.BeginAnimation(FrameworkElement.OpacityProperty, null);

            DoubleAnimation A = new DoubleAnimation();
            A.From = 0;
            A.To = 1;
            A.Duration = TimeSpan.FromSeconds(0.2);
            Img.BeginAnimation(FrameworkElement.OpacityProperty, A);
        }
        public static void HideUI_MouseDown(FrameworkElement Img)
        {
            Img.BeginAnimation(FrameworkElement.OpacityProperty, null);

            DoubleAnimation A = new DoubleAnimation();
            A.From = 1;
            A.To = 0;
            A.Duration = TimeSpan.FromSeconds(0.5);
            Img.BeginAnimation(FrameworkElement.OpacityProperty, A);
        }
        public static void PushUI_MouseDown(FrameworkElement Img)
        {
            Img.BeginAnimation(FrameworkElement.MarginProperty, null);

            ThicknessAnimation A = new ThicknessAnimation();
            A.From = Img.Margin;
            A.To = new Thickness(23,23, 20, 20);
            A.AutoReverse = true;
            A.Duration = TimeSpan.FromSeconds(0.1);
            Img.BeginAnimation(FrameworkElement.MarginProperty, A);
        }
        #endregion
    }
}
