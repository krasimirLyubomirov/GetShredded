using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using GetShredded.Common;
using GetShredded.Data;
using GetShredded.Models;
using GetShredded.Services.Contracts;
using GetShredded.ViewModels.Output.Api;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GetShredded.Services
{
    public class ApiService : BaseService, IApiService
    {
        private const int countToTake = 10;

        public ApiService(
            UserManager<GetShreddedUser> userManager, 
            GetShreddedContext context, 
            IMapper mapper)
            : base(userManager, context, mapper)
        {
        }

        public IEnumerable<ApiUserOutputModel> Users()
        {
            var result = this.Context.Users
                .Include(x => x.GetShreddedDiaries)
                .Include(x => x.Pages)
                .Where(x => x.GetShreddedDiaries.Any())
                .ProjectTo<ApiUserOutputModel>(Mapper.ConfigurationProvider)
                .ToList();

            return result;
        }

        public IEnumerable<ApiGetShreddedDiaryOutputModel> Diaries()
        {
            var result = this.Context.GetShreddedDiaries
                .Include(x => x.Pages)
                .Include(x => x.Ratings)
                .ProjectTo<ApiGetShreddedDiaryOutputModel>(Mapper.ConfigurationProvider)
                .ToList();

            return result;
        }

        public IEnumerable<ApiGetShreddedDiaryOutputModel> DiariesByType(string type)
        {
            bool typeNone = this.Context.DiaryTypes.Any(x => x.Name == type);

            if (!typeNone)
            {
                throw new ArgumentException(string.Join(GlobalConstants.NoSuchType, type));
            }

            var result = this.Context.GetShreddedDiaries
                .Include(x => x.Pages)
                .Include(x => x.Ratings)
                .Include(x => x.Type)
                .Where(x => x.Type.Name == type)
                .ProjectTo<ApiGetShreddedDiaryOutputModel>(Mapper.ConfigurationProvider)
                .ToList();

            return result;
        }

        public IEnumerable<ApiGetShreddedDiaryOutputModel> TopDiaries()
        {

            var result = this.Context.GetShreddedDiaries
                .Include(x => x.Pages)
                .Include(x => x.Ratings)
                .ProjectTo<ApiGetShreddedDiaryOutputModel>(Mapper.ConfigurationProvider)
                .OrderByDescending(x => x.Rating)
                .Take(countToTake)
                .ToList();

            return result;
        }
    }
}
