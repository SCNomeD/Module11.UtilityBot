using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Module11.UtilityBot.Configurations;
using Module11.UtilityBot.Controllers;
using Module11.UtilityBot.Services;
using System.Text;
using Telegram.Bot;

namespace Module11.UtilityBot
{
    static class Program
    {
        public static async Task Main()
        {
            Console.OutputEncoding = Encoding.Unicode;

            var host = new HostBuilder()
                .ConfigureServices((hostContext, services) => ConfigureServices(services))
                .UseConsoleLifetime()
                .Build();

            Console.WriteLine("Сервис запущен!");
            await host.RunAsync();
            Console.WriteLine("Сервис остановлен!");
        }

        static void ConfigureServices(IServiceCollection services)
        {
            AppSettings appSettings = BuildAppSettings();
            services.AddSingleton(BuildAppSettings());

            services.AddTransient<DefaultMessageController>();
            services.AddTransient<InlineKeyboardController>();
            services.AddTransient<TextMessageController>();

            services.AddTransient<CalculatorService>();
            services.AddTransient<SentenceSizeService>();

            services.AddSingleton<IStorage, MemoryStorage>();

            // Регистрируем объект TelegramBotClient c токеном подключения
            services.AddSingleton<ITelegramBotClient>(provider => new TelegramBotClient(appSettings.BotToken));
            // Регистрируем постоянно активный сервис бота
            services.AddHostedService<Bot>();
        }

        static AppSettings BuildAppSettings()
        {
            return new AppSettings()
            {
                BotToken = "5739458699:AAGqEyTrKEOmvyZU4d4Tr5rW9WrbENa0ItE"
            };
        }
    }
}