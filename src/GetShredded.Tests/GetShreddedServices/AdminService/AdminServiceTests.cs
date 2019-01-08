using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using GetShredded.Common;
using GetShredded.Models;
using GetShredded.Services.Contracts;
using GetShredded.ViewModel.Output.Users;
using GetShredded.ViewModels.Output.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace GetShredded.Tests.GetShreddedServices.AdminService
{
    [TestFixture]
    public class AdminServiceTests : BaseServiceFake
    {
        private UserManager<GetShreddedUser> userManager => this.Provider.GetRequiredService<UserManager<GetShreddedUser>>();
        private IAdminService adminService => this.Provider.GetRequiredService<IAdminService>();
        private RoleManager<IdentityRole> roleManager => this.Provider.GetRequiredService<RoleManager<IdentityRole>>();

        [Test]
        public void AllUsers_Should_Return_Correct_Info_Per_User()
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
                User = null,
                Summary = "Summary",
                Title = "Title",
                UserId = firstUser.Id,
            };

            var comments = new[]
            {
                new Comment
                {
                    GetShreddedUser= firstUser,
                    Id=1,
                    GetShreddedUserId= firstUser.Id,
                    GetShreddedDiaryId= diary.Id,
                    GetShreddedDiary= diary,
                    Message="FirstMessage"
                },
                new Comment
                {
                    GetShreddedUser=firstUser,
                    Id=2,
                    GetShreddedUserId=firstUser.Id,
                    GetShreddedDiaryId=diary.Id,
                    GetShreddedDiary=diary,
                    Message="SecondMessage"
                },
            };

            var messages = new[]
            {
                new Message
                {
                    Id=1,
                    IsReaded= false,
                    Receiver=firstUser,
                    ReceiverId=firstUser.Id,
                    Sender=secondUser,
                    SenderId=secondUser.Id
                },
                new Message
                {
                    Id=2,
                    IsReaded=true,
                    Receiver=firstUser,
                    ReceiverId=firstUser.Id,
                    Sender=secondUser,
                    SenderId=secondUser.Id
                },
                new Message
                {
                    Id=3,
                    IsReaded=false,
                    Receiver=secondUser,
                    ReceiverId=secondUser.Id,
                    Sender=firstUser,
                    SenderId=firstUser.Id
                },
                new Message
                {
                    Id=4,
                    IsReaded=true,
                    Receiver=secondUser,
                    ReceiverId=secondUser.Id,
                    Sender=firstUser,
                    SenderId=firstUser.Id
                }
            };

            this.userManager.CreateAsync(secondUser).GetAwaiter();
            this.userManager.CreateAsync(firstUser).GetAwaiter();
            this.Context.GetShreddedDiaries.Add(diary);
            this.Context.Comments.AddRange(comments);
            this.Context.Messages.AddRange(messages);
            this.Context.SaveChanges();

            //act
            var result = this.adminService.AllUsers().GetAwaiter().GetResult();

            int indexToTakeFrom = 0;
            var userToCompare = result?.ElementAt(indexToTakeFrom);
            //assert
            int totalUserDiaries = 1;

            var expectedAuthorOutput = new UserAdminOutputModel
            {
                Id = firstUser.Id,
                Comments = comments.Length,
                MessageCount = messages.Length,
                Username = firstUser.UserName,
                Diaries = totalUserDiaries,
                Role = GlobalConstants.DefaultRole
            };

            userToCompare.Should().NotBeNull()
           .And.Subject.Should().BeEquivalentTo(expectedAuthorOutput);
        }

        [Test]
        public void DeleteUserShouldDeleteUserByGivenId()
        {
            //arrange

            var user = new GetShreddedUser
            {
                Id = "UserIdTests",
                UserName = "UserTests",
                FirstName = "UserFirstNameTests",
                LastName = "UserLastNameTests",
            };

            var secondUser = new GetShreddedUser
            {
                Id = "SecondUserIdTests",
                UserName = "SecondUserTests",
                FirstName = "SecondUserFirstNameTests",
                LastName = "SecondUserLastNameTests",
            };

            this.userManager.CreateAsync(user).GetAwaiter();
            this.userManager.CreateAsync(secondUser).GetAwaiter();
            this.Context.SaveChanges();

            //act
            string userToDeleteId = user.Id;
            this.adminService.DeleteUser(userToDeleteId);

            //assert
            var users = this.Context.Users.ToList();

            users.Should().ContainSingle().And.Subject.Should().NotContain(user);
        }

        [Test]
        public void DeleteUserShouldThrowArgumentException()
        {
            //arrange

            var user = new GetShreddedUser
            {
                Id = "userId",
                UserName = "UserTests",
                FirstName = "UserFirstNameTests",
                LastName = "UserLastNameTests",
            };

            this.userManager.CreateAsync(user).GetAwaiter();
            this.Context.SaveChanges();

            //act
            string userToDeleteId = "RandomIdTests";
            Func<Task> act = async () => await this.adminService.DeleteUser(userToDeleteId);

            //assert
            act.Should().Throw<ArgumentException>().WithMessage(GlobalConstants.ErrorOnDeleteUser);
        }

        [Test]
        public void AddTypeShouldAddNewType()
        {
            //arrange

            var types = new[]
            {
                new DiaryType
                {
                    Id = 2,
                    Name = "Cutting"
                },
                new DiaryType
                {
                    Id = 3,
                    Name = "Bulking"
                }
            };
            this.Context.DiaryTypes.AddRange(types);
            this.Context.SaveChanges();

            //act

            string newType = "Shredded";
            this.adminService.AddDiaryType(newType);

            //assert
            int expectedCount = 3;

            var existingGenres = this.Context.DiaryTypes.ToList();
            existingGenres.Should().NotBeEmpty()
                .And.HaveCount(expectedCount)
                .And.Subject.Select(x => x.Name)
                .Should().Contain(newType);
        }

        [Test]
        public void AddTypeShouldFailIfTheNewTypeAlreadyExists()
        {
            //arrange

            var types = new[]
            {
                new DiaryType
                {
                    Id = 2,
                    Name = "Cutting"
                },
                new DiaryType
                {
                    Id = 3,
                    Name = "Bulking"
                }
            };
            this.Context.DiaryTypes.AddRange(types);
            this.Context.SaveChanges();

            //act

            string newType = "Cutting";
            string result = this.adminService.AddDiaryType(newType);

            //assert
            result.Should().NotBeNull().And.Subject.Should().Equals(GlobalConstants.Failed);
        }

        [Test]
        public void ChangeRoleShouldFailWithNonExistantRole()
        {
            //arrange
            var roles = new[]
            {
                "admin",
                "user"
            };

            foreach (var currentRolename in roles)
            {
                var role = new IdentityRole
                {
                    Name = currentRolename
                };

                this.roleManager.CreateAsync(role).GetAwaiter();
            }

            var user = new GetShreddedUser
            {
                Id = "UserIdTests",
                UserName = "UserTests",
                FirstName = "UserFirstNameTests",
                LastName = "UserLastNameTests",
            };

            this.userManager.CreateAsync(user).GetAwaiter();
            this.Context.SaveChanges();

            //act
            string newRole = "RoleTests";

            var model = new ChangeRoleModel
            {
                Id = user.Id,
                ApplicationRoles = roles,
                UpdatedRole = newRole,
                Username = user.UserName,
                Role = GlobalConstants.DefaultRole
            };

            var methodResult = this.adminService.ChangeRole(model);

            //assert
            methodResult.Should().Equals(IdentityResult.Failed());
        }

        [Test]
        public void ChangeRoleSucceedRole()
        {
            //arrange
            var roles = new[]
            {
                "admin",
                "user"
            };

            foreach (var currentRolename in roles)
            {
                var role = new IdentityRole
                {
                    Name = currentRolename
                };

                this.roleManager.CreateAsync(role).GetAwaiter();
            }

            var user = new GetShreddedUser
            {
                Id = "UserIdTests",
                UserName = "UserTests",
                FirstName = "UserFirstNameTests",
                LastName = "UserLastNameTests",
            };

            this.userManager.CreateAsync(user).GetAwaiter();
            this.Context.SaveChanges();

            //act
            string newRole = roles[0];

            var model = new ChangeRoleModel
            {
                Id = user.Id,
                ApplicationRoles = roles,
                UpdatedRole = newRole,
                Username = user.UserName,
                Role = GlobalConstants.DefaultRole
            };
            var methodResult = this.adminService.ChangeRole(model);

            //assert
            methodResult.Should().Equals(IdentityResult.Failed());
        }
    }
}
