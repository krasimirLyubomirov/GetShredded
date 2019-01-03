using GetShredded.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GetShredded.Web.Controllers
{
    [Authorize]
    public class NotificationsController : Controller
    {
        public NotificationsController(INotificationService notificationService)
        {
            this.NotificationService = notificationService;
        }

        protected INotificationService NotificationService { get; }

        public IActionResult DeleteNotification(int id)
        {
            this.NotificationService.DeleteNotification(id);
            return RedirectToInformation();
        }

        public IActionResult DeleteAllNotifications(string username)
        {
            this.NotificationService.DeleteAllNotifications(username);
            return RedirectToInformation();
        }

        public IActionResult MarkNotificationAsSeen(int id)
        {
            this.NotificationService.MarkNotificationAsSeen(id);
            return RedirectToInformation();
        }

        private IActionResult RedirectToInformation()
        {
            var username = this.User.Identity.Name;
            return RedirectToAction("Information", "Messages", new { username });
        }
    }
}
