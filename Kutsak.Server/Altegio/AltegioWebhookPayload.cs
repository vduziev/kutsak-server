using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace YourApp.Models.Altegio
{
    public class AltegioWebhookPayload
    {[JsonPropertyName("company_id")]
        public long? CompanyId { get; set; }

        [JsonPropertyName("resource")]
        public string Resource { get; set; }

        [JsonPropertyName("resource_id")]
        public long? ResourceId { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("data")]
        public AltegioRecordData Data { get; set; }
    }

    public class AltegioRecordData
    {
        [JsonPropertyName("id")]
        public long? Id { get; set; }

        [JsonPropertyName("company_id")]
        public long? CompanyId { get; set; }

        [JsonPropertyName("staff_id")]
        public long? StaffId { get; set; }

        [JsonPropertyName("clients_count")]
        public int? ClientsCount { get; set; }

        [JsonPropertyName("date")]
        public string Date { get; set; }

        [JsonPropertyName("comment")]
        public string Comment { get; set; }

        [JsonPropertyName("online")]
        public bool? Online { get; set; }[JsonPropertyName("visit_id")]
        public long? VisitId { get; set; }[JsonPropertyName("visit_attendance")]
        public int? VisitAttendance { get; set; }

        [JsonPropertyName("attendance")]
        public int? Attendance { get; set; }

        [JsonPropertyName("confirmed")]
        public int? Confirmed { get; set; } 

        [JsonPropertyName("seance_length")]
        public int? SeanceLength { get; set; }

        [JsonPropertyName("length")]
        public int? Length { get; set; }

        [JsonPropertyName("sms_before")]
        public int? SmsBefore { get; set; }

        [JsonPropertyName("sms_now")]
        public int? SmsNow { get; set; }

        [JsonPropertyName("sms_now_text")]
        public string SmsNowText { get; set; }

        [JsonPropertyName("email_now")]
        public int? EmailNow { get; set; }

        [JsonPropertyName("notified")]
        public int? Notified { get; set; }

        [JsonPropertyName("master_request")]
        public int? MasterRequest { get; set; }

        [JsonPropertyName("api_id")]
        public string ApiId { get; set; }

        [JsonPropertyName("from_url")]
        public string FromUrl { get; set; }[JsonPropertyName("review_requested")]
        public int? ReviewRequested { get; set; }[JsonPropertyName("created_user_id")]
        public long? CreatedUserId { get; set; }[JsonPropertyName("deleted")]
        public bool? Deleted { get; set; }[JsonPropertyName("paid_full")]
        public int? PaidFull { get; set; }

        [JsonPropertyName("prepaid")]
        public bool? Prepaid { get; set; }

        [JsonPropertyName("prepaid_confirmed")]
        public bool? PrepaidConfirmed { get; set; }

        [JsonPropertyName("is_update_blocked")]
        public bool? IsUpdateBlocked { get; set; }

        [JsonPropertyName("activity_id")]
        public long? ActivityId { get; set; }

        [JsonPropertyName("bookform_id")]
        public long? BookformId { get; set; }

        [JsonPropertyName("record_from")]
        public string RecordFrom { get; set; }

        [JsonPropertyName("is_mobile")]
        public int? IsMobile { get; set; }

        [JsonPropertyName("services")]
        public List<AltegioService> Services { get; set; }

        [JsonPropertyName("staff")]
        public AltegioStaff Staff { get; set; }

        [JsonPropertyName("goods_transactions")]
        public JsonElement? GoodsTransactions { get; set; } 

        [JsonPropertyName("sms_remain_hours")]
        public int? SmsRemainHours { get; set; }

        [JsonPropertyName("email_remain_hours")]
        public int? EmailRemainHours { get; set; }

        [JsonPropertyName("comer")]
        public string Comer { get; set; }

        [JsonPropertyName("comer_person_info")]
        public string ComerPersonInfo { get; set; }

        [JsonPropertyName("client")]
        public AltegioClient Client { get; set; }

        [JsonPropertyName("datetime")]
        public string Datetime { get; set; }

        [JsonPropertyName("create_date")]
        public string CreateDate { get; set; }

        [JsonPropertyName("last_change_date")]
        public string LastChangeDate { get; set; }

        [JsonPropertyName("custom_fields")]
        public JsonElement? CustomFields { get; set; }

        [JsonPropertyName("custom_color")]
        public string CustomColor { get; set; }

        [JsonPropertyName("custom_font_color")]
        public string CustomFontColor { get; set; }

        [JsonPropertyName("record_labels")]
        public JsonElement? RecordLabels { get; set; }

        [JsonPropertyName("documents")]
        public List<AltegioDocument> Documents { get; set; }

        [JsonPropertyName("short_link")]
        public string ShortLink { get; set; }

        [JsonPropertyName("composite")]
        public JsonElement? Composite { get; set; }
    }

    public class AltegioService
    {
        [JsonPropertyName("id")]
        public long? Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("cost")]
        public decimal? Cost { get; set; }[JsonPropertyName("cost_to_pay")]
        public decimal? CostToPay { get; set; }

        [JsonPropertyName("manual_cost")]
        public decimal? ManualCost { get; set; }[JsonPropertyName("cost_per_unit")]
        public decimal? CostPerUnit { get; set; }

        [JsonPropertyName("discount")]
        public decimal? Discount { get; set; }[JsonPropertyName("first_cost")]
        public decimal? FirstCost { get; set; }[JsonPropertyName("amount")]
        public int? Amount { get; set; }
    }

    public class AltegioStaff
    {
        [JsonPropertyName("id")]
        public long? Id { get; set; }

        [JsonPropertyName("api_id")]
        public string ApiId { get; set; }[JsonPropertyName("name")]
        public string Name { get; set; }[JsonPropertyName("specialization")]
        public string Specialization { get; set; }

        [JsonPropertyName("position")]
        public JsonElement? Position { get; set; } 

        [JsonPropertyName("avatar")]
        public string Avatar { get; set; }

        [JsonPropertyName("avatar_big")]
        public string AvatarBig { get; set; }

        [JsonPropertyName("rating")]
        public decimal? Rating { get; set; }

        [JsonPropertyName("votes_count")]
        public int? VotesCount { get; set; }
    }

    public class AltegioClient
    {
        [JsonPropertyName("id")]
        public long? Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("surname")]
        public string Surname { get; set; }

        [JsonPropertyName("patronymic")]
        public string Patronymic { get; set; }[JsonPropertyName("display_name")]
        public string DisplayName { get; set; }[JsonPropertyName("comment")]
        public string Comment { get; set; }

        [JsonPropertyName("phone")]
        public string Phone { get; set; }

        [JsonPropertyName("card")]
        public string Card { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("success_visits_count")]
        public int? SuccessVisitsCount { get; set; }

        [JsonPropertyName("fail_visits_count")]
        public int? FailVisitsCount { get; set; }

        [JsonPropertyName("discount")]
        public decimal? Discount { get; set; }

        [JsonPropertyName("custom_fields")]
        public JsonElement? CustomFields { get; set; }

        [JsonPropertyName("sex")]
        public int? Sex { get; set; }[JsonPropertyName("birthday")]
        public string Birthday { get; set; }[JsonPropertyName("client_tags")]
        public JsonElement? ClientTags { get; set; }
    }

    public class AltegioDocument
    {
        [JsonPropertyName("id")]
        public long? Id { get; set; }

        [JsonPropertyName("type_id")]
        public int? TypeId { get; set; }

        [JsonPropertyName("storage_id")]
        public long? StorageId { get; set; }

        [JsonPropertyName("user_id")]
        public long? UserId { get; set; }[JsonPropertyName("company_id")]
        public long? CompanyId { get; set; }[JsonPropertyName("number")]
        public long? Number { get; set; }

        [JsonPropertyName("comment")]
        public string Comment { get; set; }

        [JsonPropertyName("date_created")]
        public string DateCreated { get; set; }

        [JsonPropertyName("category_id")]
        public long? CategoryId { get; set; }

        [JsonPropertyName("visit_id")]
        public long? VisitId { get; set; }

        [JsonPropertyName("record_id")]
        public long? RecordId { get; set; }

        [JsonPropertyName("type_title")]
        public string TypeTitle { get; set; }

        [JsonPropertyName("is_sale_bill_printed")]
        public bool? IsSaleBillPrinted { get; set; }
    }
}