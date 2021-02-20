using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net;
using System.Threading;

namespace TelegramBot
{
    class EchoBot
    {
        private int _updateId = 0;
        private string _messageFromId = "";
        private string _messageText = "";
        private string _token = "SomeToken"; //Убрал, чтобы никто не юзал мой токен.


        public void StartBot()
        {
            WebClient webClient = new WebClient();
            string startUrl = $"https://api.telegram.org/bot{_token}";


            while (true)
            {
                string url = $"{startUrl}/getUpdates?offset={_updateId + 1}";
                string response = webClient.DownloadString(url);
                var array = JObject.Parse(response)["result"].ToArray();

                foreach (var message in array)
                {
                    _updateId = Convert.ToInt32(message["update_id"]);


                    try
                    {
                        _messageFromId = message["message"]["from"]["id"].ToString();
                        _messageText = message["message"]["text"].ToString();
                        Console.WriteLine($"{_updateId} {_messageFromId} {_messageText}");
                        url = $"{startUrl}/sendMessage?chat_id={_messageFromId}&text={_messageText}";
                        webClient.DownloadString(url);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                    }

                }


                Thread.Sleep(500);
            }
        }
    }
}
