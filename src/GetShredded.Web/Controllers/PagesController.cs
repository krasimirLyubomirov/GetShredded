using GetShredded.Common;
using GetShredded.Services.Contracts;
using GetShredded.ViewModel.Input.Page;
using GetShredded.ViewModel.Output.Page;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GetShredded.Web.Controllers
{
    [Authorize]
    public class PagesController : Controller
    {
        public PagesController(IPageService pageService)
        {
            this.PageService = pageService;
        }

        public IPageService PageService { get; }

        [HttpPost]
        public IActionResult AddPage(PageInputModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                this.ViewData[GlobalConstants.PageLength] = inputModel.Content?.Length ?? 0;
                this.ViewData[GlobalConstants.DiaryId] = inputModel.DiaryId;
                return this.View(inputModel);
            }

            this.PageService.AddPage(inputModel);

            return RedirectToAction("Details", "Diaries", new { id = inputModel.DiaryId });
        }

        [HttpGet]
        [Route(GlobalConstants.AddPageRoute)]
        public IActionResult AddPage(int diaryId)
        {
            this.ViewData[GlobalConstants.DiaryId] = diaryId;
            return this.View();
        }

        [HttpGet]
        public IActionResult DeletePage([FromQuery]int diaryId, [FromQuery] int pageId)
        {
            string username = this.User.Identity.Name;
            this.PageService.DeletePage(diaryId, pageId, username);

            this.ViewData[GlobalConstants.RedirectAfterAction] = diaryId;
            return RedirectToAction("Details", "Diaries", new { id = diaryId });
        }

        [HttpGet]
        public IActionResult EditPage(int id)
        {
            var model = this.PageService.GetPageToEditById(id);

            if (model == null)
            {
                return NotFound();
            }

            return this.View(model);
        }

        [HttpPost]
        public IActionResult EditPage(PageEditModel editModel)
        {
            if (!ModelState.IsValid)
            {
                this.ViewData[GlobalConstants.PageLength] = editModel.Content?.Length ?? 0;
                return this.View(editModel);
            }

            this.PageService.EditPage(editModel);

            return RedirectToAction("Details", "Diaries", new { id = editModel.DiaryId });
        }
    }
}
