﻿using GetShredded.ViewModel.Input.Users;
using GetShredded.ViewModel.Output.Users;
using GetShredded.ViewModels.Output.Home;
using GetShredded.ViewModels.Output.Users;

namespace GetShredded.Services.Contracts
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity;

    public interface IUserService
    {
        SignInResult LogUser(LoginInputModel loginModel);

        Task<SignInResult> RegisterUser(RegisterInputModel registerModel);

        IndexLoggedInModel GetHomeViewDetails();

        void Logout();

        UserOutputModel GetUser(string username);
    }
}
