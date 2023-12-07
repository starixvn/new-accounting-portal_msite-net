namespace Maintenance.MailStation.Criteria;

using Fanex.Data.Repository;

public abstract class UpdateCommandBase : NonQueryCommand
{
    public int ErrCode { get; set; }

    public string ErrMsg { get; set; }
}