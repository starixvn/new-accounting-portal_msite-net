using Fanex.Data.Repository;

namespace Maintenance.MailStation.Criteria.MailManage
{
    public class GetAllSitesCommand : NonQueryCommand
    {
        public byte[] TransformJson { get; set; }

        public override string GetSettingKey() => "Mail_NapSite_Select";

        public override bool IsValid() => true;
    }
}