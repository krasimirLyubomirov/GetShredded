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
            GetShreddedContext context,
            IMapper mapper)
        {
            this.UserManager = userManager;

            this.Context = context;
            this.Mapper = mapper;
        }

        protected IMapper Mapper { get; }

        protected GetShreddedContext Context { get; }

        protected UserManager<GetShreddedUser> UserManager { get; }
    }
}
