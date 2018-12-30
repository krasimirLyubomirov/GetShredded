using System;
using System.ComponentModel.DataAnnotations;
using GetShredded.Common;

namespace GetShredded.ViewModel.Input.Page
{
    public class PageInputModel
    {
        public string User { get; set; }

        [Required]
        public int DiaryId { get; set; }

        [StringLength(GlobalConstants.TitleMaxLength, MinimumLength = GlobalConstants.TitleMinLength)]
        public string Title { get; set; }

        public DateTime CreatedOn { get; set; }

        [Required]
        [StringLength(GlobalConstants.PageMaxLength, MinimumLength = GlobalConstants.PageMinLength, 
            ErrorMessage = GlobalConstants.PageInputContentError)]
        public string Content { get; set; }
    }
}
