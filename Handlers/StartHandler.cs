using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
namespace bot.Handlers
{
    public class StartHandler
    {
        private readonly TelegramBotClient _telegramBotClient;

        public StartHandler(TelegramBotClient telegramBotClient)
        {
            _telegramBotClient = telegramBotClient;
        }

        public async Task HandleCommand(Message message)
        {
            string response = "👋 Hello! Welcome to the bot!";
            await _telegramBotClient.SendTextMessageAsync(message.Chat.Id, response);
        }
    }

}
