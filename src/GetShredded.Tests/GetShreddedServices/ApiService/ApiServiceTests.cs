using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using GetShredded.Common;
using GetShredded.Models;
using GetShredded.Services.Contracts;
using GetShredded.ViewModels.Output.Api;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace GetShredded.Tests.GetShreddedServices.ApiService
{
    [TestFixture]
    public class ApiServiceTests : BaseServiceFake
    {
        public IApiService ApiService => this.Provider.GetRequiredService<IApiService>();

        public UserManager<GetShreddedUser> UserManager =>
            this.Provider.GetRequiredService<UserManager<GetShreddedUser>>();

        [Test]
        public void UsersShouldReturnOnlyUsersWithDiaries()
        {
            //arrange

            var firstUser = new GetShreddedUser
            {
                UserName = "FirstUser",
            };

            var secondUser = new GetShreddedUser
            {
                UserName = "SecondUser",
            };

            this.UserManager.CreateAsync(firstUser).GetAwaiter();
            this.UserManager.CreateAsync(secondUser).GetAwaiter();

            var diaries = new GetShreddedDiary[]
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

                    UserId = secondUser.Id
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

                    UserId = secondUser.Id
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

                    UserId = "3333"
                },
            };

            this.Context.GetShreddedDiaries.AddRange(diaries);
            this.Context.SaveChanges();

            //act

            var result = this.ApiService.Users();

            //assert

            result.Should().NotBeEmpty().And.HaveCount(1)
                .And.Subject.As<IEnumerable<ApiUserOutputModel>>().First()
                .Username.Should().Be(secondUser.UserName);

            result.First()
                 .Diaries.Should().NotBeEmpty()
                 .And.HaveCount(2).And.BeOfType<List<ApiGetShreddedDiaryOutputModel>>();
        }

        [Test]
        public void DiariesShouldReturnAllDiariesFromDatabase()
        {
            //arrange
            var firstUser = new GetShreddedUser
            {
                UserName = "FirstUser",
            };

            var secondUser = new GetShreddedUser
            {
                UserName = "SecondUser",
            };

            this.UserManager.CreateAsync(firstUser).GetAwaiter();
            this.UserManager.CreateAsync(secondUser).GetAwaiter();

            var diaries = new GetShreddedDiary[]
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

                    UserId = secondUser.Id
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

                    UserId = secondUser.Id
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

                    UserId = firstUser.Id
                },
            };

            this.Context.GetShreddedDiaries.AddRange(diaries);
            this.Context.SaveChanges();
            //act

            var result = this.ApiService.Diaries();

            //assert

            int count = diaries.Length;
            result.Should().NotBeEmpty()
                .And.HaveCount(count)
                .And.AllBeOfType<ApiGetShreddedDiaryOutputModel>();
        }

        [Test]
        public void DiariesByTypeShouldReturnOnlyDiariesWithType()
        {
            //arrange
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

                    UserId = "11111"
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

                    UserId = "22222"
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
            string type = "Cutting";
            var result = this.ApiService.DiariesByType(type);

            //assert
            result.Should().ContainSingle()
                .And.Subject.First().Type.Should().Be(type);
        }

        [Test]
        public void DiariesByTypeShouldThrowExceptionWithNoExistingType()
        {
            //arrange
            var diaries = new[]
            {
                new GetShreddedDiary
                {
                    Title = "One",
                    Id = 1,
                    CreatedOn = DateTime.Now,
                    Summary = null,
                    ImageUrl = GlobalConstants.DefaultNoImage,
                    Type = new DiaryType
                    {
                        Id = 1,
                        Name = "Cutting"
                    },

                    UserId = "11111"
                },

                new GetShreddedDiary
                {
                    Title = "Two",
                    Id = 2,
                    CreatedOn = DateTime.Now.AddMonths(1),
                    Summary = null,
                    ImageUrl = GlobalConstants.DefaultNoImage,
                    Type = new DiaryType
                    {
                        Id = 2,
                        Name = "Bulking"
                    },

                    UserId = "22222"
                },

                new GetShreddedDiary
                {
                    Title = "Three",
                    Id = 3,
                    CreatedOn = DateTime.Now.AddDays(2),
                    Summary = null,
                    ImageUrl = GlobalConstants.DefaultNoImage,
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
            string type = "NotInForm";
            Func<IEnumerable<ApiGetShreddedDiaryOutputModel>> act = () => this.ApiService.DiariesByType(type);

            //assert
            string message = string.Join(GlobalConstants.NoSuchType, type);
            act.Should().Throw<ArgumentException>().WithMessage(message);
        }

        [Test]
        public void TopDiariesShouldReturnDiariesOrderedByRating()
        {
            //arrange
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

                    Ratings = new List<GetShreddedRating>
                    {
                        new GetShreddedRating
                        {
                            DiaryRating = new DiaryRating
                            {
                                Rating = 1
                            }
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

                    Ratings = new List<GetShreddedRating>
                    {
                        new GetShreddedRating
                        {
                            DiaryRating = new DiaryRating
                            {
                                Rating = 10
                            }
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

                    UserId = "2222223333",

                    Ratings = new List<GetShreddedRating>
                    {
                        new GetShreddedRating
                        {
                            DiaryRating = new DiaryRating
                            {
                                Rating = 5
                            }
                        }
                    }
                },
            };

            this.Context.GetShreddedDiaries.AddRange(diaries);
            this.Context.SaveChanges();

            //act
            var result = this.ApiService.TopDiaries();

            //assert
            int count = diaries.Length;
            double rating = 10;
            result.Should().NotBeEmpty()
                .And.HaveCount(count)
                .And.Subject.As<IEnumerable<ApiGetShreddedDiaryOutputModel>>()
                .First().Rating.Should().Be(rating);
        }
    }
}
