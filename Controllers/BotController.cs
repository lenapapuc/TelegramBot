using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;
using Telegram.Bot;
using System.Net.Http;
using Newtonsoft.Json;
using Telegram.Bot.Types.Enums;
using bot.Handlers;


namespace bot.Controllers
{
    [ApiController]
    [Route("api/bot")]
    public class BotController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly TelegramBotClient _telegramBotClient;
        private static readonly Dictionary<long, List<string>> _savedNews = new Dictionary<long, List<string>>();

        public BotController()
        {
            _httpClient = new HttpClient();
            _telegramBotClient = new TelegramBotClient("6165512213:AAHi4HwTW91meAoQ8_9kafS6pClND-6btIQ");
            
        }

        [HttpPost]

        public async Task<IActionResult> Post([FromBody] Update update)

        {
            if (update.Type == UpdateType.Message && update.Message.Text != null)
            {
                Message message = update.Message;
                string command = message.Text.Split(' ')[0];

                switch (command)
                {
                    case "/start":
                        StartHandler startHandler = new StartHandler(_telegramBotClient);
                        await startHandler.HandleCommand(message);
                        break;
                    case "/temperature":
                        TemperatureCommandHandler temperatureCommandHandler = new TemperatureCommandHandler(_telegramBotClient);
                        await temperatureCommandHandler.HandleCommand(message);
                        break;

                    case "/latest_news":
                        LatestNewsHandler latestNewsHandler = new LatestNewsHandler(_telegramBotClient);
                        await latestNewsHandler.HandleCommand(message);
                        break;
                    case "/save_news":
                        SaveNewsHandler saveNewsHandler = new SaveNewsHandler(_telegramBotClient, _savedNews);
                        Console.WriteLine(_savedNews);
                        await saveNewsHandler.HandleCommand(message);
                        break;
                    case "/saved_news":
                        SavedNewsHandler savedNewsHandler = new SavedNewsHandler(_telegramBotClient, _savedNews);
                        await savedNewsHandler.HandleCommand(message);
                        break;
                    case "/calculate":
                        CalculateHandler calculateHandler = new CalculateHandler(_telegramBotClient);
                        await calculateHandler.HandleCommand(message);
                        break;
                    default:
                        // Handle unknown command
                        break;
                }
            }
            return Ok();

        }
    }
}
