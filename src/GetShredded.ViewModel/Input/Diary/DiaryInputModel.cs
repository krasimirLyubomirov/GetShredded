using System;
using System.ComponentModel.DataAnnotations;
using GetShredded.Common;
using Microsoft.AspNetCore.Http;

namespace GetShredded.ViewModels.Input.Diary
{
    public class DiaryInputModel
    {
        [Required]
        [StringLength(GlobalConstants.TitleMaxLength, MinimumLength = GlobalConstants.TitleMinLength)]
        [DataType(DataType.Text)]
        public string Title { get; set; }

        [DataType(DataType.MultilineText)]
        [StringLength(GlobalConstants.DiarySummaryLength)]
        public string Summary { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        public string User { get; set; }
    }
}
