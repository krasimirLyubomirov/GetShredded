using System.Security.Claims;
using System.Threading.Tasks;
using GetShredded.Common;
using GetShredded.Services.Contracts;
using GetShredded.ViewModel.Input;
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
        [Route(GlobalConstants.UserDiaries)]
        public IActionResult UserDiaries(string username)
        {
            var id = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            this.ViewData[GlobalConstants.Username] = username;
            var userDiaries = this.DiaryService.UserDiaries(id);
            return this.View(userDiaries);
        }

        [HttpGet]
        public IActionResult CreateDiary()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateDiary(DiaryInputModel inputModel)
        {
            bool haveNotImage = inputModel.StoryImage != null;
            bool wrongType = false;

            if (haveNotImage)
            {
                var fileType = inputModel.StoryImage.ContentType.Split('/')[1];

                wrongType = fileType == GlobalConstants.Jpg || fileType == GlobalConstants.Png;
            }

            if (!ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            if (!wrongType && haveNotImage)
            {
                this.ViewData[GlobalConstants.Error] = GlobalConstants.WrongImageType;
                return this.View(inputModel);
            }

            var id = await this.DiaryService.CreateDiary(inputModel);

            return RedirectToAction("Details", "Diaries", new { id });
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var fictionStory = this.DiaryService.GetDiaryById(id);
            return this.View(fictionStory);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteDiary(int id)
        {
            string username = this.User.Identity.Name;
            await this.DiaryService.DeleteDiary(id, username);
            return RedirectToAction("UserDiaries", "Diaries", new { username });
        }

        [HttpGet]
        public async Task<IActionResult> Follow(int id)
        {
            var username = this.User.Identity.Name;
            await this.DiaryService.Follow(username, id);

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
    }
}
