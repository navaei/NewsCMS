using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http;
using Tazeyab.Web.WebLogic;
using Telegram.Bot.Types;
using YoutubeExtractor;

namespace Tazeyab.Web.Controllers.Api
{
    public class GetMediaController : ApiController
    {
        public IEnumerable<string> Get()
        {
            return Mn.GetMedia.AppConfig.Logs;
        }

        public object Get(int id)
        {
            return null;
        }

        public string Post(Update update)
        {
            try
            {
                new Mn.GetMedia.Notifier().ProceessMessage(update);
            }
            catch (Exception ex)
            {
                Mn.GetMedia.AppConfig.BotApp.SendTextMessage(update.Message.Chat.Id, ex.Message);
            }

            return "OK";
        }

    }
}
