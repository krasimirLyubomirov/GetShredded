using System.Threading.Tasks;
using GetShredded.Common;
using GetShredded.Services.Contracts;
using GetShredded.ViewModel.Output.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GetShredded.Web.Areas.Administration.Controllers
{
    [Area(GlobalConstants.Administration)]
	[Authorize(Roles = "admin")]
	public class AdminsController : Controller
	{
		public AdminsController(IAdminService adminService, IDiaryService diaryService)
		{
			this.AdminService = adminService;
			this.DiaryService = diaryService;
		}

		protected IAdminService AdminService { get; }
		protected IDiaryService DiaryService { get; }

		[HttpGet]
		[Authorize(Roles = GlobalConstants.Admin)]
		public IActionResult AllUsers()
		{
			var model = this.AdminService.AllUsers().Result;
			return View(model);
		}

		[HttpGet]
		public IActionResult AllDiaries()
		{
			var model = this.DiaryService.CurrentDiaries(null);
			return View(model);
		}

		[HttpGet]
		[Authorize(Roles = GlobalConstants.Admin)]
		public async Task<IActionResult> DeleteUser(string id)
		{
			await this.AdminService.DeleteUser(id);
			return RedirectToAction("AllUsers");
		}

		[HttpGet]
		[Authorize(Roles = GlobalConstants.Admin)]
		public IActionResult EditRole(string id)
		{
			var model = this.AdminService.AdminUpdateRole(id);
			return View(model);
		}

		[HttpPost]
		[Authorize(Roles = GlobalConstants.Admin)]
		public IActionResult EditRole(ChangeRoleModel inputModel)
		{
			var result = this.AdminService.ChangeRole(inputModel).Result;

			if (result == IdentityResult.Success)
			{
				return RedirectToAction(nameof(AllUsers));
			}

			this.ViewData[GlobalConstants.Error] = GlobalConstants.RoleChangeError;
			return this.EditRole(inputModel.Id);
		}

		[HttpGet]
		public async Task<IActionResult> DeleteDiary(int Id)
		{
			if (!this.User.IsInRole(GlobalConstants.Admin))
			{
				return RedirectToAction("Error", "Home", "");
			}

			string username = this.User.Identity.Name;
			await this.DiaryService.DeleteDiary(Id, username);
			return RedirectToAction(nameof(AllDiaries));
		}

		[HttpGet]
		public IActionResult CurrentTypes()
		{
			var model = this.DiaryService.Types();
			return this.View(model);
		}

		[HttpPost]
		public IActionResult CurrentTypes(string name)
		{
			var types = this.DiaryService.Types();

			if (string.IsNullOrEmpty(name))
			{
				this.ViewData[GlobalConstants.Error] = GlobalConstants.NullName;
				return this.View(types);
			}

			var result = this.AdminService.AddDiaryType(name);

			if (result != GlobalConstants.Success)
			{
				this.ViewData[GlobalConstants.Error] = string.Join(GlobalConstants.EntityAlreadyExists, name);
				return this.View(types);
			}

			return RedirectToAction(nameof(CurrentTypes));
		}
	}
}
