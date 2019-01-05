using System.Collections.Generic;
using GetShredded.Common;
using GetShredded.Services.Contracts;
using GetShredded.ViewModels.Output.Api;
using Microsoft.AspNetCore.Mvc;

namespace GetShredded.Web.Areas.Api.Controllers
{
    [Route(GlobalConstants.ApiRoute)]
    [ApiController]
    public class GetShreddedController : ControllerBase
    {
        public GetShreddedController(IApiService apiService)
        {
            this.ApiService = apiService;
        }

        protected IApiService ApiService { get; set; }

        [HttpGet]
        [Route(GlobalConstants.Users)]
        public ActionResult<IEnumerable<ApiUserOutputModel>> Users()
        {
            var users = this.ApiService.Users();
            return new List<ApiUserOutputModel>(users);
        }
        
        [HttpGet]
        [Route(GlobalConstants.Diaries)]
        public ActionResult<IEnumerable<ApiGetShreddedDiaryOutputModel>> Diaries()
        {
            var diaries = this.ApiService.Diaries();
            return new List<ApiGetShreddedDiaryOutputModel>(diaries);
        }
        
        [HttpGet]
        [Route(GlobalConstants.TopDiaries)]
        public ActionResult<IEnumerable<ApiGetShreddedDiaryOutputModel>> TopDiaries()
        {
            var diaries = this.ApiService.TopDiaries();
            return new List<ApiGetShreddedDiaryOutputModel>(diaries);
        }
        
        [HttpGet(GlobalConstants.GetType)]
        [Route(GlobalConstants.DiariesByType)]
        public ActionResult<IEnumerable<ApiGetShreddedDiaryOutputModel>> DiariesByType(string type)
        {
            if (type == null)
            {
                return NotFound();
            }

            var diaries = this.ApiService.DiariesByType(type);
            return new List<ApiGetShreddedDiaryOutputModel>(diaries);
        }
    }
}
