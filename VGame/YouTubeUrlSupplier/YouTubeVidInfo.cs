using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;


namespace YouTubeUrlSupplier
{
    public class YoutubeVidInfo
    {
        public string DirectURL { get; }
        public string Title { get; }
        public TimeSpan Duration { get; }
        public Size Resolution { get; }
        public string ImageUrl { get; }
        public string[] PrevImagesUrl { get; }

        public YoutubeVidInfo(string url)
        {
            DirectURL = "";
            Title = "";
            Duration = TimeSpan.FromMilliseconds(0);
            Resolution = new Size(0, 0);
            ImageUrl = "";
            PrevImagesUrl = new string[] { "", "", "" };
            try
            {
                IList<VideoQuality> list = null;
                list = YouTubeDownloader.GetYouTubeVideoUrls(url);
                DirectURL = list.First().DownloadUrl;
                Title = list.First().VideoTitle;
                Duration = TimeSpan.FromSeconds(list.First().Length);
                Resolution = list.First().Dimension;
                ImageUrl = YoutubeGet.GetImage(url);
                PrevImagesUrl = YoutubeGet.GetPrevImages(url);
            }
            catch
            {
                MessageBox.Show("Error creating YoutubeVideoInfo object. ");
            }
        }
    }
}
