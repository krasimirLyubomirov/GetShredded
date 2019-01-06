using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using GetShredded.Common;
using GetShredded.Data;
using GetShredded.Models;
using GetShredded.Services.Contracts;
using GetShredded.ViewModel.Input;
using GetShredded.ViewModels.Input.Diary;
using GetShredded.ViewModels.Output.Diary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GetShredded.Services
{
    public class DiaryService : BaseService, IDiaryService
    {
        public DiaryService(
            INotificationService notificationService, 
            UserManager<GetShreddedUser> userManager,
            GetShreddedContext context, IMapper mapper)
            : base(userManager, context, mapper)
        {
            this.NotificationService = notificationService;
        }

        protected INotificationService NotificationService { get; }

        public ICollection<DiaryOutputModel> CurrentDiaries(string type = null)
        {
            if (string.IsNullOrEmpty(type) || type == GlobalConstants.All)
            {
                return this.Context.GetShreddedDiaries
                    .ProjectTo<DiaryOutputModel>(Mapper.ConfigurationProvider).ToArray();
            }

            var diaries = this.Context.GetShreddedDiaries
                .Where(x => string.Equals(x.Type.Name, type, StringComparison.CurrentCultureIgnoreCase))
                .ProjectTo<DiaryOutputModel>(Mapper.ConfigurationProvider)
                .ToArray();

            return diaries;
        }

        public ICollection<DiaryOutputModel> UserDiaries(string username)
        {
            var userStories = this.Context.GetShreddedDiaries.Include(x => x.User)
                .Where(x => x.User.UserName.ToLower() == username.ToLower())
                .ProjectTo<DiaryOutputModel>(Mapper.ConfigurationProvider).ToArray();

            return userStories;
        }

        public ICollection<DiaryTypeOutputModel> Types()
        {
            return this.Context.DiaryTypes.ProjectTo<DiaryTypeOutputModel>(Mapper.ConfigurationProvider).ToArray();
        }

        public async Task Follow(string username, int id)
        {
            var user = await this.UserManager.FindByNameAsync(username);

            var userDiary = new GetShreddedUserDiary()
            {
                GetShreddedDiaryId = id,
                GetShreddedUserId = user.Id
            };

            bool followed = IsFollowed(user.Id, id);
            if (followed)
            {
                throw new InvalidOperationException(string.Join(GlobalConstants.AlreadyFollow, user.UserName));
            }

            this.Context.GetShreddedUserDiaries.Add(userDiary);
            await this.Context.SaveChangesAsync();
        }

        public async Task UnFollow(string username, int diaryId)
        {
            var user = await this.UserManager.FindByNameAsync(username);
            var entity = this.Context.GetShreddedUserDiaries
                .Where(st => st.GetShreddedDiaryId == diaryId)
                .Select(st => new GetShreddedUserDiary
                {
                    GetShreddedDiaryId = diaryId,
                    GetShreddedUserId = user.Id
                }).FirstOrDefault();

            if (entity != null)
            {
                this.Context.GetShreddedUserDiaries.Remove(entity);
                await this.Context.SaveChangesAsync();
            }
            else
            {
                throw new ArgumentNullException(GlobalConstants.UserDontFollow);
            }
        }

        public bool IsFollowed(string userId, int id)
        {
            var entity = this.Context.GetShreddedUserDiaries
                .Where(x => x.GetShreddedDiaryId == id)
                .Select(x => new GetShreddedUserDiary
                {
                    GetShreddedDiaryId = id,
                    GetShreddedUserId = userId
                }).FirstOrDefault();

            var result = entity != null;

            return result;
        }

        public async Task<int> CreateDiary(DiaryInputModel inputModel)
        {
            var newDiary = Mapper.Map<GetShreddedDiary>(inputModel);

            newDiary.User = await this.UserManager.FindByNameAsync(inputModel.User);
            newDiary.Type = this.Context.DiaryTypes.First(x => x.Name == inputModel.Type);

            this.Context.GetShreddedDiaries.Add(newDiary);
            await this.Context.SaveChangesAsync();
            return newDiary.Id;
        }

        public DiaryDetailsOutputModel GetDiaryById(int id)
        {
            var diary = this.Context.GetShreddedDiaries
                .Include(x => x.Type)
                .Include(x => x.Pages)
                .Include(x => x.Comments)
                .Include(x => x.User)
                .Include(x => x.Ratings)
                .ThenInclude(z => z.DiaryRating)
                .FirstOrDefault(x => x.Id == id);

            if (diary == null)
            {
                throw new ArgumentException(GlobalConstants.MissingDiary);
            }

            var diaryModel = this.Mapper.Map<DiaryDetailsOutputModel>(diary);

            return diaryModel;
        }

        public void AddRating(int diaryId, double rate, string username)
        {
            var user = this.UserManager.FindByNameAsync(username).GetAwaiter().GetResult();
            var diary = this.Context.GetShreddedDiaries.Find(diaryId);

            bool rated = AlreadyRated(diary.Id, user.UserName);

            if (rated)
            {
                throw new InvalidOperationException(GlobalConstants.AlreadyRated);
            }

            var rating = new DiaryRating
            {
                Rating = rate,
                GetShreddedUserId = user.Id
            };

            this.Context.DiaryRatings.Add(rating);

            var diaryRating = new GetShreddedRating
            {
                GetShreddedDiaryId = diaryId,
                DiaryRatingId = rating.Id
            };
            this.Context.GetShreddedRatings.Add(diaryRating);
            diary.Ratings.Add(diaryRating);
            this.Context.Update(diary);
            this.Context.SaveChanges();
        }

        public bool AlreadyRated(int diaryId, string username)
        {
            var user = this.UserManager.FindByNameAsync(username).GetAwaiter().GetResult();

            var rated = this.Context.GetShreddedRatings
                .Any(x => x.GetShreddedDiaryId == diaryId && x.DiaryRating.GetShreddedUserId == user.Id);

            return rated;
        }

        public ICollection<DiaryOutputModel> FollowedDiaries(string username)
        {
            var diaries = this.Context.GetShreddedDiaries
                .Include(x => x.Followers)
                .Where(x => x.Followers.Any(z => z.GetShreddedUser.UserName == username))
                .ProjectTo<DiaryOutputModel>(Mapper.ConfigurationProvider).ToList();

            return diaries;
        }

        public ICollection<DiaryOutputModel> FollowedDiariesByType(string username, string type)
        {
            var diaries = this.FollowedDiaries(username);

            if (type == GlobalConstants.All)
            {
                return diaries;
            }

            return diaries.Where(x => x.Type.Type == type).ToList();
        }

        public async Task DeleteDiary(int id, string username)
        {
            var diary = this.Context.GetShreddedDiaries.Include(x => x.User)
                .Include(x => x.Pages).FirstOrDefaultAsync(x => x.Id == id).Result;
            var user = await this.UserManager.FindByNameAsync(username);
            var roles = await this.UserManager.GetRolesAsync(user);

            bool hasRights = roles.Any(x => x == GlobalConstants.Admin);
            bool getShreddedUser = user.UserName == diary?.User.UserName;

            if (!hasRights && !getShreddedUser)
            {
                throw new OperationCanceledException(GlobalConstants.UserRights);
            }

            this.Context.GetShreddedDiaries
                .Remove(diary ?? throw new InvalidOperationException(GlobalConstants.WithoutRecord));
            this.Context.SaveChanges();
        }

        private async Task<string> UploadImage(Cloudinary cloudinary, IFormFile fileform, string diaryName)
        {
            if (fileform == null)
            {
                return null;
            }

            byte[] storyImage;

            using (var memoryStream = new MemoryStream())
            {
                await fileform.CopyToAsync(memoryStream);
                storyImage = memoryStream.ToArray();
            }

            var ms = new MemoryStream(storyImage);

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(diaryName, ms),
                Transformation = new Transformation()
                    .Width(200).Height(250).Crop("fit").SetHtmlWidth(250).SetHtmlHeight(100)
            };

            var uploadResult = cloudinary.Upload(uploadParams);

            ms.Dispose();
            return uploadResult.SecureUri.AbsoluteUri;
        }

        private Cloudinary SetCloudinary()
        {
            Account account = new Account(
                GlobalConstants.CloudinaryCloudName,
                GlobalConstants.AccCloudinaryApiKey,
                GlobalConstants.AccCloudinarySecret);

            Cloudinary cloudinary = new Cloudinary(account);

            return cloudinary;
        }
    }
}
