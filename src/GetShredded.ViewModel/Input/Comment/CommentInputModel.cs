using System;
using System.ComponentModel.DataAnnotations;
using GetShredded.Common;

namespace GetShredded.ViewModel.Input
{
    public class CommentInputModel
    {
        public int DiaryId { get; set; }

        public string CommentUser { get; set; }

        public DateTime CommentedOn { get; set; }

        [Required]
        [StringLength(GlobalConstants.CommentLength)]
        public string Message { get; set; }
    }
}
