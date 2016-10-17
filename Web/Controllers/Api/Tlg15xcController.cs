using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Telegram.Bot.Types;
using Telegram.Bot;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Specialized;
using System.Web;
using YoutubeExtractor;

namespace Tazeyab.Web.Controllers.Api
{
    public class SoundCloudRootObject
    {
        public string kind { get; set; }
        public int id { get; set; }
        public string created_at { get; set; }
        public int user_id { get; set; }
        public int duration { get; set; }
        public bool commentable { get; set; }
        public string state { get; set; }
        public int original_content_size { get; set; }
        public string last_modified { get; set; }
        public string sharing { get; set; }
        public string tag_list { get; set; }
        public string permalink { get; set; }
        public bool streamable { get; set; }
        public string embeddable_by { get; set; }
        public bool downloadable { get; set; }
        public string purchase_url { get; set; }
        public object label_id { get; set; }
        public string purchase_title { get; set; }
        public string genre { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string label_name { get; set; }
        public string release { get; set; }
        public string track_type { get; set; }
        public string key_signature { get; set; }
        public string isrc { get; set; }
        public object video_url { get; set; }
        public string bpm { get; set; }
        public object release_year { get; set; }
        public object release_month { get; set; }
        public object release_day { get; set; }
        public string original_format { get; set; }
        public string license { get; set; }
        public string uri { get; set; }
        public string permalink_url { get; set; }
        public string artwork_url { get; set; }
        public string waveform_url { get; set; }
        public string stream_url { get; set; }
        public string download_url { get; set; }
        public int playback_count { get; set; }
        public int download_count { get; set; }
        public int favoritings_count { get; set; }
        public int comment_count { get; set; }
        public string attachments_uri { get; set; }
        public string policy { get; set; }
        public string monetization_model { get; set; }
    }

    public class Tlg15xcController : ApiController
    {
        Telegram.Bot.Api api = new Telegram.Bot.Api("140885397:AAGtsWYYyUFplkbba2Fi7es41cjBVhGSU_o");
        static List<Update> updates = new List<Update>();
        public static List<string> Logs = new List<string>();
        // GET: api/Tlg15xc
        public IEnumerable<string> Get()
        {
            return Logs;
        }

        // GET: api/Tlg15xc/5
        public object Get(int id)
        {
            //var text = CrawlerEngine.Helper.Requester.GetWebText(string.Format("http://api.soundcloud.com/resolve?url={0}&client_id={1}", "https://soundcloud.com/beshevli/snowwaltz", "ce50db690e99a644af9ff791194daac0"));
            //var audio = Mn.Framework.Serialization.JsonHelper.JsonComplexDeserialize<SoundCloudRootObject>(text);

            ////var file = CrawlerEngine.Helper.Requester.GetStreamFromUri(audio.download_url + "?client_id=ce50db690e99a644af9ff791194daac0", "");
            //var url = audio.download_url + "?client_id=ce50db690e99a644af9ff791194daac0";
            //HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(url);
            //WebResponse myResp = myReq.GetResponse();

            //StreamReader reader = new StreamReader(myResp.GetResponseStream());

            //return reader.ReadToEnd().Length.ToString();
            return updates;
        }

