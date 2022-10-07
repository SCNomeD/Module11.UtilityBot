using Telegram.Bot.Types;
using Telegram.Bot;

namespace Module11.UtilityBot.Controllers
{
    public class DefaultMessageController
    {
        private readonly ITelegramBotClient _telegramClient;

        public DefaultMessageController(ITelegramBotClient telegramClient)
        {
            _telegramClient = telegramClient;
        }

        public async Task Handle(Message message, CancellationToken cancellationToken)
        {
            Console.WriteLine($"Контроллер {GetType().Name} получил сообщение");

            await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"Получено сообщение не поддерживаемого формата", cancellationToken: cancellationToken);
        }
    }
}
