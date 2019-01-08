using System.Collections.Generic;
using System.Threading.Tasks;
using GetShredded.ViewModel.Output.Users;
using GetShredded.ViewModels.Output.Users;
using Microsoft.AspNetCore.Identity;

namespace GetShredded.Services.Contracts
{
    public interface IAdminService
    {
        Task<IEnumerable<UserAdminOutputModel>> AllUsers();

        Task DeleteUser(string Id);

        Task<IdentityResult> ChangeRole(ChangeRoleModel model);

        ChangeRoleModel AdminUpdateRole(string Id);

        string AddDiaryType(string type);
    }
}
