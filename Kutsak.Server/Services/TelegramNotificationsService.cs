namespace Kutsak.Server.Services;

public class TelegramNotificationsService
{
    private readonly IHttpClientFactory _clientFactory;
    
    private readonly string _token;
    private readonly string[] _chatIds;
    
    public TelegramNotificationsService(string token, string[] chatIds, IServiceProvider services) {
        _clientFactory = services.GetRequiredService<IHttpClientFactory>();
        
        _token = token;
        _chatIds = chatIds;
    }

    private string GetUrl(string chatId, string message)
        => $"https://api.telegram.org/bot{_token}/sendMessage?chat_id={chatId}&text={Uri.EscapeDataString(message)}";
    
    public async Task NotifyAllAsync(string message) {
        var tasks = new List<Task>();
        
        foreach (var chatId in _chatIds) {
            try {
                tasks.Add(NotifyOneAsync(chatId, message));
            }
            catch (Exception x) {
                Console.WriteLine($"Failed to notify \"{chatId}\"");
                Console.WriteLine($"Exception: {x}");
            }
        }

        try {
            await Task.WhenAll(tasks);
        }
        finally {
            foreach (var task in tasks) {
                if (task.Exception is null) continue;
                
                Console.WriteLine($"Failed notification task exception: {task.Exception.InnerException}");
            }
        }
    }
    
    public async Task NotifyOneAsync(string chatId, string message) {
        Console.WriteLine($"Notifying \"{chatId}\"...");
        
        var client = _clientFactory.CreateClient();
        var response = await client.GetAsync(GetUrl(chatId, message));

        Console.WriteLine(response.IsSuccessStatusCode
            ? $"Notified \"{chatId}\""
            : $"Failed to notify \"{chatId}\". Status code: {response.StatusCode}"
        );
    }
}
