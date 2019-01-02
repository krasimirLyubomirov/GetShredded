using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GetShredded.Common;
using GetShredded.Data;
using GetShredded.Models;
using GetShredded.Services.Contracts;
using GetShredded.ViewModel.Output.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GetShredded.Services
{
    public class AdminService : BaseService, IAdminService
    {
        public AdminService(
            UserManager<GetShreddedUser> userManager,
            GetShreddedContext context, IMapper mapper, 
            RoleManager<IdentityRole> roleManager)
            : base(userManager, context, mapper)
        {
            this.RoleManager = roleManager;
        }

        protected RoleManager<IdentityRole> RoleManager { get; }

        public async Task<IEnumerable<UserAdminOutputModel>> AllUsers()
        {
            var users = this.Context.Users
                .Include(x => x.GetShreddedDiaries)
                .Include(x => x.FollowedDiaries)
                .Include(x => x.Comments)
                .Include(x => x.SendMessages)
                .Include(x => x.ReceivedMessages)
                .ToList();

            var modelUsers = Mapper.Map<IList<UserAdminOutputModel>>(users);

            for (int i = 0; i < users.Count; i++)
            {
                var current = users[i];
                var role = await this.UserManager.GetRolesAsync(current);
                modelUsers[i].Role = role.FirstOrDefault() ?? GlobalConstants.DefaultRole;
            }

            return modelUsers;
        }

        public async Task DeleteUser(string Id)
        {
            var user = await this.UserManager.FindByIdAsync(Id);

            try
            {
                await RemovingEntitiesFromDbByUserToDeleteId(user.Id);
                await this.UserManager.DeleteAsync(user);
                await this.Context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new ArgumentException(GlobalConstants.ErrorOnDeleteUser);
            }
        }

        public string AddDiaryType(string newType)
        {
            bool notExisting = this.Context.DiaryTypes.Select(x => x.Name).All(x => x != newType);

            if (notExisting)
            {
                var addedDiaryType = new DiaryType
                {
                    Name = newType
                };

                this.Context.DiaryTypes.Add(addedDiaryType);
                this.Context.SaveChanges();

                return GlobalConstants.Success;
            }

            return GlobalConstants.Failed;
        }

        public async Task<IdentityResult> ChangeRole(ChangeRoleModel model)
        {
            bool roleExist = this.RoleManager.RoleExistsAsync(model.UpdatedRole).Result;

            var user = this.UserManager.FindByIdAsync(model.Id).Result;
            var currentRole = await this.UserManager.GetRolesAsync(user);
            IdentityResult result = null;

            if (!roleExist && !currentRole.Any())
            {
                return IdentityResult.Failed();
            }

            if (!roleExist && currentRole.Any())
            {
                await this.UserManager.RemoveFromRoleAsync(user, currentRole.First());
                return IdentityResult.Success;
            }

            if (roleExist && !currentRole.Any())
            {
                result = await this.UserManager.AddToRoleAsync(user, model.UpdatedRole);
            }
            else
            {
                await this.UserManager.RemoveFromRoleAsync(user, currentRole.First());
                result = await this.UserManager.AddToRoleAsync(user, model.UpdatedRole);
            }

            return result;
        }

        public ChangeRoleModel AdminUpdateRole(string Id)
        {
            var user = this.UserManager.FindByIdAsync(Id).Result;

            var model = this.Mapper.Map<ChangeRoleModel>(user);

            model.ApplicationRoles = this.ApplicationRoles();

            model.Role = this.UserManager.GetRolesAsync(user).Result.FirstOrDefault() ?? GlobalConstants.DefaultRole;

            return model;
        }

        private ICollection<string> ApplicationRoles()
        {
            var result = this.RoleManager.Roles.Select(x => x.Name).ToArray();

            return result;
        }

        private async Task RemovingEntitiesFromDbByUserToDeleteId(string id)
        {
            var notifications = this.Context.Notifications.Where(x => x.GetShreddedUserId == id);
            var messages = this.Context.Messages.Where(x => x.ReceiverId == id || x.SenderId == id);

            this.Context.Notifications.RemoveRange(notifications);
            this.Context.Messages.RemoveRange(messages);
            await this.Context.SaveChangesAsync();
        }
    }
}
