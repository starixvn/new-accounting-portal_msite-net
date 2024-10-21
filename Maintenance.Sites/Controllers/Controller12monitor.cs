using Microsoft.AspNetCore.Authorization;

namespace Maintenance.Sites.Controllers;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[AllowAnonymous]
[Route("12monitor")]
public class Controller12monitor : ControllerBase
{
    [HttpGet]
    [Route("")]
    public string Index() => "12monitor";
}