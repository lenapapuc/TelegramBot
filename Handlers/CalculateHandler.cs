using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace bot.Handlers
{

    public class CalculateHandler
    {
        private readonly TelegramBotClient _telegramBotClient;

        public CalculateHandler(TelegramBotClient telegramBotClient)
        {
            _telegramBotClient = telegramBotClient;
        }

        public async Task HandleCommand(Message message)
        {
            long chatId = message.Chat.Id;
            string[] commandParts = message.Text.Split(' ');

            if (commandParts.Length != 2)
            {
                await _telegramBotClient.SendTextMessageAsync(chatId, "Invalid command format. Please use /calculate <expression>");
                return;
            }

            string expression = commandParts[1];

            try
            {
                double result = EvaluateExpression(expression);
                await _telegramBotClient.SendTextMessageAsync(chatId, $"Result: {result}");
            }
            catch (Exception ex)
            {
                await _telegramBotClient.SendTextMessageAsync(chatId, $"Error: {ex.Message}");
            }
        }

        private double EvaluateExpression(string expression)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("expression", typeof(string), expression);

            DataRow dataRow = dataTable.NewRow();
            dataTable.Rows.Add(dataRow);

            return double.Parse((string)dataRow["expression"]);
        }
    }

}


