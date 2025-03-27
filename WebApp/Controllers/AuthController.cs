using Business.Services;
using Data.Entities;
using Domain.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

[Route("auth")]
public class AuthController : Controller
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [Route("signin")]
    public IActionResult SignIn(string returnUrl = "~/")
    {
        ViewBag.ErrorMessage = null;
        ViewBag.ReturnUrl = returnUrl;

        return View();
    }


    [HttpPost]
    [Route("signin")]
    public async Task<IActionResult> SignIn(MemberSignInDto dto, string returnUrl = "~/")
    {
        ViewBag.ErrorMessage = null;

        if (ModelState.IsValid)
        {
            var result = await _authService.SignInAsync(dto);
            if (result)
            {
                return Redirect(returnUrl);
            }
        }
        if (!ModelState.IsValid)
        {
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.ErrorMessage = "Invalid Account Info";
            return View(dto);
        }

        ViewBag.ReturnUrl = returnUrl;
        ViewBag.ErrorMessage = "Unable to login wait a bit";
        return View(dto);
    }




    [Route("signup")]
    public IActionResult SignUp()
    {
        ViewBag.ErrorMessage = "";

        return View();
    }


    [HttpPost]
    [Route("signup")]
    public async Task<IActionResult> SignUp(MemberSignUpDto dto)
    {
        if (ModelState.IsValid)
        {
            var result = await _authService.SignUpAsync(dto);
            if (result)
            {
                return LocalRedirect("~/");
            }
        }

        ViewBag.ErrorMessage = "";
        return View(dto);
    }


}
