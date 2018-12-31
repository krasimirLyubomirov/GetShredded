using GetShredded.ViewModel.Input;

namespace GetShredded.Services.Contracts
{
    public interface ICommentService
    {
        void AddComment(CommentInputModel inputModel);

        void DeleteComment(int id);

        void DeleteAllComments(string username);
    }
}
