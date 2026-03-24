using System.Text.Json;
using System.Text.Json.Serialization;

namespace Kutsak.Server.Altegio
{
    public class AltegioTransactionPayload
    {
        [JsonPropertyName("company_id")]
        public long? CompanyId { get; set; }

        [JsonPropertyName("resource")]
        public string Resource { get; set; }[JsonPropertyName("resource_id")]
        public long? ResourceId { get; set; }[JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("data")]
        public AltegioTransactionData Data { get; set; }
    }

    public class AltegioTransactionData
    {
        [JsonPropertyName("id")]
        public long? Id { get; set; }[JsonPropertyName("document_id")]
        public long? DocumentId { get; set; }[JsonPropertyName("date")]
        public string Date { get; set; }

        [JsonPropertyName("amount")]
        public decimal? Amount { get; set; }

        [JsonPropertyName("comment")]
        public string Comment { get; set; }

        [JsonPropertyName("last_change_date")]
        public string LastChangeDate { get; set; }

        [JsonPropertyName("record_id")]
        public long? RecordId { get; set; }

        [JsonPropertyName("visit_id")]
        public long? VisitId { get; set; }

        [JsonPropertyName("sold_item_id")]
        public long? SoldItemId { get; set; }

        [JsonPropertyName("sold_item_type")]
        public string SoldItemType { get; set; }

        [JsonPropertyName("expense")]
        public AltegioExpense Expense { get; set; }

        [JsonPropertyName("master")]
        public JsonElement? Master { get; set; } 

        [JsonPropertyName("supplier")]
        public JsonElement? Supplier { get; set; }

        [JsonPropertyName("account")]
        public AltegioAccount Account { get; set; }[JsonPropertyName("client")]
        public AltegioSimpleClient Client { get; set; }[JsonPropertyName("record")]
        public AltegioTransactionRecord Record { get; set; }
    }

    public class AltegioExpense
    {
        [JsonPropertyName("id")]
        public long? Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }[JsonPropertyName("type")]
        public int? Type { get; set; }
    }

    public class AltegioAccount
    {
        [JsonPropertyName("id")]
        public long? Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }[JsonPropertyName("is_cash")]
        public bool? IsCash { get; set; }[JsonPropertyName("is_default")]
        public bool? IsDefault { get; set; }
    }

    public class AltegioSimpleClient
    {
        [JsonPropertyName("id")]
        public long? Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("surname")]
        public string Surname { get; set; }[JsonPropertyName("patronymic")]
        public string Patronymic { get; set; }

        [JsonPropertyName("phone")]
        public string Phone { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }
    }

    public class AltegioTransactionRecord
    {[JsonPropertyName("id")]
        public long? Id { get; set; }[JsonPropertyName("location_id")]
        public long? LocationId { get; set; }[JsonPropertyName("staff_id")]
        public long? StaffId { get; set; }

        [JsonPropertyName("date")]
        public string Date { get; set; }

        [JsonPropertyName("activity_id")]
        public long? ActivityId { get; set; }

        [JsonPropertyName("visit_id")]
        public long? VisitId { get; set; }

        [JsonPropertyName("comment")]
        public string Comment { get; set; }

        [JsonPropertyName("custom_color")]
        public string CustomColor { get; set; }

        [JsonPropertyName("attendance_status")]
        public int? AttendanceStatus { get; set; }

        [JsonPropertyName("paid_full")]
        public int? PaidFull { get; set; }

        [JsonPropertyName("is_online")]
        public bool? IsOnline { get; set; }[JsonPropertyName("prepaid")]
        public bool? Prepaid { get; set; }[JsonPropertyName("clients_count")]
        public int? ClientsCount { get; set; }[JsonPropertyName("deleted")]
        public bool? Deleted { get; set; }
    }
}