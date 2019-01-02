using System.Collections.Generic;

namespace GetShredded.ViewModels.Output.Api
{
    public class ApiUserOutputModel
    {
        public ApiUserOutputModel()
        {
            this.Diaries = new HashSet<ApiGetShreddedDiaryOutputModel>();
        }

        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public ICollection<ApiGetShreddedDiaryOutputModel> Diaries { get; set; }
    }

}
