using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

public class MembersController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult AddMember()
    {
        return View();
    }

    public IActionResult EditMember()
    {
        return View();
    }

    public IActionResult NotificationMember()
    {
        return View();
    }
}
