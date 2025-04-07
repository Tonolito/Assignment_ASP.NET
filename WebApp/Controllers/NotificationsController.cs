using Business.Services;
using Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using WebApp.Hubs;

namespace WebApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class NotificationsController : ControllerBase
{
    private readonly IHubContext<NotificationHub> _notificationHub;
    private readonly INotificationService _notificationService;

    public NotificationsController(IHubContext<NotificationHub> notificationHub, INotificationService notificationService)
    {
        _notificationHub = notificationHub;
        _notificationService = notificationService;
    }

    [HttpPost]
    [HttpPost]
    public async Task<IActionResult> CreateNotification(NotificationEntity notificationEntity)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "Anon";

        // Lägg till notifikationen i databasen
        await _notificationService.AddNotificitionAsync(notificationEntity, userId);

        // Hämta uppdaterade notiser för användaren
        var notifications = await _notificationService.GetNotificationsAsync(userId);

        // Skicka via SignalR till just denna användare
        await _notificationHub.Clients.User(userId).SendAsync("ReceiveNotification", notifications);

        return Ok(new { success = true });
    }


    [HttpGet]
    public async Task<IActionResult> GetNotifications()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "anonymous";
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        var notifications = await _notificationService.GetNotificationsAsync(userId);
        return Ok(notifications);
        
    }
    [HttpPost("dismiss/{id}")]
    public async Task<IActionResult> DismissNotification(string id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "anonymous";
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }
        
        await _notificationService.DismissNotificationAsync(id, userId);
        await _notificationHub.Clients.User(userId).SendAsync("NotificationDismissed", id);
        return Ok(new {success = true});
    }

    //[HttpDelete()]
}
