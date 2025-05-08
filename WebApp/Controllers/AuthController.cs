using Business.Services;
using Data.Entities;
using Domain.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using WebApp.Hubs;
using WebApp.ViewModels;

namespace WebApp.Controllers;

[Route("auth")]
public class AuthController : Controller
{
    private readonly IAuthService _authService;
    private readonly SignInManager<MemberEntity> _signInManager;
    private readonly UserManager<MemberEntity> _userManager;
    private readonly INotificationService _notificationService;
    private readonly IHubContext<NotificationHub> _notificationHub;

    public AuthController(IAuthService authService, SignInManager<MemberEntity> signInManager, UserManager<MemberEntity> userManager, INotificationService notificationService, IHubContext<NotificationHub> notificationHub)
    {
        _authService = authService;
        _signInManager = signInManager;
        _userManager = userManager;
        _notificationService = notificationService;
        _notificationHub = notificationHub;
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
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "Anon";

                // Skapa notification
                var notification = new NotificationEntity
                {
                    Message = $"{model.Email} signed in",
                    NotificationTypeId = 1,
                    Created = DateTime.Now
                };

                // Lägg till notifikation i databasen
                await _notificationService.AddNotificitionAsync(notification, userId);

                // Hämta alla synliga notifikationer för användaren
                var notifications = await _notificationService.GetNotificationsAsync(userId);

                // Skicka listan till alla via SignalR (du kan byta till .User(userId) om du vill skicka privat)
                await _notificationHub.Clients.All.SendAsync("ReceiveNotification", notifications);
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

        if (signInResult.Succeeded)
        {
            // Parts help with ChatGpt cuase of not showing External provider in profiledropdown
            var user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
            var existingClaims = await _userManager.GetClaimsAsync(user);
            if (!existingClaims.Any(c => c.Type == ClaimTypes.AuthenticationMethod))
            {
                await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.AuthenticationMethod, info.LoginProvider));
            }

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

            }catch { }


            string email = info.Principal.FindFirstValue(ClaimTypes.Email)!;
            string userName = $"ext_{info.LoginProvider.ToLower()}_{email}";

            var user = new MemberEntity { UserName = userName, Email = email, FirstName = firstName, LastName = lastName };


            var IdentityResult = await _userManager.CreateAsync(user);
            if (IdentityResult.Succeeded)
            {
                // Parts help with ChatGpt cuase of not showing External provider in profiledropdown
                await _userManager.AddLoginAsync(user, info);
                await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.AuthenticationMethod, info.LoginProvider));
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
