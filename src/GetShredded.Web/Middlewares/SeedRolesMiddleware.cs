using System.Linq;
using System.Threading.Tasks;
using GetShredded.Common;
using GetShredded.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace GetShredded.Web.Middlewares
{
    public class SeedRolesMiddleware
    {
        private readonly RequestDelegate Next;

        public SeedRolesMiddleware(RequestDelegate next)
        {
            this.Next = next;
        }

        public async Task InvokeAsync(
            HttpContext context,
            UserManager<GetShreddedUser> userManager, 
            RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.Roles.Any())
            {
                await SeedRoles(userManager, roleManager);
            }
            
            await this.Next(context);
        }

        private async Task SeedRoles(
            UserManager<GetShreddedUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            await roleManager.CreateAsync(new IdentityRole
            {
                Name = GlobalConstants.Admin
            });

            var user = new GetShreddedUser
            {
                UserName = "Admin",
                Email = "admin@admin.admin",
                FirstName = "FirstAdmin",
                LastName = "LastAdmin"
            };

            string password = "Admin";

            await userManager.CreateAsync(user, password);

            await userManager.AddToRoleAsync(user, GlobalConstants.Admin);
        }
    }
}
