namespace Maintenance.TraceSp.Models;

using System.Web;

public class TracingSpViewModel
{
    private string _textData;
    public string Event { get; set; }

    public string SPID { get; set; }

    public string TextData
    {
        get => HttpUtility.HtmlEncode(_textData.Replace("><", "> <").Replace(",", ", "));
        set => _textData = value;
    }

    public string LoginName { get; set; }

    public string Duration { get; set; }

    public string Reads { get; set; }

    public string Writes { get; set; }

    public string CPU { get; set; }

    public string DatabaseName { get; set; }

    public string StartTime { get; set; }

    public string EndTime { get; set; }
}