namespace Maintenance.Mattermost.Params;

public class HttpGetLogParams
{
    public DateTime FromDate { get; set; }

    public DateTime ToDate { get; set; }

    public string TargetName { get; set; }

    public string Group { get; set; }

    public byte Status { get; set; }
}