using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Effects;
using System.IO;
using System.Threading;
using System.Windows.Threading;
using System.Windows.Media.Animation;
using System.Diagnostics;

using VanyaGame;
using VanyaGame.LevelsStruct;
using VanyaGame.Behaviour;
using WpfAnimatedGif;

namespace VanyaGame
{
    public class Unit : ObjToShow
    {
        public enum TState { Unload, Hide, Move }
        protected TState State;  //Состояние юнита
        public string Filename;
        public Thickness ActualMargin;
        public Shadow Shadow;
        public FlyDome flyDome;
        
        public bool isHided = true;
        public bool IsNowHiding = false; // находится ли юнит в процессе исчезновения?
        public bool IsHited = false; //Пойман ли юнит или нет
        public bool IsOnStackPanel = false; // юнит находится на стекпанели?
        public bool IsShowed = false; // юнит уже был показан?
        public WoodenGrid MyBox;
        protected double DragXShift, DragYShift;

        public bool CanDragging = true;

        protected DispatcherTimer HideWatcher;

        public void AddDropShadowEffect()
        {
            DropShadowBitmapEffect myDropShadowEffect = new DropShadowBitmapEffect();
            Color myShadowColor = new Color();
            myShadowColor.ScA = 1;
            myShadowColor.ScB = 0;
            myShadowColor.ScG = 0;
            myShadowColor.ScR = 0;
            myDropShadowEffect.Color = myShadowColor;
            myDropShadowEffect.Direction = 320;
            myDropShadowEffect.ShadowDepth = 15;
            myDropShadowEffect.Softness = 0.8;
            myDropShadowEffect.Opacity = 0.8;
            this.BitmapEffect = myDropShadowEffect;
        }
        public void AddDropShadowEffect(double Depth, double Softness, double Opacity)
        {
            DropShadowBitmapEffect myDropShadowEffect = new DropShadowBitmapEffect();
            Color myShadowColor = new Color();
            myShadowColor.ScA = 1;
            myShadowColor.ScB = 0;
            myShadowColor.ScG = 0;
            myShadowColor.ScR = 0;
            myDropShadowEffect.Color = myShadowColor;
            myDropShadowEffect.Direction = 320;
            myDropShadowEffect.ShadowDepth = Depth;
            myDropShadowEffect.Softness = Softness;
            myDropShadowEffect.Opacity = Opacity;
            this.BitmapEffect = myDropShadowEffect;
        }
        public void AddBlurEffect(int radius)
        {
            BlurBitmapEffect myBlurEffect = new BlurBitmapEffect();
            myBlurEffect.Radius = radius;
            this.BitmapEffect = myBlurEffect;
        }
        public Rect Zone
        {
            get
            {
                Rect R = new Rect(ActualMargin.Left, ActualMargin.Top, Width, Height);
                return R;
            }
        }
        public Point Position
        {
            get
            {
                Point value = new Point(ActualMargin.Left, ActualMargin.Top);
                return value;
            }
            set
            {
               // ActualMargin = new Thickness((int)value.X, (int)value.Y, 0, 0);
                //base.Margin = ActualMargin;
            }
        }
        public Point PositionOnTree;
        
        public double Width
        {
            get
            {
                return base.Width;
            }
            set
            {
                base.Width = value;
            }
        }
        public double Height
        {
            get
            {
                return base.Height;
            }
            set
            {
                base.Height = value;
            }
        }
        public double flySpeed = 10;
        public Image ThisImage;



        public override void SetPosition(Point Pos)
        {
            double X = Pos.X; //- (SubLevel.Sets.UnitsSize.Width/2);
            double Y = Pos.Y; //- (SubLevel.Sets.UnitsSize.Height/2);

            TDrawEffects.MoveTo(this, X, Y);
        }
        public override void Create(string s)
        {
            type = "Unit";
            State = TState.Unload;
            Filename = s;

            Source = new BitmapImage(new Uri(@Filename));
            //System.Drawing.Bitmap myBitmap = new System.Drawing.Bitmap(@Filename);
            //Source = Imaging.CreateBitmapSourceFromBitmap(myBitmap);
            
            Width = SubLevel.Sets.UnitsSize.Width;
            //Height = SubLevel.Sets.UnitsSize.Height;
            Position = TDrawEffects.GeneratePosition(SubLevel.Sets.UnitAppearenceZone);
            HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            VerticalAlignment = System.Windows.VerticalAlignment.Top;
            
            this.AddDropShadowEffect(2,0.3,0.95);
            
            base.PreviewMouseLeftButtonDown += Unit_PreviewMouseLeftButtonDown;
        }

