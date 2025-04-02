using Business.Services;
using Data.Entities;
using Domain.Dtos;
using Domain.Extentions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels;

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
    public async Task<IActionResult> SignIn(MemberSignInViewModel model, string returnUrl = "~/")
    {
        ViewBag.ErrorMessage = null;

        MemberSignInDto dto = model;


        if (ModelState.IsValid)
        {
            var result = await _authService.SignInAsync(model);
            if (result.Succeeded)
            {
                return Redirect(returnUrl);
            }
        }
        if (!ModelState.IsValid)
        {
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.ErrorMessage = "Invalid Account Info";
            return View(model);
        }

        ViewBag.ReturnUrl = returnUrl;
        ViewBag.ErrorMessage = "Unable to login wait a bit";
        return View(model);
    }


    //SIGN UP

    [Route("signup")]
    public IActionResult SignUp()
    {
        ViewBag.ErrorMessage = "";

        return View();
    }


    [HttpPost]
    [Route("signup")]
    public async Task<IActionResult> SignUp(MemberSignUpViewModel model)
    {
        ViewBag.ErrorMessage = null ;

        MemberSignUpDto dto = model;

        if (ModelState.IsValid)
        {
            var result = await _authService.SignUpAsync(model);
            if (result.Succeeded)
            {
                ViewBag.ErrorMessage = result.Error;
                return LocalRedirect("~/");
            }

        }

        return View();
    }


    public async Task<IActionResult> SignOut(string returnUrl = "~/")
    {
       var result = await _authService.SignOutAsync();
        if (!result.Succeeded)
        {
            Console.WriteLine("error Signing out");
        }
        
        return Redirect(returnUrl);
    }

}
