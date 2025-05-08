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

    
}
