namespace Maintenance.BotService.Criteria.Message
{
    using Fanex.Data.Repository;

    public class GetMessageCriteria : CriteriaBase
    {
        public int? ChannelId { get; set; }
        public int? BotId { get; set; }
        public int? AppId { get; set; }
        public string Status { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalRows { get; set; }

        public override string GetSettingKey() => "Message_Get";

        public override bool IsValid() => FromDate.HasValue && ToDate.HasValue && PageIndex > 0 && PageSize > 0;
    }
}