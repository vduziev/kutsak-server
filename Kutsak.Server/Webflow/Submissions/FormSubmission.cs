using System.Text.Json.Serialization;

namespace Kutsak.Server.Webflow;

public class FormSubmission
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("siteId")]
    public string SiteId { get; set; }

    [JsonPropertyName("data")]
    public ContactFormData Data { get; set; }

    [JsonPropertyName("submittedAt")]
    public DateTime SubmittedAt { get; set; }

    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("formId")]
    public string FormId { get; set; }

    [JsonPropertyName("formElementId")]
    public string FormElementId { get; set; }

    [JsonPropertyName("pageId")]
    public string PageId { get; set; }

    [JsonPropertyName("publishedPath")]
    public string PublishedPath { get; set; }

    [JsonPropertyName("pageUrl")]
    public string PageUrl { get; set; }

    [JsonPropertyName("schema")]
    public List<object> Schema { get; set; }
}

public class ContactFormData
{
    [JsonPropertyName("Customer Name")]
    public string CustomerName { get; set; }

    [JsonPropertyName("Customer Email")]
    public string CustomerEmail { get; set; }

    [JsonPropertyName("Phone Number")]
    public string PhoneNumber { get; set; }

    [JsonPropertyName("Service")]
    public string Service { get; set; }

    [JsonPropertyName("Customer Message")]
    public string CustomerMessage { get; set; }
}
