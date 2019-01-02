using System.Collections.Generic;
using GetShredded.ViewModel.Output.Page;

namespace GetShredded.ViewModels.Output.Api
{
    public class ApiGetShreddedDiaryOutputModel
    {
        public ApiGetShreddedDiaryOutputModel()
        {
            this.Pages = new HashSet<PageOutputModel>();
        }

        public int Id { get; set; }

        public string User { get; set; }

        public string Title { get; set; }

        public string Summary { get; set; }

        public double Rating { get; set; }

        public string CreatedOn { get; set; }

        public string LastUpdated { get; set; }

        public string ImageUrl { get; set; }

        public string Type { get; set; }

        public ICollection<PageOutputModel> Pages { get; set; }
    }
}
