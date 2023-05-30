using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace bot.Handlers
{
    public class SavedNewsHandler
    {
        private readonly TelegramBotClient _telegramBotClient;
        private readonly Dictionary<long, List<string>> _savedNews;

        public SavedNewsHandler(TelegramBotClient telegramBotClient, Dictionary<long, List<string>> savedNews)
        {
            _telegramBotClient = telegramBotClient;
            _savedNews = savedNews;
        }

        public async Task HandleCommand(Message message)
        {
            long chatId = message.Chat.Id;

            if (!_savedNews.ContainsKey(chatId) || _savedNews[chatId].Count == 0)
            {
                await _telegramBotClient.SendTextMessageAsync(chatId, "No saved news found.");
                return;
            }

            string response = "Saved News:\n\n";
            List<string> savedNews = _savedNews[chatId];
            for (int i = 0; i < savedNews.Count; i++)
            {
                string newsUrl = savedNews[i];
                response += $"{i + 1}. {newsUrl}\n";
            }
            await _telegramBotClient.SendTextMessageAsync(chatId, response);
        }
    }
}
