using Fanex.Data;
using Fanex.NAP.Client;
using Fanex.NAP.Client.Configuration;
using Fanex.NAP.Client.Helpers.Web;
using Fanex.NAP.SignIn.Attribute;
using Maintenance.Sites.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Newtonsoft.Json.Serialization;
using ONEbook.UM.AspNetCore.Filters;

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
services.AddOneBookUm(configuration);
services.AddNapClient(configuration);
services.AddFanexLog(configuration);
services.AddCommonServices();
services.AddMail(configuration);
services.AddMailStation();
services.AddLetstalk();
services.AddTraceSp();

services.AddControllersWithViews(options =>
{
    options.Filters.Add(new ServiceFilterAttribute(typeof(CheckUMFilterAttribute)));
    options.Filters.Add(new SessionAuthorizeAttribute());
}).AddNewtonsoftJson((options) =>
{
    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
});

services.AddBundle();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseWebOptimizer();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();
app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

NapClientManager
    .UseConfig(app.Services.GetService<INapSettings>())
    .UseSession(app.Services.GetService<INapSession>());
app.UseStatusCodePagesWithRedirects("/Error/{0}");
app.MapControllerRoute(
    name: "MailStation",
    pattern: "{area:exists}/{controller=MailManage}/{action=Index}/{id?}");
app.MapControllerRoute(
    "default",
    "{controller=Home}/{action=Index}/{id?}");
app.Run();
