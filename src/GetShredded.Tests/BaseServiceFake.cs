using System;
using AutoMapper;
using GetShredded.Data;
using GetShredded.Models;
using GetShredded.Services;
using GetShredded.Services.Contracts;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace GetShredded.Tests
{
    [TestFixture]
    public class BaseServiceFake
    {
        protected IServiceProvider Provider { get; set; }

        protected GetShreddedContext Context { get; set; }

        [SetUp]
        public void SetUp()
        {
            Mapper.Reset();
            Mapper.Initialize(x => { x.AddProfile<GetShreddedProfile>(); });
            var services = SetServices();
            this.Provider = services.BuildServiceProvider();
            this.Context = this.Provider.GetRequiredService<GetShreddedContext>();
            SetScoppedServiceProvider();
        }

        [TearDown]
        public void TearDown()
        {
            this.Context.Database.EnsureDeleted();
        }

        private void SetScoppedServiceProvider()
        {
            var httpContext = this.Provider.GetService<IHttpContextAccessor>();
            httpContext.HttpContext.RequestServices = this.Provider.CreateScope().ServiceProvider;
        }

        private ServiceCollection SetServices()
        {
            var services = new ServiceCollection();

            services.AddDbContext<GetShreddedContext>(
                opt => opt.UseInMemoryDatabase(Guid.NewGuid()
                    .ToString()));

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<IDiaryService, DiaryService>();
            services.AddScoped<IPageService, PageService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IApiService, ApiService>();

            services.AddIdentity<GetShreddedUser, IdentityRole>(opt =>
            {
                opt.Password.RequireDigit = false;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequiredLength = 3;
                opt.Password.RequiredUniqueChars = 0;
            })
                .AddEntityFrameworkStores<GetShreddedContext>()
                .AddDefaultTokenProviders();

            services.AddAutoMapper();

            var context = new DefaultHttpContext();

            services.AddSingleton<IHttpContextAccessor>(
                new HttpContextAccessor
                {
                    HttpContext = context,
                });

            return services;
        }
    }
}
