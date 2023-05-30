using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace bot.Handlers
{
    public class LatestNewsHandler
    {
        private readonly HttpClient _httpClient;
        private readonly TelegramBotClient _telegramBotClient;
        public LatestNewsHandler(TelegramBotClient telegramBotClient)
        {
            _httpClient = new HttpClient();
            _telegramBotClient = telegramBotClient;
        }

        public async Task HandleCommand(Message message)
        {
            long chatId = message.Chat.Id;

            string newsUrl = $"https://newsapi.org/v2/top-headlines?country=us&apiKey=430ee01059ba498c973226299cbb0eb0";

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(newsUrl);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    dynamic newsData = JsonConvert.DeserializeObject(content);

                    int articlesToDisplay = 5;
                    for (int i = 0; i < articlesToDisplay; i++)
                    {
                        string title = newsData.articles[i].title;
                        string url = newsData.articles[i].url;
                        string messageText = $"{i + 1}. {title}\n{url}";
                        await _telegramBotClient.SendTextMessageAsync(chatId, messageText);
                    }
                }
                else
                {
                    await _telegramBotClient.SendTextMessageAsync(chatId, "Oops! Something went wrong while retrieving the latest news.");
                }
            }
            catch (Exception ex)
            {
                await _telegramBotClient.SendTextMessageAsync(chatId, $"An error occurred: {ex.Message}");
            }
        }
    }
}
