using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;
using Telegram.Bot;
using Module11.UtilityBot.Services;

namespace Module11.UtilityBot.Controllers
{
    public class TextMessageController
    {
        private readonly ITelegramBotClient _telegramClient;
        private readonly CalculatorService _calculatorService;
        private readonly SentenceSizeService _sentenceSizeService;
        private readonly IStorage _memoryStorage;

        public TextMessageController(ITelegramBotClient telegramClient, CalculatorService calculatorService, SentenceSizeService sentenceSizeService, IStorage memoryStorage)
        {
            _telegramClient = telegramClient;
            _calculatorService = calculatorService;
            _sentenceSizeService = sentenceSizeService;
            _memoryStorage = memoryStorage;
        }

        public async Task Handle(Message message, CancellationToken cancellationToken)
        {
            Console.WriteLine($"Контроллер {GetType().Name} получил сообщение");

            switch (message.Text)
            {
                case "/start":

                    // Объект, представляющий кноки
                    var buttons = new List<InlineKeyboardButton[]>();
                    buttons.Add(new[]
                    {
                        InlineKeyboardButton.WithCallbackData($"📄 Длина сообщения" , $"Длина сообщения"),
                        InlineKeyboardButton.WithCallbackData($"➕ Сумма чисел" , $"Сумма чисел")
                    });

                    // передаем кнопки вместе с сообщением (параметр ReplyMarkup)
                    await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"<b> Выбор операции.</b> {Environment.NewLine}", cancellationToken: cancellationToken, parseMode: ParseMode.Html, replyMarkup: new InlineKeyboardMarkup(buttons));

                    return;
                    break;
            }

            if (_memoryStorage.GetSession(message.Chat.Id).Choose.Equals("Длина сообщения"))
            {
                await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"Длина сообщения - {_sentenceSizeService.GetLenth(message.Text)} символов", cancellationToken: cancellationToken);
            }
            else if (_memoryStorage.GetSession(message.Chat.Id).Choose.Equals("Сумма чисел"))
            {
                try
                {
                    var chars = message.Text.Split(" ").Select(s => Double.Parse(s)).ToList();
                    await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"Результат сложения - {_calculatorService.GetSum(chars)}");
                }
                catch (Exception ex)
                {
                    await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"Ошибка! Попробуйте писать числа через пробел.");
                }
            }
        }
    }
}
