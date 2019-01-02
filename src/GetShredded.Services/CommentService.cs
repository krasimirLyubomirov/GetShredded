using System.Linq;
using AutoMapper;
using GetShredded.Data;
using GetShredded.Models;
using GetShredded.Services.Contracts;
using GetShredded.ViewModel.Input;
using Microsoft.AspNetCore.Identity;

namespace GetShredded.Services
{
    public class CommentService : BaseService, ICommentService
    {
        public CommentService(
            UserManager<GetShreddedUser> userManager,
            GetShreddedContext context,
            IMapper mapper)
            : base(userManager, context, mapper)
        {
        }

        public void AddComment(CommentInputModel inputModel)
        {
            var user = this.UserManager.FindByNameAsync(inputModel.CommentUser).GetAwaiter().GetResult();

            var comment = Mapper.Map<Comment>(inputModel);
            comment.GetShreddedUser = user;

            this.Context.Comments.Add(comment);
            user.Comments.Add(comment);

            this.Context.SaveChanges();
        }

        public void DeleteComment(int id)
        {
            var comment = this.Context.Comments.Find(id);
            this.Context.Comments.Remove(comment);
            this.Context.SaveChanges();
        }

        public void DeleteAllComments(string username)
        {
            var comments = this.Context.Comments.Where(x => x.GetShreddedUser.UserName == username).ToList();

            this.Context.Comments.RemoveRange(comments);

            this.Context.SaveChanges();
        }
    }
}
