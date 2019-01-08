using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Castle.Core.Internal;
using FluentAssertions;
using GetShredded.Common;
using GetShredded.Services.Contracts;
using GetShredded.ViewModels.Output.Diary;
using GetShredded.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace GetShredded.Tests.GetShreddedControllers.AdminsController
{
    [TestFixture]
    public class AdminsControllerTests
    {
        protected Mock<IAdminService> adminService => new Mock<IAdminService>();
        protected Mock<IDiaryService> diaryService => new Mock<IDiaryService>();

        [Test]
        public async Task DeleteDiaryShouldRedirectToErrorWhenRoleIsMissing()
        {
            //arrange
            var user = new Mock<ClaimsPrincipal>();
            user.Setup(x => x.IsInRole(It.IsAny<string>())).Returns(false);
            var adminsController = new Web.Areas.Administration.Controllers
                .AdminsController(adminService.Object, diaryService.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = user.Object }
                }
            };

            //act
            int id = 1;
            var result = await adminsController.DeleteDiary(id);

            //assert
            Assert.AreEqual(((RedirectToActionResult)result).ActionName, nameof(HomeController.Error));
        }

        [Test]
        public async Task DeleteDiaryShouldRedirectToAllDiaries()
        {
            //arrange
            string username = "UserTests";
            var user = new Mock<ClaimsPrincipal>();
            user.Setup(x => x.IsInRole(It.IsAny<string>())).Returns(true);
            user.Setup(x => x.Identity.Name).Returns(username);

            var adminsController = new Web.Areas.Administration.Controllers
                .AdminsController(adminService.Object, diaryService.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = user.Object }
                }
            };

            //act
            int id = 1;
            var result = await adminsController.DeleteDiary(id);

            //assert
            Assert.AreEqual(((RedirectToActionResult)result)
                .ActionName, nameof(Web.Controllers.DiariesController.AllDiaries));
        }

        [Test]
        public void CurrentTypesShouldReturnSameViewWithEmptyStringOnPost()
        {
            //arrange

            diaryService.Setup(x => x.Types()).Returns(new List<DiaryTypeOutputModel>
            {
                new DiaryTypeOutputModel
                {
                    Id = 1,
                    Type = "Cutting"
                },
                new DiaryTypeOutputModel
                {
                    Id = 2,
                    Type = "Bulking"
                }
            });

            var adminController = new Web.Areas.Administration.Controllers
                .AdminsController(adminService.Object, diaryService.Object);

            //act
            var result = adminController.CurrentTypes(null);

            //assert

            result.As<ViewResult>().ViewData.ContainsKey(GlobalConstants.Error).Equals(GlobalConstants.NullName);
        }

        [Test]
        public void CurrentTypesShouldRedirectBackWhenTypeAlreadyExists()
        {
            //arrange
            diaryService.Setup(x => x.Types()).Returns(new List<DiaryTypeOutputModel>
                {
                    new DiaryTypeOutputModel
                    {
                        Id = 1,
                        Type = "Cutting"
                    },
                    new DiaryTypeOutputModel
                    {
                        Id = 2,
                        Type = "Bulking"
                    }
                });
            string name = "TypeTests";

            adminService.Setup(x => x.AddDiaryType(name)).Returns(GlobalConstants.Failed);

            var adminController = new Web.Areas.Administration.Controllers
                .AdminsController(adminService.Object, diaryService.Object);

            //act

            var result = adminController.CurrentTypes(name);

            //assert
            string expected = string.Join(GlobalConstants.AlreadyExistsInDatabase, name);
            result.As<ViewResult>().ViewData.ContainsKey(GlobalConstants.Error).Equals(expected);
        }

        [Test]
        public void AdminsControllerShouldBeAccessedOnlyByAdministrationRoles()
        {
            var result = typeof(Web.Areas.Administration.Controllers.AdminsController)
                .GetAttribute<AuthorizeAttribute>();

            var roles = $"{GlobalConstants.Admin}";

            Assert.AreEqual(result.Roles, roles);
        }
    }
}
