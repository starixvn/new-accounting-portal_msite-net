namespace Maintenance.BotService.Params;

public class GetMessageParams
{
    public int? ChannelId { get; set; }
    public int? BotId { get; set; }
    public int? AppId { get; set; }
    public string Status { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 100;
}