        public event SenderDelegate OnCatch;

        public void AddOnCach(SenderDelegate U_OnCatch) 
        {
                OnCatch += U_OnCatch;
  //              MessageBox.Show("Подписан" + this.type.ToString());
        }

        public void RemoveOnCach(SenderDelegate U_OnCatch)
        {
                OnCatch = null;
           //     MessageBox.Show("Отписан" + this.type.ToString());
        } 

        protected void Unit_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            
            
            if(!CanDragging) return;

            AddDropShadowEffect(15,0.7, 0.7);
            this.PreviewMouseMove+=Unit_PreviewMouseMove;
            this.LostMouseCapture += Unit_LostMouseCapture;
            this.PreviewMouseUp +=Unit_PreviewMouseUp; 
            Mouse.Capture(this);

            double X = PositionOnTree.X; //this.ActualMargin.Left;
            DragXShift =  e.GetPosition(Game.Owner.GridMain).X - X;
            double Y = PositionOnTree.Y; //this.ActualMargin.Top;
            DragYShift = e.GetPosition(Game.Owner.GridMain).Y - Y;
            
            UpdatePosition(e);

        }

        protected void Unit_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            FinishDrag(sender, e);
            Mouse.Capture(null);

            AddDropShadowEffect(2, 0.3, 0.95);
            if (OnCatch != null)
            {
                if (!this.IsHited)
                {
                  //  IsHited = true;

                    OnCatch(this);
                    
                   // HideMethod();
                   // isHided = true;
                }
            }
        
        }

        protected void Unit_LostMouseCapture(object sender, MouseEventArgs e)
        {
            FinishDrag(sender, e);
        }

        protected void Unit_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            UpdatePosition(e);
        }

        protected void UpdatePosition(MouseEventArgs e)
        {
            //if (IsDragging)
            {Point point = e.GetPosition(Game.Owner.GridMain);
            TDrawEffects.AllAnimationNull(this);
            TDrawEffects.MoveTo(this, point.X - this.DragXShift, point.Y - this.DragYShift);}
            BasketLight();


        }

        protected void BasketLightOn() 
        {
                Basket BUp = ((IIterationGameTree)Game.Level.SubLevel.Iteration.Behaviour).basketUp;
                if (BUp.LightState == "No")
                {
                    BUp.ChangeImg(@"pack://application:,,,/Images/baskeUp2.png");
                    BUp.LightState = "Yes";
                }
                Basket BDown = ((IIterationGameTree)Game.Level.SubLevel.Iteration.Behaviour).basketDown;
                if (BDown.LightState == "No")
                {
                    BDown.ChangeImg(@"pack://application:,,,/Images/baskeDown2.png");
                    BDown.LightState = "Yes";
                }
        }
        protected void BasketLightOff() 
        {
            Basket BUp = ((IIterationGameTree)Game.Level.SubLevel.Iteration.Behaviour).basketUp;
            if (BUp.LightState == "Yes")
            {
                BUp.ChangeImg(@"pack://application:,,,/Images/baskeUp.png");
                BUp.LightState = "No";
            }
            Basket BDown = ((IIterationGameTree)Game.Level.SubLevel.Iteration.Behaviour).basketDown;
            if (BDown.LightState == "Yes")
            {
                BDown.ChangeImg(@"pack://application:,,,/Images/baskeDown.png");
                BDown.LightState = "No";
            }
        }
        protected void BasketLight() 
        {
            double X = Mouse.GetPosition(Game.Owner.GridMain).X;// ((Unit)sender).ActualMargin.Left;
            double Y = Mouse.GetPosition(Game.Owner.GridMain).Y;//((Unit)sender).ActualMargin.Top;
            double W = this.ActualWidth;
            double H = this.ActualHeight;
            Rect R = new Rect(X, Y, W, H);
            if (Tools.IsIntersect(R, Game.Level.SubLevel.Sets.basketOkHitZone))
            {
                BasketLightOn();
            }
            else
            {
                BasketLightOff();
            }
        }

        protected void FinishDrag(object sender, MouseEventArgs e)
        {
           // IsDragging = false;
            this.PreviewMouseMove -= Unit_PreviewMouseMove;
            this.LostMouseCapture -= Unit_LostMouseCapture;
            this.PreviewMouseUp -= Unit_PreviewMouseUp; 
            UpdatePosition(e);
            BasketLightOff();
        }

        public override void ShowMethod()
        {
            //TDrawEffects.BlurWide_Show(this);
            if (Game.Owner.GridMain.Children.Contains(this)) 
            { }
            else 
            {
                this.Opacity = 0;
                Game.Owner.GridMain.Children.Add(this);           
          

            }
            

                TDrawEffects.SlowDifferOpacity(this, 1, 1,0);
                this.Visibility = System.Windows.Visibility.Visible;
          
          
            
            
            isHided = false;
            IsShowed = true;

        }

        protected void HideWacherEnd(object sender, EventArgs e) 
        {
            
            Tools.StopAndNull_DispatcherTimer(ref HideWatcher);
            if (!this.IsOnStackPanel)
            {
                TDrawEffects.AllAnimationNull(this);
                this.Opacity = 0;

                base.PreviewMouseLeftButtonDown -= Unit_PreviewMouseLeftButtonDown;
                this.IsNowHiding = false;
                TDrawEffects.ChildrenRemoveAndAdd(this, Game.Owner.GridMain, Game.Owner.StackPanelGame);
            }
        }

        public override void HideMethod()
        {
            if (isHided) return;
            if (IsOnStackPanel) return;
            if (IsNowHiding) return;
            
           Tools.CreateAndStart_DispatcherTimer(ref HideWatcher, HideWacherEnd,new TimeSpan(0,0,0,0,1200));
           Panel.SetZIndex(this, (Panel.GetZIndex(this)-1));
           this.BeginAnimation(OpacityProperty, null);
           // this.Opacity = 0;
           double curW = this.ActualWidth;
           this.IsNowHiding = true;                 
            
           //TDrawEffects.Zoom(this, 1, 5, 1);
           TDrawEffects.SlowDifferOpacity(this, 0, 1, 0, (s, _) =>
            {
                TDrawEffects.AllAnimationNull(this);
                this.Opacity = 0;
                this.Width = curW;
                base.PreviewMouseLeftButtonDown -= Unit_PreviewMouseLeftButtonDown;
                this.IsNowHiding = false;
                TDrawEffects.ChildrenRemoveAndAdd(this, Game.Owner.GridMain, Game.Owner.StackPanelGame);
                

                //TDrawEffects.Zoom(this, 5, 1, 0.1);
            });
        }
  
        public void RefreshMargin()
        {
            double dx = ActualMargin.Left - base.Margin.Left;
            double dy = ActualMargin.Top - base.Margin.Top;
            TDrawEffects.MoveShift(this, dx, dy, 1);
        }
    }
 
    public class Fruit : Unit 
    {
        public List<string> filenames;
        public List<string> frames;
        private int MaxFrameCount = 0;
        private int framecounter;
        private int FrameCounter 
        {
            get
            {
                return framecounter;
            }
            set
            {
                if ((value <= MaxFrameCount) && ((value >= 0)))
                framecounter = value;
            }
        }

        private ImageAnimationController GifController;
        public override void Create(string s)
        {
            type = "Fruit";
            State = TState.Unload;
            Filename = s;
            filenames = new List<string>();

            filenames.Add(@"pack://application:,,,/Images/Fruit/flower1.png");
            filenames.Add(@"pack://application:,,,/Images/Fruit/zigote1.png");
            filenames.Add(@"pack://application:,,,/Images/Fruit/fruit1.png");
            filenames.Add(@"pack://application:,,,/Images/Fruit/wheel.gif");

            Source = new BitmapImage(new Uri(@filenames[0]));
            Width = SubLevel.Sets.UnitsSize.Width;
            Position = TDrawEffects.GeneratePosition(SubLevel.Sets.UnitAppearenceZone);
            HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            VerticalAlignment = System.Windows.VerticalAlignment.Top;

            this.AddDropShadowEffect(2, 0.3, 0.95);

            CanDragging = false;
            base.PreviewMouseLeftButtonDown += Unit_PreviewMouseLeftButtonDown;
            Behaviour = new TUnitBehaviour();
            Behaviour.IsShow = false;
            
            //настройки Gif-отображения
            //GifController = ImageBehavior.GetAnimationController(this);
            
            //BitmapImage image = new BitmapImage();
            //image.BeginInit();
            //image.UriSource = new Uri(filenames[3]);
            //image.EndInit();
            //ImageBehavior.SetAnimatedSource(this, image);

            //ImageBehavior.SetRepeatBehavior(this, new RepeatBehavior(5));

        }

        private void LoadFrames() 
        {
            
            frames = new List<string>();
            string Dir = Game.Sets.MainDir + Game.Level.Sets.LevelDir + Game.Level.SubLevel.Sets.SubLevelDir + Game.Level.SubLevel.Sets.ImgDir + @"\img\";

            Dir = Dir + Game.Level.SubLevel.Sets.FruitType + @"\";
            for (int i = 1; i < 30; i++)
            {
                string filepath = Dir + i.ToString() + ".png";
                frames.Add(filepath);
                MaxFrameCount = i - 1;
            }
            FrameCounter = 0;
        }
        private void Grow(TimeSpan Time)
        {
            double[] t = { 5, 3, 3 };
            if (Game.Sets.DebugMode) { t = new double[] { 1, 0.5, 0.5 }; }
            double tt = Game.RandomGenerator.Next(20, 50);

            FrameJump(tt/100);
        }
        private void Grow2(TimeSpan Time)
        {
            if (GifController != null)
            {
                GifController.Play();
            }
            
        }
        private void FrameJump(double timeInSec)
        {
            TDrawEffects.Delay(this, timeInSec, () =>
            {
                FrameCounter++;
                Source = new BitmapImage(new Uri(@frames[FrameCounter]));
                if (FrameCounter >= MaxFrameCount)
                {
                    CanDragging = true;
                }
                else FrameJump(timeInSec);
            });
        }

        public void StartGrow ()
        {
            LoadFrames();
            Source = new BitmapImage(new Uri(@frames[0]));
            //this.Opacity = 1;
            //this.Visibility = Visibility.Visible;
            TDrawEffects.Delay(this, 0.5,() => 
            {
            this.BeginAnimation(FrameworkElement.OpacityProperty, null);
            TDrawEffects.SlowDifferOpacity(this, 1, 1, 0, () =>
            {
                Grow(new TimeSpan(0, 0, 0, 5, 0));
              //  Grow2(new TimeSpan(0,0,0,5,0));
            });
            });
        }

        public override void ShowMethod()
        {
            //TDrawEffects.BlurWide_Show(this);
            if (Game.Owner.GridMain.Children.Contains(this))
            { }
            else
            {
                this.Opacity = 0;
                Game.Owner.GridMain.Children.Add(this);
                this.BeginAnimation(OpacityProperty, null);
               // this.Opacity = 0.2;
                this.BeginAnimation(OpacityProperty, null);

               // TDrawEffects.SlowDifferOpacity(this, 1, 1, 0);
                this.Visibility = System.Windows.Visibility.Visible;
            }







            isHided = false;
            IsShowed = true;

        }
        public override void HideMethod()
        {
            if (isHided) return;
            if (IsOnStackPanel) return;
            if (IsNowHiding) return;

            Tools.CreateAndStart_DispatcherTimer(ref HideWatcher, HideWacherEnd, new TimeSpan(0, 0, 0, 0, 1200));
            Panel.SetZIndex(this, (Panel.GetZIndex(this) - 1));
            this.BeginAnimation(OpacityProperty, null);
            double curW = this.ActualWidth;
            this.IsNowHiding = true;

            TDrawEffects.SlowDifferOpacity(this, 0, 0.5, 0, (s, _) =>
            {
                TDrawEffects.AllAnimationNull(this);
                this.Opacity = 0;
                this.Width = curW;
                base.PreviewMouseLeftButtonDown -= Unit_PreviewMouseLeftButtonDown;
                this.IsNowHiding = false;
                TDrawEffects.ChildrenRemoveAndAdd(this, Game.Owner.GridMain, Game.Owner.StackPanelGame);
            });
        }
    }
    
    public class Net : Unit 
    {
        public override void Create(string s) 
        {
            base.Create(s);
            type = "Net";
            Width = 350;
        }
    }
    
    public class Basket : Net
    {
        public string LightState = "No";

        public void ChangeImg(string S) 
        {
            this.Source = new BitmapImage(new Uri(S));
        }

        
        public override void Create(string s)
        {
            State = TState.Unload;
            Filename = s;

            Source = new BitmapImage(new Uri(@Filename));


            Width = SubLevel.Sets.UnitsSize.Width;
            //Height = SubLevel.Sets.UnitsSize.Height;
            Position = TDrawEffects.GeneratePosition(SubLevel.Sets.UnitAppearenceZone);
            HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            VerticalAlignment = System.Windows.VerticalAlignment.Top;




            type = "Basket";
            Width = SubLevel.Sets.basketSize.Width;
        }

        public override void ShowMethod()
        {
            if (Game.Owner.GridMain.Children.Contains(this))
            { }
            else
            {
                this.Visibility = System.Windows.Visibility.Visible;
                TDrawEffects.AllAnimationNull(this);
                TDrawEffects.MoveTo(this, SubLevel.Sets.basketZone.Left,SubLevel.Sets.basketZone.Top);
                TDrawEffects.AllAnimationNull(this);
                this.Opacity = 0;
                TDrawEffects.SlowDifferOpacity(this, 1, 1, 0);
                Game.Owner.GridMain.Children.Add(this);
            }
        }
        public override void HideMethod()
        {
            this.BeginAnimation(OpacityProperty, null);
            TDrawEffects.SlowDifferOpacity(this, 0, 0.1, 0);
        }
    }

    public class FlyDome : Unit
    {
        public Unit MyUnit;
        public void Create(string s, Unit _MyUnit)
        {
            base.Create(s);
            type = "FlyDome";
            Width = 150;
        }

        public override void ShowMethod()
        {
            //TDrawEffects.BlurWide_Show(this);
            if (Game.Owner.GridMain.Children.Contains(this))
            { }
            else
            {
                this.BeginAnimation(OpacityProperty, null);
                this.Opacity = 0;
                TDrawEffects.SlowDifferOpacity(this, 1, 1, 0.1);
                this.Visibility = System.Windows.Visibility.Visible;
                Game.Owner.GridMain.Children.Add(this);
            }



        }
        public override void HideMethod()
        {
            // 
            // TDrawEffects.BlurWide_Hide(this);
            this.BeginAnimation(OpacityProperty, null);
            // this.Opacity = 0;
            //TDrawEffects.BlurHide(this, 0.1);
            TDrawEffects.SlowDifferOpacity(this, 0, 0.1, 0);
        }
    }

    public class Tree : Unit
    {
        public override void Create(string s)
        {
            type = "Tree";
            State = TState.Unload;
            Filename = s;
            Source = new BitmapImage(new Uri(@Filename));
            Width = Game.Owner.GridMain.ActualWidth;
            HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            VerticalAlignment = System.Windows.VerticalAlignment.Bottom;
            Margin = new Thickness(410);
            //this.AddDropShadowEffect(30);
        }

        public override void ShowMethod()
        {
            if (Game.Owner.GridMain.Children.Contains(this))
            { }
            else
            {
                this.Visibility = System.Windows.Visibility.Visible;
                TDrawEffects.AllAnimationNull(this);
                TDrawEffects.MoveTo(this, SubLevel.Sets.treePoint.X, SubLevel.Sets.treePoint.Y);
                TDrawEffects.AllAnimationNull(this);
                this.Opacity = 0;
                TDrawEffects.SlowDifferOpacity(this, 1, 1, 0);
                Game.Owner.GridMain.Children.Add(this);
            }
        }
        public override void HideMethod()
        {
            this.BeginAnimation(OpacityProperty, null);
            TDrawEffects.SlowDifferOpacity(this, 0, 0.1, 0);
        }
    }

    public class Shadow : Unit
    {
        public override void Create(string s)
        {
            Filename = s;
            type = "Shadow";

            Tools.LoadAndConvertToShadow(@Filename, this);
          
            Width = 50;
            Height = 50;

            Position = TDrawEffects.GeneratePosition(SubLevel.Sets.UnitAppearenceZone);
            HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            VerticalAlignment = System.Windows.VerticalAlignment.Center;
        }

        public override void ShowMethod()
        {
            //TDrawEffects.BlurWide_Show(this);
            if (Game.Owner.GridMain.Children.Contains(this))
            { }
            else
            { Game.Owner.GridMain.Children.Add(this); }

            
            this.Opacity = 0;
            TDrawEffects.SlowDifferOpacity(this, 0.73, 1, 1);
            this.Visibility = System.Windows.Visibility.Visible;
        }

        public override void HideMethod()
        {
            if (isHided) return;
            if (IsOnStackPanel) return;
            if (IsNowHiding) return;

            Tools.CreateAndStart_DispatcherTimer(ref HideWatcher, HideWacherEnd, new TimeSpan(0, 0, 0, 0, 1200));
            Panel.SetZIndex(this, (Panel.GetZIndex(this) - 1));
            this.BeginAnimation(OpacityProperty, null);
            double curW = this.ActualWidth;
            this.IsNowHiding = true;

            TDrawEffects.SlowDifferOpacity(this, 0, 0.5, 0, (s, _) =>
            {
                TDrawEffects.AllAnimationNull(this);
                this.Opacity = 0;
                this.Width = curW;
                base.PreviewMouseLeftButtonDown -= Unit_PreviewMouseLeftButtonDown;
                this.IsNowHiding = false;
                TDrawEffects.ChildrenRemoveAndAdd(this, Game.Owner.GridMain, Game.Owner.StackPanelGame);
            });
        }
    }
}
