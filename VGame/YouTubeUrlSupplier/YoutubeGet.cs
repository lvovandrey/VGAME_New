using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;

using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace YouTubeUrlSupplier
{
   

    public static class YoutubeGet
    {
        /// <summary>
        /// Return direct url to video from Youtube
        /// </summary>
        /// <param name="url">Link (string) from Youtube, like "https://www.youtube.com/watch?v=ХХХХХХХХХ" or embed version</param>
        /// <returns>Direct link to video stream (videofile)</returns>
        public static string GetVideoDirectURL(string url)
        {
            IList<VideoQuality> list = null;
            list = YouTubeDownloader.GetYouTubeVideoUrls(url);
            string DirectURL = list.First().DownloadUrl;

            return DirectURL;
        }

        /// <summary>
        /// Return Title of video from Youtube
        /// </summary>
        /// <param name="url">Link (string) from Youtube, like "https://www.youtube.com/watch?v=ХХХХХХХХХ" or embed version</param>
        /// <returns>Video Title</returns>
        public static string GetTitle(string url)
        {

            string t = YouTubeDownloader.GetYouTubeVideoTitle(url);
            return t;
        }

        /// <summary>
        /// Return duration of video from Youtube
        /// </summary>
        /// <param name="url">Link (string) from Youtumbe, like "https://www.youtube.com/watch?v=ХХХХХХХХХ" or embed version</param>
        /// <returns>Video duration as timespan</returns>
        public static TimeSpan GetDuraion(string url)
        {
            TimeSpan t = YouTubeDownloader.GetYouTubeVideoDuration(url);
            return t;
        }

        /// <summary>
        /// Return resolution (as string like "123x456") of video from Youtube
        /// </summary>
        /// <param name="url">Link (string) from Youtumbe, like "https://www.youtube.com/watch?v=ХХХХХХХХХ" or embed version</param>
        /// <returns>Video resolution</returns>
        public static string GetResolution(string url)
        {
            return "";
        }

        /// <summary>
        /// Return direct url to image video from Youtube
        /// </summary>
        /// <param name="url">Link (string) from Youtube, like "https://www.youtube.com/watch?v=ХХХХХХХХХ" or embed version</param>
        /// <returns>Direct link to image</returns>
        public static string GetImage(string url)
        {
            return string.Format(CultureInfo.InvariantCulture, "http://i3.ytimg.com/vi/{0}/hqdefault.jpg", YouTubeDownloader.GetVideoIdFromUrl(url));
        }

        /// <summary>
        /// Return direct urls to images video preview from Youtube
        /// </summary>
        /// <param name="url">Link (string) from Youtube, like "https://www.youtube.com/watch?v=ХХХХХХХХХ" or embed version</param>
        /// <returns>Direct link to image</returns>
        public static string[] GetPrevImages(string url)
        {
            string[] Urls = new string[3];
            for (int i = 0; i < 3; i++)
                Urls[i] = string.Format(CultureInfo.InvariantCulture, "http://i3.ytimg.com/vi/{0}/1.jpg", YouTubeDownloader.GetVideoIdFromUrl(url));
            return Urls;
        }


    }
}
