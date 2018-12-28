using System;
using System.ComponentModel.DataAnnotations;

namespace GetShredded.Models
{
    public class Comment
    {
        public int Id { get; set; }

        public string GetShreddedUserId { get; set; }
        
        public GetShreddedUser GetShreddedUser { get; set; }

        public int? GetShreddedDiaryId { get; set; }
        
        public virtual GetShreddedDiary GetShreddedDiary { get; set; }

        [Required]
        public string Message { get; set; }

        [Required]
        public DateTime CommentedOn { get; set; }
    }
}
