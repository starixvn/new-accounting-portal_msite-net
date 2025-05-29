using Maintenance.Mattermost.Services;

namespace Maintenance.Sites.Utilities;

using Fanex.Data.Repository;
using Fanex.Logging.Extensions;
using Fanex.Logging.Extensions.AspNetCore;
using Fanex.Logging.Extensions.Database;
using Fanex.MailDeliver.MailClient;
using Letstalk.Services;
using Mail.Services;
using MailStation.Services;
using Maintenance.BotService.Services;
using Maintenance.BotService.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using NUglify.Css;
using NUglify.JavaScript;
using Services;
using StackExchange.Redis;
using TraceSp.Services;
using WebOptimizer.Processors;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCookieAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = configuration.GetSection("AuthSettings:LoginPath").Value;
                options.LogoutPath = configuration.GetSection("AuthSettings:LogoutPath").Value;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(int.Parse(configuration.GetSection("AuthSettings:CookieTimeOut").Value));
                options.Cookie = new CookieBuilder
                {
                    Name = "mSite.Cookie",
                    SecurePolicy = CookieSecurePolicy.SameAsRequest
                };
                options.Events.OnSigningOut = context =>
                {
                    context.Response.Redirect(options.LoginPath);
                    return Task.CompletedTask;
                };
            });

        return services;
    }

    public static IServiceCollection AddRedisSession(this IServiceCollection services, IConfiguration configuration)
    {
        var redisConfig = configuration.GetSection("AuthSettings:RedisConfig").Value;
        var redisMultiplexer = ConnectionMultiplexer.Connect(redisConfig);

        services.AddDataProtection().PersistKeysToStackExchangeRedis(redisMultiplexer, "mSite-ProtectionKeys");

        services.AddSingleton(redisMultiplexer);

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = redisConfig;
            options.InstanceName = "mSite.Session-";
        });

        services.AddSession(options =>
        {
            options.Cookie.IsEssential = true;
            options.IdleTimeout = TimeSpan.FromMinutes(int.Parse(configuration.GetSection("AuthSettings:SessionTimeOut").Value));
            options.Cookie.Name = "mSite.Session";
        });

        return services;
    }

    public static IServiceCollection AddBundle(this IServiceCollection services)
    {
        services.AddWebOptimizer(pipeline =>
        {
            var cssSetting = new CssSettings { OutputMode = NUglify.OutputMode.MultipleLines, CommentMode = CssComment.None };
            var jsSettings = new JsSettings { GenerateSourceMap = true, CodeSettings = new CodeSettings { MinifyCode = false } };
            pipeline.AddRawLess();
            pipeline.AddLayoutBundle(cssSetting, jsSettings);
            pipeline.AddMailBundle();
        });

        return services;
    }

    public static IServiceCollection AddCommonServices(this IServiceCollection services)
    {
        services.AddSingleton<IDynamicRepository, DynamicRepository>();

        return services;
    }

    public static IServiceCollection AddExceptionHandler(this IServiceCollection services, IConfiguration configuration)
    {
        ExceptionHandler
            .UseConfig(new LoggingConfiguration())
            .UseGetDbExceptionInfo(DatabaseInfoGetters.GetDbExceptionInfo);

        return services;
    }

    public static IServiceCollection AddMail(this IServiceCollection services, IConfiguration configuration)
    {
        AppSettings.AppMailId = configuration.GetValue<string>("MailSettings:MailId");
        AppSettings.AppMailAddress = configuration.GetValue<string>("MailSettings:MailAddress");
        services.AddTransient<IMailService, MailService>();
        services.AddTransient<IMailRepository, MailRepository>();

        return services;
    }

    public static IServiceCollection AddMailStation(this IServiceCollection services)
    {
        services.AddTransient<IJobOperationService, JobOperationService>();
        services.AddTransient<IMailGroupService, MailGroupService>();
        services.AddTransient<IMailManageService, MailManageService>();
        services.AddTransient<IMailReportService, MailReportService>();
        services.AddTransient<IManageRecipientService, ManageRecipientService>();
        services.AddTransient<IPermissionService, PermissionService>();

        return services;
    }

    public static IServiceCollection AddLetstalk(this IServiceCollection services)
    {
        services.AddTransient<IHttpClientService, HttpClientService>();
        services.AddTransient<ILetstalkReportRestClientService, LetstalkReportRestClientService>();
        services.AddTransient<IManageLetstalkMessageService, ManageLetstalkMessageService>();

        return services;
    }

    public static IServiceCollection AddMattermost(this IServiceCollection services)
    {
        services.AddTransient<Mattermost.Services.IHttpClientService, Mattermost.Services.HttpClientService>();
        services.AddTransient<MattermostReportRestClientService, MattermostReportRestClientServiceImp>();

        return services;
    }

    public static IServiceCollection AddTraceSp(this IServiceCollection services)
    {
        services.AddTransient<IDbServerService, DbServerService>();
        services.AddTransient<ISiteService, SiteService>();
        services.AddTransient<ITracingSpService, TracingSpService>();

        return services;
    }

    public static IServiceCollection AddBotService(this IServiceCollection services)
    {
        services.AddHttpClient();
        services.AddSingleton<IApplicationSettingService, ApplicationSettingService>();
        services.AddSingleton<IMessageService, MessageService>();
        services.AddSingleton<IBotConnector, BotConnector>();
        return services;
    }
}