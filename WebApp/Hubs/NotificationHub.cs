using Microsoft.AspNetCore.SignalR;

namespace WebApp.Hubs;

public class NotificationHub : Hub
{
    public async Task SendNotificationToAll(object notification)
    {
        await Clients.All.SendAsync("ReceiveNotification", notification);
    }

    //// KAN SKICKA TILL OLIKA GRUPPER
    //public async Task SendNotificationToAdmin(object notification)
    //{
    //    await Clients.All.SendAsync("AdminReceiveNotification", notification);
    //}
   
}
