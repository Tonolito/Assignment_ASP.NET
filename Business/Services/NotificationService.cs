using Data.Contexts;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
namespace Business.Services;

public interface INotificationService
{
    Task AddNotificitionAsync(NotificationEntity notificationEntity, string userId = "Anon");
    Task DismissNotificationAsync(string notificationId, string userId);
    Task<IEnumerable<NotificationEntity>> GetNotificationsAsync(string userId, int take = 5);
}

public class NotificationService : INotificationService
{
    // GÅ TILL REPOSITORY?

    private readonly AppDbContext _context;

    public NotificationService(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddNotificitionAsync(NotificationEntity notificationEntity, string userId = "Anon")
    {
        if (string.IsNullOrWhiteSpace(notificationEntity.Icon))
        {
            switch (notificationEntity.NotificationTypeId)
            {
                case 1:
                    notificationEntity.Icon = "~/images/profiles/user-template.svg";
                    break;
                case 2:
                    notificationEntity.Icon = "~/images/projects/project-tempalte.svg";
                    break;
            }
        }

        _context.Add(notificationEntity);
        await _context.SaveChangesAsync();
    }


    public async Task<IEnumerable<NotificationEntity>> GetNotificationsAsync(string userId, int take = 5)
    {
        var dismissedIds = await _context.DismissedNotifications
            .Where(x => x.UserId == userId)
            .Select(x => x.NotificationId)
            .ToListAsync();

        var notifications = await _context.Notifications
            .Where(x => !dismissedIds.Contains(x.Id))
            .OrderByDescending(x => x.Created)
            .Take(take)
            .ToListAsync();

        return notifications;
        //return [];
    }

    public async Task DismissNotificationAsync(string notificationId, string userId)
    {
        var alreadyDismissed = await _context.DismissedNotifications.AnyAsync(x => x.NotificationId == notificationId && x.UserId == userId);
        if (!alreadyDismissed)
        {
            var dismissed = new NotificationDismissedEntity()
            {
                NotificationId = notificationId,
                UserId = userId
            };
            _context.Add(dismissed);
            await _context.SaveChangesAsync();
        }
    }
}
