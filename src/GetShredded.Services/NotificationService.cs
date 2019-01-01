using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GetShredded.Common;
using GetShredded.Data;
using GetShredded.Models;
using GetShredded.Services.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GetShredded.Services
{
    public class NotificationService : BaseService, INotificationService
    {
        public NotificationService(
            UserManager<GetShreddedUser> userManager,
            SignInManager<GetShreddedUser> signInManager,
            GetShreddedContext context, IMapper mapper)
            : base(userManager, signInManager, context, mapper)
        {
        }

        public void AddNotification(int diaryId, string username, string diaryTitle)
        {
            var notificationRange = new List<Notification>();

            var users = this.Context.Users
                .Where(x => x.FollowedDiaries
                    .Any(xx => xx.GetShreddedDiaryId == diaryId && x.UserName != username)).ToArray();

            foreach (var u in users)
            {
                var notice = new Notification
                {
                    GetShreddedUserId = u.Id,
                    Message = string.Format(GlobalConstants.NotificationMessage, diaryTitle),
                    Seen = false,
                    UpdatedDiaryId = diaryId
                };

                notificationRange.Add(notice);
                u.Notifications.Add(notice);
            }

            this.Context.Notifications.AddRange(notificationRange);
            this.Context.Users.UpdateRange(users);
            this.Context.SaveChanges();
        }

        public int NewNotifications(string username)
        {
            var user = this.UserManager.FindByNameAsync(username).GetAwaiter().GetResult();

            var newNotices = this.Context.Notifications
                .Include(x => x.GetShreddedUser)
                .Where(x => x.GetShreddedUserId == user.Id && x.Seen == false)
                .ToList()
                .Count;

            return newNotices;
        }

        public void DeleteNotification(int id)
        {
            var notification = this.Context.Notifications.Find(id);

            this.Context.Notifications.Remove(notification);

            this.Context.SaveChanges();
        }

        public void DeleteAllNotifications(string username)
        {
            var notifications = this.Context.Notifications
                .Where(x => x.GetShreddedUser.UserName == username)
                .ToArray();

            this.Context.Notifications.RemoveRange(notifications);

            this.Context.SaveChanges();
        }

        public void MarkNotificationAsSeen(int id)
        {
            var notice = this.Context.Notifications.Find(id);
            notice.Seen = true;

            this.Context.Notifications.Update(notice);

            this.Context.SaveChanges();
        }

        public bool DiaryExists(int id)
        {
            bool exists = this.Context.GetShreddedDiaries.Any(x => x.Id == id);

            return exists;
        }
    }
}
