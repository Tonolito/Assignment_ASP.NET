using Domain.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace WebApp.Controllers;

public class CookiesController : Controller
{
    [HttpPost]
    public IActionResult SetCookies([FromBody] CookieConsentDto consent)
    {

        Response.Cookies.Append("SessionCookie", "Essential", new CookieOptions
        {
            IsEssential = true,
            Expires = DateTimeOffset.UtcNow.AddDays(365),
        });

        if (consent == null)
            return BadRequest();

        if (consent.Functional)
        {
            
            Response.Cookies.Append("FunctionalCookie", "Non-Essential", new CookieOptions
            {
                IsEssential = false,
                Expires = DateTimeOffset.UtcNow.AddDays(30),
                SameSite = SameSiteMode.Lax,
                Path = "/",
            });
        }
        else
        {
            Response.Cookies.Delete("FunctionalCookie");
        }
        Response.Cookies.Append("CookieConsent", JsonSerializer.Serialize(consent), new CookieOptions
        {
            IsEssential = true,
            Expires = DateTimeOffset.UtcNow.AddDays(90),
            SameSite = SameSiteMode.Lax,
            Path = "/",
        });

        return Ok();
    }
}
