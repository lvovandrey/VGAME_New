using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            return "";
        }

        /// <summary>
        /// Return duration of video from Youtube
        /// </summary>
        /// <param name="url">Link (string) from Youtumbe, like "https://www.youtube.com/watch?v=ХХХХХХХХХ" or embed version</param>
        /// <returns>Video duration as timespan</returns>
        public static TimeSpan GetDuraion(string url)
        {
            return TimeSpan.FromSeconds(0);
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



    }
}
