using System;

namespace GetShredded.ViewModel.Output.Page
{
    public class PageOutputModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public int Length { get; set; }

        public string User { get; set; }

        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
