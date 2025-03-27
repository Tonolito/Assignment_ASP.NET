using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

[Authorize]
[Route("admin")]
public class AdminController : Controller
{
    //[Authorize(Roles = "admin")]
    [Route("members")]
    public IActionResult Members()
    {
        return View();
    }

    //[Authorize(Roles = "admin")]
    [Route("clients")]
    public IActionResult Clients()
    {
        return View();
    }

    
}
