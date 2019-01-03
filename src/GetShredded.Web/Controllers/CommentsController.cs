using GetShredded.Common;
using GetShredded.Services.Contracts;
using GetShredded.ViewModel.Input;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GetShredded.Web.Controllers
{
    [Authorize]
    public class CommentsController : Controller
    {
        public CommentsController(ICommentService commentService)
        {
            this.CommentService = commentService;
        }

        protected ICommentService CommentService { get; set; }

        [HttpGet]
        public IActionResult DeleteComment(int diaryId, int id)
        {
            this.CommentService.DeleteComment(id);

            return RedirectToAction("Details", "Diaries", new { id = diaryId });
        }

        [HttpPost]
        public IActionResult AddComment(CommentInputModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                this.TempData[GlobalConstants.Error] = GlobalConstants.CommentsLength;
                return RedirectToAction("Details", "Diaries", new { id = inputModel.DiaryId });
            }

            this.CommentService.AddComment(inputModel);

            return RedirectToAction("Details", "Diaries", new { id = inputModel.DiaryId });
        }

        [HttpGet]
        public IActionResult DeleteCommentFromInformation(int id)
        {
            this.CommentService.DeleteComment(id);

            return RedirectToInformation();
        }

        [HttpGet]
        public IActionResult DeleteAllComments(string username)
        {
            this.CommentService.DeleteAllComments(username);

            return RedirectToInformation();
        }

        private IActionResult RedirectToInformation()
        {
            var username = this.User.Identity.Name;

            return RedirectToAction("Information", "Messages", new { username });
        }
    }
}
