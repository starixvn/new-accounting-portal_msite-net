namespace Maintenance.Sites.Controllers;

using Fanex.NAP.Client.Attributes;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using VaultSharp;
using VaultSharp.V1.AuthMethods.AppRole;

public class VaultRequest
{
    public string Address { get; set; }
    
    public string SecretPath { get; set; }

    public string MountPath { get; set; }

    public string Role { get; set; }

    public string SecretId { get; set; }
}

[SessionAuthorize]
public class VaultController(IConfiguration configuration) : Controller
{
    [HttpGet]
    public ActionResult Index()
    {
        var vaultRequest = new VaultRequest();
        configuration.GetSection("VaultOptions").Bind(vaultRequest);
        vaultRequest.SecretPath = "database/alias";
        return View(vaultRequest);
    }

    [HttpPost]
    public async Task<ActionResult> Index(VaultRequest vaultRequest)
    {
        var vaultClient = CreateVaultClient(vaultRequest);

        var secrets = await vaultClient.V1.Secrets.KeyValue.V2
            .ReadSecretAsync(vaultRequest.SecretPath, null, vaultRequest.MountPath)
            .ConfigureAwait(false);
        var secretData = secrets.Data.Data.ToDictionary(v => v.Key,
            v => v.Value == null ? string.Empty : v.Value.ToString().Trim());
        ViewBag.SecretData = JsonConvert.SerializeObject(secretData, Formatting.Indented);
        return View(vaultRequest);
    }

    private static VaultClient CreateVaultClient(VaultRequest vaultOptions)
    {
        var vaultClientSettings = new VaultClientSettings(
            vaultOptions.Address,
            new AppRoleAuthMethodInfo(vaultOptions.Role, vaultOptions.SecretId)
        );

        return new VaultClient(vaultClientSettings);
    }
}