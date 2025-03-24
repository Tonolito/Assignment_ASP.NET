using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

[Route("projects")]
public class ProjectsController : Controller
{
    [Route("")]
    public IActionResult Projects()
    {
        return View();
    }
    //public IActionResult AddProject()
    //{
    //    return View();
    //}

    //public IActionResult OptionsProject()
    //{
    //    return View();
    //}
    
    //public IActionResult EditProject()
    //{
    //    return View();
    //}

    //public IActionResult NotificationProject()
    //{
    //    return View();
    //}


    //// LOG out??
    //public IActionResult Logout()
    //{
    //    return View();
    //}
}
