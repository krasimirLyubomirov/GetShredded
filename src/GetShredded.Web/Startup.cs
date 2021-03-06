﻿using GetShredded.Data;
using GetShredded.Models;
using GetShredded.Services;
using GetShredded.Services.Contracts;
using GetShredded.Web.Extensions;
using GetShredded.Web.Middlewares;

namespace GetShredded.Web
{
    using AutoMapper;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<GetShreddedContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

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

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<IDiaryService, DiaryService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<IPageService, PageService>();
            services.AddScoped<IApiService, ApiService>();

            services.AddAutoMapper(x => x.AddProfile<GetShreddedProfile>());

            services.AddMvc(opt =>
            {
                opt.Filters.Add<CustomActionFilterAttribute>();
                opt.Filters.Add<LogExceptionHandleActionFilter>();
                opt.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseSeedRolesMiddleware();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "areas",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                     name: "default",
                     template: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseCookiePolicy();
        }
    }
}