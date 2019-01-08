using System.Security.Claims;
using System.Threading.Tasks;
using GetShredded.Common;
using GetShredded.Services.Contracts;
using GetShredded.ViewModels.Input.Diary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GetShredded.Web.Controllers
{
    [Authorize]
    public class DiariesController : Controller
    {
        public DiariesController(IDiaryService diaryService)
        {
            this.DiaryService = diaryService;
        }

        protected IDiaryService DiaryService { get; }

        [HttpGet]
        public IActionResult AllDiaries()
        {
            var model = this.DiaryService.CurrentDiaries(null);
            return this.View(model);
        }

        [HttpPost]
        public IActionResult AllDiaries(string type)
        {
            var model = this.DiaryService.CurrentDiaries(type);
            return this.View(model);
        }

        [HttpGet]
        [Route(GlobalConstants.Motivation)]
        public IActionResult Motivation(string username)
        {
            var id = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            this.ViewData[GlobalConstants.Username] = username;
            var motivation = this.DiaryService.Motivation(id);
            return this.View(motivation);
        }

        [HttpGet]
        public IActionResult CreateDiary()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateDiary(DiaryInputModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            var id = await this.DiaryService.CreateDiary(inputModel);

            return RedirectToAction("Details", "Diaries", new { id });
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var getShreddedDiary = this.DiaryService.GetDiaryById(id);
            return this.View(getShreddedDiary);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteDiary(int id)
        {
            string username = this.User.Identity.Name;
            await this.DiaryService.DeleteDiary(id, username);
            return RedirectToAction("Motivation", "Diaries", new { username });
        }

        [HttpGet]
        public async Task<IActionResult> Follow(int id)
        {
            var username = this.User.Identity.Name;
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            await this.DiaryService.Follow(username, userId, id);

            return RedirectToAction("Details", "Diaries", new { id });
        }

        [HttpGet]
        public async Task<IActionResult> UnFollow(int id)
        {
            var username = this.User.Identity.Name;
            await this.DiaryService.UnFollow(username, id);
            return RedirectToAction("Details", "Diaries", new { id });
        }

        [HttpPost]
        public IActionResult AddRating([FromForm]int diaryId, [FromForm]double rating)
        {
            string username = this.User.Identity.Name;
            this.DiaryService.AddRating(diaryId, rating, username);
            return RedirectToAction("Details", "Diaries", new { id = diaryId });
        }

        [HttpGet]
        public IActionResult FollowedDiaries()
        {
            string name = this.User.Identity.Name;
            var model = this.DiaryService.FollowedDiaries(name);
            return this.View(model);
        }

        [HttpPost]
        public IActionResult FollowedDiaries(string type)
        {
            string name = this.User.Identity.Name;
            var model = this.DiaryService.FollowedDiariesByType(name, type);
            return this.View(model);
        }

        [HttpGet]
        public IActionResult DeleteAllDiaries(string username)
        {
            this.DiaryService.DeleteAllDiaries(username);
            return RedirectToAction("Index", "Home");
        }
    }
}
