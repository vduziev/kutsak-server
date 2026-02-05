namespace Kutsak.Server;

public class Program
{
    public static void Main(string[] args) {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();

        if (builder.Environment.IsDevelopment()) {
            builder.WebHost.UseUrls("http://*:31303");
        }

        var app = builder.Build();

        app.MapControllers();

        app.Run();
    }
}
