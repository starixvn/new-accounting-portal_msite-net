using Maintenance.Mattermost.Model;

namespace Maintenance.Mattermost.Params;

public class HttpResendMultipleParams
{
    public DateTime FromDate { get; set; }

    public DateTime ToDate { get; set; }

    public IEnumerable<ResendMattermostModel> Data { get; set; }
}