using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OpenWeatherMap;
using Telegram.Bot;
using Telegram.Bot.Types;
using System.Net.Http;
using Newtonsoft.Json;


namespace bot.Handlers
{
    public class TemperatureCommandHandler
    {
        private readonly TelegramBotClient _telegramBotClient;
   

        public TemperatureCommandHandler(TelegramBotClient telegramBotClient)
        {
            _telegramBotClient = telegramBotClient;
        }

        public async Task HandleCommand(Message message)
        {
            long chatId = message.Chat.Id;

            try
            {
                string command = message.Text.ToLower();
                string[] commandParts = command.Split(' ');

                if (commandParts.Length < 2)
                {
                    await _telegramBotClient.SendTextMessageAsync(chatId, "Please provide a location. Usage: /temperature <location>");
                    return;
                }

                string location = commandParts[1];

                string apiKey = "65bdbcadfdc4510f459bec9be120d4bf";
                string url = $"http://api.weatherstack.com/current?access_key={apiKey}&query={location}";
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        dynamic weatherResponse = JsonConvert.DeserializeObject(responseBody);

                        if (weatherResponse != null && weatherResponse.current != null && weatherResponse.location != null)
                        {
                            string temperature = weatherResponse.current.temperature.ToString("0.0");
                            string locationName = weatherResponse.location.name.ToString();

                            string messageText = $"Current temperature in {locationName}: {temperature}°C";
                            await _telegramBotClient.SendTextMessageAsync(chatId, messageText);
                        }
                        else
                        {
                            await _telegramBotClient.SendTextMessageAsync(chatId, "Oops! Unable to retrieve the temperature at the moment.");
                        }
                    }

                    else
                    {
                        await _telegramBotClient.SendTextMessageAsync(chatId, "Oops! An error occurred while fetching the temperature.");
                    }
                }
            }
            catch (Exception ex)
            {
                await _telegramBotClient.SendTextMessageAsync(chatId, $"An error occurred: {ex.Message}");
            }
        }


    }


}
