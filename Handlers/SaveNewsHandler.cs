using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace bot.Handlers
{
    public class SaveNewsHandler
    {
        private readonly TelegramBotClient _telegramBotClient;
        private readonly Dictionary<long, List<string>> _savedNews;

        public SaveNewsHandler(TelegramBotClient telegramBotClient, Dictionary<long, List<string>> savedNews)
        {
            _telegramBotClient = telegramBotClient;
            _savedNews = savedNews;
        }

        public async Task HandleCommand(Message message)
        {
            string url = message.Text.Substring("/save_news ".Length);
            long chatId = message.Chat.Id;

            if (!_savedNews.ContainsKey(chatId))
            {
                _savedNews[chatId] = new List<string>();
            }

            _savedNews[chatId].Add(url);
            await _telegramBotClient.SendTextMessageAsync(chatId, "News saved successfully!");
        }
    }
}