        // POST: api/Tlg15xc
        public string Post(Update update)
        {
            //updates.Add(update);
            Logs.Add(update.Message.Text + " " + update.Message.Chat.Id + " " + update.Message.MessageId);
            //api.SendTextMessage(update.Message.Chat.Id, "hello world! " + update.Message.Text);
            if (update.Message.Text.ToLower().StartsWith("http://soundcloud") || update.Message.Text.ToLower().StartsWith("http://www.soundcloud"))
            {
                try
                {
                    var text = CrawlerEngine.Helper.Requester.GetWebText(string.Format("http://api.soundcloud.com/resolve?url={0}&client_id={1}", update.Message.Text.ToLower(), "ce50db690e99a644af9ff791194daac0"));
                    var audio = Mn.Framework.Serialization.JsonHelper.JsonComplexDeserialize<SoundCloudRootObject>(text);
                    api.SendTextMessage(update.Message.Chat.Id, audio.title + " " + audio.download_url);

                    using (var client = new WebClient())
                    {
                        var filepath = System.Web.Hosting.HostingEnvironment.MapPath("~/Files/" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".mp3");
                        client.DownloadFile(audio.stream_url + "?client_id=ce50db690e99a644af9ff791194daac0", filepath);
                        api.SendTextMessage(update.Message.Chat.Id, audio.title);
                        var stream = System.IO.File.Open(filepath, FileMode.Open);
                        api.SendAudio(update.Message.Chat.Id, new FileToSend(audio.id.ToString() + ".mp3", stream), audio.duration / 1000, "", audio.title);
                    }

                }
                catch (Exception ex)
                {
                    Logs.Add(ex.Message);
                    api.SendTextMessage(update.Message.Chat.Id, ex.Message);
                }
            }
            else if (update.Message.Text.StartsWith("https://www.youtube") || update.Message.Text.StartsWith("https://youtube") || update.Message.Text.StartsWith("http://www.youtube") || update.Message.Text.StartsWith("http://youtube"))
            {
                Match match = Regex.Match(update.Message.Text, @"v=([^&#]{5,})");

                if (match.Success)
                {
                    try
                    {
                        Logs.Add("130 start video");
                        GetVideo(update.Message.Text, update);

                        //string videoID = match.Value.Replace("v=", "");
                        //string[] itagByPriority = { "5", "6", "34", "35" };

                        //string videoUrl = "https://www.youtube.com/get_video_info?asv=3&el=detailpage&hl=en_US&video_id=" + videoID;
                        //string encodedVideo = null;

                        //using (var client = new WebClient())
                        //{
                        //    encodedVideo = client.DownloadString(videoUrl);
                        //}

                        //NameValueCollection video = HttpUtility.ParseQueryString(encodedVideo);

                        //string encodedStreamsCommaDelimited = video["url_encoded_fmt_stream_map"];
                        //string[] encodedStreams = encodedStreamsCommaDelimited.Split(new char[] { ',' });
                        //var streams = encodedStreams.Select(s => HttpUtility.ParseQueryString(s));

                        //var streamsByPriority = streams.OrderBy(s => Array.IndexOf(itagByPriority, s["itag"]));
                        //NameValueCollection preferredStream = streamsByPriority.LastOrDefault();

                        //if (preferredStream != null)
                        //{
                        //    var stream_url = preferredStream["url"];
                        //    using (var client = new WebClient())
                        //    {
                        //        var filepath = System.Web.Hosting.HostingEnvironment.MapPath("~/Files/" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".mp4");
                        //        client.DownloadFile(stream_url, filepath);
                        //        api.SendTextMessage(update.Message.Chat.Id, video["title"]);
                        //        var stream = System.IO.File.Open(filepath, FileMode.Open);
                        //        api.SendVideo(update.Message.Chat.Id, new FileToSend(DateTime.Now.ToString("yyyyMMddHHmmss") + ".mp4", stream), 120, video["title"]);
                        //    }
                        //}
                    }
                    catch (Exception ex)
                    {
                        Logs.Add(ex.Message);
                        api.SendTextMessage(update.Message.Chat.Id, ex.Message);
                    }
                }
            }


            return "OK";
        }

        // PUT: api/Tlg15xc/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Tlg15xc/5
        public void Delete(int id)
        {
        }

        #region private method

        private void GetVideo(string link, Update update)
        {
            Logs.Add("187 start get video " + link);
            api.SendChatAction(update.Message.Chat.Id, ChatAction.UploadVideo);
            IEnumerable<VideoInfo> videoInfos = DownloadUrlResolver.GetDownloadUrls(link);
            VideoInfo video = videoInfos.First(info => info.VideoType == VideoType.Mp4 && info.Resolution == 360);

            if (video.RequiresDecryption)
            {
                DownloadUrlResolver.DecryptDownloadUrl(video);
            }
            var videoDownloader = new VideoDownloader(video, System.Web.Hosting.HostingEnvironment.MapPath(Path.Combine("/Files/Video", update.Message.Chat.Id.ToString() + "_" + update.Message.MessageId.ToString() + video.VideoExtension)));
            Logs.Add("196 " + "complete download");
            videoDownloader.DownloadFinished += VideoDownloader_DownloadFinished;
            videoDownloader.Execute();
            Logs.Add("198 " + "excute completed");
        }

        private void VideoDownloader_DownloadFinished(object sender, EventArgs e)
        {
            var downloader = (VideoDownloader)sender;
            Logs.Add("203 " + downloader.SavePath);
            var chatId = downloader.SavePath.Substring(downloader.SavePath.LastIndexOf("\\") + 1, downloader.SavePath.IndexOf("_") - downloader.SavePath.LastIndexOf("\\") - 1);
            var stream = System.IO.File.Open(downloader.SavePath, FileMode.Open);
            var videoFile = new FileToSend(DateTime.Now.ToString("yyyyMMddHHmmss") + ".mp4", stream);            
            api.SendVideo(int.Parse(chatId), videoFile, 0, downloader.Video.Title);            
            Logs.Add("218 video sent! " + chatId);

        }

        #endregion
    }


}
