using AutoMapper;
using GetShredded.Data;
using GetShredded.Models;
using Microsoft.AspNetCore.Identity;

namespace GetShredded.Services
{
    public abstract class BaseService
    {
        protected BaseService(
            UserManager<GetShreddedUser> userManager,
            SignInManager<GetShreddedUser> signInManager,
            GetShreddedContext context,
            IMapper mapper)
        {
            this.UserManager = userManager;
            this.SingInManager = signInManager;
            this.Context = context;
            this.Mapper = mapper;
        }

        protected IMapper Mapper { get; }

        protected GetShreddedContext Context { get; }

        protected SignInManager<GetShreddedUser> SingInManager { get; }

        protected UserManager<GetShreddedUser> UserManager { get; }
    }
}
