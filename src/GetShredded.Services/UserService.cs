using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using GetShredded.Common;
using GetShredded.Data;
using GetShredded.Models;
using GetShredded.Services.Contracts;
using GetShredded.ViewModel.Input.Users;
using GetShredded.ViewModel.Output.Users;
using GetShredded.ViewModels.Output.Diary;
using GetShredded.ViewModels.Output.Home;
using GetShredded.ViewModels.Output.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GetShredded.Services
{
    public class UserService : BaseService, IUserService
    {
        public UserService(
            UserManager<GetShreddedUser> userManager,
            SignInManager<GetShreddedUser> signInManager,
            GetShreddedContext context,
            IMapper mapper)
            : base(userManager, context, mapper)
        {
            this.SignInManager = signInManager;
        }

        protected SignInManager<GetShreddedUser> SignInManager { get; }

        public SignInResult LogUser(LoginInputModel loginModel)
        {
            var user = this.Context.Users
                .FirstOrDefault(x => x.UserName == loginModel.Username);

            if (user == null)
            {
                return SignInResult.Failed;
            }

            var password = loginModel.Password;
            var result = this.SignInManager.PasswordSignInAsync(user, password, true, false).Result;

            return result;
        }

        public async Task<SignInResult> RegisterUser(RegisterInputModel registerModel)
        {
            bool uniqueUsername = this.Context.Users
                .All(x => x.UserName != registerModel.Username);

            if (!uniqueUsername)
            {
                return SignInResult.Failed;
            }

            var user = Mapper.Map<GetShreddedUser>(registerModel);

            await this.UserManager.CreateAsync(user);
            await this.UserManager.AddPasswordAsync(user, registerModel.Password);
            var result = await this.SignInManager.PasswordSignInAsync(user, registerModel.Password, true, false);

            return result;
        }
        
        public IndexLoggedInModel GetHomeViewDetails()
        {
            var homeViewModel = new IndexLoggedInModel
            {
                Diaries = this.Context.GetShreddedDiaries
                .Include(x => x.Ratings)
                .Include(x => x.User)
                .OrderByDescending(x => x.CreatedOn)
                .Take(2).ProjectTo<DiaryIndexOutputModel>(Mapper.ConfigurationProvider).ToList(),
            };

            return homeViewModel;
        }

        public async void Logout()
        {
            await this.SignInManager.SignOutAsync();
        }

        public UserOutputModel GetUser(string username)
        {
            var user = this.Context.Users
                .Include(x => x.GetShreddedDiaries)
                .Include(x => x.FollowedDiaries)
                .ThenInclude(x => x.GetShreddedDiary)
                .Include(x => x.Comments)
                .Include(x => x.Notifications)
                .Include(x => x.SendMessages)
                .Include(x => x.ReceivedMessages)
                .Include(x => x.DiaryRatings)
                .FirstOrDefault(x => x.UserName == username);

            var result = Mapper.Map<UserOutputModel>(user);
            result.Role = this.UserManager.GetRolesAsync(user).Result.FirstOrDefault() ?? GlobalConstants.DefaultRole;

            return result;
        }
    }
}
