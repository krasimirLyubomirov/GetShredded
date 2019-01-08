using System;
using FluentAssertions;
using GetShredded.Services.Contracts;
using GetShredded.ViewModel.Input.Page;
using GetShredded.ViewModels.Output.Page;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace GetShredded.Tests.GetShreddedControllers.PagesController
{
    [TestFixture]
    public class PagesControllerTests
    {
        [Test]
        public void AddPageShouldReturnErrorAddPage()
        {
            var page = new PageInputModel()
            {
                DiaryId = 1,
                User = "UserTests",
                Content = null,
                CreatedOn = DateTime.UtcNow,
                Title = "TitleTests"
            };

            var pageService = new Mock<IPageService>();

            var controller = new Web.Controllers.PagesController(pageService.Object);
            controller.ModelState.AddModelError("Content", "StringLength");

            var result = controller.AddPage(page);

            result.Should().BeOfType<ViewResult>().Which.Model.Should().BeOfType<PageInputModel>();
        }

        [Test]
        public void AddPageShouldAddPage()
        {
            var page = new PageInputModel()
            {
                DiaryId = 1,
                User = "UserTests",
                Content = null,
                CreatedOn = DateTime.UtcNow,
                Title = "TitleTests"
            };

            var pageService = new Mock<IPageService>();

            var controller = new Web.Controllers.PagesController(pageService.Object);

            var result = controller.AddPage(page);

            int diaryId = page.DiaryId;
            string redirectActionName = "Details";
            string controlerToRedirectTo = "Diaries";
            result.Should().BeOfType<RedirectToActionResult>().Which.ActionName.Should().Be(redirectActionName);
            result.Should().BeOfType<RedirectToActionResult>().Which.ControllerName.Should().Be(controlerToRedirectTo);
            result.Should().BeOfType<RedirectToActionResult>()
                .Which.RouteValues.Values.Count
                .Should().Be(1).And.Subject
                .Should().Be(diaryId);
        }

        [Test]
        public void EditPageShouldReturnNotFound()
        {
            var pageService = new Mock<IPageService>();

            var controller = new Web.Controllers.PagesController(pageService.Object);

            int id = 0;
            pageService.Setup(x => x.GetPageToEditById(id)).Returns(() => null);
            var result = controller.EditPage(id);

            result.Should().BeOfType<NotFoundResult>();
        }

        [Test]
        public void EditPageShouldReturnErrorAddPage()
        {
            var page = new PageEditModel
            {
                DiaryId = 1,
                User = "UserTests",
                Content = null,
                CreatedOn = DateTime.UtcNow,
                Title = "TitleTests"
            };

            var pageService = new Mock<IPageService>();

            var controller = new Web.Controllers.PagesController(pageService.Object);
            controller.ModelState.AddModelError("Content", "StringLength");

            var result = controller.EditPage(page);

            result.Should().BeOfType<ViewResult>().Which.Model.Should().BeOfType<PageEditModel>();
        }

        [Test]
        public void ControllerShouldHaveAuthorizeAttribute()
        {
            typeof(Web.Controllers.PagesController).Should().BeDecoratedWith<AuthorizeAttribute>();
        }
    }
}
