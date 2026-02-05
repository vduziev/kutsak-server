using System.Text.Json;
using System.Text.Json.Serialization;

namespace Kutsak.Server.Webflow;

public class WebhookSubmission
{
    [JsonPropertyName("triggerType")]
    public string TriggerType { get; set; }
    
    [JsonPropertyName("payload")]
    public JsonDocument Payload { get; set; }
}
