using Kutsak.Server.Services;

namespace Kutsak.Server;

public class Program
{
    public static void Main(string[] args) {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddHttpClient();
        
        builder.Services.AddSingleton<TelegramNotificationsService>(services => new TelegramNotificationsService(
            Environment.GetEnvironmentVariable("TELEGRAM_BOT_TOKEN") ?? throw new Exception("TELEGRAM_BOT_TOKEN is not set"),
            Environment.GetEnvironmentVariable("CHATS")?.Split(",") ?? [],
            services
        ));

        if (builder.Environment.IsDevelopment()) {
            builder.WebHost.UseUrls("http://*:31303");
        }

        var app = builder.Build();

        app.MapControllers();

        app.Run();
    }
}
