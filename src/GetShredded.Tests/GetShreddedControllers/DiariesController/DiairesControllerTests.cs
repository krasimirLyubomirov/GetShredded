using System;
using FluentAssertions;
using GetShredded.Services.Contracts;
using GetShredded.ViewModels.Input.Diary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace GetShredded.Tests.GetShreddedControllers.DiariesController
{
    [TestFixture]
    public class DiariesControllerTests
    {
        protected Mock<IDiaryService> diaryService => new Mock<IDiaryService>();

        [Test]
        public void CreateDiaryShouldReturnInvalidModel()
        {
            var diary = new DiaryInputModel
            {
                User = "UserTests",
                CreatedOn = DateTime.Now,
                Type = "TypeTests",
                Summary = null,
                Title = null
            };

            var controller = new Web.Controllers.DiariesController(diaryService.Object);
            controller.ModelState.AddModelError("Title", "StringLength");
            var result = controller.CreateDiary(diary).GetAwaiter().GetResult();

            result.Should().BeOfType<ViewResult>().Which.Model.Should().BeOfType<DiaryInputModel>();
        }

        [Test]
        public void ControllerShouldHaveAuthorizeAttribute()
        {
            typeof(Web.Controllers.DiariesController).Should().BeDecoratedWith<AuthorizeAttribute>();
        }
    }
}
