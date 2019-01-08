using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using NUnit.Framework;

namespace GetShredded.Tests.GetShreddedControllers.CommentsController
{
    [TestFixture]
    public class CommentsControllerTests
    {
        [Test]
        public void ControllerShouldHaveAuthorizeAttribute()
        {
            typeof(Web.Controllers.CommentsController).Should().BeDecoratedWith<AuthorizeAttribute>();
        }
    }
}
