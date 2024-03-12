namespace Maintenance.Letstalk.Model;

using System;

public class LetstalkRecord
{
    public int Id { get; set; }

    public string TargetName { get; set; }

    public string Subject { get; set; }

    public string Content { get; set; }

    public string Group { get; set; }

    public string Attachments { get; set; }

    public string InlineImages { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime? LastSentDate { get; set; }

    public int SendSuccess { get; set; }

    public int SendFail { get; set; }

    public bool IsMessage { get; set; }
}