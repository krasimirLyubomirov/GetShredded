using GetShredded.ViewModel.Input.Page;
using GetShredded.ViewModel.Output.Page;

namespace GetShredded.Services.Contracts
{
    public interface IPageService
    {
        void DeletePage(int storyId, int chapterid, string username);

        void AddPage(PageInputModel inputModel);

        PageEditModel GetPageToEditById(int id);

        void EditPage(PageEditModel editModel);
    }
}
