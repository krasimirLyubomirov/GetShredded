using System;
using System.Linq;
using AutoMapper.QueryableExtensions;
using FluentAssertions;
using GetShredded.Common;
using GetShredded.Models;
using GetShredded.Services.Contracts;
using GetShredded.ViewModel.Input.Page;
using GetShredded.ViewModels.Output.Page;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace GetShredded.Tests.GetShreddedServices.PageService
{
    [TestFixture]
    public class PageServiceTests : BaseServiceFake
    {
        private UserManager<GetShreddedUser> userManager => this.Provider.GetRequiredService<UserManager<GetShreddedUser>>();
        private IPageService pageService => this.Provider.GetRequiredService<IPageService>();
        private RoleManager<IdentityRole> roleManager => this.Provider.GetRequiredService<RoleManager<IdentityRole>>();

        [Test]
        public void GetPageToEditByIdShouldReturnThePageEditModelById()
        {
            //arrange

            var user = new GetShreddedUser
            {
                Id = "UserId",
                UserName = "User",
            };

            var diary = new GetShreddedDiary
            {
                Id = 1,
                User = user,
                Summary = "Summary",
                Title = "Title",
                UserId = "UserId",
            };

            var page = new Page
            {
                Id = 1,
                Content = "SomeContent",
                UserId = user.Id,
                GetShreddedUser = user,
                GetShreddedDiary = diary,
                GetShreddedDiaryId = diary.Id,
                Title = "Title",
                CreatedOn = DateTime.UtcNow
            };

            var record = new PageEditModel
            {
                User = user.UserName,
                Content = page.Content,
                CreatedOn = page.CreatedOn,
                Id = page.Id,
                DiaryId = diary.Id,
                Title = page.Title
            };

            userManager.CreateAsync(user).GetAwaiter();
            this.Context.Pages.Add(page);
            this.Context.GetShreddedDiaries.Add(diary);
            this.Context.SaveChanges();

            //act
            int pageId = page.Id;
            var result = this.pageService.GetPageToEditById(pageId);

            //assert
            result.Should().NotBeNull().And.Subject.Should().BeEquivalentTo(record);
        }

        [Test]
        public void EditPageShouldEditPageContentOrTitle()
        {
            //arrange

            var user = new GetShreddedUser
            {
                Id = "UserId",
                UserName = "User",
            };

            var diary = new GetShreddedDiary
            {
                Id = 1,
                User = user,
                Summary = "Summary",
                Title = "Title",
                UserId = "UserId",
            };

            var page = new Page
            {
                Id = 1,
                Content = "Content",
                UserId = user.Id,
                GetShreddedUser = user,
                GetShreddedDiary = diary,
                GetShreddedDiaryId = diary.Id,
                Title = "Title",
                CreatedOn = DateTime.UtcNow
            };

            var record = new PageEditModel
            {
                User = user.UserName,
                Content = "Some New Content",
                CreatedOn = page.CreatedOn,
                Id = page.Id,
                DiaryId = diary.Id,
                Title = "New Title"
            };

            userManager.CreateAsync(user).GetAwaiter();
            this.Context.Pages.Add(page);
            this.Context.GetShreddedDiaries.Add(diary);
            this.Context.SaveChanges();

            //act
            this.pageService.EditPage(record);

            //assert
            var result = this.Context.Pages.ProjectTo<PageEditModel>().FirstOrDefault(x => x.Id == page.Id);

            result.Should().NotBeNull().And.Subject.Should().BeEquivalentTo(record);
        }

        [Test]
        public void AddPageShouldAddNewPageToDiary()
        {
            //arrange

            var user = new GetShreddedUser
            {
                Id = "UserId",
                UserName = "User",
            };

            var diary = new GetShreddedDiary
            {
                Id = 1,
                User = user,
                Summary = "Summary",
                Title = "Title",
                UserId = "UserId",
            };

            var page = new Page
            {
                Id = 2,
                Content = "Content",
                UserId = user.Id,
                GetShreddedUser = user,
                GetShreddedDiary = diary,
                GetShreddedDiaryId = diary.Id,
                Title = "Title",
                CreatedOn = DateTime.UtcNow
            };

            var newPage = new PageInputModel
            {
                User = user.UserName,
                Content = "Some New Content",
                CreatedOn = DateTime.Now,
                DiaryId = diary.Id,
                Title = "New Page"
            };

            userManager.CreateAsync(user).GetAwaiter();
            this.Context.Pages.Add(page);
            this.Context.GetShreddedDiaries.Add(diary);
            this.Context.SaveChanges();

            //act
            this.pageService.AddPage(newPage);

            //assert

            var result = this.Context.Pages.OrderBy(x => x.CreatedOn).Last(x => x.GetShreddedDiaryId == diary.Id);

            result.Should().NotBeNull()
                .And.Subject.As<Page>()
                .Title.Should().BeSameAs(newPage.Title);
        }

        [Test]
        public void DeletePageAdminShouldDeletePage()
        {
            //arrange
            var admin = new GetShreddedUser
            {
                Id = "AdminId",
                UserName = "Admin",
            };

            var user = new GetShreddedUser
            {
                Id = "UserId",
                UserName = "User",
            };

            var diary = new GetShreddedDiary
            {
                Id = 1,
                User = user,
                Summary = "Summary",
                Title = "Title",
                UserId = "UserId",
            };

            var pages = new[]
            {
                new Page
                {
                    Id = 1,
                    GetShreddedUser = user,
                    GetShreddedDiary = diary,
                    UserId = user.Id,
                    GetShreddedDiaryId = diary.Id,
                    Content = "Content",
                    Title = "Title",
                    CreatedOn = DateTime.UtcNow
                },

                new Page
                {
                    Id=2,
                    GetShreddedUser = user,
                    GetShreddedDiary = diary,
                    UserId = user.Id,
                    GetShreddedDiaryId = diary.Id,
                    Content = "Some New Content",
                    Title = "New Page",
                    CreatedOn = DateTime.Now,
                }
            };

            var role = new IdentityRole
            {
                Name = GlobalConstants.Admin
            };

            roleManager.CreateAsync(role).GetAwaiter();
            userManager.CreateAsync(user).GetAwaiter();
            userManager.CreateAsync(admin).GetAwaiter();
            userManager.AddToRoleAsync(admin, nameof(GlobalConstants.Admin)).GetAwaiter();
            this.Context.Pages.AddRange(pages);
            this.Context.GetShreddedDiaries.Add(diary);
            this.Context.SaveChanges();

            //act
            int diaryId = diary.Id;
            int pageId = pages[0].Id;
            string username = admin.UserName;
            this.pageService.DeletePage(diaryId, pageId, username);

            //assert
            var result = this.Context.Pages.First();
            var pageLeft = pages[1];

            result.Should().NotBeNull()
                .And.Subject.As<Page>().Title.Should().BeEquivalentTo(pageLeft.Title);
        }

        [Test]
        public void DeletePageUserShouldDeletePage()
        {
            //arrange

            var user = new GetShreddedUser
            {
                Id = "UserId",
                UserName = "User",
            };

            var diary = new GetShreddedDiary
            {
                Id = 1,
                User = user,
                Summary = "Summary",
                Title = "Title",
                UserId = "UserId",
            };

            var pages = new[]
            {
                new Page
                {
                    Id = 1,
                    GetShreddedUser = user,
                    GetShreddedDiary = diary,
                    UserId = user.Id,
                    GetShreddedDiaryId = diary.Id,
                    Content = "Content",
                    Title = "Title",
                    CreatedOn = DateTime.UtcNow
                },

                new Page
                {
                    Id = 2,
                    GetShreddedUser = user,
                    GetShreddedDiary = diary,
                    UserId = user.Id,
                    GetShreddedDiaryId = diary.Id,
                    Content = "Some New Content",
                    Title = "New Page",
                    CreatedOn = DateTime.Now,
                }
            };

            var role = new IdentityRole
            {
                Name = GlobalConstants.Admin
            };

            roleManager.CreateAsync(role).GetAwaiter();
            userManager.CreateAsync(user).GetAwaiter();

            this.Context.Pages.AddRange(pages);
            this.Context.GetShreddedDiaries.Add(diary);
            this.Context.SaveChanges();

            //act
            int diaryId = diary.Id;
            int pageId = pages[0].Id;
            string username = user.UserName;
            this.pageService.DeletePage(diaryId, pageId, username);

            //assert
            var result = this.Context.Pages.First();
            var pageLeft = pages[1];

            result.Should().NotBeNull()
                .And.Subject.As<Page>().Title.Should().BeEquivalentTo(pageLeft.Title);
        }

        [Test]
        public void DeletePageShouldThrowInvalidOperationException()
        {
            //arrange
            var firstUser = new GetShreddedUser
            {
                Id = "FirstUserId",
                UserName = "FirstUser",
            };

            var secondUser = new GetShreddedUser
            {
                Id = "SecondUserId",
                UserName = "SecondUser",
            };

            var diary = new GetShreddedDiary
            {
                Id = 1,
                User = secondUser,
                Summary = "Summary",
                Title = "Title",
                UserId = "UserId",
            };

            var pages = new[]
            {
                new Page
                {
                    Id = 1,
                    GetShreddedUser = secondUser,
                    GetShreddedDiary = diary,
                    UserId = secondUser.Id,
                    GetShreddedDiaryId = diary.Id,
                    Content = "Content",
                    Title = "Title",
                    CreatedOn = DateTime.UtcNow
                },

                new Page
                {
                    Id=2,
                    GetShreddedUser = secondUser,
                    GetShreddedDiary = diary,
                    UserId=secondUser.Id,
                    GetShreddedDiaryId = diary.Id,
                    Content = "Some New Content",
                    Title = "New Page",
                    CreatedOn = DateTime.Now,
                }
            };

            var role = new IdentityRole
            {
                Name = GlobalConstants.DefaultRole
            };

            roleManager.CreateAsync(role).GetAwaiter();
            userManager.CreateAsync(secondUser).GetAwaiter();
            userManager.CreateAsync(firstUser).GetAwaiter();

            this.Context.Pages.AddRange(pages);
            this.Context.GetShreddedDiaries.Add(diary);
            this.Context.SaveChanges();

            //act
            int diaryId = diary.Id;
            int pageId = pages[0].Id;
            string username = firstUser.UserName;

            Action act = () => this.pageService.DeletePage(diaryId, pageId, username);

            //assert
            string message = GlobalConstants.UserHasNoRights + " " + GlobalConstants.NotUser;
            act.Should().Throw<InvalidOperationException>().WithMessage(message);
        }

        [Test]
        public void DeletePageShouldThrowArgumentException()
        {
            var user = new GetShreddedUser
            {
                Id = "UserId",
                UserName = "User",
            };

            var diary = new GetShreddedDiary
            {
                Id = 1,
                User = user,
                Summary = "Summary",
                Title = "Title",
                UserId = "UserId",
            };

            var diaryTwo = new GetShreddedDiary
            {
                Id = 2,
                User = user,
                Summary = "Summary",
                Title = "Title",
                UserId = "UserId",
            };

            var pages = new[]
            {
                new Page
                {
                    Id = 1,
                    GetShreddedUser = user,
                    GetShreddedDiary = diary,
                    UserId = user.Id,
                    GetShreddedDiaryId = diary.Id,
                    Content = "Content",
                    Title = "Title",
                    CreatedOn = DateTime.UtcNow
                },

                new Page
                {
                    Id = 2,
                    GetShreddedUser = user,
                    GetShreddedDiary = diaryTwo,
                    UserId = user.Id,
                    GetShreddedDiaryId = diaryTwo.Id,
                    Content = "Some New Content",
                    Title = "New Page",
                    CreatedOn = DateTime.Now,
                }
            };

            userManager.CreateAsync(user).GetAwaiter();

            this.Context.Pages.AddRange(pages);
            this.Context.GetShreddedDiaries.Add(diary);
            this.Context.GetShreddedDiaries.Add(diaryTwo);
            this.Context.SaveChanges();

            //act
            int diaryId = diary.Id;
            int pageId = pages[1].Id;
            string username = user.UserName;
            Action act = () => this.pageService.DeletePage(diaryId, pageId, username);

            //assert
            string message = string.Join(GlobalConstants.NotValidPageDiaryConnection, diaryId, pageId);
            act.Should().Throw<ArgumentException>().WithMessage(message);
        }
    }
}
