using Business.Services;
using Data.Entities;
using Domain.Dtos;
using Domain.Extentions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApp.ViewModels;

namespace WebApp.Controllers;

[Route("auth")]
public class AuthController : Controller
{
    private readonly IAuthService _authService;
    private readonly SignInManager<MemberEntity> _signInManager;
    private readonly UserManager<MemberEntity> _userManager;

    public AuthController(IAuthService authService, SignInManager<MemberEntity> signInManager, UserManager<MemberEntity> userManager)
    {
        _authService = authService;
        _signInManager = signInManager;
        _userManager = userManager;
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

    [Route("signin/external")]

    [HttpPost]
    public IActionResult ExternalSignIn(string provider, string returnUrl = null!)
    {
        if (string.IsNullOrEmpty(provider))
        {
            ModelState.AddModelError("", "Invalid Provider");
            return View("SignIn");
        }

        var redirectUrl = Url.Action("ExternalSignInCallback", "Auth", new { returnUrl })!;
        var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
        return Challenge(properties, provider);
    }

    [Route("signin/externalcallback")]

    public async Task<IActionResult> ExternalSignInCallback(string returnUrl = null!, string remoteError = null!)
    {
        returnUrl ??= Url.Content("~/");

        if (!string.IsNullOrEmpty(remoteError))
        {
            ModelState.AddModelError("", $"Error from external provider {remoteError}");
            return View("SignIn");
        }

        var info = await _signInManager.GetExternalLoginInfoAsync();
        if (info == null)
        {
            return RedirectToAction("SignIn");
        }

        var signInResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent:false, bypassTwoFactor:true);
        Console.Write($"Sign-in result: {signInResult.Succeeded}, " +
                  $"LockedOut: {signInResult.IsLockedOut}, " +
                  $"NotAllowed: {signInResult.IsNotAllowed}, " +
                  $"RequiresTwoFactor: {signInResult.RequiresTwoFactor}");
        if (signInResult.Succeeded)
        {
            return LocalRedirect(returnUrl);
        }
        else
        {
            string firstName = string.Empty;
            string lastName = string.Empty;
            try
            {
                firstName = info.Principal.FindFirstValue(ClaimTypes.GivenName)!;
                lastName = info.Principal.FindFirstValue(ClaimTypes.Surname)!;
            }
            catch { }


            string email = info.Principal.FindFirstValue(ClaimTypes.Email)!;
            string userName = $"ext_{info.LoginProvider.ToLower()}{email}";

            var user = new MemberEntity { UserName = firstName, Email = email, FirstName = firstName, LastName = lastName };

            var IdentityResult = await _userManager.CreateAsync(user);
            if (IdentityResult.Succeeded)
            {
                await _userManager.AddLoginAsync(user, info);
                await _signInManager.SignInAsync(user, isPersistent: false);

                return LocalRedirect(returnUrl);
            }
            foreach (var error in IdentityResult.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View("SignIn");
        }
    }


}
