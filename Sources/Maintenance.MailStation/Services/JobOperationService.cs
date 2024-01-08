namespace Maintenance.MailStation.Services;

using EasyNetQ;
using EasyNetQ.DI;
using Fanex.MailDelivery.Core;
using Fanex.MailDelivery.Core.Models;
using Logging;
using Microsoft.Extensions.Configuration;

public interface IJobOperationService : IDisposable
{
    void PublishScheduleChange(Schedule schedule);

    void RunNow(Schedule schedule);

    void Retry(Schedule schedule);
}

public class JobOperationService : IJobOperationService
{
    private readonly IBus bus;
    private readonly IBus legacyBus;

    public JobOperationService(IConfiguration configuration)
    {
        var rabbitMqConnectionString = configuration.GetValue<string>("MailStation:RabbitMQConnectionString");
        bus = RabbitHutch.CreateBus(
            rabbitMqConnectionString,
            register => register.Register(typeof(EasyNetQ.Logging.ILogger<>), typeof(RabbitMqLogProvider<>)));
        legacyBus = RabbitHutch.CreateBus(
            rabbitMqConnectionString,
            register =>
            {
                register.Register<ITypeNameSerializer>(_ => new LegacyTypeNameSerializer());
                register.Register(typeof(EasyNetQ.Logging.ILogger<>), typeof(RabbitMqLogProvider<>));
            });
    }

    public void PublishScheduleChange(Schedule schedule)
    {
        var updateSchedulePublishId = string.Format(MailDeliveryConstants.UpdateScheduleRoutingKeyFormat, schedule.SiteId);

        legacyBus.PubSub.Publish(schedule, updateSchedulePublishId);
        bus.PubSub.Publish(schedule, updateSchedulePublishId);
    }

    public void RunNow(Schedule schedule)
    {
        var runNowJobPublishId = string.Format(MailDeliveryConstants.RunNowJobRoutingKeyFormat, schedule.SiteId);

        bus.PubSub.Publish(schedule, runNowJobPublishId);

        legacyBus.PubSub.Publish(schedule, runNowJobPublishId);
    }

    public void Retry(Schedule schedule)
    {
        var retryJobPublishId = string.Format(MailDeliveryConstants.RetryJobRoutingKeyFormat, schedule.SiteId);

        bus.PubSub.Publish(schedule, retryJobPublishId);

        legacyBus.PubSub.Publish(schedule, retryJobPublishId);
    }

    #region IDisposable Support
    private bool disposedValue;

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                // Free managed resources
            }

            bus?.Dispose();
            legacyBus?.Dispose();

            disposedValue = true;
        }
    }

    ~JobOperationService()
    {
        Dispose(false);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    #endregion
}