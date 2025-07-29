using Fanex.Data;
using Fanex.Logging.Sentry.RavenClient;
using Fanex.NAP.Client;
using Fanex.NAP.Client.Attributes;
using Maintenance.Sites.Services;
using Maintenance.Sites.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Newtonsoft.Json.Serialization;
using ONEbook.UM.DependencyInjection;
using ONEbook.UM.Filters;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

// Add services to the container.
services.AddCookieAuthentication(configuration);
services.AddControllers(config =>
{
    var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
    config.Filters.Add(new AuthorizeFilter(policy));
});
services.AddRedisSession(configuration);
services.AddFanexData(configuration);
services.AddFanexLogSentry(configuration);
services.AddOneBookUmForWeb(configuration);
services.AddNapClient(configuration);
services.AddExceptionHandler(configuration);
services.AddCommonServices();
services.AddMail(configuration);
services.AddMailStation();
services.AddLetstalk(configuration);
services.AddTraceSp();
services.AddBotService();

// Add background services
services.AddHostedService<SettleDurationBackgroundService>();

services.AddControllersWithViews(options =>
{
    options.Filters.Add(new ServiceFilterAttribute(typeof(CheckUMFilterAttribute)));
    options.Filters.Add(new SessionAuthorizeAttribute());
}).AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
});

services.AddBundle();
services.AddHealthChecks();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseWebOptimizer();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();
app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "MailStation",
    pattern: "{area:exists}/{controller=MailManage}/{action=Index}/{id?}");
app.MapControllerRoute(
    "default",
    "{controller=Home}/{action=Index}/{id?}");
app.MapHealthChecks("/healthz");

await app.RunAsync();