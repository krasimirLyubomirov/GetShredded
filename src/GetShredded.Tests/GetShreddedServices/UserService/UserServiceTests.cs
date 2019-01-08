using System;
using System.Linq;
using AutoMapper.QueryableExtensions;
using FluentAssertions;
using GetShredded.Common;
using GetShredded.Models;
using GetShredded.Services.Contracts;
using GetShredded.ViewModel.Input.Users;
using GetShredded.ViewModel.Output.Information;
using GetShredded.ViewModels.Output.Diary;
using GetShredded.ViewModels.Output.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace GetShredded.Tests.GetShreddedServices.UserService
{
    [TestFixture]
    public class UserServiceTests : BaseServiceFake
    {
        protected UserManager<GetShreddedUser> userManager => this.Provider.GetRequiredService<UserManager<GetShreddedUser>>();
        protected IUserService userService => this.Provider.GetRequiredService<IUserService>();
        protected RoleManager<IdentityRole> roleService => this.Provider.GetRequiredService<RoleManager<IdentityRole>>();

        [Test]
        public void LogUser_Should_Return_Sucess()
        {
            //arrange
            var user = new GetShreddedUser
            {
                UserName = "User",
            };
            string userPassword = "123";

            this.userManager.CreateAsync(user).GetAwaiter().GetResult();
            this.userManager.AddPasswordAsync(user, userPassword).GetAwaiter().GetResult();
            this.Context.SaveChanges();

            //act

            var loginUser = new LoginInputModel
            {
                Password = userPassword,
                Username = user.UserName
            };

            var result = this.userService.LogUser(loginUser);

            //assert
            result.Should().BeEquivalentTo(SignInResult.Success);
        }

        [Test]
        public void LogUserShouldReturnFailed()
        {
            //arrange
            string username = "Username";
            string userPassword = "123";

            //act

            var loginUser = new LoginInputModel
            {
                Password = userPassword,
                Username = username
            };

            var result = this.userService.LogUser(loginUser);

            //assert
            result.Should().BeEquivalentTo(SignInResult.Failed);
        }

        [Test]
        public void RegisterUserShouldSucceed()
        {
            //arrange
            var toRegister = new RegisterInputModel
            {
                Password = "123",
                ConfirmPassword = "123",
                Email = "user@mail.com",
                Username = "User"
            };

            //act
            this.roleService.CreateAsync(new IdentityRole { Name = GlobalConstants.DefaultRole }).GetAwaiter();
            var result = this.userService.RegisterUser(toRegister).GetAwaiter().GetResult();

            //assert

            result.Should().BeEquivalentTo(SignInResult.Success);
        }

        [Test]
        public void RegisterUserShouldFailWithSameUsername()
        {
            //arrange
            var toRegister = new RegisterInputModel
            {
                Password = "123",
                ConfirmPassword = "123",
                Email = "some@mail.com",
                Username = "User"
            };

            var user = new GetShreddedUser
            {
                Email = "user@mail.com",
                UserName = "User"
            };

            this.userManager.CreateAsync(user).GetAwaiter();
            this.Context.SaveChanges();
            //act
            this.roleService.CreateAsync(new IdentityRole { Name = GlobalConstants.DefaultRole }).GetAwaiter();
            var result = this.userService.RegisterUser(toRegister).GetAwaiter().GetResult();

            //assert

            result.Should().BeEquivalentTo(SignInResult.Failed);
        }

        [Test]
        public void GetHomeViewDetailsShouldReturnCorrectIEnumumerables()
        {
            //arrange

            var diaryType = new DiaryType
            {
                Name = "Cutting"
            };

            var user = new GetShreddedUser
            {
                UserName = "User"
            };

            this.userManager.CreateAsync(user).GetAwaiter();

            var diaries = new[]
            {
                new GetShreddedDiary
                {
                    Id=1,
                    CreatedOn = DateTime.Now.AddHours(2),
                    Type = diaryType,
                    User = user,
                    Summary = "Summary",
                    Title = "Title"
                },

                new GetShreddedDiary
                {
                    Id=2,
                    CreatedOn = DateTime.Now,
                    Type = diaryType,
                    User = user,
                    Summary = null,
                    Title = "Title2"
                },

                new GetShreddedDiary
                {
                    Id=3,
                    CreatedOn = DateTime.Now.AddDays(-1),
                    Type = diaryType,
                    User = user,
                    Summary = "Summary2",
                    Title = "Title3"
                },
            };

            this.Context.DiaryTypes.Add(diaryType);
            this.Context.GetShreddedDiaries.AddRange(diaries);
            this.Context.SaveChanges();

            //act

            var result = this.userService.GetHomeViewDetails();

            //assert
            int storiesCount = 2;
            string summary = diaries[2].Summary;
            var date = diaries[2].CreatedOn;
            var title = diaries[2].Title;
            var storyToNotBeIn = new DiaryIndexOutputModel
            {
                Id = 3,
                Summary = summary,
                User = user.UserName,
                CreatedOn = date.ToString(),
                Rating = 0,
                DiaryType = diaryType.Name,
                Title = title
            };

            result.Diaries.Should().NotBeNullOrEmpty()
                .And.HaveCount(storiesCount)
                .And.Subject.Should().NotContain(storyToNotBeIn);
        }

        [Test]
        public void GetUserShouldReturnCorrectModel()
        {
            //arrange
            var user = new GetShreddedUser
            {
                Email = "user@mail.com",
                UserName = "User"
            };

            var userOne = new GetShreddedUser
            {
                Email = "some@mail.com",
                UserName = "UserOne"
            };

            var usertwo = new GetShreddedUser
            {
                UserName = "UserTwo"
            };

            var diaryType = new DiaryType
            {
                Name = "Cutting"
            };

            var diaries = new[]
            {
                new GetShreddedDiary
                {
                    Id=1,
                    CreatedOn = DateTime.Now.AddHours(2),
                    Type = diaryType,
                    UserId = user.Id,
                    Summary = "Summary1",
                    Title = "Title1"
                },

                new GetShreddedDiary
                {
                    Id=2,
                    CreatedOn = DateTime.Now,
                    Type = diaryType,
                    UserId = user.Id,
                    Summary = null,
                    Title = "SomeTitle2"
                },

                new GetShreddedDiary
                {
                    Id=3,
                    CreatedOn = DateTime.Now.AddDays(-1),
                    Type = diaryType,
                    UserId = userOne.Id,
                    Summary = "someSummary2",
                    Title = "SomeTitle3"
                },
            };

            var comments = new[]
            {
                new Comment
                {
                    CommentedOn = DateTime.Now,
                    GetShreddedUser = user,
                    GetShreddedDiary = diaries[0],
                    Id = 1,
                    Message = "Message"
                },
                new Comment
                {
                    CommentedOn = DateTime.Now.AddHours(2),
                    GetShreddedUser = user,
                    GetShreddedDiary = diaries[1],
                    Id = 2,
                    Message = "Message2"
                },
            };

            var followed = new[]
            {
                new GetShreddedUserDiary
                {
                    GetShreddedUserId = user.Id,
                    GetShreddedDiaryId = diaries[2].Id
                },

                new GetShreddedUserDiary
                {
                    GetShreddedUserId = usertwo.Id,
                    GetShreddedDiaryId = diaries[1].Id
                },
            };

            var notice = new Notification
            {
                GetShreddedUser = user,
                Seen = false,
                Message = "Message3",
                UpdatedDiaryId = 3
            };

            var messages = new[]
            {
                new Message
                {
                    SenderId = usertwo.Id,
                    IsReaded = true,
                    Text = "BodyBuilding",
                    ReceiverId = user.Id,
                    Id = 1,
                    SendOn = DateTime.Now.AddHours(2)
                },

                new Message
                {
                    SenderId = user.Id,
                    IsReaded = false,
                    Text = "SimplyShredded",
                    ReceiverId = userOne.Id,
                    Id = 2,
                    SendOn = DateTime.Now
                },
            };

            this.Context.Notifications.Add(notice);
            this.Context.GetShreddedUserDiaries.AddRange(followed);
            this.Context.Messages.AddRange(messages);
            this.Context.DiaryTypes.Add(diaryType);
            this.Context.GetShreddedDiaries.AddRange(diaries);
            this.Context.Comments.AddRange(comments);
            this.userManager.CreateAsync(user).GetAwaiter();
            this.userManager.CreateAsync(userOne).GetAwaiter();
            this.userManager.CreateAsync(usertwo).GetAwaiter();
            this.Context.SaveChanges();

            //act
            string username = user.UserName;
            var model = this.userService.GetUser(username);

            var notifications = this.Context.Notifications
                .Where(x => x.GetShreddedUserId == user.Id)
                .ProjectTo<NotificationOutputModel>().ToList();
            var messagesUser = this.Context.Messages.Where(x => x.ReceiverId == user.Id || x.SenderId == user.Id)
                .ProjectTo<MessageOutputModel>()
                .ToList();
            var followedDiaries = this.Context.GetShreddedUserDiaries.Where(x => x.GetShreddedUserId == user.Id)
                .Select(x => x.GetShreddedDiary)
                .ProjectTo<DiaryOutputModel>().ToList();
            var userDiaries = this.Context.GetShreddedDiaries.Where(x => x.UserId == user.Id)
                .ProjectTo<DiaryOutputModel>().ToList();

            var modelToCompare = new UserOutputModel
            {
                Comments = 2,
                Email = user.Email,
                FollowedDiaries = followedDiaries,
                Id = user.Id,
                Username = user.UserName,
                Messages = messagesUser,
                MessagesCount = messagesUser.Count,
                Notifications = notifications,
                Role = GlobalConstants.DefaultRole,
                UserDiaries = userDiaries,
                Diaries = userDiaries.Count,
            };

            model.Should().NotBeNull().And.Subject.Should().Equals(modelToCompare);
        }
    }
}
