using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using GetShredded.Common;
using GetShredded.Models;
using GetShredded.Services.Contracts;
using GetShredded.ViewModels.Output.Diary;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace GetShredded.Tests.GetShreddedServices.DiaryService
{
    [TestFixture]
    public class DiaryServiceTests : BaseServiceFake
    {
        [Test]
        public void CurrentDiariesWithTypeNullShouldReturnAllDiaries()
        {
            //arrange
            var diaries = new GetShreddedDiary[]
            {
                new GetShreddedDiary
                {
                    Title="One",
                    Id=1,
                    CreatedOn=DateTime.Now,
                    Summary=null,
                    Type=new DiaryType
                    {
                        Id=1,
                        Name="Cutting"
                    },

                    UserId= "1111"
                },

                new GetShreddedDiary
                {
                    Title="Two",
                    Id=2,
                    CreatedOn=DateTime.Now.AddMonths(1),
                    Summary=null,
                    Type=new DiaryType
                    {
                        Id=2,
                        Name="Bulking"
                    },

                    UserId="2222"
                },

                new GetShreddedDiary
                {
                    Title="Three",
                    Id=3,
                    CreatedOn=DateTime.Now.AddDays(2),
                    Summary=null,
                    Type=new DiaryType
                    {
                        Id=3,
                        Name="Shredded"
                    },

                    UserId="3333"
                },
            };
            this.Context.GetShreddedDiaries.AddRange(diaries);
            this.Context.SaveChanges();

            //act

            var diaryServices = GetService();
            var result = diaryServices.CurrentDiaries(null);

            //assert

            result.Should().NotBeEmpty()
                .And.HaveCount(3);
        }

        [Test]
        public void CurrentDiariesWithTypeNullShouldReturnOnlyOneTypeDiaries()
        {
            //arrange
            var diaries = new GetShreddedDiary[]
            {
                new GetShreddedDiary
                {
                    Title="One",
                    Id=1,
                    CreatedOn=DateTime.Now,
                    Summary=null,
                    Type=new DiaryType
                    {
                        Id=1,
                        Name="Cutting"
                    },

                    UserId= "1111"
                },

                new GetShreddedDiary
                {
                    Title="Two",
                    Id=2,
                    CreatedOn=DateTime.Now.AddMonths(1),
                    Summary=null,
                    Type=new DiaryType
                    {
                        Id=2,
                        Name="Bulking"
                    },

                    UserId= "2222"
                },

                new GetShreddedDiary
                {
                    Title="Three",
                    Id=3,
                    CreatedOn=DateTime.Now.AddDays(2),
                    Summary=null,
                    Type=new DiaryType
                    {
                        Id=3,
                        Name="Shredded"
                    },

                    UserId= "3333"
                },
            };

            this.Context.GetShreddedDiaries.AddRange(diaries);
            this.Context.SaveChanges();

            //act
            string diaryType = "Cutting";
            var diaryService = GetService();
            var result = diaryService.CurrentDiaries(diaryType);

            //assert
            result.Should().NotBeEmpty()
                .And.ContainSingle(x => x.Type.Type == "Cutting");
        }

        [Test]
        public void TypesGetCorrectlyTakenFromDatabase()
        {
            //arrange
            var types = new DiaryType[]
            {
                new DiaryType
                {
                    Id=1,
                    Name = "Cutting"
                },

                new DiaryType
                {
                    Id=2,
                    Name="Bulking"
                },

                new DiaryType
                {
                    Id=3,
                    Name="Shredded"
                }
            };

            this.Context.DiaryTypes.AddRange(types);
            this.Context.SaveChanges();

            //act

            var diaryService = GetService();

            var genresFromDatabase = diaryService.Types();

            genresFromDatabase.Should().HaveCount(3);
        }

        [Test]
        public void GetDiaryByIdThrowExceptionwithNonExistantId()
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

                UserId = "1111"
            };

            this.Context.GetShreddedDiaries.Add(diary);
            this.Context.SaveChanges();

            //act
            var diaryService = GetService();
            Action act = () => diaryService.GetDiaryById(2);

            act.Should().Throw<ArgumentException>().WithMessage(GlobalConstants.MissingDiary);
        }

        [Test]
        public void GetDiaryByIdShouldReturnDiaryDetailsModelWithCorrectId()
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

                UserId = "1111"
            };

            this.Context.GetShreddedDiaries.Add(diary);
            this.Context.SaveChanges();

            //act
            var diaryService = GetService();

            var result = diaryService.GetDiaryById(1);

            result.Should().BeOfType<DiaryDetailsOutputModel>().And.Should().NotBeNull();
        }

        [Test]
        public async Task FollowShouldCreateUserDiaryEntityInDatabase()
        {
            //arrange

            var diary = new GetShreddedDiary
            {
                Id = 1,
                User = null,
                Summary = "Summary",
                Title = "Title"
            };

            var user = new GetShreddedUser
            {
                Id = "UserId",
                UserName = "User",
            };
            this.Context.GetShreddedDiaries.Add(diary);

            var usermanager = this.Provider.GetRequiredService<UserManager<GetShreddedUser>>();
            await usermanager.CreateAsync(user);

            this.Context.SaveChanges();

            //act
            var diaryService = GetService();

            var diaryId = diary.Id;
            var username = user.UserName;
            var userId = user.Id;
            await diaryService.Follow(username, userId, diaryId);

            //assert

            var result = this.Context.GetShreddedUserDiaries.FirstOrDefault();

            var userDiary = new GetShreddedUserDiary
            {
                GetShreddedUserId = user.Id,
                GetShreddedDiaryId = diary.Id
            };

            result.Should().BeOfType<GetShreddedUserDiary>().And.Subject.Should().Equals(userDiary);
        }

        [Test]
        public async Task FollowShouldThrowOperationExceptionWhenTryToAddDuplicateEntity()
        {
            //arrange

            var diary = new GetShreddedDiary
            {
                Id = 1,
                User = null,
                Summary = "Summary",
                Title = "Title"
            };

            var user = new GetShreddedUser
            {
                Id = "UserId",
                UserName = "User",
            };

            this.Context.GetShreddedDiaries.Add(diary);

            var usermanager = this.Provider.GetRequiredService<UserManager<GetShreddedUser>>();
            await usermanager.CreateAsync(user);

            this.Context.SaveChanges();

            //act
            var storyService = GetService();

            var diaryId = diary.Id;
            var username = user.UserName;
            var userId = user.Id;
            await storyService.Follow(username, userId, diaryId);

            //assert

            Action act = () => storyService.Follow(username, userId, diaryId).GetAwaiter().GetResult();

            string message = string.Join(GlobalConstants.AlreadyFollow, user.UserName);
            act.Should().Throw<InvalidOperationException>().WithMessage(message);
        }

        [Test]
        public async Task AddRatingShouldAddRatingToADiary()
        {
            //arrange

            var diary = new GetShreddedDiary
            {
                Id = 1,
                User = null,
                Summary = "Summary",
                Title = "Title"
            };

            var user = new GetShreddedUser
            {
                Id = "UserId",
                UserName = "User",
            };

            var usermanager = this.Provider.GetRequiredService<UserManager<GetShreddedUser>>();
            await usermanager.CreateAsync(user);
            this.Context.GetShreddedDiaries.Add(diary);
            this.Context.SaveChanges();
            //act

            var diaryService = GetService();
            diaryService.AddRating(diary.Id, 10, user.UserName);

            //assert

            var diaryRating = new GetShreddedRating
            {
                GetShreddedDiaryId = 1,
                DiaryRatingId = 1
            };

            var result = this.Context.GetShreddedRatings.FirstOrDefault();

            result.Should().NotBeNull().And.
                Subject.Should()
                .BeOfType<GetShreddedRating>().And.Should()
                .BeEquivalentTo(diaryRating, opt => opt.ExcludingMissingMembers());
        }

        [Test]
        public void AddRatingShouldThrowExceptionIfAlreadyRatedIsTrue()
        {
            //arrange
            var diary = new GetShreddedDiary
            {
                Id = 1,
                User = null,
                Summary = "Summary",
                Title = "Title"
            };

            var user = new GetShreddedUser
            {
                Id = "UserId",
                UserName = "User",
            };

            var rating = new DiaryRating
            {
                Rating = 8,
                GetShreddedUserId = user.Id,
                Id = 1,
                GetShreddedUser = user
            };

            var diaryRating = new GetShreddedRating
            {
                GetShreddedDiaryId = diary.Id,
                DiaryRatingId = rating.Id,
                GetShreddedDiary = diary,
                DiaryRating = rating
            };

            var usermanager = this.Provider.GetRequiredService<UserManager<GetShreddedUser>>();
            usermanager.CreateAsync(user).GetAwaiter();

            this.Context.GetShreddedDiaries.Add(diary);
            this.Context.DiaryRatings.Add(rating);
            this.Context.GetShreddedRatings.Add(diaryRating);
            this.Context.SaveChanges();

            //act

            var diaryService = GetService();

            Action act = () => diaryService.AddRating(diary.Id, 1, user.UserName);

            //assert
            act.Should().Throw<InvalidOperationException>().WithMessage(GlobalConstants.AlreadyRated);
        }

        [Test]
        public void DeleteDiaryShouldBeSuccessfulForAdmin()
        {
            //arrange
            var firstUser = new GetShreddedUser
            {
                Id = "FirstId",
                UserName = "First",
            };
            var secondUser = new GetShreddedUser
            {
                Id = "SecondId",
                UserName = "Second",
            };

            var diary = new GetShreddedDiary
            {
                Id = 1,
                User = null,
                Summary = "Summary",
                Title = "Title",
                UserId = secondUser.Id,
            };

            var role = new IdentityRole { Name = "admin" };

            var usermanager = this.Provider.GetRequiredService<UserManager<GetShreddedUser>>();
            var roleManager = this.Provider.GetRequiredService<RoleManager<IdentityRole>>();

            usermanager.CreateAsync(secondUser).GetAwaiter();
            usermanager.CreateAsync(firstUser).GetAwaiter();
            roleManager.CreateAsync(role).GetAwaiter();

            usermanager.AddToRoleAsync(firstUser, "admin").GetAwaiter();

            this.Context.GetShreddedDiaries.Add(diary);
            this.Context.SaveChanges();

            //act
            var diaryService = GetService();
            diaryService.DeleteDiary(diary.Id, firstUser.UserName).GetAwaiter();

            //assert

            var result = this.Context.GetShreddedDiaries.FirstOrDefault();

            result.Should().BeNull();
        }

        [Test]
        public void DeleteDiaryShouldThrowExceptionWithAdminAndUserBoolsFalse()
        {
            //arrange
            var firstUser = new GetShreddedUser
            {
                Id = "FirstId",
                UserName = "First",
            };
            var secondUser = new GetShreddedUser
            {
                Id = "SecondId",
                UserName = "Second",
            };

            var diary = new GetShreddedDiary
            {
                Id = 1,
                User = null,
                Summary = "Summary",
                Title = "Title",
                UserId = secondUser.Id,
            };

            var usermanager = this.Provider.GetRequiredService<UserManager<GetShreddedUser>>();
            usermanager.CreateAsync(firstUser).GetAwaiter();
            usermanager.CreateAsync(secondUser).GetAwaiter();

            this.Context.GetShreddedDiaries.Add(diary);
            this.Context.SaveChanges();

            //act
            var diaryService = GetService();
            Func<Task> act = async () => await diaryService.DeleteDiary(diary.Id, firstUser.UserName);

            //assert

            act.Should().Throw<OperationCanceledException>();
        }

        [Test]
        public void DeleteDiaryShouldThrowExceptionForNullNotExistingDiaryOnDelete()
        {
            //arrange
            var firstUser = new GetShreddedUser
            {
                Id = "FirstId",
                UserName = "First",
            };
            var secondUser = new GetShreddedUser
            {
                Id = "SecondId",
                UserName = "Second",
            };

            var role = new IdentityRole { Name = "admin" };

            var usermanager = this.Provider.GetRequiredService<UserManager<GetShreddedUser>>();
            var roleManager = this.Provider.GetRequiredService<RoleManager<IdentityRole>>();

            usermanager.CreateAsync(secondUser).GetAwaiter();
            usermanager.CreateAsync(firstUser).GetAwaiter();
            roleManager.CreateAsync(role).GetAwaiter();

            usermanager.AddToRoleAsync(firstUser, "admin").GetAwaiter();
            this.Context.SaveChanges();

            //act
            var diaryService = GetService();
            Func<Task> act = async () => await diaryService.DeleteDiary(1, firstUser.UserName);

            //assert

            act.Should().ThrowAsync<InvalidOperationException>().Wait();
        }
        
        [Test]
        public void FollowedDiariesShouldReturnOnlyDiariesUserIsFollowing()
        {
            //arrange
            var user = new GetShreddedUser
            {
                UserName = "Follower"
            };

            var userManager = this.Provider.GetRequiredService<UserManager<GetShreddedUser>>();
            userManager.CreateAsync(user).GetAwaiter();
            var diaryService = GetService();
            var diaries = new[]
            {
                    new GetShreddedDiary
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
                        UserId = "11111",
                        Followers = new List<GetShreddedUserDiary>
                        {
                            new GetShreddedUserDiary
                            {
                                GetShreddedDiaryId = 1,
                                GetShreddedUser = user,
                            }
                        }
                    },

                    new GetShreddedDiary
                    {
                        Title = "Two",
                        Id = 2,
                        CreatedOn = DateTime.Now.AddMonths(1),
                        Summary = null,
                        Type = new DiaryType
                        {
                            Id = 2,
                            Name = "Bulking"
                        },
                        UserId = "22222",
                        Followers = new List<GetShreddedUserDiary>
                        {
                            new GetShreddedUserDiary
                            {
                                GetShreddedDiaryId = 2,
                                GetShreddedUser = user,
                            }
                        }
                    },

                    new GetShreddedDiary
                    {
                        Title = "Three",
                        Id = 3,
                        CreatedOn = DateTime.Now.AddDays(2),
                        Summary = null,
                        Type = new DiaryType
                        {
                            Id = 3,
                            Name = "Shredded"
                        },
                        UserId = "2222223333"
                    },
                };

            this.Context.GetShreddedDiaries.AddRange(diaries);
            this.Context.SaveChanges();

            //act
            string name = user.UserName;
            var result = diaryService.FollowedDiaries(name);

            //assert
            int count = diaries.Length - 1;
            string username = user.UserName;
            result.Should().NotBeEmpty().And.HaveCount(count)
                .And.Subject.As<IEnumerable<DiaryOutputModel>>()
                .SelectMany(x => x.Followers.Select(u => u.Username))
                .Should().OnlyContain(x => x == username);
        }

        [Test]
        public void FollowedDiariesByTypeShouldReturnOnlyDiariesUserIsFollowingAndTypeSelected()
        {
            //arrange
            var user = new GetShreddedUser
            {
                UserName = "Follower"
            };

            var userManager = this.Provider.GetRequiredService<UserManager<GetShreddedUser>>();
            userManager.CreateAsync(user).GetAwaiter();
            var diaryService = GetService();
            var diaries = new[]
            {
                    new GetShreddedDiary
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
                        UserId = "11111",
                        Followers = new List<GetShreddedUserDiary>
                        {
                            new GetShreddedUserDiary
                            {
                                GetShreddedDiaryId = 1,
                                GetShreddedUser = user,
                            }
                        }
                    },

                    new GetShreddedDiary
                    {
                        Title = "Two",
                        Id = 2,
                        CreatedOn = DateTime.Now.AddMonths(1),
                        Summary = null,
                        Type = new DiaryType
                        {
                            Id = 2,
                            Name = "Bulking"
                        },
                        UserId = "22222",
                        Followers = new List<GetShreddedUserDiary>
                        {
                            new GetShreddedUserDiary
                            {
                                GetShreddedDiaryId = 2,
                                GetShreddedUser = user,
                            }
                        }
                    },

                    new GetShreddedDiary
                    {
                        Title = "Three",
                        Id = 3,
                        CreatedOn = DateTime.Now.AddDays(2),
                        Summary = null,
                        Type = new DiaryType
                        {
                            Id = 3,
                            Name = "Shredded"
                        },
                        UserId = "2222223333"
                    },
                };

            this.Context.GetShreddedDiaries.AddRange(diaries);
            this.Context.SaveChanges();

            //act
            string name = user.UserName;
            string type = "Cutting";
            var result = diaryService.FollowedDiariesByType(name, type);

            //assert

            string username = user.UserName;
            result.Should().ContainSingle()
                .And.Subject.As<IEnumerable<DiaryOutputModel>>()
                .SelectMany(x => x.Followers.Select(u => u.Username))
                .Should().OnlyContain(x => x == username);
        }

        [Test]
        public void FollowedDiariesByTypeShouldReturnOnlyDiariesUserIsFollowingAndAllTypes()
        {
            //arrange
            var user = new GetShreddedUser
            {
                UserName = "Follower"
            };

            var userManager = this.Provider.GetRequiredService<UserManager<GetShreddedUser>>();
            userManager.CreateAsync(user).GetAwaiter();
            var diaryService = GetService();
            var diaries = new[]
            {
                    new GetShreddedDiary
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
                        UserId = "11111",
                        Followers = new List<GetShreddedUserDiary>
                        {
                            new GetShreddedUserDiary
                            {
                                GetShreddedDiaryId = 1,
                                GetShreddedUser = user,
                            }
                        }
                    },

                    new GetShreddedDiary
                    {
                        Title = "Two",
                        Id = 2,
                        CreatedOn = DateTime.Now.AddMonths(1),
                        Summary = null,
                        Type = new DiaryType
                        {
                            Id = 2,
                            Name = "Bulking"
                        },
                        UserId = "22222",
                        Followers = new List<GetShreddedUserDiary>
                        {
                            new GetShreddedUserDiary
                            {
                                GetShreddedDiaryId = 2,
                                GetShreddedUser = user,
                            }
                        }
                    },

                    new GetShreddedDiary
                    {
                        Title = "Three",
                        Id = 3,
                        CreatedOn = DateTime.Now.AddDays(2),
                        Summary = null,
                        Type = new DiaryType
                        {
                            Id = 3,
                            Name = "Shredded"
                        },
                        UserId = "2222223333"
                    },
                };

            this.Context.GetShreddedDiaries.AddRange(diaries);
            this.Context.SaveChanges();

            //act
            string name = user.UserName;
            string type = GlobalConstants.All;
            var result = diaryService.FollowedDiariesByType(name, type);

            //assert
            int count = diaries.Length - 1;
            string username = user.UserName;
            result.Should().NotBeEmpty().And.HaveCount(count)
                .And.Subject.As<IEnumerable<DiaryOutputModel>>()
                .SelectMany(x => x.Followers.Select(u => u.Username))
                .Should().OnlyContain(x => x == username);
        }

        private IDiaryService GetService()
        {
            return this.Provider.GetRequiredService<IDiaryService>();
        }
    }
}
