using System;
using System.ComponentModel.DataAnnotations;
using GetShredded.Common;

namespace GetShredded.ViewModels.Output.Page
{
    public class PageEditModel
    {
        public int Id { get; set; }

        public int DiaryId { get; set; }

        [StringLength(GlobalConstants.TitleMaxLength, MinimumLength = GlobalConstants.TitleMinLength)]
        public string Title { get; set; }

        public int Length => this.Content?.Length ?? 0;

        public string User { get; set; }

        [Required]
        [StringLength(GlobalConstants.PageMaxLength, MinimumLength = GlobalConstants.PageMinLength, 
            ErrorMessage = GlobalConstants.PageInputContentError)]
        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
