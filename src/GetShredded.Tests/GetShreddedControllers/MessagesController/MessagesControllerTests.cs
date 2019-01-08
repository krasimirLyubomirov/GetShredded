using System;
using FluentAssertions;
using GetShredded.Common;
using GetShredded.Services.Contracts;
using GetShredded.ViewModel.Input;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using NUnit.Framework;

namespace GetShredded.Tests.GetShreddedControllers.MessagesController
{
    [TestFixture]
    public class MessagesControllerTests
    {
        [Test]
        public void ControllerShouldHaveAuthorizeAttribute()
        {
            typeof(Web.Controllers.MessagesController).Should().BeDecoratedWith<AuthorizeAttribute>();
        }

        [Test]
        public void MessagesControllerShouldReturnViewWithErrorIfMessageContentIsEmpty()
        {
            var messageService = new Mock<IMessageService>();

            var message = new MessageInputModel
            {
                Message = null,
                ReceiverName = "ReceiverTests",
                SenderName = "SenderTests",
                SendDate = DateTime.Now
            };

            var httpContext = new DefaultHttpContext();

            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>())
            {
                [GlobalConstants.Error] = GlobalConstants.EmptyMessage
            };

            var controller = new Web.Controllers.MessagesController(messageService.Object)
            {
                TempData = tempData
            };

            var result = controller.SendMessage(message);

            string action = "Profile";
            result.Should().BeOfType<RedirectToActionResult>().Which.ActionName.Should().Be(action);
        }
    }
}
