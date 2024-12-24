namespace Maintenance.Sites.Controllers;

using Fanex.Data.Repository;
using Fanex.NAP.Client.Attributes;
using Microsoft.AspNetCore.Mvc;

[SessionAuthorize]
public class SaveBetController(IDynamicRepository repository) : Controller
{
    public class GetJobModel
    {
        public int GroupId { get; set; }

        public int Count { get; set; }
    }

    public class GetPendingJobCriteria : CriteriaBase
    {
        public override string GetSettingKey() => "NAP_SaveBet_GetQueueCnt";

        public override bool IsValid() => true;
    }

    public async Task<ActionResult> Index()
    {
        var jobs = await GetJobs();

        return Json(jobs);
    }

    private async Task<IEnumerable<GetJobModel>> GetJobs()
    {
        var criteria = new GetPendingJobCriteria( );

        return await repository.FetchAsync<GetJobModel>(criteria);
    }
}