using System.Collections.Generic;
using GetShredded.ViewModels.Output.Api;

namespace GetShredded.Services.Contracts
{
    public interface IApiService
    {
        IEnumerable<ApiUserOutputModel> Users();

        IEnumerable<ApiGetShreddedDiaryOutputModel> TopDiaries();

        IEnumerable<ApiGetShreddedDiaryOutputModel> Diaries();

        IEnumerable<ApiGetShreddedDiaryOutputModel> DiariesByType(string type);
    }
}
