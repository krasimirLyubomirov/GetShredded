using System;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using GetShredded.Common;
using GetShredded.Data;
using GetShredded.Models;
using GetShredded.Services.Contracts;
using GetShredded.ViewModel.Input.Page;
using GetShredded.ViewModel.Output.Page;
using GetShredded.ViewModels.Output.Page;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GetShredded.Services
{
    public class PageService : BaseService, IPageService
    {
        public PageService(
            UserManager<GetShreddedUser> userManager,
            GetShreddedContext context, 
            IMapper mapper, 
            INotificationService notificationService)
            : base(userManager, context, mapper)
        {
            this.NotificationService = notificationService;
        }

        public INotificationService NotificationService { get; }

        public PageEditModel GetPageToEditById(int id)
        {
            var result = this.Context.Pages
                .Include(x => x.GetShreddedUser)
                .Include(x => x.GetShreddedDiary)
                .ProjectTo<PageEditModel>(Mapper.ConfigurationProvider)
                .FirstOrDefault(x => x.Id == id);

            return result;
        }

        public void DeletePage(int diaryId, int pageId, string username)
        {
            var diary = this.Context.GetShreddedDiaries.Find(diaryId);
            var page = this.Context.Pages.Find(pageId);

            var user = this.UserManager.FindByNameAsync(username).GetAwaiter().GetResult();

            var userRoles = UserManager.GetRolesAsync(user).GetAwaiter().GetResult();

            bool RoleAdmin = userRoles.Any(x => x == GlobalConstants.Admin);
            bool RoleUser = user.Id == diary.UserId;

            if (!RoleAdmin && !RoleUser)
            {
                throw new InvalidOperationException(GlobalConstants.UserHasNoRights + " " + GlobalConstants.NotUser);
            }

            bool noPageInDiary = diary.Pages.All(x => x.Id != page.Id);
            bool diaryIdForThisPage = page.GetShreddedDiaryId != diary.Id;

            if (noPageInDiary || diaryIdForThisPage)
            {
                throw new ArgumentException
                (string.Join(GlobalConstants.NotValidPageDiaryConnection, diary.Id, page.Id));
            }

            this.Context.Pages.Remove(page);
            this.Context.SaveChangesAsync().GetAwaiter().GetResult();
        }

        public void AddPage(PageInputModel inputModel)
        {
            var user = this.UserManager.FindByNameAsync(inputModel.User).GetAwaiter().GetResult();
            var page = Mapper.Map<Page>(inputModel);
            page.UserId = user.Id;
            var diary = this.Context.GetShreddedDiaries.Find(inputModel.DiaryId);
            diary.LastEditedOn = inputModel.CreatedOn;

            this.Context.GetShreddedDiaries.Update(diary);
            this.Context.Pages.Add(page);
            this.Context.SaveChanges();

            this.NotificationService.AddNotification(inputModel.DiaryId, inputModel.User, diary.Title);
        }

        public void EditPage(PageEditModel editModel)
        {
            var chapter = this.Context.Pages.Find(editModel.Id);

            chapter.Content = editModel.Content;
            chapter.Title = editModel.Title;
            chapter.CreatedOn = editModel.CreatedOn;

            this.Context.Pages.Update(chapter);
            this.Context.SaveChanges();
        }
    }
}
