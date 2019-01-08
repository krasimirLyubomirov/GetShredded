using System;
using System.Linq;
using AutoMapper;
using FluentAssertions;
using GetShredded.Common;
using GetShredded.Models;
using GetShredded.Services.Contracts;
using GetShredded.ViewModel.Input;
using GetShredded.ViewModels.Output.Comment;
using Microsoft.AspNetCore.Identity;
using NUnit.Framework;

namespace GetShredded.Tests.GetShreddedServices.CommentService
{
    [TestFixture]
    public class CommentServiceTests : BaseServiceFake
    {
        private ICommentService CommentService =>
            (ICommentService)this.Provider.GetService(typeof(ICommentService));

        [Test]
        public void AddCommentShouldAddComment()
        {
            //arrange
            var user = new GetShreddedUser
            {
                Id = "UserId",
                UserName = "User"
            };

            var userManager =
                (UserManager<GetShreddedUser>)this.Provider.GetService(typeof(UserManager<GetShreddedUser>));

            userManager.CreateAsync(user).GetAwaiter();
            this.Context.SaveChanges();

            var comment = new CommentInputModel
            {
                DiaryId = 1,
                CommentUser = "User",
                CommentedOn = DateTime.Now.Date,
                Message = "Comment",
            };

            var commentOut = new CommentOutputModel
            {
                Id = 1,
                User = "User",
                CommentedOn = DateTime.Now.Date,
                Message = "Comment",
                DiaryId = comment.DiaryId
            };

            //act
            this.CommentService.AddComment(comment);

            //assert
            var result = this.Context.Comments.First();

            var mappedResult = Mapper.Map<CommentOutputModel>(result);

            mappedResult.Should().BeEquivalentTo(commentOut);
        }

        [Test]
        public void DeleteCommentShouldDeleteComment()
        {
            //arrange

            var user = new GetShreddedUser
            {
                Id = "UserId",
                UserName = "User"
            };

            var type = new DiaryType
            {
                Id = 1,
                Name = "Cutting"
            };

            var diary = new GetShreddedDiary
            {
                Title = "One",
                Id = 1,
                CreatedOn = DateTime.Now,
                Summary = null,
                Type = type,
                UserId = "UserId"
            };

            var comment = new Comment
            {
                GetShreddedDiaryId = 1,
                GetShreddedUser = user,
                GetShreddedDiary = diary,
                CommentedOn = DateTime.Now.Date,
                Message = "Comment",
                GetShreddedUserId = user.Id,
                Id = 1
            };

            var userManager =
                (UserManager<GetShreddedUser>)this.Provider.GetService(typeof(UserManager<GetShreddedUser>));
            userManager.CreateAsync(user).GetAwaiter();
            this.Context.Comments.Add(comment);
            this.Context.DiaryTypes.Add(type);
            this.Context.GetShreddedDiaries.Add(diary);
            this.Context.SaveChanges();

            //act

            this.CommentService.DeleteComment(1);
            var result = this.Context.Comments.ToList();

            //assert

            result.Should().BeEmpty();
        }

        [Test]
        public void DeleteAllCommentsForUserShouldDeleteAll()
        {
            //arrange

            var user = new GetShreddedUser
            {
                Id = "UserId",
                UserName = "User"
            };

            var type = new DiaryType
            {
                Id = 1,
                Name = "Cutting"
            };

            var diary = new GetShreddedDiary
            {
                Title = "One",
                Id = 1,
                CreatedOn = DateTime.Now,
                Summary = null,
                ImageUrl = GlobalConstants.DefaultNoImage,
                Type = type,
                UserId = "UserId"
            };

            var comments = new[]
            {
                new Comment
                {
                    GetShreddedDiaryId = 1,
                    GetShreddedUser = user,
                    GetShreddedDiary = diary,
                    CommentedOn = DateTime.Now.Date,
                    Message = "Comment",
                    GetShreddedUserId = user.Id,
                    Id = 1
                },

                new Comment
                {
                    GetShreddedDiaryId = 1,
                    GetShreddedUser = user,
                    GetShreddedDiary = diary,
                    CommentedOn = DateTime.Now.Date,
                    Message = "CommentTwo",
                    GetShreddedUserId = user.Id,
                    Id = 2
                }
        };

            var userManager = 
                (UserManager<GetShreddedUser>)this.Provider.GetService(typeof(UserManager<GetShreddedUser>));
            userManager.CreateAsync(user).GetAwaiter();
            this.Context.Comments.AddRange(comments);
            this.Context.DiaryTypes.Add(type);
            this.Context.GetShreddedDiaries.Add(diary);
            this.Context.SaveChanges();

            //act

            string username = user.UserName;
            this.CommentService.DeleteAllComments(username);

            //assert
            var result = this.Context.Comments.ToList();
            result.Should().BeEmpty();
        }
    }
}
