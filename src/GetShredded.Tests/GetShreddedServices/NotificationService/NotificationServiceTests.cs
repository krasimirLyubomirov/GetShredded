using System;
using System.Linq;
using FluentAssertions;
using GetShredded.Common;
using GetShredded.Models;
using GetShredded.Services.Contracts;
using Microsoft.AspNetCore.Identity;
using NUnit.Framework;

namespace GetShredded.Tests.GetShreddedServices.NotificationService
{
    [TestFixture]
    public class NotificationServiceTests : BaseServiceFake
    {
        private INotificationService notificationService =>
            (INotificationService)this.Provider.GetService(typeof(INotificationService));

        private UserManager<GetShreddedUser> userManager =>
            (UserManager<GetShreddedUser>)this.Provider.GetService(typeof(UserManager<GetShreddedUser>));

        [Test]
        public void AddNotificationShouldSendToAllFollowersNotificationForNewPage()
        {
            //arrange
            var notificationUserOne = new GetShreddedUser
            {
                Id = "OneId",
                UserName = "UserOne"
            };

            var notificationUserTwo = new GetShreddedUser
            {
                Id = "TwoId",
                UserName = "UserTwo"
            };

            var diary = new GetShreddedDiary
            {
                Title = "One",
                Id = 1,
                CreatedOn = DateTime.Now,
                Summary = null,
                Type = new DiaryType
                {
                    Id = 1,
                    Name = "Cutting"
                },

                UserId = "1111",
            };

            var userDiaryOne = new GetShreddedUserDiary
            {
                GetShreddedUserId = notificationUserOne.Id,

                GetShreddedDiaryId = diary.Id
            };

            var userDiaryTwo = new GetShreddedUserDiary
            {
                GetShreddedUserId = notificationUserTwo.Id,

                GetShreddedDiaryId = diary.Id
            };

            diary.Followers.Add(userDiaryOne);
            diary.Followers.Add(userDiaryTwo);

            userManager.CreateAsync(notificationUserOne).GetAwaiter().GetResult();
            userManager.CreateAsync(notificationUserTwo).GetAwaiter().GetResult();
            this.Context.GetShreddedDiaries.Add(diary);
            this.Context.GetShreddedUserDiaries.Add(userDiaryOne);
            this.Context.GetShreddedUserDiaries.Add(userDiaryTwo);
            this.Context.SaveChanges();

            //act
            int diaryId = diary.Id;
            string diaryTitle = diary.Title;

            this.notificationService.AddNotification(diaryId, null, diaryTitle);

            //assert
            var result = this.Context.GetShreddedUserDiaries.ToList();

            result.Should().NotBeEmpty().And.HaveCount(2);
            result[0].GetShreddedUser.Should().BeEquivalentTo(notificationUserOne);
            result[1].GetShreddedUser.Should().BeEquivalentTo(notificationUserTwo);
        }

        [Test]
        public void NewNotifications_Should_Return_User_By_Username_Notifications_Where_Seen_IsFalse()
        {
            var user = new GetShreddedUser
            {
                Id = "OneId",
                UserName = "UserOne"
            };

            var notifications = new[]
            {
                new Notification
                {
                    GetShreddedUser = user,
                    GetShreddedUserId = user.Id,
                    Seen = false,
                    Message = GlobalConstants.NotificationMessage,
                    UpdatedDiaryId = 1
                },

                new Notification
                {
                    GetShreddedUser = user,
                    GetShreddedUserId = user.Id,
                    Seen = false,
                    Message = GlobalConstants.NotificationMessage,
                    UpdatedDiaryId = 2
                }
            };

            this.userManager.CreateAsync(user).GetAwaiter().GetResult();
            this.Context.Notifications.AddRange(notifications);
            this.Context.SaveChanges();

            //act
            string username = user.UserName;
            var count = this.notificationService.NewNotifications(username);

            //assert
            int countExpected = notifications.Count();
            count.Should().Be(countExpected);
        }

        [Test]
        public void DeleteNotificationShouldDeleteOnlyNotificationWithGivenId()
        {
            //arrange
            var notifications = new[]
            {
                new Notification
                {
                    Id = 1,
                    GetShreddedUserId = "UserId",
                    Seen = false,
                    Message = GlobalConstants.NotificationMessage,
                    UpdatedDiaryId = 1
                },

                new Notification
                {
                    Id = 2,
                    GetShreddedUserId = "UserIdTwo",
                    Seen = false,
                    Message = GlobalConstants.NotificationMessage,
                    UpdatedDiaryId = 2
                }
            };

            this.Context.Notifications.AddRange(notifications);
            this.Context.SaveChanges();

            //act
            int notificationId = notifications[0].Id;
            this.notificationService.DeleteNotification(notificationId);
            //assert
            var notificationsLeft = this.Context.Notifications.ToList();
            notificationsLeft.Should().ContainSingle()
                .And.Subject.Select(x => x.Id).Should().Contain(2);
        }

        [Test]
        public void DeleteAllNotificationsShouldRemoveAllNotificationsForUserByUsername()
        {
            //arrange
            var user = new GetShreddedUser
            {
                Id = "OneId",
                UserName = "UserOne"
            };

            var notifications = new[]
            {
                new Notification
                {
                    GetShreddedUser = user,
                    GetShreddedUserId = user.Id,
                    Seen = false,
                    Message = GlobalConstants.NotificationMessage,
                    UpdatedDiaryId = 1
                },

                new Notification
                {
                    GetShreddedUser = user,
                    GetShreddedUserId = user.Id,
                    Seen = false,
                    Message = GlobalConstants.NotificationMessage,
                    UpdatedDiaryId = 2
                }
            };

            this.userManager.CreateAsync(user).GetAwaiter().GetResult();
            this.Context.Notifications.AddRange(notifications);
            this.Context.SaveChanges();

            //act
            string username = user.UserName;
            this.notificationService.DeleteAllNotifications(username);

            //assert
            var userNotifications = this.Context.Notifications.Count();
            int expectedCount = 0;
            userNotifications.Should().Be(expectedCount);
        }

        [Test]
        public void MarkNotificationAsSeenShouldMarkNotificationWithGivenIdAsSeen()
        {
            //arrange

            var notifications = new[]
            {
                new Notification
                {
                    Seen = false,
                    Message = GlobalConstants.NotificationMessage,
                    UpdatedDiaryId= 1
                },
                new Notification
                {
                    Seen = false,
                    Message = GlobalConstants.NotificationMessage,
                    UpdatedDiaryId =2
                }
            };

            this.Context.Notifications.AddRange(notifications);
            this.Context.SaveChanges();
            //act
            int notificationId = notifications[0].Id;
            this.notificationService.MarkNotificationAsSeen(notificationId);

            //assert
            var notification = this.Context.Notifications.Find(notificationId);

            notification.Should().NotBeNull()
                .And.Subject.As<Notification>()
                .Seen.Should().BeTrue();
        }

        [Test]
        public void DiaryExistsShouldReturnTrue()
        {
            //arrange
            var diary = new GetShreddedDiary
            {
                Title = "One",
                Id = 1,
                CreatedOn = DateTime.Now,
                Summary = null,
                Type = new DiaryType
                {
                    Id = 1,
                    Name = "Cutting"
                },

                UserId = "1111",
            };

            this.Context.GetShreddedDiaries.Add(diary);
            this.Context.SaveChanges();

            //act
            int diaryId = diary.Id;
            var exists = this.Context.GetShreddedDiaries.Any(x => x.Id == diaryId);

            //assert

            exists.Should().BeTrue();
        }
    }
}
