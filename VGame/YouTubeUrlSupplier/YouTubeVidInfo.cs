using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;
using YoutubeExplode;
using YoutubeExplode.Models.MediaStreams;
using System.Threading.Tasks;

namespace YouTubeUrlSupplier
{
    public class YoutubeVidInfo
    {
        public string DirectURL { get; private set; }
        public string Title { get; private set; }
        public TimeSpan Duration { get; private set; }
        public Size Resolution { get; private set; }
        public string ImageUrl { get; private set; }
        public string[] PrevImagesUrl { get; private set; }

        private string Url;

        MediaStreamInfoSet streamInfoSet;
        MuxedStreamInfo streamInfo;

        public YoutubeVidInfo(string url)
        {
            DirectURL = "";
            Title = "";
            Duration = TimeSpan.FromMilliseconds(0);
            Resolution = new Size(0, 0);
            ImageUrl = "";
            PrevImagesUrl = new string[] { "", "", "" };

            Url = url;
            try
            {
           //     NEWLIBRARY_GetVideoAsync(url);

                //IList<VideoQuality> list = null;
                //list = YouTubeDownloader.GetYouTubeVideoUrls(url);
                //DirectURL = list.First().DownloadUrl;
                //Title = list.First().VideoTitle;
                //Duration = TimeSpan.FromSeconds(list.First().Length);
                //Resolution = list.First().Dimension;



            }
            catch
            {
                MessageBox.Show("Error creating YoutubeVideoInfo object. ");
            }
        }

        public async void NEWLIBRARY_GetVideoAsync(string urlstring)
        {
            try
            {
                var id = YoutubeClient.ParseVideoId(urlstring);
                var client = new YoutubeClient();
                var video = await client.GetVideoAsync(id.ToString());
                streamInfoSet = await client.GetVideoMediaStreamInfosAsync(id.ToString());
                streamInfo = streamInfoSet.Muxed.WithHighestVideoQuality();

                DirectURL = streamInfo.Url;
                Title = video.Title;
                Duration = video.Duration;
                Resolution = new Size(streamInfo.Resolution.Width, streamInfo.Resolution.Height);
                ImageUrl = YoutubeGet.GetImage(urlstring);
                PrevImagesUrl = YoutubeGet.GetPrevImages(urlstring);
            }
            catch
            {
                MessageBox.Show("Error creating YoutubeVideoInfo object. ");
            }
        }

        public async Task NEWLIBRARY_GetVideoAsync()
        {
            try
            {
                var id = YoutubeClient.ParseVideoId(Url);
                var client = new YoutubeClient();
                var video = await client.GetVideoAsync(id.ToString());
                streamInfoSet = await client.GetVideoMediaStreamInfosAsync(id.ToString());
                streamInfo = streamInfoSet.Muxed.WithHighestVideoQuality();

                DirectURL = streamInfo.Url;
                Title = video.Title;
                Duration = video.Duration;
                Resolution = new Size(streamInfo.Resolution.Width, streamInfo.Resolution.Height);
                ImageUrl = YoutubeGet.GetImage(Url);
                PrevImagesUrl = YoutubeGet.GetPrevImages(Url);
            }
            catch
            {
                MessageBox.Show("Error creating YoutubeVideoInfo object. ");
            }
        }

    }
}
