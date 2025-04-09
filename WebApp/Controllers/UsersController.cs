using Data.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Controllers;

public class UsersController : Controller
{
    private readonly AppDbContext _dataContext;

    public UsersController(AppDbContext dataContext)
    {
        _dataContext = dataContext;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public async Task<JsonResult> SearchUsers(string term)
    {
        if(string.IsNullOrEmpty(term))
            return Json(new List<object>());

        var users = await _dataContext.Users
            .Where(x => x.FirstName.Contains(term) || x.LastName.Contains(term) || x.Email.Contains(term))
            .Select(x => new { x.Id, Fullname = x.FirstName + " " + x.LastName})
            .ToListAsync();

        return Json(users);
    }
}
