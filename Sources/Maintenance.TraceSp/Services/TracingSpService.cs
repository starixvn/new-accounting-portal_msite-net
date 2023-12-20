namespace Maintenance.TraceSp.Services;

using System.Linq;
using Criteria;
using Fanex.Data.Repository;
using Models;

public interface ITracingSpService
{
    List<TracingSpViewModel> GetTracingSpData(TracingSp model);
}

public class TracingSpService : ITracingSpService
{
    private readonly IDynamicRepository dynamicRepository = new DynamicRepository();

    private bool IsExistedError(TracingSpCriteria criteria, TracingSp model)
    {
        dynamicRepository.Execute(criteria, model.Server);

        model.HasError = criteria.HasError;
        model.ErrorMsg = criteria.ErrorMsg;

        return model.HasError;
    }

    public List<TracingSpViewModel> GetTracingSpData(TracingSp model)
    {
        var criteria = new TracingSpCriteria
        {
            Site = model.Site,
            Mode = model.Mode,
            TextDataLike = model.TextDataLike,
            TextDataLike2 = model.TextDataLike2,
            TextDataLike3 = model.TextDataLike3,
            TraceStartEvents = model.TraceStartEvents,
            TraceTimer = model.TraceTimer,
            ErrorMsg = string.Empty,
            HasError = false
        };
        if (IsExistedError(criteria, model))
        {
            return new List<TracingSpViewModel>();
        }

        return dynamicRepository.Fetch<TracingSpViewModel>(criteria, model.Server).ToList();
    }
}