using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using VGameCore.Struct;
using VanyaGame.Tools;
using VGameCore;

namespace VanyaGame.PrevMenuNS
{
    /// <summary>
    /// Interaction logic for PrevMenuItem.xaml
    /// </summary>
    public partial class PrevMenuItem : UserControl
    {
        public string filename;
        public Level Level;
        public PrevMenuItem()
        {
            InitializeComponent();
        }



        public PrevMenuItem(string ImgFilename) : this()
        {
            LoadImage(ImgFilename);
        }
        public PrevMenuItem(string ImgFilename,string ImgType) : this()
        {
            if (ImgFilename == "" || ImgFilename == null) return;
            if (ImgType == "youtube")
                Img.Source = PictHelper.GetBitmapImage(new Uri(@ImgFilename));
            else
                LoadImage(ImgFilename);
        }
        public PrevMenuItem(string ImgFilename, Level level, string imgType) : this(ImgFilename,imgType)
        {
            Level = level;
            Text.Text = level.Sets.Description;
            TextToolTip.Text = level.Sets.Description;
            DataContext = Level;
        }

        private void LoadImage(string ImgFilename)
        {
            System.Drawing.Bitmap myBitmap = new System.Drawing.Bitmap(@ImgFilename);
            Img.Source = Imaging.CreateBitmapSourceFromBitmap(myBitmap);
            filename = ImgFilename;
        }

        private void Img_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var uri = new Uri("Images/HandPushDown.cur", UriKind.Relative);
            var stream = Application.GetResourceStream(uri).Stream;
            var cursor = new Cursor(stream);
            Img.Cursor = cursor;
        //    AddDropShadowEffect(Img, 10, 20, 80);
            
        }

        private void Img_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var uri = new Uri("Images/HandPush.cur", UriKind.Relative);
            var stream = Application.GetResourceStream(uri).Stream;
            var cursor = new Cursor(stream);
            Img.Cursor = cursor;
            
        }
        
        public void AddDropShadowEffect(Image img, double Depth, double Softness, double Opacity)
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
            img.BitmapEffect = myDropShadowEffect;
        }

        private void Img_MouseLeave(object sender, MouseEventArgs e)
        {
            var uri = new Uri("Images/HandPush.cur", UriKind.Relative);
            var stream = Application.GetResourceStream(uri).Stream;
            var cursor = new Cursor(stream);
            Img.Cursor = cursor;
        }

    }
}
